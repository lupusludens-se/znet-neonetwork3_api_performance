import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import carbonOffsetPurchaseData from "../../data/project-types/CarbonOffsetPurchasing.json";
import { ApiCore } from "../../api-tests/core/ApiCore";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven Carbon Offset Purchasing Tests", { tag: ["@carbonOffset", "@all_projects_parallel", "@core_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.CarbonOffsetTestInfo = {};
    page.CarbonOffsetTestInfo.poManager = new POManager(page);
    page.CarbonOffsetTestInfo.login = page.CarbonOffsetTestInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.CarbonOffsetTestInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.CarbonOffsetTestInfo.login.logOut();

      // When project created, the projectId will exist
      if (page.CarbonOffsetTestInfo?.projectId) {
        // login as Corporate user
        await page.CarbonOffsetTestInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.CarbonOffsetTestInfo.login.apiIsValidProjectTitle(
          page.CarbonOffsetTestInfo.projectId,
          page.CarbonOffsetTestInfo.projectData.projectTitle
        );
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.CarbonOffsetTestInfo.login.apiIsValidUserName(page.CarbonOffsetTestInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.CarbonOffsetTestInfo.login.logOut();
        // Login as user that created the Project type
        await page.CarbonOffsetTestInfo.login.loginAs(page.CarbonOffsetTestInfo.role);
        // API delete project
        await page.CarbonOffsetTestInfo.login.apiDeleteProjectById(page.CarbonOffsetTestInfo.projectId);
        await page.CarbonOffsetTestInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("Carbon Offset Purchasing only on Windows PC", async () => {});

  // Loop through each role and each test case
  for (const role of roles) {
    for (const [index, projectData] of carbonOffsetPurchaseData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1; // Use testCaseId from JSON or fallback to index
      test(`Test Case #${testCaseNumber} as ${role}`, async ({ page }) => {
        page.CarbonOffsetTestInfo.role = role;
        page.CarbonOffsetTestInfo.projectData = projectData;
        await page.CarbonOffsetTestInfo.login.loginAs(role); // Login as current role
        const projectPage = page.CarbonOffsetTestInfo.poManager.getProjectPage(projectData.projectType);
        page.CarbonOffsetTestInfo.projectId = await projectPage.createProject(projectData, role);
      });
    }
  }
});
