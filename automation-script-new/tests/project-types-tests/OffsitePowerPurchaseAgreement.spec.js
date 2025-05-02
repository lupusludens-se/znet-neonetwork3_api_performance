import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import OffsitePowerPurchaseAgreementData from "../../data/project-types/OffsitePowerPurchaseAgreement.json";
import { ApiCore } from "../../api-tests/core/ApiCore";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven Offsite Power Purchase Agreement Tests", { tag: ["@offsitePPA", "@all_projects_parallel", "@core_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.OffsitePPATestInfo = {};
    page.OffsitePPATestInfo.poManager = new POManager(page);
    page.OffsitePPATestInfo.login = page.OffsitePPATestInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.OffsitePPATestInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.OffsitePPATestInfo.login.logOut();

      // When project created, the projectId will exist
      if (page.OffsitePPATestInfo?.projectId) {
        // login as Corporate user
        await page.OffsitePPATestInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.OffsitePPATestInfo.login.apiIsValidProjectTitle(page.OffsitePPATestInfo.projectId, page.OffsitePPATestInfo.projectData.projectTitle);
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.OffsitePPATestInfo.login.apiIsValidUserName(page.OffsitePPATestInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.OffsitePPATestInfo.login.logOut();
        // Login as user that created the Project type
        await page.OffsitePPATestInfo.login.loginAs(page.OffsitePPATestInfo.role);
        // API delete project
        await page.OffsitePPATestInfo.login.apiDeleteProjectById(page.OffsitePPATestInfo.projectId);
        await page.OffsitePPATestInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("Offset PPA only on Windows PC", async () => {});

  for (const role of roles) {
    for (const [index, projectData] of OffsitePowerPurchaseAgreementData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1; // Use testCaseId from JSON or fallback to index
      test(`TC-SET-3 => TC-${testCaseNumber} as ${role} user`, async ({ page }) => {
        page.OffsitePPATestInfo.role = role;
        page.OffsitePPATestInfo.projectData = projectData;
        await page.OffsitePPATestInfo.login.loginAs(role); // Login as current role
        const offsetPPA = page.OffsitePPATestInfo.poManager.getProjectPage(projectData.projectType);
        page.OffsitePPATestInfo.projectId = await offsetPPA.createProject(projectData, role);
      });
    }
  }
});
