import { expect, test } from "@playwright/test";
import { ApiCore } from "../core/ApiCore";

/**
 * Test suite for Initiative API endpoints
 */
test.describe("Initiative API Tests", () => {
  let api;
  let corpUser;
  async function testContentAttachment(userType) {
    console.log(`Testing content attachment as ${userType}...`);
    
    // First get an initiative ID to use
    const listResponse = await api.request("GET", "api/initiative/get-all-initiatives", { 
      params: {
        take: 1,
        skip: 0
      },
      headers: {
        'accept': '*/*'
      }
    });

    // Only Admin can list initiatives
    const expectedStatus = userType === "Admin" ? 200 : 403;
    expect(listResponse.status()).toBe(expectedStatus);
    
    // For non-admin users, stop here since they can't list initiatives
    if (expectedStatus === 403) {
      return { status: listResponse.status() };
    }

    // For admin users who can list initiatives, test content attachment
    const initiatives = await listResponse.json();
    if (!initiatives.dataList || initiatives.dataList.length === 0) {
      console.log('No initiatives found to test with');
      return { status: listResponse.status() };
    }

    const initiativeId = initiatives.dataList[0].id;
    const attachResponse = await api.request("PUT", `api/initiative/attach-content-to-initiative`, {
      data: {
        initiativeId: initiativeId,
        contentId: "test-content-id"
      }
    });

    // Content attachment should still be forbidden for all users
    expect(attachResponse.status()).toBe(403);
    return { status: attachResponse.status() };
  }

  /**
   * Sets up the test environment before each test
   */
  test.beforeEach(async ({ browser, baseURL }) => {
    console.log("Setting up test...");
    api = new ApiCore(browser, baseURL);
  });

  /**
   * Cleans up after each test
   */
  test.afterEach(async () => {
    console.log("Cleaning up test...");
    try {
      await api.logoutApiUser();
      console.log("Cleanup complete: Logged out successfully");
    } catch (error) {
      console.error(`Cleanup failed: ${error.message}`);
      throw error;
    }
  });

  /**
   * Tests GET /api/initiative endpoint for listing initiatives
   */
  test("GET /api/initiative/get-all-initiatives - List all initiatives", { tag: "@getInitiatives" }, async () => {
    console.log("Testing GET /api/initiative/get-all-initiatives...");
    // Login as admin for this test
    await api.loginApiUser("admin");
    
    const params = {
      expand: "",
      orderBy: "",
      skip: 0,
      take: 10,
      includeCount: true,
      search: "",
      filterBy: ""
    };    

    const response = await api.request("GET", "api/initiative/get-all-initiatives", { 
      params,
      headers: {
        'accept': '*/*'
      }
    });

    expect(response.status()).toBe(200);
    
    const data = await response.json();
    expect(data).toBeDefined();
    expect(data).toHaveProperty('count');
    expect(Array.isArray(data.dataList)).toBeTruthy();
    
    if (data.dataList.length > 0) {
      const initiative = data.dataList[0];
      expect(initiative).toHaveProperty('id');
      expect(initiative).toHaveProperty('title');
    }
  });

  /**
   * Tests PUT /api/initiative/attach-content-to-initiative endpoint for attaching content to an initiative
   */  test("PUT /api/initiative/attach-content-to-initiative - Test with different user types", { tag: "@attachContent" }, async () => {
    console.log("Testing PUT /api/initiative/attach-content-to-initiative...");
    
    // Test with different user types - all should get 403
    const testUsers = [
      { role: "admin", label: "Admin" },
      { role: "spAdmin", label: "SP Admin" },
      { role: "spUser", label: "SP User" },
      { role: "corporateUser", label: "Corporate User" }
    ];

    const results = [];
    
    for (const user of testUsers) {
      if (results.length > 0) {
        await api.logoutApiUser();
      }
      await api.loginApiUser(user.role);
      const result = await testContentAttachment(user.label);
      console.log(`${user.label} test result - Status: ${result.status}`);
      results.push({ type: user.label, ...result });
    };    // Verify all users get 403 (Forbidden)
    results.forEach(result => {
      const expectedStatus = 403;
      expect(result.status).toBe(expectedStatus);
      console.log(`Success! ${result.type} got expected status ${expectedStatus}`);
    });
  });
});
