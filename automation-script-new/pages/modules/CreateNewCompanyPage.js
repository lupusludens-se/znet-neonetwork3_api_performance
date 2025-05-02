import { BasePage } from "../shared/BasePage.js";
import resources from "../../utils/CommonTestResources.js";
import fs from "fs";
import path from "path";
import { parse } from "csv-parse/sync";
import { faker } from "@faker-js/faker";

//SPadminPage page methods
export class CreateNewCompanyPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page; // this belongs to current class
    this.timeout = 500;
    this.initializeSelectors();
  }

  // Initialize selectors
  initializeSelectors() {
    this.enterCompanyName = this.page.locator("neo-text-input").filter({ hasText: "Company Name" }).getByRole("textbox");
    this.enterURL = this.page.locator("neo-text-input:nth-child(3) > .ng-untouched");
    this.searchCountry = this.page.getByPlaceholder("Search");
    this.selectIndustry = this.page.locator("neo-dropdown").filter({ hasText: "Industry Consumer Goods" }).getByPlaceholder("Select one");

    // Industry types
    this.industries = {
      consumerGoods: this.page.getByText("Consumer Goods"),
      extraMinerals: this.page.getByText("Extractives and Minerals"),
      financials: this.page.getByText("Financials"),
      foodBeverage: this.page.getByText("Food and Beverage"),
      govt: this.page.getByText("Government and Municipality"),
      healthCare: this.page.getByText("Health Care"),
      infrastructure: this.page.getByText("Infrastructure"),
      renewable: this.page.getByText("Renewable Resources and"),
      resource: this.page.getByText("Resource Transformation"),
      service: this.page.getByText("Services"),
      technology: this.page.getByText("Technology and Communications"),
      transport: this.page.getByText("Transportation"),
    };
    this.aboutCompanyTxt = this.page.locator("#editor");

    // Company types
    this.companyTypes = {
      corporate: this.page.locator("label").filter({ hasText: "Corporation" }),
      solutionProvider: this.page.locator("label").filter({ hasText: "Solution Provider" }),
    };
    this.addCompanyBtn = this.page.getByRole("button", { name: "Add Company" });
    this.searchTxt = this.page.getByPlaceholder("Search");
    this.mdmKeyDD = this.page.locator("neo-dropdown").filter({ hasText: "MDM Key ORG- MDM-" }).getByPlaceholder("Select one");

    // MDM options
    this.mdmOptions = {
      org: this.page.getByText("ORG-"),
      mdm: this.page.getByText("MDM-"),
    };
    this.mdmInput = this.page.getByPlaceholder("Maximum 10 digits");
    this.linkNameInput = this.page.getByPlaceholder("Link Name");
    this.linkNameURLInput = this.page.getByPlaceholder("URL");
    // this.threeUsersDot = this.page.getByRole('table').getByRole('button').first() // No such element on DOM dt 12 mar 25
  }

  // Parsing excel sheet data
  async addcompNamefrmCSV() {
    const comprecords = parse(fs.readFileSync(path.join(__dirname, "../../data/CompanyDatatobeAddedToPreProdCSV.csv")), {
      columns: true,
      skip_empty_lines: true,
    });
    for (const comprecord of comprecords) {
      await this.enterCompanyName.click();
      await this.enterCompanyName.clear();
      await this.page.waitForTimeout(this.timeout);
      await this.enterCompanyName.type(comprecord["Company_Name"]);
      await this.enterValidURL();
      await this.selectCountry(resources.COUNTRY.USA);
      await this.selectIndustryType("renewable");
      await this.addCompanyText();
      await this.selectCompanyType("corporate");
      await this.searchTxt.fill("");
      await this.searchTxt.type(comprecord["Company_Name"]);
      await this.page.waitForTimeout(this.timeout * 3);
      // await expect(this.threeUsersDot).toBeEnabled() // No such element on DOM dt 12 mar 25
      await this.page.waitForTimeout(this.timeout * 1.8);
      await this.clickOnAddCompany();
    }
  }

  // Add About company text func..
  async addCompanyText() {
    await this.scrollDown();
    await this.page.waitForTimeout(this.timeout);
    await this.aboutCompanyTxt.type(resources.TEXT.ABOUT_COMPANY);
  }

  // Enter link details
  async enterLinkDetails() {
    await this.scrollDown();
    await this.linkNameInput.type(faker.company.name()); // ("Test Link Name");
    const fakeURL = `www.${faker.internet.domainWord()}.co.in`;
    await this.linkNameURLInput.type(fakeURL); // ("www.testURL.co.in");
  }

  // Select corporate company type
  async selectCompanyType(type) {
    await this.scrollDown();
    await this.companyTypes[type].click();
  }

  // Select enter valid URL
  async enterValidURL() {
    await this.enterURL.click();
    const fakeURL = `www.${faker.internet.domainWord()}.co.in`;
    await this.enterURL.type(fakeURL); // ("www.testdomai.co.in");
  }

  // Select MDM mdm
  async selectMDMOption(option) {
    await this.mdmKeyDD.click();
    await this.mdmOptions[option].click();
    await this.mdmInput.type(resources.MDM_KEY_VALUE);
  }

  // Enter new generated corporate name
  async enterCorporateName() {
    await this.enterCompanyName.click();
    await this.page.waitForTimeout(this.timeout);
    this.generatedCorporateName = this.generateRandomCorporateName();
    await this.enterCompanyName.fill("");
    await this.enterCompanyName.type(this.generatedCorporateName);
    return this.generatedCorporateName;
  }

  // Select specific country
  async selectCountry(countryName) {
    await this.searchCountry.type(countryName);
    await this.page.waitForTimeout(this.timeout * 2);
    // Locate the country dynamically
    let countryLocator = this.page.locator("div").filter({ hasText: new RegExp(`^${countryName}$`) });
    if (countryName === resources.COUNTRY.FRANCE) {
      countryLocator = countryLocator.nth(3);
    }
    await countryLocator.click();
  }

  // Select consumer industry type
  async selectIndustryType(industry) {
    await this.selectIndustry.click();
    await this.page.waitForTimeout(this.timeout * 2);
    await this.industries[industry].click();
  }

  // Click on Add company button func..
  async clickOnAddCompany() {
    await this.addCompanyBtn.click();
    await this.page.waitForTimeout(this.timeout);
  }

  // Search newly generated company func..
  async searchNewgenerate() {
    await this.searchTxt.fill("");
    await this.searchTxt.type(this.generatedCorporateName);
    await this.page.waitForTimeout(this.timeout * 3);
  }
}
