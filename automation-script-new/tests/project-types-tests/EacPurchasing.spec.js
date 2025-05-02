import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import EacPurchasingData from "../../data/project-types/EacPurchasing.json";
import { ApiCore } from "../../api-tests/core/ApiCore.js";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven EACPurchasing Tests", { tag: ["@eacPurchasing", "@all_projects_parallel", "@small_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.EacPurchasingInfo = {};
    page.EacPurchasingInfo.poManager = new POManager(page);
    page.EacPurchasingInfo.login = page.EacPurchasingInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.EacPurchasingInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.EacPurchasingInfo.login.logOut();

      // When project created, the projectId will exist
      if (page.EacPurchasingInfo?.projectId) {
        // login as Corporate user
        await page.EacPurchasingInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.EacPurchasingInfo.login.apiIsValidProjectTitle(page.EacPurchasingInfo.projectId, page.EacPurchasingInfo.projectData.projectTitle);
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.EacPurchasingInfo.login.apiIsValidUserName(page.EacPurchasingInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.EacPurchasingInfo.login.logOut();
        // Login as user that created the Project type
        await page.EacPurchasingInfo.login.loginAs(page.EacPurchasingInfo.role);
        // API delete project
        await page.EacPurchasingInfo.login.apiDeleteProjectById(page.EacPurchasingInfo.projectId);
        await page.EacPurchasingInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("Efficiency EACPurchasing only on Windows PC", async () => {});

  for (const role of roles) {
    for (const [index, projectData] of EacPurchasingData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1;
      test(`Test Case #${testCaseNumber} as ${role}`, async ({ page }) => {
        page.EacPurchasingInfo.role = role;
        page.EacPurchasingInfo.projectData = projectData;
        await page.EacPurchasingInfo.login.loginAs(role); // Login as current role
        const projectPage = page.EacPurchasingInfo.poManager.getProjectPage(projectData.projectType);
        page.EacPurchasingInfo.projectId = await projectPage.createProject(projectData, role);
      });
    }
  }
});
