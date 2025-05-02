import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import efficiencyAuditsConsultingData from "../../data/project-types/EfficiencyAuditsConsulting.json";
import { ApiCore } from "../../api-tests/core/ApiCore";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven Efficiency Audits & Consulting Tests", { tag: ["@efficiencyAuditsConsulting", "@all_projects_parallel", "@small_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.EfficiencyAuditsConsultingTestInfo = {};
    page.EfficiencyAuditsConsultingTestInfo.poManager = new POManager(page);
    page.EfficiencyAuditsConsultingTestInfo.login = page.EfficiencyAuditsConsultingTestInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.EfficiencyAuditsConsultingTestInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.EfficiencyAuditsConsultingTestInfo.login.logOut();

      // When project created, the projectId will exist
      if (page.EfficiencyAuditsConsultingTestInfo?.projectId) {
        // login as Corporate user
        await page.EfficiencyAuditsConsultingTestInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.EfficiencyAuditsConsultingTestInfo.login.apiIsValidProjectTitle(
          page.EfficiencyAuditsConsultingTestInfo.projectId,
          page.EfficiencyAuditsConsultingTestInfo.projectData.projectTitle
        );
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.EfficiencyAuditsConsultingTestInfo.login.apiIsValidUserName(page.EfficiencyAuditsConsultingTestInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.EfficiencyAuditsConsultingTestInfo.login.logOut();
        // Login as user that created the Project type
        await page.EfficiencyAuditsConsultingTestInfo.login.loginAs(page.EfficiencyAuditsConsultingTestInfo.role);
        // API delete project
        await page.EfficiencyAuditsConsultingTestInfo.login.apiDeleteProjectById(page.EfficiencyAuditsConsultingTestInfo.projectId);
        await page.EfficiencyAuditsConsultingTestInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("Efficiency Audits & Consulting only on Windows PC", async () => {});

  for (const role of roles) {
    for (const [index, projectData] of efficiencyAuditsConsultingData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1;
      test(`Test Case #${testCaseNumber} as ${role}`, async ({ page }) => {
        page.EfficiencyAuditsConsultingTestInfo.role = role;
        page.EfficiencyAuditsConsultingTestInfo.projectData = projectData;
        await page.EfficiencyAuditsConsultingTestInfo.login.loginAs(role);
        const projectPage = page.EfficiencyAuditsConsultingTestInfo.poManager.getProjectPage(projectData.projectType);
        page.EfficiencyAuditsConsultingTestInfo.projectId = await projectPage.createProject(projectData, role);
      });
    }
  }
});
