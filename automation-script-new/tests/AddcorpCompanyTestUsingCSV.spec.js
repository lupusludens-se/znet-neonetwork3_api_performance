import { test } from "@playwright/test";
import { POManager } from "../utils/POManager.js";

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
  });

  test.afterEach(async ({ page }) => {
    await login.logOut();
    test.slow(500);
    await page.close();
  });

  // Skip as a one-time bulk data insertion in PreProd; may reuse CSV if needed.
  test.skip("Adding new Corporate Company using CSV file ", async ({ page }) => {
    await login.loginAs("admin");
    await admin.clickOnAdmin();
    await createCompany.clickOnAddCompany();
    await createCompany.addcompNamefrmCSV();
  });
});
