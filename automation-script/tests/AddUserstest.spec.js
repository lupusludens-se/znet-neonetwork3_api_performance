import { test, expect } from '@playwright/test';
const { POManager } = require('../pages/src/pageObjectManager');


test('Test to add new SP user and verify that same user by Admin ', async ({ page }) => {
  /*Test case stesps:1. Login as Admin > Admin Manaagment > Add user > fill first Name, Last Name and Email Id > Company : Tesla >select role : SP > Country: Inida > Heard Via : Conference/Event > Add user*/

    const poManager = new POManager(page)
    const loginpage = poManager.getLoginPage() // login page object 
    const adminPage = poManager.getAdminPage()
    await loginpage.tstAdminUserLogin(); //test admin login 
    await adminPage.addingNewSPuserandVerifying()
    test.slow(500)
        

})

test('Test to add new Corporate user and verify that same user by Admin ', async ({ page }) => {
    /*Test case stesps:1. Login as Admin > Admin Manaagment > Add user > fill first Name, Last Name and Email Id > Company : Corporate Company >select role : Corporate > Country: Inida > Heard Via : Zeigo Network Member > Add user*/
  
      const poManager = new POManager(page)
      const loginpage = poManager.getLoginPage() // login page object 
      const adminPage = poManager.getAdminPage()
      await loginpage.tstAdminUserLogin(); //test admin login 
      await adminPage.addingNewCorporateuserandVerifying()
      test.slow(500)
          
  
  })

  test('Test to add new Internal SE user and verify that same user by Admin ', async ({ page }) => {
    /*Test case stesps:1. Login as Admin > Admin Manaagment > Add user > fill first Name, Last Name and Email Id > Company : Tesla >select role : Internal SE > Country: Inida > Add user*/

      const poManager = new POManager(page)
      const loginpage = poManager.getLoginPage() // login page object 
      const adminPage = poManager.getAdminPage()
      await loginpage.tstAdminUserLogin(); //test admin login 
      await adminPage.addingNewInternalSEandVerifying()
      test.slow(500)
          
  
  })





