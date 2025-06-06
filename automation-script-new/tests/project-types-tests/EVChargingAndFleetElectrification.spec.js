import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import evChargingAndFleetElectrificationData from "../../data/project-types/EVChargingAndFleetElectrification.json";
import { ApiCore } from "../../api-tests/core/ApiCore";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven EV Charging and Fleet Electrification Tests", { tag: ["@evChargingAndFleetElectrification", "@all_projects_parallel", "@small_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.EvChargingAndFleetElectrificationTestInfo = {};
    page.EvChargingAndFleetElectrificationTestInfo.poManager = new POManager(page);
    page.EvChargingAndFleetElectrificationTestInfo.login = page.EvChargingAndFleetElectrificationTestInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.EvChargingAndFleetElectrificationTestInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.EvChargingAndFleetElectrificationTestInfo.login.logOut();

      // When project created, the projectId will exist
      if (page.EvChargingAndFleetElectrificationTestInfo?.projectId) {
        // login as Corporate user
        await page.EvChargingAndFleetElectrificationTestInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.EvChargingAndFleetElectrificationTestInfo.login.apiIsValidProjectTitle(
          page.EvChargingAndFleetElectrificationTestInfo.projectId,
          page.EvChargingAndFleetElectrificationTestInfo.projectData.projectTitle
        );
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.EvChargingAndFleetElectrificationTestInfo.login.apiIsValidUserName(page.EvChargingAndFleetElectrificationTestInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.EvChargingAndFleetElectrificationTestInfo.login.logOut();
        // Login as user that created the Project type
        await page.EvChargingAndFleetElectrificationTestInfo.login.loginAs(page.EvChargingAndFleetElectrificationTestInfo.role);
        // API delete project
        await page.EvChargingAndFleetElectrificationTestInfo.login.apiDeleteProjectById(page.EvChargingAndFleetElectrificationTestInfo.projectId);
        await page.EvChargingAndFleetElectrificationTestInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("EV Charging and Fleet Electrification only on Windows PC", async () => {});

  // Loop through each role and each test case
  for (const role of roles) {
    for (const [index, projectData] of evChargingAndFleetElectrificationData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1; // Use testCaseId from JSON or fallback to index
      test(`Test Case #${testCaseNumber} as ${role}`, async ({ page }) => {
        page.EvChargingAndFleetElectrificationTestInfo.role = role;
        page.EvChargingAndFleetElectrificationTestInfo.projectData = projectData;
        await page.EvChargingAndFleetElectrificationTestInfo.login.loginAs(role); // Login as current role
        const projectPage = page.EvChargingAndFleetElectrificationTestInfo.poManager.getProjectPage(projectData.projectType);
        page.EvChargingAndFleetElectrificationTestInfo.projectId = await projectPage.createProject(projectData, role);
      });
    }
  }
});
