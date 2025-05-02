import { test, expect } from "@playwright/test";
import { POManager } from "../utils/POManager.js";

test.describe("Role verification Tests", () => {
  let poManager;
  let login;
  let admin;
  let spAdmin;
  let corporateUser;

  test.beforeEach(async ({ page }) => {
    poManager = new POManager(page); // Dynamically initialize Page Objects
    login = poManager.getPage("login");
    admin = poManager.getPage("admin");
    spAdmin = poManager.getPage("spAdmin");
    corporateUser = poManager.getPage("corporateUser");
  });

  test.afterEach(async ({ page }) => {
    await login.logOut();
    test.slow(500);
    await page.close();
  });

  test("Test to verify loggedIn user is Admin", async ({ page }) => {
    await login.loginAs("admin");
    await expect(admin.adminLMBtn).toContainText("Admin");
    await admin.adminLMBtn.click();
  });

  test("Test to verify loggedIn user is SP Admin", async ({ page }) => {
    await login.loginAs("spAdmin");
    await expect(spAdmin.manageLMBtn).toContainText("Manage");
    await spAdmin.manageLMBtn.click();
  });

  test("Test to verify loggedIn user is SP User", async ({ page }) => {
    await login.loginAs("spUser");
    await expect(admin.adminLMBtn).toBeHidden();
    await expect(spAdmin.manageLMBtn).toBeHidden();
  });

  test("Test to verify loggedIn user is Corporate User", async ({ page }) => {
    await login.loginAs("corporateUser");
    await expect(admin.adminLMBtn).toBeHidden();
    await expect(corporateUser.projectLMbtn).toBeVisible();
  });
});
