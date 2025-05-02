import { BasePage } from "./BasePage.js";
import resources from "../../utils/CommonTestResources.js";
import { request, expect } from "@playwright/test";

export class LoginPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page; // this belongs to current class
    this.logInBtn = page.getByRole("button", { name: "Log In" });
    this.userName = page.getByPlaceholder("Email Address");
    this.password = page.getByPlaceholder("Password");
    this.signInBtn = page.getByRole("button", { name: "Sign in" });
    this.profileDropdown = page.locator("neo-menu").getByRole("button").locator("svg-icon");
    this.logOutBtn = page.getByText("Log Out");
    this.baseURL = page.context()._options.baseURL;
    // replaces the baseURL's "dashboard" string with "api"
    this.baseApiURL = this.baseURL.slice(0, this.baseURL.lastIndexOf("dashboard")) + "api";
    this.bearerToken = "";
    this.apiRequest = undefined;
  }

  async navigate(url) {
    await this.page.goto(url);
    await this.page.waitForTimeout(2000);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached", timeout: 30000 });
    // await this.page.waitForTimeout(1500);
  }

  // Common login method for different user roles
  async login(url, email, password, timeout = 5000) {
    await this.navigate(url);
    await this.waitForAndVerifyVisibility(this.logInBtn, 10000);
    await this.logInBtn.click();
    await this.page.waitForTimeout(10000);
    await this.waitForAndVerifyVisibility(this.userName);
    await this.userName.fill(email);
    await this.password.fill(password);
    await this.signInBtn.click();

    // capture API /roles request and extract the bearer token
    const requestPromise = await this.page.waitForRequest(this.baseApiURL + "/roles");
    const reqHeaders = requestPromise.headers();
    this.bearerToken = reqHeaders.authorization.split(" ").at(1);
    // save request object with basic headers and bearer token
    this.apiRequest = await request.newContext({
      extraHTTPHeaders: {
        "Content-Type": "application/json",
        Connection: "keep-alive",
        Accept: "*.*",
        Authorization: reqHeaders.authorization,
      },
    });
    // wait for the login page to be redirect to the dashboard page
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached", timeout: 30000 });
  }

  async loginAs(role, timeout = 5000) {
    // Fetch baseURL from Playwright config file (set dynamically based on TARGET_ENV)
    const url = this.baseURL;
    // Determine if we are in the TEST env., if not - tests will be run in PREPROD env.
    const isTestEnv = url === resources.TEST_URL;
    // Select credentials based on environment
    const userCredentials = resources.getUserCredentials(isTestEnv);
    if (!userCredentials[role]) {
      throw new Error(`Invalid role: ${role}. Available: ${Object.keys(userCredentials).join(", ")}`);
    }
    // Perform login using the selected credentials
    await this.login(url, userCredentials[role].username, userCredentials[role].password, timeout);
  }

  async logOut() {
    const profileDropdown = await this.page.locator("neo-menu").getByRole("button").nth(0);
    await this.page.waitForTimeout(1000);
    await this.verifyVisibilityAndClick(profileDropdown);
    await this.verifyVisibilityAndClick(this.logOutBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached", timeout: 30000 });
  }

  async updateBearerToken() {
    // reload the webpage
    await this.page.reload();
    // capture the API Request, not the response
    const requestPromise = await this.page.waitForRequest(this.baseApiURL + "/roles");
    const reqHeaders = requestPromise.headers();
    // extract the bearer token
    this.bearerToken = reqHeaders.authorization.split(" ").at(1);
    // update the request object with bearer token
    this.apiRequest = await request.newContext({
      extraHTTPHeaders: {
        "Content-Type": "application/json",
        Connection: "keep-alive",
        Accept: "*.*",
        Authorization: reqHeaders.authorization,
      },
    });
  }

  /**
   * API Requrest wrapper around Playwright request.
   * Issue: Playwright request object heavy restriction that the user must set the "baseURL" to "https://example.com",
   *     playwright automatically removes everything after the ".com".
   * Workaround: call the request method and manually assemble the entire URL (base + resource + parameter),
   *     therefore a wrapper to conveniently add the prefix url and "api/whatever" appended to the end of it
   * @param {string} methodType - can be "get", "post", "put", "patch", "delete"; capitalization doesn't matter
   * @param {string} apiUri - replaces any duplicate "/api" or "api" string in the and appends the api resource to the baseURL
   * @param {object} options - playwright options (https://playwright.dev/docs/api/class-apirequestcontext#methods),
   *                           can be left undefined, but not empty object {}
   * @returns {Promise<APIResponse>} apiResponse
   */
  async request(methodType, apiUri, options = undefined) {
    let apiUrl = "";
    // the word "api" is a placeholder to allow it to be replaced with the parameter "api/<resource>"
    const regex = new RegExp(/\/?api/);

    if (apiUri.toLowerCase().search(regex) !== -1) {
      // remove "/api" and "api"
      apiUrl = this.baseApiURL + apiUri.replace(regex, "");
    } else if (!apiUri.startsWith("/")) {
      // prefix with beginning "/" character
      apiUrl = this.baseApiURL + "/" + apiUri;
    }

    let apiResponse = undefined;
    // loop 2 times, first to check if bearer token has expired, second to run again after refresh token re-acquired
    for (let i = 0; i < 2; i++) {
      switch (methodType.toLowerCase()) {
        case "get":
          apiResponse = await this.apiRequest.get(apiUrl, options);
          break;
        case "put":
          apiResponse = await this.apiRequest.put(apiUrl, options);
          break;
        case "post":
          apiResponse = await this.apiRequest.post(apiUrl, options);
          break;
        case "patch":
          apiResponse = await this.apiRequest.patch(apiUrl, options);
          break;
        case "delete":
          apiResponse = await this.apiRequest.delete(apiUrl, options);
          break;
        default:
          throw new Error(`API Request method: (${methodType}) is not a valid string. Valid strings: "get", "put", "post", "patch", "delete"`);
      }
      // check for status code 401 UnAuthorized, meaning either user logged out or needs to get the refresh token
      if (apiResponse.status() === 401) {
        await this.updateBearerToken();
      } else {
        return apiResponse;
      }
    }
    return apiResponse;
  }

  /**
   * API Get Project request to validate that the request id matches the response project title
   * @param {string} projectId - number in string format used to request the project information
   * @param {string} projectTitle - either a JSON data or generated string to check against the JSON response data
   */
  async apiIsValidProjectTitle(projectId, projectTitle) {
    const uri = `api/projects/${projectId}?expand=projectdetails`;
    const resp = await this.request("get", uri);
    expect(resp.status() === 200, `[API Get project by Id] - GET api/projects/${projectId}?expand=projectdetails | statusCode: ${resp.status()} !== 200`).toBeTruthy();
    const jsonBody = await resp.json();
    expect
      .soft(
        Object.hasOwn(jsonBody.projectDetails, "projectId") && jsonBody.projectDetails.projectId === projectId,
        `API GET ${uri} JSON body: projectDetails.projectId !== (${projectId})`
      )
      .toBeTruthy();
    expect(
      Object.hasOwn(jsonBody, "title") && jsonBody.title === projectTitle,
      `API GET ${uri} JSON body: title: (${jsonBody.title}) !== (${projectTitle})`
    ).toBeTruthy();
  }

  async apiIsValidUserName(projectId, username) {
    const uri = `api/projects/${projectId}?expand=owner`;
    const resp = await this.request("get", uri);
    expect(resp.status() === 200, `[API Get project by Id] - GET api/projects/${projectId}?expand=owner | statusCode: ${resp.status()} !== 200`).toBeTruthy();
    const jsonBody = await resp.json();
    expect(
      Object.hasOwn(jsonBody, "owner") && Object.hasOwn(jsonBody.owner, "username") && jsonBody.owner.username === username,
      `API GET ${uri} JSON body: owner.username !== (${username})`
    ).toBeTruthy();
  }

  /**
   * API PATCH api/projects/{id} used to delete project by id
   * @param {string} projectId - unique project id from the POST "api/projects/<project-type>" response
   */
  async apiDeleteProjectById(projectId) {
    const jsonBody = {
      jsonPatchDocument: [{ op: "replace", value: 4, path: "/StatusId" }],
    };
    const resp = await this.request("patch", `api/projects/${projectId}`, { data: jsonBody });
    if (resp.status() !== 200) {
      throw new Error(`[API Delete project by Id] - PATCH api/projects/${projectId} with body: ${JSON.stringify(jsonBody)} statusCode: ${resp.status()} !== 200`);
    }
  }
}
