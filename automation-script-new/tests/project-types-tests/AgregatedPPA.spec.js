import { test, expect } from "@playwright/test";
import { POManager } from "../../utils/POManager.js";
import aggregatedPPAData from "../../data/project-types/AggregatedPPA.json";

// Define roles to run all test cases
const roles = ["admin", "spAdmin", "spUser"];

test.describe("Data-driven Aggregated PPAs Tests", { tag: ["@aggregatedPPA", "@all_projects_parallel", "@core_projects"] }, () => {
  test.beforeEach(async ({ page }) => {
    page.AggregatedPPATestInfo = {};
    page.AggregatedPPATestInfo.poManager = new POManager(page);
    page.AggregatedPPATestInfo.login = page.AggregatedPPATestInfo.poManager.getPage("login");
  });

  test.afterEach(async ({ page }) => {
    // logout when login session exist
    const loginCookies = await page.context().cookies();
    if (loginCookies?.length > 0) {
      // API Get user credentials
      // const userResponse = await page.AggregatedPPATestInfo.login.request("get", "/api/users/current");
      // expect(userResponse.status() === 200, `API Get current user status: ${userResponse.status()}`).toBeTruthy();
      // const userProfile = await userResponse.json();
      // logout of current user
      await page.AggregatedPPATestInfo.login.logOut();

      // When project created, the projectId will exist
      if (page.AggregatedPPATestInfo?.projectId) {
        // login as Corporate user
        await page.AggregatedPPATestInfo.login.loginAs("corporateUser");
        // API GET project by id, and verify that project title matches the API response JSON payload
        await page.AggregatedPPATestInfo.login.apiIsValidProjectTitle(page.AggregatedPPATestInfo.projectId, page.AggregatedPPATestInfo.projectData.projectTitle);
        // API GET project by id, and verify that the username matches the author of the created project
        // await page.AggregatedPPATestInfo.login.apiIsValidUserName(page.AggregatedPPATestInfo.projectId, userProfile.username);
        // logout Corporate user
        await page.AggregatedPPATestInfo.login.logOut();
        // Login as user that created the Project type
        await page.AggregatedPPATestInfo.login.loginAs(page.AggregatedPPATestInfo.role);
        // API delete project
        await page.AggregatedPPATestInfo.login.apiDeleteProjectById(page.AggregatedPPATestInfo.projectId);
        await page.AggregatedPPATestInfo.login.logOut();
      }
    }
    await page.close();
  });

  // Problem: Windows PC laptop's CPU struggles to run parallel tests.
  //      When a PC laptop fan runs at a high-rate, it can cause a 90% test failure rate.
  // Workaround: run a single empty test to interrupt a set of parallel workers.
  //      This causes the workers to still run in parallel, but out-of-sequence.
  test.beforeAll("Aggregated PPAs only on Windows PC", async () => {});

  for (const role of roles) {
    for (const [index, projectData] of aggregatedPPAData.testCases.entries()) {
      const testCaseNumber = projectData.testCaseId || index + 1; // Use testCaseId from JSON or fallback to index
      test(`TC-SET-3 => TC-${testCaseNumber} as ${role} user`, async ({ page }) => {
        page.AggregatedPPATestInfo.role = role;
        page.AggregatedPPATestInfo.projectData = projectData;
        await page.AggregatedPPATestInfo.login.loginAs(role); // Login as current role
        const aggregatedPPAPage = page.AggregatedPPATestInfo.poManager.getProjectPage(projectData.projectType);
        page.AggregatedPPATestInfo.projectId = await aggregatedPPAPage.createProject(projectData, role);
      });
    }
  }
});

/** All properties as referenced
 * 
 *     {
      "testCaseId": 1000,
      "projectType": "Aggregated PPAs",

      "technology": ["Groundmount Solar", "Onshore Wind"],

      "regions": ["Europe (Ireland)", "Asia(Indonesia)"],
      "regionsPageNavBtn": "Next",

      "mapLocation": { "searchText": "02115", "dropdownText": "Boston" },
      "isoRto": "",
      "productType": "energy only",
      "calendarDate": "select any date",
      "valueToOfftaker": ["select all the checkboxes"],
      "ppaTermLength": "enter numerical value",
      "totalProjectNameplateCapacity": "enter numerical value 2147483647",
      "totalProjectExpectedAnnualProdP50": "enter numerical value 2147483647",
      "minOfftakeVolumeRequired": "enter numerical value 2147483647",
      "notesForPotentialOfftakers": "enter string value",

      "settlementType": "Hub",
      "settlementHub": "Other",
      "currencyForAllPriceEntries": "select USD from the dropdown",
      "contractPrice": "enter numerical value 2147483647",
      "floatIndex": "enter alphanumerical value",
      "floatFloor": "enter alphanumerical value",
      "floatCap": "enter alphanumerical value",
      "pricingStructure": "Plain CFD",
      "upsidePercentageToDeveloper": "enter numeric value",
      "upsidePercentageToOfftaker": "enter numeric value",
      "discountAmount": "enter numeric value",
      "eacType": "REC",
      "customEacType": "enter string value",
      "eacValue": "enter numeric value",
      "settlementPriceInterval": "Other",
      "customSettlementPriceInterval": "enter numeric value",
      "settlementCalculationInterval": "Hourly",
      "projectMWCurrentlyAvailable": "enter a numeric value 2147483647",
      "notesForSEOpsTeam": "enter a alphanumeric value",


      "projectTitle": "enter a alpha numeric value",
      "subTitle": "enter a alphanumeric value",
      "describeOpportunity": "enter a alpha numeric value",
      "aboutTheProvider": "should be auto populated from the company description page",
      "projectDescriptionPageNavBtn": "Publish"
    },
 */
