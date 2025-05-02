import { test, expect } from "@playwright/test";
import { POManager } from "../utils/POManager.js";

test.describe.configure({ mode: "serial" });
let poManager;
let login;
let admin;
let userPopFunc;
let projectLibrary;
let project;

test.beforeEach(async ({ page }) => {
  poManager = new POManager(page); // Dynamically initialize Page Objects
  login = poManager.getPage("login");
  admin = poManager.getPage("admin");
  userPopFunc = poManager.getPage("userPopFunc");
  projectLibrary = poManager.getPage("projectLibrary");
  project = poManager.getPage("project");
});

test.afterEach(async ({ page }) => {
  await login.logOut();
  test.slow(500);
  await page.close();
});

test.skip("User deleting", async ({ page }) => {
  await login.loginAs("admin");
  await expect(admin.adminLMBtn).toContainText("Admin");
  await admin.adminLMBtn.click();
  test.slow(500);
  await userPopFunc.clickViewUserBtn();
  await userPopFunc.clickSearchUser();
  await userPopFunc.popUser();
});

// Test may fail sometimes due to duplicate project names in the database from Project Library. Once cleanup is done via API, it should work better.
test("verify user is able to delete project, verify it's not exist by both - current role and corporate user", async ({ page }) => {
  await login.loginAs("admin");
  await projectLibrary.verifyVisibilityAndClick(projectLibrary.projectLibraryBtn);
  const projectTitle = await projectLibrary.deleteRandomProjectInTable();
  await login.logOut();
  await page.waitForTimeout(1000);
  await login.loginAs("corporateUser", 11000);
  await project.searchAndVerifyProjectNotFound(projectTitle, project.projectBtn);
});
