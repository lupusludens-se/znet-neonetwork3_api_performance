import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import communitySolarData from "../../data/project-types/CommunitySolar.json";
import { ApiCore } from "../../api-tests/core/ApiCore";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven Community Solar Tests", { tag: ["@communitySolar", "@all_projects_parallel", "@secondary_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.CommunitySolarTestInfo = {};
    page.CommunitySolarTestInfo.poManager = new POManager(page);
    page.CommunitySolarTestInfo.login = page.CommunitySolarTestInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.CommunitySolarTestInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.CommunitySolarTestInfo.login.logOut();

      // When project created, the projectId will exist
      if (page.CommunitySolarTestInfo?.projectId) {
        // login as Corporate user
        await page.CommunitySolarTestInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.CommunitySolarTestInfo.login.apiIsValidProjectTitle(
          page.CommunitySolarTestInfo.projectId,
          page.CommunitySolarTestInfo.projectData.projectTitle
        );
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.CommunitySolarTestInfo.login.apiIsValidUserName(page.CommunitySolarTestInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.CommunitySolarTestInfo.login.logOut();
        // Login as user that created the Project type
        await page.CommunitySolarTestInfo.login.loginAs(page.CommunitySolarTestInfo.role);
        // API delete project
        await page.CommunitySolarTestInfo.login.apiDeleteProjectById(page.CommunitySolarTestInfo.projectId);
        await page.CommunitySolarTestInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("Community Solar only on Windows PC", async () => {});

  // Loop through each role and each test case
  for (const role of roles) {
    for (const [index, projectData] of communitySolarData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1;
      test(`Test Case #${testCaseNumber} as ${role}`, async ({ page }) => {
        page.CommunitySolarTestInfo.role = role;
        page.CommunitySolarTestInfo.projectData = projectData;
        await page.CommunitySolarTestInfo.login.loginAs(role);
        const projectPage = page.CommunitySolarTestInfo.poManager.getProjectPage(projectData.projectType);
        page.CommunitySolarTestInfo.projectId = await projectPage.createProject(projectData, role);
      });
    }
  }
});
