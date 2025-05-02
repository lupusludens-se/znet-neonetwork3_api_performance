import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import EmergingTechnologiesData from "../../data/project-types/EmergingTechnologies.json";
import { ApiCore } from "../../api-tests/core/ApiCore";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven Emerging Technologies Tests", { tag: ["@emergingTechnologies", "@all_projects_parallel", "@small_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.EmergingTechnologiesTestInfo = {};
    page.EmergingTechnologiesTestInfo.poManager = new POManager(page);
    page.EmergingTechnologiesTestInfo.login = page.EmergingTechnologiesTestInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.EmergingTechnologiesTestInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.EmergingTechnologiesTestInfo.login.logOut();

      // When project created, the projectId will exist
      if (page.EmergingTechnologiesTestInfo?.projectId) {
        // login as Corporate user
        await page.EmergingTechnologiesTestInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.EmergingTechnologiesTestInfo.login.apiIsValidProjectTitle(
          page.EmergingTechnologiesTestInfo.projectId,
          page.EmergingTechnologiesTestInfo.projectData.projectTitle
        );
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.EmergingTechnologiesTestInfo.login.apiIsValidUserName(page.EmergingTechnologiesTestInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.EmergingTechnologiesTestInfo.login.logOut();
        // Login as user that created the Project type
        await page.EmergingTechnologiesTestInfo.login.loginAs(page.EmergingTechnologiesTestInfo.role);
        // API delete project
        await page.EmergingTechnologiesTestInfo.login.apiDeleteProjectById(page.EmergingTechnologiesTestInfo.projectId);
        await page.EmergingTechnologiesTestInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("Emerging Technologies only on Windows PC", async () => {});

  for (const role of roles) {
    for (const [index, projectData] of EmergingTechnologiesData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1;
      test(`Test Case #${testCaseNumber} as ${role}`, async ({ page }) => {
        page.EmergingTechnologiesTestInfo.role = role;
        page.EmergingTechnologiesTestInfo.projectData = projectData;
        await page.EmergingTechnologiesTestInfo.login.loginAs(role);
        const projectPage = page.EmergingTechnologiesTestInfo.poManager.getProjectPage(projectData.projectType);
        page.EmergingTechnologiesTestInfo.projectId = await projectPage.createProject(projectData, role);
      });
    }
  }
});
