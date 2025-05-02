import { test, expect } from "@playwright/test";
import { POManager } from "../utils/POManager.js";
import resources from "../../automation-script-new/utils/CommonTestResources.js";

test.describe("Role verification Tests", () => {
  let poManager;
  let login;
  let admin;
  let createCompany;

  test.beforeEach(async ({ page }) => {
    poManager = new POManager(page); // Dynamically initialize Page Objects
    login = poManager.getPage("login");
    admin = poManager.getPage("admin");
    createCompany = poManager.getPage("createCompany");
    // Common before each test case
    await login.loginAs("admin");
    await admin.clickOnAdmin();
    await createCompany.clickOnAddCompany();
    await createCompany.enterCorporateName();
    await createCompany.enterValidURL();
  });

  test.afterEach(async ({ page }) => {
    // Common test after each test case
    await createCompany.addCompanyText();
    await createCompany.enterLinkDetails();
    await createCompany.selectCompanyType("corporate");
    await createCompany.clickOnAddCompany();
    await createCompany.searchNewgenerate();
    // await expect(admin.threeUsersDot).toBeEnabled(); // No such element on DOM dt 12 mar 25
    await login.logOut();
    test.slow(500);
    await page.close();
  });

  test("Test to add new Corporate Company and verify the same by Admin : Industry type - Consumer Goods and Country - United States ", async ({ page }) => {
    /*Test case steps:1. Login as Admin > Admin Management > Add company > fill company Name > Country: United States > Company type: Consumer Goods >select role : corporation */
    await createCompany.selectCountry(resources.COUNTRY.USA);
    await createCompany.selectIndustryType("consumerGoods");
    await createCompany.selectMDMOption("org");
  });

  test("Test to add new Corporate Company and verify the same by Admin Industry type - Extractives and Minerals and Country - India ", async ({ page }) => {
    /*Test case steps:1. Login as Admin > Admin Management > Add company > fill company Name > Country:  India > Company type: Extractives and Minerals >select role : corporation */
    await createCompany.selectCountry(resources.COUNTRY.INDIA);
    await createCompany.selectIndustryType("extraMinerals");
    await createCompany.selectMDMOption("mdm");
  });

  test("Test to add new Corporate Company and verify the same by Admin Industry type - Financials and Country - France ", async ({ page }) => {
    /*Test case steps:1. Login as Admin > Admin Management > Add company > fill company Name > Country:  France > Company type: Financials >select role : corporation */
    await createCompany.selectCountry(resources.COUNTRY.FRANCE);
    await createCompany.selectIndustryType("financials");
    await createCompany.selectMDMOption("mdm");
  });

  test("Test to add new Corporate Company and verify the same by Admin Industry type - Food and Beverage - France ", async ({ page }) => {
    /*Test case steps:1. Login as Admin > Admin Managment > Add company > fill company Name > Country:  France > Company type: Food and Beverage  >select role : corporation */
    await createCompany.selectCountry(resources.COUNTRY.FRANCE);
    await createCompany.selectIndustryType("foodBeverage");
    await createCompany.selectMDMOption("mdm");
  });

  test("Test to add new Corporate Company and verify the same by Admin Industry type - Government and municipality, Country - France ", async ({ page }) => {
    /*Test case steps:1. Login as Admin > Admin Managment > Add company > fill company Name > Country:  France > Company type: Government and municipality >select role : corporation */
    await createCompany.selectCountry(resources.COUNTRY.FRANCE);
    await createCompany.selectIndustryType("govt");
    await createCompany.selectMDMOption("mdm");
  });

  test("Test to add new Corporate Company and verify the same by Admin Industry type - Health care, Country - France ", async ({ page }) => {
    /*Test case steps:1. Login as Admin > Admin Managment > Add company > fill company Name > Country:  France > Company type: Health care  >select role : corporation */
    await createCompany.selectCountry(resources.COUNTRY.FRANCE);
    await createCompany.selectIndustryType("healthCare");
    await createCompany.selectMDMOption("mdm");
  });

  test("Test to add new Corporate Company and verify the same by Admin Industry type - Infrastructure, Country - France ", async ({ page }) => {
    /*Test case steps:1. Login as Admin > Admin Managment > Add company > fill company Name > Country:  France > Company type: Infrastructure  >select role : corporation */
    await createCompany.selectCountry(resources.COUNTRY.FRANCE);
    await createCompany.selectIndustryType("infrastructure");
    await createCompany.selectMDMOption("mdm");
  });

  test("Test to add new Corporate Company and verify the same by Admin Industry type - Renewable Resources and Alternative Energy, Country - France ", async ({
    page,
  }) => {
    /*Test case steps:1. Login as Admin > Admin Managment > Add company > fill company Name > Country:  France > Company type: Renewable Resources and Alternative Energy  >select role : corporation */
    await createCompany.selectCountry(resources.COUNTRY.FRANCE);
    await createCompany.selectIndustryType("renewable");
    await createCompany.selectMDMOption("mdm");
  });

  test("Test to add new Corporate Company and verify the same by Admin Industry type - Resource transformation - France ", async ({ page }) => {
    /*Test case steps:1. Login as Admin > Admin Managment > Add company > fill company Name > Country:  France > Company type: Resource transformation  >select role : corporation */
    await createCompany.selectCountry(resources.COUNTRY.FRANCE);
    await createCompany.selectIndustryType("resource");
    await createCompany.selectMDMOption("mdm");
  });

  test("Test to add new Corporate Company and verify the same by Admin Industry type - Services - France ", async ({ page }) => {
    /*Test case steps:1. Login as Admin > Admin Managment > Add company > fill company Name > Country:  France > Company type: Services  >select role : corporation */
    await createCompany.selectCountry(resources.COUNTRY.FRANCE);
    await createCompany.selectIndustryType("service");
    await createCompany.selectMDMOption("mdm");
  });

  test("Test to add new Corporate Company and verify the same by Admin Industry type - Technology and Communications - France ", async ({ page }) => {
    /*Test case steps:1. Login as Admin > Admin Managment > Add company > fill company Name > Country:  France > Company type: Technology and Communications  >select role : corporation */
    await createCompany.selectCountry(resources.COUNTRY.FRANCE);
    await createCompany.selectIndustryType("technology");
    await createCompany.selectMDMOption("mdm");
  });

  test("Test to add new Corporate Company and verify the same by Admin Industry type - Transportation - France ", async ({ page }) => {
    /*Test case steps:1. Login as Admin > Admin Management > Add company > fill company Name > Country:  France > Company type: Transportation  >select role : corporation */
    await createCompany.selectCountry(resources.COUNTRY.FRANCE);
    await createCompany.selectIndustryType("transport");
    await createCompany.selectMDMOption("mdm");
  });
});
