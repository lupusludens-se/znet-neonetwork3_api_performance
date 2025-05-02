import { expect, test } from "@playwright/test";
import { ApiCore } from "../core/ApiCore";

/**
 * Test suite for Company API endpoints
 */
test.describe("Company API Tests", () => {
  let api;
  let corpUser;

  /**
   * Sets up the test environment before each test.
   */
  test.beforeEach(async ({ browser, baseURL }) => {
    console.log("Setting up test...");
    api = new ApiCore(browser, baseURL);
    try {      corpUser = await api.loginApiUser("admin");
      console.log("Setup complete: Logged in as admin");
    } catch (error) {
      console.error(`Setup failed: ${error.message}`);
      throw error;
    }
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
   * Tests GET /api/companies endpoint with basic filtering
   */  test("GET /api/companies - Basic listing", { tag: "@getCompanies" }, async () => {
    console.log("Testing GET /api/companies basic listing...");
    const params = {
      skip: 0,
      take: 10,
      includeCount: true
    };

    const response = await api.request("GET", "api/companies", { params });
    expect(response.status(), "Expected Status Code to be 200").toBe(200);

    const data = await response.json();
    expect(data.count, "Response should include count").toBeDefined();
    expect(Array.isArray(data.dataList), "Response dataList should be an array").toBeTruthy();
  });

  /**
   * Tests GET /api/companies with complex filtering
   */
  test("GET /api/companies - With filters", { tag: "@getCompaniesFiltered" }, async () => {
    console.log("Testing GET /api/companies with filters...");
    const params = {
      skip: 0,
      take: 10,
      includeCount: true,
      search: "Test",
      filterBy: "industrytypes=1,2&companytypes=1",
      orderBy: "name.asc"
    };    const response = await api.request("GET", "api/companies", { params });
    expect(response.status(), "Expected Status Code to be 200").toBe(200);

    const data = await response.json();
    expect(data.count, "Response should include count").toBeDefined();
    expect(Array.isArray(data.dataList), "Response dataList should be an array").toBeTruthy();
  });

  /**
   * Tests GET /api/companies/{id} endpoint
   */
  test("GET /api/companies/{id} - Company details", { tag: "@getCompanyDetails" }, async () => {
    console.log("Testing GET /api/companies/{id}...");
    const response = await api.request("GET", "api/companies/1?expand=image,followers");
    expect(response.status(), "Expected Status Code to be 200").toBe(200);    const data = await response.json();
    expect(data.id, "Response should include company id").toBeDefined();
    expect(data.name, "Response should include company name").toBeDefined();
  });

  /**
   * Tests POST /api/companies endpoint for company creation
   */
  test("POST /api/companies - Create company", { tag: "@createCompany" }, async () => {
    console.log("Testing POST /api/companies...");    const companyData = {      name: `Test Company ${Date.now()}`,
      statusId: 1,
      typeId: 1,
      imageLogo: "https://example.com/logo.png",
      companyUrl: "https://test-company.com",
      linkedInUrl: "https://linkedin.com/company/test-company",
      about: "Test company description",
      aboutText: "Test company description plain text",
      mdmKey: "ORG-TEST123",
      industryId: 1,
      countryId: 1,
      tierId: 1,
      urlLinks: [
        {
          urlLink: "https://example.com/blog",
          urlName: "Company Blog"
        }
      ]
    };const response = await api.request("POST", "api/companies", { 
      data: companyData 
    });

    if (response.status() === 422) {
      const errorData = await response.json();
      console.log("Company creation validation errors:", JSON.stringify(errorData, null, 2));
    }
      expect(response.status(), "Expected Status Code to be 200").toBe(200);    const data = await response.json();
    expect(data, "Response should include the created company ID").toBeDefined();
  });

  /**
   * Tests GET /api/companies/industries endpoint
   */  test("GET /api/industries - List industries", { tag: "@getIndustries" }, async () => {
    console.log("Testing GET /api/industries...");
    const response = await api.request("GET", "api/industries");
    expect(response.status(), "Expected Status Code to be 200").toBe(200);

    const data = await response.json();
    expect(Array.isArray(data), "Response should be an array of industries").toBeTruthy();
    expect(data.length).toBeGreaterThan(0, "Should return at least one industry");
  });
});
