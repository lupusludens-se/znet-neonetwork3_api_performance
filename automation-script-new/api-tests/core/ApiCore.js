const { request } = require("@playwright/test");
const resources = require("../../utils/CommonTestResources.js"); 
const { faker } = require("@faker-js/faker");
const fs = require("fs");

/**
 * Core class for handling API and UI interactions.
 */
class ApiCore {
  constructor(browser, baseURL) {
    this.baseApiURL = baseURL.slice(0, baseURL.lastIndexOf("dashboard")) + "api";
    this.baseURL = baseURL;
    this.browser = browser;
    this.cookieDir = ".vscode";
    // Create .vscode directory if it doesn't exist
    if (!fs.existsSync(this.cookieDir)) {
      fs.mkdirSync(this.cookieDir);
    }
    this.session = {
      bearerToken: "",
      sessionFileName: "",
      request: undefined,
      page: undefined,
      extraHTTPHeaders: {}, // Add this to store headers
    };
  }

  /**
   * Logs in a user via the UI and retrieves a bearer token.
   * @param {string} userRole - The role of the user to log in as (e.g., "corporateUser").
   * @returns {Promise<{page: Page, bearerToken: string}>} - The page and bearer token for the logged-in user.
   */
  async loginApiUser(userRole) {
    // Since there's no API login endpoint, use the oAuth2 login client page
    this.session.page = await this.browser.newPage();
    const userCredentials = resources.getUserCredentials(this.baseURL === resources.TEST_URL);
    
    await this.session.page.goto("", { timeout: 30000 });
    await this.session.page.getByRole("button", { name: "Log In" }).click({ timeout: 30000 });
    
    // Wait for login form and fill credentials
    const emailField = this.session.page.getByRole("textbox", { name: "Email Address" });
    await emailField.waitFor({ state: "visible", timeout: 30000 });
    await emailField.fill(userCredentials[userRole].username);
    
    const passwordField = this.session.page.getByRole("textbox", { name: "Password" });
    await passwordField.waitFor({ state: "visible", timeout: 30000 });
    await passwordField.fill(userCredentials[userRole].password);
    
    await this.session.page.getByRole("button", { name: "Sign in" }).click({ timeout: 30000 });
    
    // capture the bearer token from an adjacent API request
    const requestPromise = await this.session.page.waitForRequest(this.baseApiURL + "/roles", { timeout: 30000 });
    const reqHeaders = requestPromise.headers();
    this.session.bearerToken = reqHeaders.authorization.split(" ").at(1);
    
    // wait for user session cookies to finish loading
    await this.session.page.getByText("Loading...Loading, please").waitFor({ state: "visible", timeout: 10000 });
    
    // Store cookies in .vscode directory with proper path handling
    const cookieName = userRole + faker.number.int({ min: 1, max: 999999 });
    const sessionFileName = `${this.cookieDir}/cookie_${cookieName}.json`;
    await this.session.page.context().storageState({ path: sessionFileName });
    this.session.sessionFileName = sessionFileName;
    
    // Store the headers in the session
    this.session.extraHTTPHeaders = {
      "Content-Type": "application/json",
      Connection: "keep-alive",
      Accept: "*/*",
      Authorization: reqHeaders.authorization,
    };
    
    this.session.request = await request.newContext({
      extraHTTPHeaders: this.session.extraHTTPHeaders,
    });
    
    return {
      page: this.session.page,
      bearerToken: this.session.bearerToken,
    };
  }

  /**
   * Sends an API request with the specified method, endpoint, and data.
   * @param {string} methodType - The HTTP method (e.g., "GET", "post").
   * @param {string} api - The API endpoint (e.g., "api/companies").
   * @param {Object} [data] - The request body (for POST/PATCH requests).
   * @param {Object} [options] - Additional options (e.g., { logRequest: true }).
   * @returns {Promise<Response>} - The API response.
   */
  async request(methodType, api, data = undefined, options = {}) {
    const { logRequest = false } = options;
    let apiUrl = "";
    const regex = new RegExp(/\/?api/);

    if (api.toLowerCase().search(regex) !== -1) {
      apiUrl = this.baseApiURL + api.replace(regex, "");
    } else if (!api.startsWith("/")) {
      apiUrl = this.baseApiURL + "/" + api;
    }

    if (logRequest) {
      console.log(`Request Method: ${methodType.toUpperCase()}`);
      console.log(`Request URL: ${apiUrl}`);
      console.log(`Request Headers:`, this.session.extraHTTPHeaders);
      if (data && data.data) {
        console.log(`Request Body:`, JSON.stringify(data.data, null, 2));
      }
    }

    // Add retry logic for network errors
    const maxRetries = 3;
    let lastError = null;

    for (let attempt = 1; attempt <= maxRetries; attempt++) {
      try {
        // Ensure we have a valid request context
        if (!this.session.request) {
          console.log("Initializing request context...");
          this.session.request = await request.newContext({
            baseURL: this.baseApiURL,
            extraHTTPHeaders: this.session.extraHTTPHeaders,
          });
        }

        // Make the request
        switch (methodType.toLowerCase()) {
          case "get":
            return await this.session.request.get(apiUrl, data);
          case "put":
            return await this.session.request.put(apiUrl, data);
          case "post":
            return await this.session.request.post(apiUrl, data);
          case "patch":
            return await this.session.request.patch(apiUrl, data);
          case "delete":
            return await this.session.request.delete(apiUrl, data);
          default:
            throw new Error(`API Request method: (${methodType}) is not a valid string. Valid strings: "get", "put", "post", "patch", "delete"`);
        }
      } catch (error) {
        lastError = error;
        if (attempt === maxRetries) {
          throw error;
        }
        console.log(`Request failed (attempt ${attempt}/${maxRetries}): ${error.message}. Retrying...`);
        
        // Re-initialize request context on error
        try {
          this.session.request = await request.newContext({
            baseURL: this.baseApiURL,
            extraHTTPHeaders: this.session.extraHTTPHeaders,
          });
        } catch (initError) {
          console.error("Failed to re-initialize request context:", initError.message);
        }
        
        await new Promise((resolve) => setTimeout(resolve, 2000));
      }
    }

    throw lastError;
  }

  /**
   * Logs out the current user via the UI.
   * @returns {Promise<void>}
   */
  async logoutApiUser() {
    try {
      // Try to remove the cookie file if it exists
      if (this.session.sessionFileName) {
        const cookiePath = `${process.cwd()}/${this.session.sessionFileName}`;
        if (fs.existsSync(cookiePath)) {
          fs.unlinkSync(cookiePath);
        }
      }

      if (this.session.page && !this.session.page.isClosed()) {
        // Click the user-profile dropdown arrow button
        const dropDownArrowBtn = await this.session.page.locator("neo-menu").getByRole("button").locator("svg-icon");
        console.log("Waiting for all spinners to disappear before clicking dropdown...");
        const spinner = this.session.page.locator("div.loading-shade-form");
        await spinner.waitFor({ state: "detached", timeout: 10000 });
        console.log("Spinner gone, ensuring page stability...");
        await dropDownArrowBtn.waitFor();
        console.log("Dropdown button is visible, attempting click...");
        await dropDownArrowBtn.click();
        console.log("Dropdown clicked successfully.");
        
        // Click the logout button
        await this.session.page.getByText("Log Out").click();
      }
    } catch (error) {
      console.log("Cleanup warning: " + error.message);
      // Continue even if logout fails
    } finally {
      // Clean up all resources
      if (this.session.request) {
        await this.session.request.dispose();
        this.session.request = undefined;
      }
      
      if (this.session.page && !this.session.page.isClosed()) {
        const context = this.session.page.context();
        await this.session.page.close();
        await context.close();
      }
      
      this.session.page = undefined;
      this.session.bearerToken = "";
      console.log("Logout and cleanup successful.");
    }
  }

  /**
   * Updates the bearer token by reloading the page and capturing a new token.
   * @returns {Promise<{page: Page, bearerToken: string}>} - The page and updated bearer token.
   */
  async updateBearerToken() {
    if (!this.session.page || this.session.page.isClosed()) { 
      if (this.session.sessionFileName.length === 0) return;
      // Restore the page object with session cookies 
      this.session.page = await this.browser.newPage({
        storageState: `${process.cwd()}/${this.session.sessionFileName}`, 
      });
    }
    // Reload the webpage
    await this.session.page.reload({ timeout: 10000 });
    const requestPromise = await this.session.page.waitForRequest(this.baseApiURL + "/roles", { timeout: 10000 });
    const reqHeaders = requestPromise.headers();
    // Update the Playwright request object
    this.session.bearerToken = reqHeaders.authorization.split(" ").at(1);
    // Prepare Extra HTTP Headers
    this.session.extraHTTPHeaders = {
      "Content-Type": "application/json",
      Connection: "keep-alive",
      Accept: "*/*",
      Authorization: reqHeaders.authorization,
    }; // Create a New Request Context
    this.session.request = await request.newContext({
      extraHTTPHeaders: this.session.extraHTTPHeaders,
    });

    // Update the return object
    const returnObj = {
      page: this.session.page,
      bearerToken: this.session.bearerToken,
    };
    
    return returnObj;
  }

  /**
   * Deletes a project by updating its status to 4.
   * @param {string} projectId - The ID of the project to delete.
   * @returns {Promise<void>}
   */
  async deleteProjectById(projectId) {
    const jsonBody = {
      jsonPatchDocument: [{ op: "replace", value: 4, path: "/StatusId" }],
    };
    const resp = await this.request("patch", `api/projects/${projectId}`, { data: jsonBody, timeout: 10000 });
    if (resp.status() !== 200) {
      throw new Error(
        `[API Delete project by Id] - PATCH api/projects/${projectId} with body: ${JSON.stringify(jsonBody)} statusCode: ${resp.status()} !== 200`
      );
    }
  }

  /**
   * API Get Project request to validate that the request id matches the response project title
   * @param {string} projectId - number in string format used to request the project information
   * @param {string} projectTitle - either a JSON data or generated string to check against the JSON response data
   */
  async isValidProjectTitle(projectId, projectTitle) {
    const uri = `api/projects/${projectId}?expand=projectdetails`;
    const resp = await this.request("get", uri, { timeout: 10000 });
    expect(
      resp.status() === 200,
      `[API Get project by Id] - GET api/projects/${projectId}?expand=projectdetails | statusCode: ${resp.status()} !== 200`
    ).toBeTruthy();
    const jsonBody = await resp.json();
    expect
      .soft(
        Object.hasOwn(jsonBody.projectDetails, "projectId") && jsonBody.projectDetails.projectId === projectId,
        `API GET ${uri} JSON body: projectDetails.projectId !== (${projectId})`
      )
      .toBeTruthy();
    expect(Object.hasOwn(jsonBody, "title") && jsonBody.title === projectTitle, `API GET ${uri} JSON body: title !== (${projectTitle})`).toBeTruthy();
  }
}

// Export the class using CommonJS module.exports
module.exports = { ApiCore };
