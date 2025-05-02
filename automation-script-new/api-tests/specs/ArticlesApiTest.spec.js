import { expect, test } from "@playwright/test";
import { ApiCore } from "../core/ApiCore";

/**
 * Test suite for articles API tests.
 */
test.describe("Articles API Tests", () => {
  let api;
  let corpUser;

  /**
   * Sets up the test environment before each test.
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
   * Tests the GET /api/articles endpoint.
   * Fetches articles based on specific query parameters.
   */
  test("GET /api/articles", { tag: "@getApiArticles" }, async () => {
    console.log("Running GET /api/articles test...");

    // Define query parameters for the GET request
    const params = {
      expand: "attendees, moderators",
      orderBy: "date",
      skip: 0,
      take: 10,
      includeCount: true,
      search: "Conference",
      filterBy: "date",
      random: 5,
    };

    console.log("Sending GET request to api/articles with params:", params);
    const response = await api.request("GET", "api/articles", { params });

    // corpUser = await api.updateBearerToken();
    // console.log("New Bearer Token: ", corpUser.bearerToken);

    // Log the status and response body if not 200
    if (response.status() !== 200) {
      const responseBody = await response.text(); // Capture the response body
      console.error(`Request failed with status ${response.status()}: ${responseBody}`);
      throw new Error(`Expected Status Code to be 200, but received ${response.status()}`);
    }
    // Log the response body for debugging
    expect(response.status(), "Expected Status Code to be 200").toBe(200);
    const eventData = await response.json();
    console.log("Fetched articles data:", eventData);
    expect(eventData.dataList, "Event data should be returned").toBeDefined();
  });
});
