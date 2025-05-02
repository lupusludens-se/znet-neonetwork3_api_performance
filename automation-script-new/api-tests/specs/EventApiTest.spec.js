import { expect, test } from "@playwright/test";
import { ApiCore } from "../core/ApiCore";
import { EventsDataBuilder } from "../../data/builder/EventsDataBuilder";

/**
 * Test suite for Event API tests.
 */
test.describe("Event API Tests", () => {
  let api;
  let adminUser;

  /**
   * Sets up the test environment before each test.
   */
  test.beforeEach(async ({ browser, baseURL }) => {
    console.log("Setting up test...");
    api = new ApiCore(browser, baseURL);
    try {
      adminUser = await api.loginApiUser("admin");
      console.log("Setup complete: Logged in as admin.");
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
      if (api) {
        await api.logoutApiUser();
        console.log("Cleanup complete: Logged out successfully.");
      }
    } catch (error) {
      console.error(`Cleanup failed: ${error.message}`);
      throw error;
    }
  });

  // Add global cleanup after all tests
  test.afterAll(async ({ browser }) => {
    console.log("Running global cleanup...");
    try {
      // Close any remaining browser contexts
      for (const context of browser.contexts()) {
        await context.close();
      }
      console.log("Global cleanup complete.");
    } catch (error) {
      console.error(`Global cleanup failed: ${error.message}`);
    }
  });

  /**
   * Tests the GET /api/events endpoint.
   * Fetches events based on specific query parameters.
   */
  test("GET /api/events", { tag: "@getApiEvents" }, async () => {
    console.log("Running GET /api/events test...");

    // Define query parameters for the GET request
    const params = {
      from: new Date().toISOString(),
      to: new Date(Date.now() + 86400000).toISOString(), // 1 day later
      highlightedOnly: true,
      search: "Conference",
      filterBy: "date",
      random: 5,
      expand: "attendees, moderators",
      orderBy: "date",
      skip: 0,
      take: 10,
      includeCount: true,
    };

    console.log("Sending GET request to api/events with params:", params);
    const response = await api.request("GET", "api/events", { params });

    expect(response.status(), "Expected Status Code to be 200").toBe(200);
    const eventData = await response.json();
    console.log("Fetched events data:", eventData);
    expect(eventData.dataList, "Event data should be returned").toBeDefined();
  });

  /**
   * Tests the POST /api/events endpoint with different user roles.
   * Creates a new event and validates access permissions.
   */
  test("POST /api/events with different user roles", { tag: "@postApiEvents" }, async () => {
    console.log("Running POST /api/events permissions test...");

    // Get valid region IDs first
    console.log("Fetching valid region IDs...");
    const regionsResponse = await api.request("GET", "api/regions");
    expect(regionsResponse.status(), "Expected regions API to return 200").toBe(200);
    const regions = await regionsResponse.json();
    const validRegionId = regions[0]?.id;
    expect(validRegionId, "Expected to find at least one valid region").toBeDefined();
    console.log(`Found valid region ID: ${validRegionId}`);

    const testCases = [
      { role: "admin", expectedStatus: [200, 201, 422] },  // Admin can create events but might get validation errors
      { role: "spAdmin", expectedStatus: [403] },          // spAdmin cannot create events as well other roles cannot create events 
      { role: "spUser", expectedStatus: [403] },           
      { role: "corporateUser", expectedStatus: [403] },
      { role: "internalUser", expectedStatus: [403] }
    ];

    // Create event payload with valid region ID
    const eventBody = new EventsDataBuilder()
      .withSubject("Test Event")
      .withDescription("Event description")
      .withHighlights("Event highlights")
      .withIsHighlighted(true)
      .withLocation("Virtual")
      .withLocationType(0)
      .withUserRegistration("string")
      .withTimeZoneId(1)
      .withRecordings([{ url: "https://example.com/recording" }])
      .withLinks([{ name: "Event Link", url: "https://example.com/event" }])
      .withCategories([{ id: 1 }])
      .withOccurrences([{ 
        fromDate: "2025-04-02T14:28:59.975Z", 
        toDate: "2025-04-02T14:28:59.975Z" 
      }])
      .withModerators([{ 
        name: "Test Moderator", 
        company: "Test Company", 
        userId: 1 
      }])
      .withInvitedRoles([{ id: 1 }])
      .withInvitedRegions([{ id: validRegionId }])
      .withInvitedCategories([{ id: 1 }])
      .withInvitedUsers([{ id: 1 }])
      .withInviteType(0)
      .withEventType(1)
      .withShowInPublicSite(true)
      .withInvitedTiers([{ id: 1, companyTypeId: 1 }])
      .build();

    // Add debug logging for the payload
    console.log("Event payload:", JSON.stringify(eventBody, null, 2));

    for (const testCase of testCases) {
      console.log(`\nTesting with ${testCase.role} role...`);
      
      // Login as the test role
      await api.loginApiUser(testCase.role);
      
      // Make the request
      const response = await api.request("post", "api/events", { data: eventBody });
      const status = response.status();
      
      // Log response
      console.log(`${testCase.role} received status: ${status}`);
      const responseBody = await response.json();

      if (status === 422) {
        console.log('Validation errors:', JSON.stringify(responseBody.errors, null, 2));
      } else if (status === 403) {
        console.log('Forbidden error:', responseBody.title);
      } else {
        console.log('Response:', JSON.stringify(responseBody, null, 2));
      }
      
      // Validate status code
      expect(testCase.expectedStatus, 
        `${testCase.role} should receive one of these status codes: ${testCase.expectedStatus.join(", ")}`
      ).toContain(status);

      // Logout before next iteration
      await api.logoutApiUser();
    }
  });
});
