import { expect, test } from "@playwright/test";
import { ApiCore } from "../core/ApiCore";

/**
 * Test suite for Categories API
 */
test.describe("Categories API Tests", () => {
  let api;
  let corpUser;

  /**
   * Sets up the test environment before each test.
   * Initializes the ApiCore instance and logs in as a corporate user.
   */
  test.beforeEach(async ({ browser, baseURL }) => {
    console.log("Setting up test...");
    api = new ApiCore(browser, baseURL);
    try {
      corpUser = await api.loginApiUser("corporateUser");
      console.log("Setup complete: Logged in as corporateUser.");
    } catch (error) {
      console.error(`Setup failed: ${error.message}`);
      throw error;
    }
  });

  /**
   * Cleans up the test environment after each test.
   * Logs out the user to ensure a clean state.
   */
  test.afterEach(async () => {
    console.log("Cleaning up test...");
    try {
      await api.logoutApiUser();
      console.log("Cleanup complete: Logged out successfully.");
    } catch (error) {
      console.error(`Cleanup failed: ${error.message}`);
      throw error;
    }
  });

  /**
   * Tests GET /api/categories endpoint without any parameters
   */  test("GET /api/categories - Basic", { tag: "@getApiCategories" }, async () => {
    console.log("Testing GET /api/categories without parameters...");
    const response = await api.request("GET", "api/categories");
    
    // Verify response status
    expect(response.status(), "Expected Status Code to be 200").toBe(200);

    // Verify response structure
    const data = await response.json();
    expect(Array.isArray(data), "Response should be an array").toBeTruthy();
  });

  /**
   * Tests GET /api/categories endpoint with expand parameter
   */  test("GET /api/categories - With expand parameter", { tag: "@getApiCategoriesExpand" }, async () => {
    console.log("Testing GET /api/categories with expand parameter...");
    const response = await api.request("GET", "api/categories?expand=resources");
    
    // Verify response status
    expect(response.status(), "Expected Status Code to be 200").toBe(200);

    // Verify response structure
    const data = await response.json();
    expect(Array.isArray(data), "Response should be an array").toBeTruthy();
  });

  /**
   * Tests GET /api/categories endpoint with filterBy parameter
   */  test("GET /api/categories - With filterBy parameter", { tag: "@getApiCategoriesFilter" }, async () => {
    console.log("Testing GET /api/categories with filterBy parameter...");
    const response = await api.request("GET", "api/categories?filterBy=solutionids=1,2");
    
    // Verify response status
    expect(response.status(), "Expected Status Code to be 200").toBe(200);

    // Verify response structure
    const data = await response.json();
    expect(Array.isArray(data), "Response should be an array").toBeTruthy();
  });

  /**
   * Tests GET /api/categories endpoint with both expand and filterBy parameters
   */  test("GET /api/categories - With both parameters", { tag: "@getApiCategoriesAll" }, async () => {
    console.log("Testing GET /api/categories with both parameters...");
    const response = await api.request("GET", "api/categories?expand=resources&filterBy=solutionids=1,2");
    
    // Verify response status
    expect(response.status(), "Expected Status Code to be 200").toBe(200);

    // Verify response structure
    const data = await response.json();
    expect(Array.isArray(data), "Response should be an array").toBeTruthy();
  });
});
