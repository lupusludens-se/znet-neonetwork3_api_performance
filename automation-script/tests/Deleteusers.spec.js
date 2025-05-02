import { test, expect } from '@playwright/test';
const { POManager } = require('../pages/src/pageObjectManager');



test.describe.configure({mode:'serial'});

test.skip('User deleting', async ({ page }) => {

    
    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    const adminPage = poManager.getAdminPage()
    const userPage =  poManager.getUserPopPage()
    await loginpage.tstAdminUserLogin();
    await expect(adminPage.adminLMbutton).toContainText('Admin')
    await adminPage.adminLMbutton.click()
    test.slow(500)
    await userPage.clickViewUserbtn()
    await userPage.clickSearchUser()
    await userPage.popUserfunc()


});

