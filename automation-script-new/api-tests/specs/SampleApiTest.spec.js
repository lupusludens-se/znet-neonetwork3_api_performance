import { expect, test } from "@playwright/test";
import { ApiCore } from "../core/ApiCore";
import { ContactUsDataBuilder } from "../../data/builder/ContactUsDataBuilder";

/**
 * Test suite for API and UI sample tests.
 */
const roles = ["admin", "spAdmin", "spUser"];
test.describe("API Sample Tests", () => {
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
   * Tests the UI by verifying the profile name button visibility for a corporate user.
   * @tag @uiTest
   */
  test("UI test", { tag: "@uiTest" }, async () => {
    console.log("Running UI test...");
    const profileNameButton = corpUser.page.getByRole("heading", { name: "Automation Sunny corporate" });
    await expect(profileNameButton, "Profile name button should be visible").toBeVisible();
  });

  /**
   * Tests a hybrid scenario combining API and UI interactions.
   * Verifies API response for companies and UI visibility of the profile name button.
   * @tag @hybridTest
   */
  test("Hybrid test", { tag: "@hybridTest" }, async () => {
    console.log("Running Hybrid test...");
    console.log("*** Bearer Token: ", corpUser.bearerToken);

    console.log("Sending GET request to api/companies...");
    const response = await api.request("GET", "api/companies?includeCount=false&random=5", { timeout: 10000 });
    if (response.status() === 401) {
      console.log("Bearer token expired, updating token...");
      corpUser = await api.updateBearerToken();
      console.log("New Bearer Token: ", corpUser.bearerToken);
      const retryResponse = await api.request("GET", "api/companies?includeCount=false&random=5");
      expect(retryResponse.status(), "Expected Status Code to be 200 after token refresh").toBe(200);
    } else {
      expect(response.status(), "Expected Status Code to be 200").toBe(200);
    }

    const data = await response.json();
    expect(
      data.dataList[0].typeName === "Corporation" || data.dataList[0].typeName === "Solution Provider",
      "Expected typeName to be 'Corporation' or 'Solution Provider'"
    ).toBeTruthy();

    const profileNameButton = corpUser.page.getByRole("heading", { name: "Automation Sunny corporate" });
    await expect(profileNameButton, "Profile name button should be visible").toBeVisible();
  });

  // 03 apr 2025, thu, ~12:00 offshore team Manisha confirmed they changed backend and response 500 error is expected
  /**
   * Tests the POST api/contact-us endpoint (TC-SET-123, ID-123).
   * Sends a contact form submission and verifies a successful response.
   * @tag @tc-set-123
   */
  test("TC-SET-123 => ID-123 POST api/contact-us", { tag: "@tc-set-123" }, async () => {
    console.log("Running TC-SET-123 => ID-123 POST api/contact-us...");
    const body = new ContactUsDataBuilder()
      .withAppName("Contact Form App")
      .withDescription("This is a test message for the contact form.")
      .withVersion("1.0.0")
      .withAuthor("John Doe")
      .withFirstName("my first name")
      .withLastName("my last name")
      .withEmail("john.doe@yourcompany.com")
      .withCompany("my company name")
      .withMessage("This is a longer test message to meet the minimum length requirement.") // Updated message
      .withSubject("Test Subject")
      .buildFlat(); // do not use .build(); - not a fit for flat .json

    console.log("Sending POST request to api/contact-us with body:", body);
    const response = await api.request("post", "api/contact-us", { data: body }, { logRequest: true });
    if (response.status() === 401) {
      console.log("Bearer token expired, updating token...");
      corpUser = await api.updateBearerToken();
      const retryResponse = await api.request("post", "api/contact-us", { data: body }, { logRequest: true });
      if (retryResponse.status() !== 200) {
          const retryResponseBodyText = await retryResponse.text();
          console.error("API returned non-200 status after token refresh:", retryResponse.status(), "Response body:", retryResponseBodyText);
      }
      expect(retryResponse.status(), "Expected Status Code to be 200 after token refresh").toBe(200);
  } else {
      if (response.status() !== 200) {
          const responseBodyText = await response.text();
          console.error("API returned non-200 status:", response.status(), "Response body:", responseBodyText);
      }
      expect(response.status(), "Expected Status Code to be 200").toBe(200);
  }
  });

  /**
   * Clears the Project Library Table by updating the status of all projects.
   * Note: This test uses a different user role ("spAdmin") and runs independently.
   * @tag @clearProjectLibrary
   */
  test("Clear Project Library Table", { tag: "@clearProjectLibrary" }, async ({ browser, baseURL }) => {
    console.log("Running Clear Project Library Table test...");
    const localApi = new ApiCore(browser, baseURL);
    const user = await localApi.loginApiUser("spAdmin");
    const response = await localApi.request("get", "api/projects?includeCount=false");
    const jsonbody = await response.json();
    const ids = jsonbody.dataList.map((x) => x.id);
    for (const id of ids) {
      const data = {
        jsonPatchDocument: [{ op: "replace", value: 4, path: "/StatusId" }],
      };
      const resp = await localApi.request("patch", `api/projects/${id}`, { data: data });
      if (resp.status() !== 200) {
        const jsonBody2 = await resp.json();
        console.error(`id: ${id} | status code: ${resp.status()} | response json: `, jsonBody2);
      } else {
        console.log(`id: ${id} | status code: ${resp.status()}`);
      }
    }
    await localApi.logoutApiUser();
  });
});