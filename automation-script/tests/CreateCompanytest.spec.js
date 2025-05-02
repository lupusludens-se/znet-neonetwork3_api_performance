import { test, expect } from '@playwright/test';
const { POManager } = require('../pages/src/pageObjectManager');



test('Test to add new Corporate Company and verify the same by Admin : Industry type - Consumer Goods and Country - United States ', async ({ page }) => {
  /*Test case stesps:1. Login as Admin > Admin Manaagment > Add company > fill company Name > Country: United States > Company type  : Consumer Goods >select role : corporation */

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    const adminPage = poManager.getAdminPage()
    const creatComp = poManager.getCreateCompanyPage()
    await loginpage.tstAdminUserLogin(); //test admin login 
    await adminPage.clickOnAdmin()
    await creatComp.clickOnAddCompany()
    await creatComp.enterCorporateNamefunc()
    await creatComp.enterValidURL()
    await creatComp.selectUSCountry()
    await creatComp.consumerGoodsIndustryType()
    await creatComp.selectORGmdm()
    await creatComp.addCompanyText()
    await creatComp.enterLinkDetails()
    await creatComp.selectCorporateCompanyType()
    await creatComp.clickOnAddCompany()
    await creatComp.searchNewgeneratefunc()
    await expect(adminPage.threeUsersDot).toBeEnabled()
    console.log('Corporate company added with  Industry type - Consumer Goods and Country - United States')

    test.slow(500)
        

})


test('Test to add new Corporate Company and verify the same by Admin Industry type - Extractives and Minerals and Country - India ', async ({ page }) => {
   /*Test case stesps:1. Login as Admin > Admin Manaagment > Add company > fill company Name > Country:  India > Company type  : Extractives and Minerals >select role : corporation */

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    const adminPage = poManager.getAdminPage()
    const creatComp = poManager.getCreateCompanyPage()
    await loginpage.tstAdminUserLogin(); //test admin login 
    await adminPage.clickOnAdmin()
    await creatComp.clickOnAddCompany()
    await creatComp.enterCorporateNamefunc()
    await creatComp.enterValidURL()
    await creatComp.selectINDCountry()
    await creatComp.extraMineralsIndustryType()
    await creatComp.selectMDMmdm()
    await creatComp.addCompanyText()
    await creatComp.enterLinkDetails()
    await creatComp.selectCorporateCompanyType()
    await creatComp.clickOnAddCompany()
    await creatComp.searchNewgeneratefunc()
    await expect(adminPage.threeUsersDot).toBeEnabled()
    console.log('Corporate company added with  Industry type - Extractives and Minerals and Country - India')
    test.slow(500)
        

})

test('Test to add new Corporate Company and verify the same by Admin  Industry type - Financials and Country - France ', async ({ page }) => {
  /*Test case stesps:1. Login as Admin > Admin Manaagment > Add company > fill company Name > Country:  India > Company type  : Extractives and Minerals >select role : corporation */

   const poManager = new POManager(page)
   const loginpage = poManager.getLoginPage() // login page object 
   const adminPage = poManager.getAdminPage()
   const creatComp = poManager.getCreateCompanyPage()
   await loginpage.tstAdminUserLogin(); //test admin login 
   await adminPage.clickOnAdmin()
   await creatComp.clickOnAddCompany()
   await creatComp.enterCorporateNamefunc()
   await creatComp.enterValidURL()
   await creatComp.selectFRANCECountry()
   await creatComp.financialsIndustryType()
   await creatComp.selectMDMmdm()
   await creatComp.addCompanyText()
   await creatComp.enterLinkDetails()
   await creatComp.selectCorporateCompanyType()
   await creatComp.clickOnAddCompany()
   await creatComp.searchNewgeneratefunc()
   await expect(adminPage.threeUsersDot).toBeEnabled()
   console.log('Corporate company added with Industry type - Financials and Country - France')
   test.slow(500)
       

})


test('Test to add new Corporate Company and verify the same by Admin  Industry type - Food and Beverage - France ', async ({ page }) => {
  /*Test case stesps:1. Login as Admin > Admin Manaagment > Add company > fill company Name > Country:  India > Company type  : Food and Beverage  >select role : corporation */

   const poManager = new POManager(page)
   const loginpage = poManager.getLoginPage() // login page object 
   const adminPage = poManager.getAdminPage()
   const creatComp = poManager.getCreateCompanyPage()
   await loginpage.tstAdminUserLogin(); //test admin login 
   await adminPage.clickOnAdmin()
   await creatComp.clickOnAddCompany()
   await creatComp.enterCorporateNamefunc()
   await creatComp.enterValidURL()
   await creatComp.selectFRANCECountry()
   await creatComp.foodBeverageIndustryType()
   await creatComp.selectMDMmdm()
   await creatComp.addCompanyText()
   await creatComp.enterLinkDetails()
   await creatComp.selectCorporateCompanyType()
   await creatComp.clickOnAddCompany()
   await creatComp.searchNewgeneratefunc()
   await expect(adminPage.threeUsersDot).toBeEnabled()
   console.log('Corporate company added with Industry type - Food and Beverage and Country - France')
   test.slow(500)
       

})

test('Test to add new Corporate Company and verify the same by Admin  Industry type - Government and municipality ,Country - France ', async ({ page }) => {
  /*Test case stesps:1. Login as Admin > Admin Manaagment > Add company > fill company Name > Country:  france > Company type  : Government and municipality >select role : corporation */

   const poManager = new POManager(page)
   const loginpage = poManager.getLoginPage() // login page object 
   const adminPage = poManager.getAdminPage()
   const creatComp = poManager.getCreateCompanyPage()
   await loginpage.tstAdminUserLogin(); //test admin login 
   await adminPage.clickOnAdmin()
   await creatComp.clickOnAddCompany()
   await creatComp.enterCorporateNamefunc()
   await creatComp.enterValidURL()
   await creatComp.selectFRANCECountry()
   await creatComp.govtandmunicipalIndustryType()
   await creatComp.selectMDMmdm()
   await creatComp.addCompanyText()
   await creatComp.enterLinkDetails()
   await creatComp.selectCorporateCompanyType()
   await creatComp.clickOnAddCompany()
   await creatComp.searchNewgeneratefunc()
   await expect(adminPage.threeUsersDot).toBeEnabled()
   console.log('Corporate company added with Industry type - Government and municipality and Country - France')
   test.slow(500)
       
})

test('Test to add new Corporate Company and verify the same by Admin  Industry type - Health care ,Country - France ', async ({ page }) => {
  /*Test case stesps:1. Login as Admin > Admin Manaagment > Add company > fill company Name > Country:  France > Company type  : Health care  >select role : corporation */

   const poManager = new POManager(page)
   const loginpage = poManager.getLoginPage() // login page object 
   const adminPage = poManager.getAdminPage()
   const creatComp = poManager.getCreateCompanyPage()
   await loginpage.tstAdminUserLogin(); //test admin login 
   await adminPage.clickOnAdmin()
   await creatComp.clickOnAddCompany()
   await creatComp.enterCorporateNamefunc()
   await creatComp.enterValidURL()
   await creatComp.selectFRANCECountry()
   await creatComp.healthcareIndustryType()
   await creatComp.selectMDMmdm()
   await creatComp.addCompanyText()
   await creatComp.enterLinkDetails()
   await creatComp.selectCorporateCompanyType()
   await creatComp.clickOnAddCompany()
   await creatComp.searchNewgeneratefunc()
   await expect(adminPage.threeUsersDot).toBeEnabled()
   console.log('Corporate company added with Industry type - Health care and Country - France')
   test.slow(500)
       
})

test('Test to add new Corporate Company and verify the same by Admin  Industry type - Infrastructure ,Country - France ', async ({ page }) => {
  /*Test case stesps:1. Login as Admin > Admin Manaagment > Add company > fill company Name > Country:  France > Company type  : Infrastructure  >select role : corporation */

   const poManager = new POManager(page)
   const loginpage = poManager.getLoginPage() // login page object 
   const adminPage = poManager.getAdminPage()
   const creatComp = poManager.getCreateCompanyPage()
   await loginpage.tstAdminUserLogin(); //test admin login 
   await adminPage.clickOnAdmin()
   await creatComp.clickOnAddCompany()
   await creatComp.enterCorporateNamefunc()
   await creatComp.enterValidURL()
   await creatComp.selectFRANCECountry()
   await creatComp.infrastructureIndustryType()
   await creatComp.selectMDMmdm()
   await creatComp.addCompanyText()
   await creatComp.enterLinkDetails()
   await creatComp.selectCorporateCompanyType()
   await creatComp.clickOnAddCompany()
   await creatComp.searchNewgeneratefunc()
   await expect(adminPage.threeUsersDot).toBeEnabled()
   console.log('Corporate company added with Industry type - Infrastructure and Country - France')
   test.slow(500)
       
})

test('Test to add new Corporate Company and verify the same by Admin  Industry type - Renewable Resources and Alternative Energy ,Country - France ', async ({ page }) => {
  /*Test case stesps:1. Login as Admin > Admin Manaagment > Add company > fill company Name > Country:  France > Company type  : Renewable Resources and Alternative Energy  >select role : corporation */

   const poManager = new POManager(page)
   const loginpage = poManager.getLoginPage() // login page object 
   const adminPage = poManager.getAdminPage()
   const creatComp = poManager.getCreateCompanyPage()
   await loginpage.tstAdminUserLogin(); //test admin login 
   await adminPage.clickOnAdmin()
   await creatComp.clickOnAddCompany()
   await creatComp.enterCorporateNamefunc()
   await creatComp.enterValidURL()
   await creatComp.selectFRANCECountry()
   await creatComp.renewableresourcesIndustryType()
   await creatComp.selectMDMmdm()
   await creatComp.addCompanyText()
   await creatComp.enterLinkDetails()
   await creatComp.selectCorporateCompanyType()
   await creatComp.clickOnAddCompany()
   await creatComp.searchNewgeneratefunc()
   await expect(adminPage.threeUsersDot).toBeEnabled()
   console.log('Corporate company added with Industry type - Renewable Resources and Alternative Energy and Country - France')
   test.slow(500)
       
})

test('Test to add new Corporate Company and verify the same by Admin  Industry type - Resource transformation - France ', async ({ page }) => {
  /*Test case stesps:1. Login as Admin > Admin Manaagment > Add company > fill company Name > Country:  India > Company type  : Food and Beverage  >select role : corporation */

   const poManager = new POManager(page)
   const loginpage = poManager.getLoginPage() // login page object 
   const adminPage = poManager.getAdminPage()
   const creatComp = poManager.getCreateCompanyPage()
   await loginpage.tstAdminUserLogin(); //test admin login 
   await adminPage.clickOnAdmin()
   await creatComp.clickOnAddCompany()
   await creatComp.enterCorporateNamefunc()
   await creatComp.enterValidURL()
   await creatComp.selectFRANCECountry()
   await creatComp.resourceIndustryType()
   await creatComp.selectMDMmdm()
   await creatComp.addCompanyText()
   await creatComp.enterLinkDetails()
   await creatComp.selectCorporateCompanyType()
   await creatComp.clickOnAddCompany()
   await creatComp.searchNewgeneratefunc()
   await expect(adminPage.threeUsersDot).toBeEnabled()
   console.log('Corporate company added with Industry type - Resource transformation and Country - France')
   test.slow(500)
       
})

test('Test to add new Corporate Company and verify the same by Admin  Industry type - Services - France ', async ({ page }) => {
  /*Test case stesps:1. Login as Admin > Admin Manaagment > Add company > fill company Name > Country:  India > Company type  : Services  >select role : corporation */

   const poManager = new POManager(page)
   const loginpage = poManager.getLoginPage() // login page object 
   const adminPage = poManager.getAdminPage()
   const creatComp = poManager.getCreateCompanyPage()
   await loginpage.tstAdminUserLogin(); //test admin login 
   await adminPage.clickOnAdmin()
   await creatComp.clickOnAddCompany()
   await creatComp.enterCorporateNamefunc()
   await creatComp.enterValidURL()
   await creatComp.selectFRANCECountry()
   await creatComp.serviceIndustryType()
   await creatComp.selectMDMmdm()
   await creatComp.addCompanyText()
   await creatComp.enterLinkDetails()
   await creatComp.selectCorporateCompanyType()
   await creatComp.clickOnAddCompany()
   await creatComp.searchNewgeneratefunc()
   await expect(adminPage.threeUsersDot).toBeEnabled()
   console.log('Corporate company added with Industry type - Services and Country - France')
   test.slow(500)
       
})

test('Test to add new Corporate Company and verify the same by Admin  Industry type - Technology and Communications - France ', async ({ page }) => {
  /*Test case stesps:1. Login as Admin > Admin Manaagment > Add company > fill company Name > Country:  India > Company type  : Technology and Communications  >select role : corporation */

   const poManager = new POManager(page)
   const loginpage = poManager.getLoginPage() // login page object 
   const adminPage = poManager.getAdminPage()
   const creatComp = poManager.getCreateCompanyPage()
   await loginpage.tstAdminUserLogin(); //test admin login 
   await adminPage.clickOnAdmin()
   await creatComp.clickOnAddCompany()
   await creatComp.enterCorporateNamefunc()
   await creatComp.enterValidURL()
   await creatComp.selectFRANCECountry()
   await creatComp.technologyandcommIndustryType()
   await creatComp.selectMDMmdm()
   await creatComp.addCompanyText()
   await creatComp.enterLinkDetails()
   await creatComp.selectCorporateCompanyType()
   await creatComp.clickOnAddCompany()
   await creatComp.searchNewgeneratefunc()
   await expect(adminPage.threeUsersDot).toBeEnabled()
   console.log('Corporate company added with Industry type - Technology and Communications and Country - France')
   test.slow(500)
       
})

test('Test to add new Corporate Company and verify the same by Admin  Industry type - Transportation - France ', async ({ page }) => {
  /*Test case stesps:1. Login as Admin > Admin Manaagment > Add company > fill company Name > Country:  India > Company type  : Transportation  >select role : corporation */

   const poManager = new POManager(page)
   const loginpage = poManager.getLoginPage() // login page object 
   const adminPage = poManager.getAdminPage()
   const creatComp = poManager.getCreateCompanyPage()
   await loginpage.tstAdminUserLogin(); //test admin login 
   await adminPage.clickOnAdmin()
   await creatComp.clickOnAddCompany()
   await creatComp.enterCorporateNamefunc()
   await creatComp.enterValidURL()
   await creatComp.selectFRANCECountry()
   await creatComp.transportationIndustryType()
   await creatComp.selectMDMmdm()
   await creatComp.addCompanyText()
   await creatComp.enterLinkDetails()
   await creatComp.selectCorporateCompanyType()
   await creatComp.clickOnAddCompany()
   await creatComp.searchNewgeneratefunc()
   await expect(adminPage.threeUsersDot).toBeEnabled()
   console.log('Corporate company added with Industry type - Transportation and Country - France')
   test.slow(500)
       
})
  




