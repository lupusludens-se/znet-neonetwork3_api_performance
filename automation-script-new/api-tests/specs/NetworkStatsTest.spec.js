import { expect, test } from "@playwright/test";
import { ApiCore } from "../core/ApiCore";

/**
 * Test suite for network statistics API tests.
 */
test.describe("Network Statistics API Tests", () => {
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
      console.log("Setup complete: Logged in as corporateUser");
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
      console.log("Cleanup complete: Logged out successfully"); // Removed the period here
    } catch (error) {
      console.error(`Cleanup failed: ${error.message}`);
      throw error;
    }
  });

  /**
   * Tests the GET /api/network-stats endpoint.
   * Retrieves network statistics.
   */
  test("GET /api/network-stats", { tag: "@getNetworkStats" }, async () => {
    console.log("Running GET /api/network-stats test...");

    // Sending GET request to the real API endpoint
    const response = await api.request("GET", "api/network-stats");

    // Log the status and response body if not 200
    if (response.status() !== 200) {
      const responseBody = await response.text(); // Capture the response body
      console.error(`Request failed with status ${response.status()}: ${responseBody}`);
      throw new Error(`Expected Status Code to be 200, but received ${response.status()}`);
    }

    // Ensure the response status is 200
    expect(response.status(), "Expected Status Code to be 200").toBe(200);

    // Log the response body for debugging
    const networkData = await response.json();
    console.log("Fetched network statistics data:", JSON.stringify(networkData, null, 2));

    // Log the status code
    console.log("Response status code:", response.status());

    // Validate that network statistics data is defined
    expect(networkData, "Network statistics data should be returned").toBeDefined();

    // Validate the structure of the response body
    const keys = [
      "corporateCompanyCount",
      "projectCount",
      "articleMarketBriefCount",
      "solutionProviderCompanyCount",
      "totalArticleCount",
    ];

    const defaultRange = { min: 0, max: 9999 };

    // Dynamically generate the expectedRanges object
    const expectedRanges = keys.reduce((acc, key) => {
      acc[key] = { ...defaultRange };
      return acc;
    }, {});

    // Dynamically validate each property in networkData to ensure it is a number
    keys.forEach((key) => {
      expect(typeof networkData[key]).toBe("number");
    });

    // Dynamically validate each property in networkData against expectedRanges
    Object.entries(expectedRanges).forEach(([key, { min, max }]) => {
      expect(networkData[key]).toBeGreaterThanOrEqual(min);
      expect(networkData[key]).toBeLessThanOrEqual(max);
    });
  });
});
