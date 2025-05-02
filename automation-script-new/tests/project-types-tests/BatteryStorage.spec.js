import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import batteryStorageData from "../../data/project-types/BatteryStorage.json";
import { ApiCore } from "../../api-tests/core/ApiCore";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven Battery Storage Tests", { tag: ["@batteryStorage", "@all_projects_parallel", "@core_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.BatteryStorageTestInfo = {};
    page.BatteryStorageTestInfo.poManager = new POManager(page);
    page.BatteryStorageTestInfo.login = page.BatteryStorageTestInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.BatteryStorageTestInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.BatteryStorageTestInfo.login.logOut();

      // When project created, the projectId will exist
      if (page.BatteryStorageTestInfo?.projectId) {
        // login as Corporate user
        await page.BatteryStorageTestInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.BatteryStorageTestInfo.login.apiIsValidProjectTitle(
          page.BatteryStorageTestInfo.projectId,
          page.BatteryStorageTestInfo.projectData.projectTitle
        );
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.BatteryStorageTestInfo.login.apiIsValidUserName(page.BatteryStorageTestInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.BatteryStorageTestInfo.login.logOut();
        // Login as user that created the Project type
        await page.BatteryStorageTestInfo.login.loginAs(page.BatteryStorageTestInfo.role);
        // API delete project
        await page.BatteryStorageTestInfo.login.apiDeleteProjectById(page.BatteryStorageTestInfo.projectId);
        await page.BatteryStorageTestInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("Battery Storage only on Windows PC", async () => {});

  // Loop through each role and each test case
  for (const role of roles) {
    for (const [index, projectData] of batteryStorageData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1; // Use testCaseId from JSON or fallback to index
      test(`Test Case #${testCaseNumber} as ${role}`, async ({ page }) => {
        page.BatteryStorageTestInfo.role = role;
        page.BatteryStorageTestInfo.projectData = projectData;
        await page.BatteryStorageTestInfo.login.loginAs(role); // Login as current role
        const projectPage = page.BatteryStorageTestInfo.poManager.getProjectPage(projectData.projectType);
        page.BatteryStorageTestInfo.projectId = await projectPage.createProject(projectData, role);
      });
    }
  }
});
