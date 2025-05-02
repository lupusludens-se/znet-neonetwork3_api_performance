import { test, expect } from '@playwright/test';
const { POManager } = require('../pages/src/pageObjectManager');



test('Adding new Corporate Company using CSV file ', async ({ page }) => {


    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    const adminPage = poManager.getAdminPage()
    const creatComp = poManager.getCreateCompanyPage()
    await loginpage.tstAdminUserLogin(); //test admin login 
    await adminPage.clickOnAdmin()
    await creatComp.clickOnAddCompany()
    await creatComp.addcompNamefrmCSVfunc()
    console.log('CSV Data Corporate company added with  Industry type - Consumer Goods and Country - United States..')
    test.slow(500)
        

})

