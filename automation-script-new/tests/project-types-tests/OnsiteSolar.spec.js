import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import onsiteSolarData from "../../data/project-types/OnsiteSolar.json";
import { ApiCore } from "../../api-tests/core/ApiCore";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven Onsite Solar Tests", { tag: ["@onsiteSolar", "@all_projects_parallel", "@secondary_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.OnsiteSolarTestInfo = {};
    page.OnsiteSolarTestInfo.poManager = new POManager(page);
    page.OnsiteSolarTestInfo.login = page.OnsiteSolarTestInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.OnsiteSolarTestInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.OnsiteSolarTestInfo.login.logOut();

      // When project created, the projectId will exist
      if (page.OnsiteSolarTestInfo?.projectId) {
        // login as Corporate user
        await page.OnsiteSolarTestInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.OnsiteSolarTestInfo.login.apiIsValidProjectTitle(page.OnsiteSolarTestInfo.projectId, page.OnsiteSolarTestInfo.projectData.projectTitle);
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.OnsiteSolarTestInfo.login.apiIsValidUserName(page.OnsiteSolarTestInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.OnsiteSolarTestInfo.login.logOut();
        // Login as user that created the Project type
        await page.OnsiteSolarTestInfo.login.loginAs(page.OnsiteSolarTestInfo.role);
        // API delete project
        await page.OnsiteSolarTestInfo.login.apiDeleteProjectById(page.OnsiteSolarTestInfo.projectId);
        await page.OnsiteSolarTestInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("Onsite Solar only on Windows PC", async () => {});

  // Loop through each role and each test case
  for (const role of roles) {
    for (const [index, projectData] of onsiteSolarData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1; // Use testCaseId from JSON or fallback to index
      test(`Test Case #${testCaseNumber} as ${role}`, async ({ page }) => {
        page.OnsiteSolarTestInfo.role = role;
        page.OnsiteSolarTestInfo.projectData = projectData;
        await page.OnsiteSolarTestInfo.login.loginAs(role); // Login as current role
        const projectPage = page.OnsiteSolarTestInfo.poManager.getProjectPage(projectData.projectType);
        page.OnsiteSolarTestInfo.projectId = await projectPage.createProject(projectData, role);
      });
    }
  }
});
