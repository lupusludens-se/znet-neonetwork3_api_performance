import { test, expect } from '@playwright/test';
const { POManager } = require('../pages/src/pageObjectManager');



test('Verfiying add New Forum test for Project type Aggregated PPA region Asia', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicAggtPPA()
    await addforum.selectLocationYES()
    await addforum.selectLocationASIA()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Battery Storage and region Africa', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicBattryStrge()
    await addforum.selectLocationYES()
    await addforum.selectLocationAFRICA()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Carbon Offset Purchasing and region Europe', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicCarbonOffset()
    await addforum.selectLocationYES()
    await addforum.selectLocationEurope()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Decarbonization and region Oceania ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicDecarbonization()
    await addforum.selectLocationYES()
    await addforum.selectLocationOceania()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type EAC Purchasing and region South America  ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicEACPurchasing()
    await addforum.selectLocationYES()
    await addforum.selectLocationSouthAmerica()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Efficiency Equipment Measures and region USA & Canada  ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicEfficiencyEquipmentMeasures()
    await addforum.selectLocationYES()
    await addforum.selectLocationUSAandCanada()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Efficiency Audit & Consulting and region USA & Canada  ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicEfficiencyAuditConsulting()
    await addforum.selectLocationYES()
    await addforum.selectLocationUSAandCanada()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Emerging Technology and region None ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicEmergingTechnology()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});


test('Verfiying add New Forum test for Project type Energy News and region None ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicEnergyNews()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for EVChargingFleetElectrification and region None ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicEVChargingFleetElectrification()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Fuel Cell and region USA & Canada  ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicFuelCell()
    await addforum.selectLocationYES()
    await addforum.selectLocationUSAandCanada()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type New Community Solar  and region Asia ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicNewCommunitySolar()
    await addforum.selectLocationYES()
    await addforum.selectLocationASIA()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Offsite PPAs  and region Africa ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicOffsitePPA()
    await addforum.selectLocationYES()
    await addforum.selectLocationAFRICA()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});


test('Verfiying add New Forum test for Project type Onsite Solar  and region Europe ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicOnsiteSolar()
    await addforum.selectLocationYES()
    await addforum.selectLocationEurope()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});
 

test('Verfiying add New Forum test for Project type ReNewable Retail Electricity and region Mexico ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicReNewableRetailElectricity()
    await addforum.selectLocationYES()
    await addforum.selectLocationMexico()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type ReNewable Responsible and region South America ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicResponsibleReNewables()
    await addforum.selectLocationYES()
    await addforum.selectLocationSouthAmerica()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});
 


test('Verfiying add New Forum test for Project type Utility Green Tariff and region Oceania ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicUtilityGreenTariff()
    await addforum.selectLocationYES()
    await addforum.selectLocationOceania()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Aggregated PPA, Battery Storage & carbon Offset and region Asia', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicAggtPPA()
    await addforum.selectForumTopicBattryStrge()
    await addforum.selectForumTopicCarbonOffset()
    await addforum.selectLocationYES()
    await addforum.selectLocationASIA()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Battery Storage, carbon Offset & Community Solar and  region Africa', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicBattryStrge()
    await addforum.selectForumTopicCarbonOffset()
    await addforum.selectForumTopicCommunitySolar()
    await addforum.selectLocationYES()
    await addforum.selectLocationAFRICA()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});
 


test('Verfiying add New Forum test for Project type Carbon Offset, Community Solar & Decorbonization and region Europe', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicCarbonOffset()
    await addforum.selectForumTopicCommunitySolar()
    await addforum.selectForumTopicDecarbonization()
    await addforum.selectLocationYES()
    await addforum.selectLocationEurope()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});
 

test('Verfiying add New Forum test for Project type Community Solar, Decorbonization & EACPurchasing and region Europe', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicCommunitySolar()
    await addforum.selectForumTopicDecarbonization()
    await addforum.selectForumTopicEACPurchasing()
    await addforum.selectLocationYES()
    await addforum.selectLocationEurope()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});



test('Verfiying add New Forum test for Project type Decorbonization, EACPurchasing & Efficiency Audit Consulting and region Mexico', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicDecarbonization()
    await addforum.selectForumTopicEACPurchasing()
    await addforum.selectForumTopicEfficiencyAuditConsulting()
    await addforum.selectLocationYES()
    await addforum.selectLocationMexico()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});


test('Verfiying add New Forum test for Project type EACPurchasing, Efficiency Audit Consulting &  Efficiency Equipment Measures and region USA and Canada', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicEACPurchasing()
    await addforum.selectForumTopicEfficiencyAuditConsulting()
    await addforum.selectForumTopicEfficiencyEquipmentMeasures()
    await addforum.selectLocationYES()
    await addforum.selectLocationUSAandCanada()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Efficiency Audit Consulting,  Efficiency Equipment Measures & EmergingTechnology and region USA and Canada', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicEfficiencyAuditConsulting()
    await addforum.selectForumTopicEfficiencyEquipmentMeasures()
    await addforum.selectForumTopicEmergingTechnology()
    await addforum.selectLocationYES()
    await addforum.selectLocationUSAandCanada()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Efficiency Equipment Measures, EmergingTechnology & Energy News and region South America ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicEfficiencyEquipmentMeasures()
    await addforum.selectForumTopicEmergingTechnology()
    await addforum.selectForumTopicEnergyNews()
    await addforum.selectLocationYES()
    await addforum.selectLocationSouthAmerica()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});


test('Verfiying add New Forum test for Project type EmergingTechnology, Energy News & EV ChargingFleet Electrification and region Oceania ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicEmergingTechnology()
    await addforum.selectForumTopicEnergyNews()
    await addforum.selectForumTopicEVChargingFleetElectrification()
    await addforum.selectLocationYES()
    await addforum.selectLocationOceania()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});


test('Verfiying add New Forum test for Project type Energy News, EV ChargingFleet Electrification & Feul Cell and region None ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicEnergyNews()
    await addforum.selectForumTopicEVChargingFleetElectrification()
    await addforum.selectForumTopicFuelCell()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type EV ChargingFleet Electrification, Feul Cell & New Community Solar and region None ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicEVChargingFleetElectrification()
    await addforum.selectForumTopicFuelCell()
    await addforum.selectForumTopicNewCommunitySolar()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Feul Cell, New Community Solar & Offsite PPA and region None ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicFuelCell()
    await addforum.selectForumTopicNewCommunitySolar()
    await addforum.selectForumTopicOffsitePPA()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type New Community Solar, Offsite PPA & Onsite Solar and region None ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicNewCommunitySolar()
    await addforum.selectForumTopicOffsitePPA()
    await addforum.selectForumTopicOnsiteSolar()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});


test('Verfiying add New Forum test for Project type Offsite PPA, Onsite Solar & ReNewable Retail Electricity and region None ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicOffsitePPA()
    await addforum.selectForumTopicOnsiteSolar()
    await addforum.selectForumTopicReNewableRetailElectricity()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Onsite Solar, ReNewable Retail Electricity & Responsible ReNewables and region None ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicAggtPPA()
    await addforum.selectForumTopicBattryStrge()
    await addforum.selectForumTopicCarbonOffset()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});

test('Verfiying add New Forum test for Project type Onsite Solar, ReNewable Retail Electricity & UtilityGreenTariff and region None ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicReNewableRetailElectricity()
    await addforum.selectForumTopicResponsibleReNewables()
    await addforum.selectForumTopicUtilityGreenTariff()
    await addforum.clickOnPostForum()
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
    await addforum.deletingForumfunc()

});


/** Test cases of 2025 */

test('Test case to verify the functionality of the private forum for specified project types: Aggregatted PPA, Battery Storage & Carbon offset and region None ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.selectForumTopicAggtPPA()
    await addforum.selectForumTopicBattryStrge()
    await addforum.selectForumTopicCarbonOffset()
    await addforum.privateForumAssigntoMemberfunc()
    await addforum.clickOnPostForum()
    await loginpage.logOut()
    await loginpage.tstCorporateUserLogin(); //test corporate login 
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
});



test('Test case to verify the functionality of the private forum for specified project types: Battery Storage, Carbon offset & Community Solar and region Europe ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.pintheForumfunc()
    await addforum.selectForumTopicBattryStrge()
    await addforum.selectForumTopicCarbonOffset()
    await addforum.selectForumTopicCommunitySolar()
    await addforum.selectLocationYES()
    await addforum.selectLocationEurope()
    await addforum.privateForumAssigntoMemberfunc()
    await addforum.clickOnPostForum()
    await loginpage.logOut()
    await loginpage.tstCorporateUserLogin(); //test corporate login 
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
});


test('Test case to verify the functionality of the private forum for specified project types: Carbon offset, Community Solar & Decorbonization and region Asia ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.pintheForumfunc()
    await addforum.selectForumTopicCarbonOffset()
    await addforum.selectForumTopicCommunitySolar()
    await addforum.selectForumTopicDecarbonization()
    await addforum.selectLocationYES()
    await addforum.selectLocationASIA()
    await addforum.privateForumAssigntoMemberfunc()
    await addforum.clickOnPostForum()
    await loginpage.logOut()
    await loginpage.tstCorporateUserLogin(); //test corporate login 
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
});

test('Test case to verify the functionality of the private forum for specified project types: Decorbonization, EAC purchasing & Efficiency Audit Counsulting and region Africa ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.pintheForumfunc()
    await addforum.selectForumTopicDecarbonization()
    await addforum.selectForumTopicEACPurchasing()
    await addforum.selectForumTopicEfficiencyAuditConsulting()
    await addforum.selectLocationYES()
    await addforum.selectLocationAFRICA()
    await addforum.privateForumAssigntoMemberfunc()
    await addforum.clickOnPostForum()
    await loginpage.logOut()
    await loginpage.tstCorporateUserLogin(); //test corporate login 
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
});

test.only('Test case to verify the functionality of the private forum for specified project types: Community Solar, Decorbonization & EAC purchasing and region Mexico ', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    await loginpage.tstAdminUserLogin(); //test admin login 
    const addforum = poManager.getAddForum()
    await addforum.clickOnForumModule()
    await addforum.clickOnStartDiscussionBtn()
    await addforum.enterForumNamefunc()
    await addforum.clickOnCreateNewDiscussionBtn()
    await addforum.enterDiscussionFunc()
    await addforum.pintheForumfunc()
    await addforum.selectForumTopicCommunitySolar()
    await addforum.selectForumTopicDecarbonization()
    await addforum.selectForumTopicEACPurchasing()
    await addforum.selectLocationYES()
    await addforum.selectLocationMexico()
    await addforum.privateForumAssigntoMemberfunc()
    await addforum.clickOnPostForum()
    await loginpage.logOut()
    await loginpage.tstCorporateUserLogin(); //test corporate login 
    await addforum.clickOnForumModule()
    await addforum.searchNewgeneratedForum()
});