import { test, expect } from '@playwright/test';
const { POManager } = require('../pages/src/pageObjectManager');



test('Test to verify loggedIn user is Admin', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    const adminPage = poManager.getAdminPage()
    await loginpage.tstAdminUserLogin();
    await expect(adminPage.adminLMbutton).toContainText('Admin')
    await adminPage.adminLMbutton.click()
    await loginpage.logOut()
    test.slow(500)

});

test('Test to verify loggedIn user is SP Admin', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    const spadminPage = poManager.getSPadminPage()
    await loginpage.tstSPadminUserLogin();
    await expect(spadminPage.mangeLMbutton).toContainText('Manage')
    await spadminPage.mangeLMbutton.click()
    test.slow(500)
    await loginpage.logOut()
    

});

test('Test to verify loggedIn user is SP User', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    const spadminPage = poManager.getSPadminPage()
    const adminPage = poManager.getAdminPage()
    await loginpage.tstSPUserLogin();
    await expect(adminPage.adminLMbutton).toBeHidden()
    await expect(spadminPage.mangeLMbutton).toBeHidden()
    
    test.slow(500)
    await loginpage.logOut()

});

test('Test to verify loggedIn user is Corporate User', async ({ page }) => {

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object
    const corpoPage = poManager.getCorporatePage()  
    const adminPage = poManager.getAdminPage()
    await loginpage.tstCorporateUserLogin();
    await expect(adminPage.adminLMbutton).toBeHidden()
    await expect(corpoPage.projectLMbutton).toBeVisible()
    
    test.slow(500)
    await loginpage.logOut()

});



