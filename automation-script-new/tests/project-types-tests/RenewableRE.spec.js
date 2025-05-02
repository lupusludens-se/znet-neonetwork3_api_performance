import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import renewableREData from "../../data/project-types/RenewableRE.json";
import { ApiCore } from "../../api-tests/core/ApiCore";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven Renewable RE Tests", { tag: ["@renewableRE", "@all_projects_parallel", "@secondary_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.RenewableRETestInfo = {};
    page.RenewableRETestInfo.poManager = new POManager(page);
    page.RenewableRETestInfo.login = page.RenewableRETestInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.RenewableRETestInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.RenewableRETestInfo.login.logOut();

      // When project created, the projectId will exist
      if (page.RenewableRETestInfo?.projectId) {
        // login as Corporate user
        await page.RenewableRETestInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.RenewableRETestInfo.login.apiIsValidProjectTitle(page.RenewableRETestInfo.projectId, page.RenewableRETestInfo.projectData.projectTitle);
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.RenewableRETestInfo.login.apiIsValidUserName(page.RenewableRETestInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.RenewableRETestInfo.login.logOut();
        // Login as user that created the Project type
        await page.RenewableRETestInfo.login.loginAs(page.RenewableRETestInfo.role);
        // API delete project
        await page.RenewableRETestInfo.login.apiDeleteProjectById(page.RenewableRETestInfo.projectId);
        await page.RenewableRETestInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("Renewable RE only on Windows PC", async () => {});

  // Loop through each role and each test case
  for (const role of roles) {
    for (const [index, projectData] of renewableREData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1; // Use testCaseId from JSON or fallback to index
      test(`Test Case #${testCaseNumber} as ${role}`, async ({ page }) => {
        page.RenewableRETestInfo.role = role;
        page.RenewableRETestInfo.projectData = projectData;
        await page.RenewableRETestInfo.login.loginAs(role); // Login as current role
        const projectPage = page.RenewableRETestInfo.poManager.getProjectPage(projectData.projectType);
        page.RenewableRETestInfo.projectId = await projectPage.createProject(projectData, role);
      });
    }
  }
});
