import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import efficiencyEquipmentMeasureData from "../../data/project-types/EfficiencyEquipmentMeasures.json";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven Efficiency Equipment Measures Tests", { tag: ["@efficiencyEquipmentMeasures", "@all_projects_parallel", "@small_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.EfficiencyEquipmentMeasuresInfo = {};
    page.EfficiencyEquipmentMeasuresInfo.poManager = new POManager(page);
    page.EfficiencyEquipmentMeasuresInfo.login = page.EfficiencyEquipmentMeasuresInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.EfficiencyEquipmentMeasuresInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.EfficiencyEquipmentMeasuresInfo.login.logOut();
  
      // When project created, the projectId will exist
      if (page.EfficiencyEquipmentMeasuresInfo?.projectId) {
        // login as Corporate user
        await page.EfficiencyEquipmentMeasuresInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.EfficiencyEquipmentMeasuresInfo.login.apiIsValidProjectTitle(
          page.EfficiencyEquipmentMeasuresInfo.projectId,
          page.EfficiencyEquipmentMeasuresInfo.projectData.projectTitle
        );
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.EfficiencyEquipmentMeasuresInfo.login.apiIsValidUserName(page.EfficiencyEquipmentMeasuresInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.EfficiencyEquipmentMeasuresInfo.login.logOut();
        // Login as user that created the Project type
        await page.EfficiencyEquipmentMeasuresInfo.login.loginAs(page.EfficiencyEquipmentMeasuresInfo.role);
        // API delete project
        await page.EfficiencyEquipmentMeasuresInfo.login.apiDeleteProjectById(page.EfficiencyEquipmentMeasuresInfo.projectId);
        await page.EfficiencyEquipmentMeasuresInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("Efficiency Equipment Measures only on Windows PC", async () => {});

  for (const role of roles) {
    for (const [index, projectData] of efficiencyEquipmentMeasureData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1;
      test(`Test Case #${testCaseNumber} as ${role}`, async ({ page }) => {
        page.EfficiencyEquipmentMeasuresInfo.role = role;
        page.EfficiencyEquipmentMeasuresInfo.projectData = projectData;
        await page.EfficiencyEquipmentMeasuresInfo.login.loginAs(role);
        const projectPage = page.EfficiencyEquipmentMeasuresInfo.poManager.getProjectPage(projectData.projectType);
        page.EfficiencyEquipmentMeasuresInfo.projectId = await projectPage.createProject(projectData, role);
      });
    }
  }
});
