import { test, expect } from "@playwright/test";
import { POManager } from "../utils/POManager.js";

test.describe("Role verification Tests", () => {
  let poManager;
  let login;
  let admin;

  test.beforeEach(async ({ page }) => {
    poManager = new POManager(page); // Dynamically initialize Page Objects
    login = poManager.getPage("login");
    admin = poManager.getPage("admin");
  });

  test.afterEach(async ({ page }) => {
    await login.logOut();
    test.slow(500);
    await page.close();
  });

  test("Test to add new SP user and verify that same user by Admin ", async ({ page }) => {
    /*Test case stesps:1. Login as Admin > Admin Manaagment > Add user > fill first Name, Last Name and Email Id > Company : Tesla >select role : SP > Country: Inida > Heard Via : Conference/Event > Add user*/
    await login.loginAs("admin");
    await admin.addNewSPuserandVerify();
  });

  test("Test to add new Corporate user and verify that same user by Admin ", async ({ page }) => {
    /*Test case stesps:1. Login as Admin > Admin Manaagment > Add user > fill first Name, Last Name and Email Id > Company : Corporate Company >select role : Corporate > Country: Inida > Heard Via : Zeigo Network Member > Add user*/
    await login.loginAs("admin");
    await admin.addNewCorporateuserandVerify();
  });

  test("Test to add new Internal SE user and verify that same user by Admin ", async ({ page }) => {
    /*Test case stesps:1. Login as Admin > Admin Manaagment > Add user > fill first Name, Last Name and Email Id > Company : Tesla >select role : Internal SE > Country: Inida > Add user*/
    await login.loginAs("admin");
    await admin.addNewInternalSEandVerify();
  });
});
