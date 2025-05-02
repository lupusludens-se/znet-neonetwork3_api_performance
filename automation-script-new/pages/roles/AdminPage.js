import { expect } from "@playwright/test";
import { BasePage } from "../shared/BasePage.js";
import resources from "../../utils/CommonTestResources.js";

export class AdminPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page;
    this.adminLMBtn = page.getByRole("link", { name: "Admin", exact: true });
    this.viewMyuserBtn = page.getByRole("button", { name: "View My Users" });
    this.editCompanyBtn = page.getByRole("button", {
      name: "Edit Company Profile",
    });
    this.viewallPM = page.locator("//div[3]/button[@class='btn-m green-frame flex-center']");
    this.viewallMM = page.locator("//div[4]/button[@class='btn-m green-frame flex-center']");
    this.projrctMangment = page.locator("//h4[.='Project Management']");
    this.messageManagement = page.locator("//h4[.='Messages Management']");
    this.companyManagement = page.locator("//h4[.='Company Management']");
    this.userManagement = page.locator("//h4[.='User Management']");
    this.dasboardLMBtn = page.getByRole("link", { name: "Dashboard" });
    this.projectLibraryLMBtn = page.getByRole("link", {
      name: "Project Library",
    });
    this.learnLMBtn = page.getByRole("link", { name: "Learn" });
    this.eventLMBtn = page.getByRole("link", { name: "Events" });
    this.toolLMBtn = page.getByRole("link", { name: "Tools" });
    this.communityLMBtn = page.getByRole("link", { name: "Community" });
    this.forumLMBtn = page.getByRole("link", { name: "Forum" });
    this.messageLMBtn = page.getByRole("link", { name: "Messages" });
    this.addProjectBtn = page.getByRole("button", { name: "Add Project" });
    this.exportProjectBtn = page.getByRole("button", {
      name: "Export Projects",
    });
    this.addUserBtn = page.getByRole("button", { name: "Add User" });
    this.firstName = page.getByPlaceholder("First Name");
    this.lastName = page.getByPlaceholder("Last Name");
    this.email = page.getByPlaceholder("Email");
    this.companyName = page.getByLabel("Company", { exact: true });
    this.getSPCompanyName = page.getByText(resources.TEXT.SP_COMPANY_NAME);
    this.getCorpoCompanyName = page.getByText(resources.TEXT.CORPORATION_COMPANY);
    this.selectSPRole = page.getByText("Solution Provider").first();
    this.selectCountry = page.getByLabel("Country");
    this.countryIndiaSelected = page.locator("div").filter({ hasText: /^India$/ });
    this.heardVia = page.locator("neo-dropdown").filter({ hasText: "Heard via Conference/Event Co" }).getByPlaceholder("Select one");
    this.heardViaSelectedCE = page.getByText("Conference/Event");
    this.heardViaSelectedZNM = page.getByText("Zeigo Network Member");
    this.AddUser = page.getByRole("button", { name: "Add User" });
    this.searchUser = page.locator("//div/neo-users-list/neo-search-bar/div/input");
    this.threeUsersDot = page.getByRole("table").getByRole("button").first();
    this.selectCorporateRole = page.locator("label").filter({ hasText: "Corporation" });
    this.internalSERoleLocator = page.locator("label").filter({ hasText: "Internal SE" });
    this.roleCorporate = page.locator("neo-table-row:nth-of-type(1) .corporation");
    this.roleInterSE = page.locator("neo-table-row:nth-of-type(1) .internal");
    this.roleSP = page.locator("neo-table-row:nth-of-type(1) .provider");

    this.addCompanyBtn = page.getByRole("button", { name: "Add Company" });
    this.addcompanyName = page.locator("neo-text-input").filter({ hasText: "Company Name" }).getByRole("textbox");
  }

  async verifyingAdminaccess(username, password) {
    // Verifying wether the user is SP admin or not
    await this.adminLMBtn.click();
  }

  async clickOnAdmin() {
    await this.adminLMBtn.click();
    await this.page.waitForTimeout(800);
  }

  async clickOnuserManagement() {
    await this.userManagement.click();
    await this.page.waitForTimeout(800);
  }

  async clickOnaddUserBtn() {
    await this.addUserBtn.click();
    await this.page.waitForTimeout(800);
  }

  //Enter the generated first name
  async enterFirstName(nameLength) {
    this.randomFirstName = this.generateRandomFirstName(nameLength);
    await this.firstName.fill("");
    await this.firstName.type(this.randomFirstName);
    return this.randomFirstName;
  }

  //Enter the generated last name
  async enterLastName(nameLength) {
    this.randomLastName = this.generateRandomLastName(nameLength);
    await this.lastName.fill("");
    await this.lastName.type(this.randomLastName);
    return this.randomLastName;
  }

  async selectSProle() {
    await this.scrollDown();
    await this.selectSPRole.click();
    await this.scrollToEnd();
    await this.page.waitForTimeout(800);
  }

  async selectCorporaterole() {
    await this.scrollDown();
    await this.selectCorporateRole.click();
    await this.scrollToEnd();
    await this.page.waitForTimeout(800);
  }

  async selectInternalSErole() {
    await this.scrollDown();
    await this.internalSERoleLocator.click();
    await this.scrollToEnd();
    await this.page.waitForTimeout(800);
  }

  async selectIndCountryfuc() {
    await this.selectCountry.type(resources.COUNTRY.INDIA);
    await this.page.waitForTimeout(500);
    await this.countryIndiaSelected.click();
  }

  async selectHeardViaCE() {
    await this.heardVia.click();
    await this.heardViaSelectedCE.click();
    await this.page.waitForTimeout(800);
  }

  async selectHeardViaZNM() {
    await this.heardVia.click();
    await this.heardViaSelectedZNM.click();
    await this.page.waitForTimeout(800);
  }

  async selectSPcompany() {
    await this.companyName.type(resources.TEXT.SP_COMPANY_NAME);
    await this.page.waitForTimeout(800);
    await this.getSPCompanyName.click();
  }

  async selectCorporatecompany() {
    await this.companyName.type(resources.TEXT.CORPORATION_COMPANY);
    await this.page.waitForTimeout(800);
    await this.getCorpoCompanyName.click();
  }

  async verifyCorporateAddUser() {
    await this.page.waitForTimeout(500);
    await expect(this.roleCorporate).toContainText(resources.TEXT.CORPORATION);
  }

  async verifySPAddUser() {
    await this.page.waitForTimeout(500);
    await expect(this.roleSP).toContainText(resources.TEXT.SOLUTION_PROVIDER);
  }

  async verifyInternalSEAddUser() {
    await this.page.waitForTimeout(500);
    await expect(this.roleInterSE).toContainText(resources.TEXT.INTERNAL_COMPANY);
  }

  //  Add new SP user function
  async addNewSPuserandVerify() {
    await this.clickOnAdmin();
    await this.clickOnuserManagement();
    await this.clickOnaddUserBtn();
    await this.enterFirstName();
    await this.enterLastName();
    await this.selectSPcompany();
    // Generate email
    this.generatedEmail = this.generateRandomEmail();
    await this.email.fill("");
    await this.email.type(this.generatedEmail);
    await this.selectSProle(); // select SP profile
    await this.selectIndCountryfuc(); // select India Country
    await this.selectHeardViaCE(); //select heard via conference/event
    await this.AddUser.click();
    await this.page.waitForTimeout(1500);
    await this.searchMailIDgenerate();
    await this.verifySPAddUser();
  }

  //  Add new Corporate user function
  async addNewCorporateuserandVerify() {
    await this.clickOnAdmin();
    await this.clickOnuserManagement();
    await this.clickOnaddUserBtn();
    await this.enterFirstName();
    await this.enterLastName();
    await this.selectCorporatecompany();
    // Generate email
    this.generatedEmail = this.generateRandomEmail();
    await this.email.fill("");
    await this.email.type(this.generatedEmail);
    await this.selectCorporaterole(); // select SP profile
    await this.selectIndCountryfuc(); // select India Country
    await this.selectHeardViaZNM(); //select heard via ZNM
    await this.AddUser.click();
    await this.page.waitForTimeout(1500);
    await this.searchMailIDgenerate();
    await this.verifyCorporateAddUser();
  }

  // function to search and verify genearated mailID
  async searchMailIDgenerate() {
    await this.searchUser.fill("");
    await this.searchUser.type(this.generatedEmail);
    await this.page.waitForTimeout(2500);
    await expect(this.threeUsersDot).toBeEnabled();
    await this.threeUsersDot.click();
  }

  //  Add new Internal SE user function
  async addNewInternalSEandVerify() {
    await this.clickOnAdmin();
    await this.clickOnuserManagement();
    await this.clickOnaddUserBtn();
    await this.enterFirstName();
    await this.enterLastName();
    // await this.selectCorporatecompanyfunc()
    // Generate email
    this.generatedEmail = this.generateRandomEmail();
    await this.email.fill("");
    await this.email.type(this.generatedEmail);
    await this.selectInternalSErole(); // select InternalSE profile
    await this.selectIndCountryfuc(); // select India Country
    await this.scrollToEnd();
    await this.page.waitForTimeout(800);
    await this.AddUser.click();
    await this.page.waitForTimeout(1500);
    await this.searchMailIDgenerate();
    await this.verifyInternalSEAddUser();
  }
}
