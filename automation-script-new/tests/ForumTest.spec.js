import { test, expect } from "@playwright/test";
import { POManager } from "../utils/POManager.js";

test.describe("Role verification Tests", () => {
  let poManager;
  let login;
  let addForum;

  test.beforeEach(async ({ page }) => {
    poManager = new POManager(page); // Dynamically initialize Page Objects
    login = poManager.getPage("login");
    addForum = poManager.getPage("addForum");
    await login.loginAs("admin");
    await addForum.clickOnForumModule();
    await addForum.clickOnStartDiscussionBtn();
    await addForum.enterForumName();
    await addForum.clickOnCreateNewDiscussionBtn();
    await addForum.enterDiscussion();
  });

  test.afterEach(async ({ page }) => {
    await login.logOut();
    test.slow(500);
    await page.close();
  });

  test("Verfiying add New Forum test for Project type Aggregated PPA region Asia", async ({ page }) => {
    await addForum.selectForumTopic("AggtPPA");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Asia");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verifying add New Forum test for Project type Battery Storage and region Africa", async ({ page }) => {
    await addForum.selectForumTopic("BattryStrge");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Africa");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Carbon Offset Purchasing and region Europe", async ({ page }) => {
    await addForum.selectForumTopic("CarbonOffset");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Europe");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Decarbonization and region Oceania ", async ({ page }) => {
    await addForum.selectForumTopic("Decarbonization");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Oceania");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type EAC Purchasing and region South America  ", async ({ page }) => {
    await addForum.selectForumTopic("EACPurchasing");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("SouthAmerica");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Efficiency Equipment Measures and region USA & Canada  ", async ({ page }) => {
    await addForum.selectForumTopic("EfficiencyEquipmentMeasures");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("USAandCanada");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Efficiency Audit & Consulting and region USA & Canada  ", async ({ page }) => {
    await addForum.selectForumTopic("EfficiencyAuditConsulting");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("USAandCanada");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Emerging Technology and region None ", async ({ page }) => {
    await addForum.selectForumTopic("EmergingTechnology");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Energy News and region None ", async ({ page }) => {
    await addForum.selectForumTopic("EnergyNews");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for EVChargingFleetElectrification and region None ", async ({ page }) => {
    await addForum.selectForumTopic("EVChargingFleetElectrification");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Fuel Cell and region USA & Canada  ", async ({ page }) => {
    await addForum.selectForumTopic("FuelCell");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("USAandCanada");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type New Community Solar  and region Asia ", async ({ page }) => {
    await addForum.selectForumTopic("NewCommunitySolar");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Asia");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Offsite PPAs  and region Africa ", async ({ page }) => {
    await addForum.selectForumTopic("OffsitePPA");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Africa");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Onsite Solar  and region Europe ", async ({ page }) => {
    await addForum.selectForumTopic("OnsiteSolar");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Europe");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type ReNewable Retail Electricity and region Mexico ", async ({ page }) => {
    await addForum.selectForumTopic("ReNewableRetailElectricity");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Mexico");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type ReNewable Responsible and region South America ", async ({ page }) => {
    await addForum.selectForumTopic("ResponsibleReNewables");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("SouthAmerica");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Utility Green Tariff and region Oceania ", async ({ page }) => {
    await addForum.selectForumTopic("UtilityGreenTariff");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Oceania");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Aggregated PPA, Battery Storage & carbon Offset and region Asia", async ({ page }) => {
    await addForum.selectForumTopic("AggtPPA");
    await addForum.selectForumTopic("BattryStrge");
    await addForum.selectForumTopic("CarbonOffset");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Asia");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Battery Storage, carbon Offset & Community Solar and  region Africa", async ({ page }) => {
    await addForum.selectForumTopic("BattryStrge");
    await addForum.selectForumTopic("CarbonOffset");
    await addForum.selectForumTopic("CommunitySolar");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Africa");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Carbon Offset, Community Solar & Decorbonization and region Europe", async ({ page }) => {
    await addForum.selectForumTopic("CarbonOffset");
    await addForum.selectForumTopic("CommunitySolar");
    await addForum.selectForumTopic("Decarbonization");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Europe");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Community Solar, Decorbonization & EACPurchasing and region Europe", async ({ page }) => {
    await addForum.selectForumTopic("CommunitySolar");
    await addForum.selectForumTopic("Decarbonization");
    await addForum.selectForumTopic("EACPurchasing");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Europe");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Decorbonization, EACPurchasing & Efficiency Audit Consulting and region Mexico", async ({ page }) => {
    await addForum.selectForumTopic("Decarbonization");
    await addForum.selectForumTopic("EACPurchasing");
    await addForum.selectForumTopic("EfficiencyAuditConsulting");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Mexico");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type EACPurchasing, Efficiency Audit Consulting &  Efficiency Equipment Measures and region USA and Canada", async ({
    page,
  }) => {
    await addForum.selectForumTopic("EACPurchasing");
    await addForum.selectForumTopic("EfficiencyAuditConsulting");
    await addForum.selectForumTopic("EfficiencyEquipmentMeasures");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("USAandCanada");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Efficiency Audit Consulting,  Efficiency Equipment Measures & EmergingTechnology and region USA and Canada", async ({
    page,
  }) => {
    await addForum.selectForumTopic("EfficiencyAuditConsulting");
    await addForum.selectForumTopic("EfficiencyEquipmentMeasures");
    await addForum.selectForumTopic("EmergingTechnology");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("USAandCanada");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Efficiency Equipment Measures, EmergingTechnology & Energy News and region South America ", async ({ page }) => {
    await addForum.selectForumTopic("EfficiencyEquipmentMeasures");
    await addForum.selectForumTopic("EmergingTechnology");
    await addForum.selectForumTopic("EnergyNews");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("SouthAmerica");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type EmergingTechnology, Energy News & EV ChargingFleet Electrification and region Oceania ", async ({ page }) => {
    await addForum.selectForumTopic("EmergingTechnology");
    await addForum.selectForumTopic("EnergyNews");
    await addForum.selectForumTopic("EVChargingFleetElectrification");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Oceania");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Energy News, EV ChargingFleet Electrification & Feul Cell and region None ", async ({ page }) => {
    await addForum.selectForumTopic("EnergyNews");
    await addForum.selectForumTopic("EVChargingFleetElectrification");
    await addForum.selectForumTopic("FuelCell");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type EV ChargingFleet Electrification, Feul Cell & New Community Solar and region None ", async ({ page }) => {
    await addForum.selectForumTopic("EVChargingFleetElectrification");
    await addForum.selectForumTopic("FuelCell");
    await addForum.selectForumTopic("NewCommunitySolar");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Feul Cell, New Community Solar & Offsite PPA and region None ", async ({ page }) => {
    await addForum.selectForumTopic("FuelCell");
    await addForum.selectForumTopic("NewCommunitySolar");
    await addForum.selectForumTopic("OffsitePPA");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type New Community Solar, Offsite PPA & Onsite Solar and region None ", async ({ page }) => {
    await addForum.selectForumTopic("NewCommunitySolar");
    await addForum.selectForumTopic("OffsitePPA");
    await addForum.selectForumTopic("OnsiteSolar");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Offsite PPA, Onsite Solar & ReNewable Retail Electricity and region None ", async ({ page }) => {
    await addForum.selectForumTopic("OffsitePPA");
    await addForum.selectForumTopic("OnsiteSolar");
    await addForum.selectForumTopic("ReNewableRetailElectricity");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Onsite Solar, ReNewable Retail Electricity & Responsible ReNewables and region None ", async ({ page }) => {
    await addForum.selectForumTopic("AggtPPA");
    await addForum.selectForumTopic("BattryStrge");
    await addForum.selectForumTopic("CarbonOffset");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  test("Verfiying add New Forum test for Project type Onsite Solar, ReNewable Retail Electricity & UtilityGreenTariff and region None ", async ({ page }) => {
    await addForum.selectForumTopic("ReNewableRetailElectricity");
    await addForum.selectForumTopic("ResponsibleReNewables");
    await addForum.selectForumTopic("UtilityGreenTariff");
    await addForum.clickOnPostForum();
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
    await addForum.deleteForum();
  });

  /** Test cases of 2025 */

  test("Test case to verify the functionality of the private forum for specified project types: Aggregated PPA, Battery Storage & Carbon offset and region None ", async ({
    page,
  }) => {
    await addForum.selectForumTopic("AggtPPA");
    await addForum.selectForumTopic("BattryStrge");
    await addForum.selectForumTopic("CarbonOffset");
    await addForum.privateForumAssigntoMember();
    await addForum.clickOnPostForum();
    await login.logOut();
    await login.loginAs("corporateUser");
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
  });

  test("Test case to verify the functionality of the private forum for specified project types: Battery Storage, Carbon offset & Community Solar and region Europe ", async ({
    page,
  }) => {
    await addForum.pintheForum();
    await addForum.selectForumTopic("BattryStrge");
    await addForum.selectForumTopic("CarbonOffset");
    await addForum.selectForumTopic("CommunitySolar");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Europe");
    await addForum.privateForumAssigntoMember();
    await addForum.clickOnPostForum();
    await login.logOut();
    await login.loginAs("corporateUser");
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
  });

  test("Test case to verify the functionality of the private forum for specified project types: Carbon offset, Community Solar & Decorbonization and region Asia ", async ({
    page,
  }) => {
    test.slow();
    await addForum.pintheForum();
    await addForum.selectForumTopic("CarbonOffset");
    await addForum.selectForumTopic("CommunitySolar");
    await addForum.selectForumTopic("Decarbonization");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Asia");
    await addForum.privateForumAssigntoMember();
    await addForum.clickOnPostForum();
    await login.logOut();
    await login.loginAs("corporateUser");
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
  });

  test("Test case to verify the functionality of the private forum for specified project types: Decorbonization, EAC purchasing & Efficiency Audit Counsulting and region Africa ", async ({
    page,
  }) => {
    test.slow();
    await addForum.pintheForum();
    await addForum.selectForumTopic("Decarbonization");
    await addForum.selectForumTopic("EACPurchasing");
    await addForum.selectForumTopic("EfficiencyAuditConsulting");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Africa");
    await addForum.privateForumAssigntoMember();
    await addForum.clickOnPostForum();
    await login.logOut();
    await login.loginAs("corporateUser");
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
  });

  test("Test case to verify the functionality of the private forum for specified project types: Community Solar, Decorbonization & EAC purchasing and region Mexico ", async ({
    page,
  }) => {
    test.slow();
    await addForum.pintheForum();
    await addForum.selectForumTopic("CommunitySolar");
    await addForum.selectForumTopic("Decarbonization");
    await addForum.selectForumTopic("EACPurchasing");
    await addForum.selectLocation("Yes");
    await addForum.selectLocation("Mexico");
    await addForum.privateForumAssigntoMember();
    await addForum.clickOnPostForum();
    await login.logOut();
    await login.loginAs("corporateUser");
    await addForum.clickOnForumModule();
    await addForum.searchNewgeneratedForum();
  });
});
