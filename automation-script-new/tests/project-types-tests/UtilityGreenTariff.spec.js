import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import utilityGreenTariffData from "../../data/project-types/UtilityGreenTariff.json";
import { ApiCore } from "../../api-tests/core/ApiCore";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven Utility Green Tariff Tests", { tag: ["@utilityGreenTariff", "@all_projects_parallel", "@secondary_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.UtilityGreenTestInfo = {};
    page.UtilityGreenTestInfo.poManager = new POManager(page);
    page.UtilityGreenTestInfo.login = page.UtilityGreenTestInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.UtilityGreenTestInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.UtilityGreenTestInfo.login.logOut();

      // When project created, the projectId will exist
      if (page.UtilityGreenTestInfo?.projectId) {
        // login as Corporate user
        await page.UtilityGreenTestInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.UtilityGreenTestInfo.login.apiIsValidProjectTitle(page.UtilityGreenTestInfo.projectId, page.UtilityGreenTestInfo.projectData.projectTitle);
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.UtilityGreenTestInfo.login.apiIsValidUserName(page.UtilityGreenTestInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.UtilityGreenTestInfo.login.logOut();
        // Login as user that created the Project type
        await page.UtilityGreenTestInfo.login.loginAs(page.UtilityGreenTestInfo.role);
        // API delete project
        await page.UtilityGreenTestInfo.login.apiDeleteProjectById(page.UtilityGreenTestInfo.projectId);
        await page.UtilityGreenTestInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("Utility Green Tariff only on Windows PC", async () => {});

  // Loop through each role and each test case
  for (const role of roles) {
    for (const [index, projectData] of utilityGreenTariffData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1; // Use testCaseId from JSON or fallback to index
      test(`Test Case #${testCaseNumber} as ${role}`, async ({ page }) => {
        page.UtilityGreenTestInfo.role = role;
        page.UtilityGreenTestInfo.projectData = projectData;
        await page.UtilityGreenTestInfo.login.loginAs(role); // Login as current role
        const projectPage = page.UtilityGreenTestInfo.poManager.getProjectPage(projectData.projectType);
        page.UtilityGreenTestInfo.projectId = await projectPage.createProject(projectData, role);
      });
    }
  }
});
