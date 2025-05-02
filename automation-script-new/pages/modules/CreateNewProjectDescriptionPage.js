import { expect } from "@playwright/test";
import { BasePage } from "../shared/BasePage.js";
import { faker } from "@faker-js/faker";

export class CreateNewProjectDescriptionPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page;
    this.publishedByTxtBox = page.getByRole("textbox", { name: "Published By" });
    this.publishedByDropDnItemBox = page.locator(".results-wrapper").locator(".name"); // append with ".getByText("<contains item name>") "
    this.projectTitleTxtBox = page.getByRole("textbox", { name: "Project Title Create a catchy" });
    this.projectSubTitleTxtBox = page.getByRole("textbox", { name: "Sub-Title Use words people" });
    this.projectDescribeTheOpportunityTxtBox = page.locator("#opportunity").locator("#editor");
    this.projectAboutTheProvider = page.locator("#about").locator("#editor");
    this.initialProviderField = this.page.getByText("Test About");
    this.backBtn = page.getByRole("button", { name: "Back" });
    this.saveAsDraftBtn = page.getByRole("button", { name: "Save as Draft" });
    this.cancelBtn = page.getByRole("button", { name: "Cancel" });
    this.publishBtn = page.getByRole("button", { name: "Publish" });
    this.saveAsDraftPopUp = page.getByRole("heading", { name: "Save as Draft" });
    this.projectNameField = page.getByRole("textbox", { name: "Name" });
    this.saveDraftPopUp = page.getByRole("button", { name: "Save", exact: true });
    this.projectLibraryBtnPopUp = page.getByRole("button", { name: "Project Library" });
  }

  generateRandomTitle(length = 35) {
    if (length <= 200) {
      const randomKeyword = faker.commerce.product();
      const randomLetter = faker.string.alpha({ length: 1, casing: "upper" });
      const randomNumber = this.generateRandomNumber();
      let text = `Reliable ${randomKeyword} - ${randomLetter}${randomNumber}`;
      // Ensure exact character length when required (100 or 200)
      let missingChars = length - text.length;
      let additionalChars = missingChars > 0 ? faker.string.alpha({ length: missingChars }) : "";
      const projectTitle = `${text} ${additionalChars}`.substring(0, length);
      return projectTitle;
    }
    // Generate an initial text for long sentences (4000 characters)
    let text = faker.string.alpha({ length });
    const longSentence = text.substring(0, length);
    return longSentence;
  }

  generateAlphaNumString(text) {
    let isGeneratedText = false;
    text = text ? text.trim() : "";
    const regex = /enter (?:a|an)?\s*(?:alpha\s*numeric(?:al)? value|string)(?:\s*value)? with (?:full\s*length|maximum)\s*of (\d+)\s*character[s]?\s*(?:string[s]?)?/i;
    const match = text.match(regex);
    if (match) {
      const number = parseInt(match[1]);
      text = this.generateRandomTitle(number); // Generates exactly an 'A-z' number of characters
      isGeneratedText = true;
    } else if (text.match(/(enter [a]?\s*alpha\s*numeric(?:al)? value)/i)) {
      text = this.generateRandomTitle(); // Default 35-character title
      isGeneratedText = true;
    }
    return { text: text, isGeneratedText: isGeneratedText };
  }

  /**
   * Select "Publish By" drop down item using the user's profile name from the top-right corner in the UI
   */
  async selectPublishedByDropDownItem() {
    const searchString = await this.page.locator("neo-user-avatar").evaluate((node) => node.nextElementSibling.innerText.replaceAll("\n", " "));
    this.verifyVisibilityAndClick(this.publishedByTxtBox); // triggers a partial page refresh
    this.page.waitForTimeout(1000); // wait for the spinner to disappear
    await this.enterGenericTextBox(this.publishedByTxtBox, "", searchString);
    this.page.waitForTimeout(1000); // wait for the dropdown to be satified and automatically disappear
    // await this.verifyVisibilityAndClick(this.publishedByDropDnItemBox.getByText(searchString), 3060);
  }

  /**
   * Enter text into the "Project Title" textbox
   * @param {string} text - when "enter a alpha numerical value", then generate text
   * @returns {Object {text: string, isGeneratedText: boolean}}
   */
  async enterProjectTitleTextBox(text) {
    const reusableObj = this.generateAlphaNumString(text);
    this.enterGenericTextBox(this.projectTitleTxtBox, "projectTitle", reusableObj.text, true);
    await expect(this.projectTitleTxtBox).toHaveValue(reusableObj.text);
    return { text: reusableObj.text, isGeneratedText: reusableObj.isGeneratedText };
  }

  /**
   * Enter text into the "Sub-Title" textbox
   * @param {string} text  - when "enter a alpha numerical value", then generate text
   * @returns
   */
  async enterProjectSubTitleTextBox(text) {
    const reusableObj = this.generateAlphaNumString(text);
    this.enterGenericTextBox(this.projectSubTitleTxtBox, "subTitle", reusableObj.text, true);
    await expect(this.projectSubTitleTxtBox).toHaveValue(reusableObj.text);
    return { text: reusableObj.text, isGeneratedText: reusableObj.isGeneratedText };
  }

  /**
   * Enter text into the "Describe The Opportunity" textbox
   * @param {string} text  - when "enter a alpha numerical value", then generate text
   */
  async enterDescribeTheOpportunityTextBox(text) {
    const reusableObj = this.generateAlphaNumString(text);
    this.enterGenericTextBox(this.projectDescribeTheOpportunityTxtBox, "describeOpportunity", reusableObj.text, true);
  }

  /**
   * Enter text into the "About the Provider" textbox
   * @param {string} text - "should be auto-popuated from the company description page" will leave the textbox alone
   */
  async enterAboutTheProviderTextBox(text) {
    const providerField = this.page.locator("div[contenteditable='true']").nth(1);
    if (!(await providerField.isVisible({ timeout: 3000 }))) {
      throw new Error("'About The Provider' field is not visible on the page");
    }
    const existingText = (await providerField.innerText()).trim();
    // Checks if JSON contains auto-populated instruction
    if (text === "should be auto populated from the company description page") {
      await expect(providerField).toBeVisible();
      if (!existingText || existingText.length === 0) {
        throw new Error("'About The Provider' field is empty but expected to be auto-populated.");
      }
      return; // Skips filling as expected
    }
    const additionalText = faker.commerce.productAdjective();
    // Adds new content only if projectTitle isn't already included
    if (!existingText.includes(additionalText)) {
      let finalValue = `${existingText.trim()} ${additionalText}`;
      await providerField.click();
      await providerField.fill(finalValue);
      await this.page.waitForTimeout(1000);
      await expect(this.page.getByText(finalValue)).toBeVisible();
    }
  }

  //choose Cancel/ Save As Draft/ Publish to click
  async performAction(action, saveAsDraftTxt = "") {
    await this.page.locator("body").click();
    let publishedProjectId = "";
    switch (action.trim().toLowerCase()) {
      case "save as draft":
        await this.verifyVisibilityAndClick(this.saveAsDraftBtn);
        await this.page.waitForTimeout(1500);
        await this.waitForAndVerifyVisibility(this.projectNameField);
        await this.projectNameField.fill(saveAsDraftTxt);
        await this.waitForAndVerifyVisibility(this.saveAsDraftPopUp);
        await this.saveDraftPopUp.click();
        await this.page.waitForTimeout(500);
        await this.verifyVisibilityAndClick(this.projectLibraryBtnPopUp);
        break;
      case "publish":
        await this.waitForAndVerifyVisibility(this.publishBtn);
        await this.waitForAndVerifyEnabled(this.publishBtn, 800);
        await this.publishBtn.click();
        await this.page.waitForTimeout(1);
        // API capture the created Project unique Id
        const response = await this.page.waitForResponse((node) => node.url().includes("api/projects") && node.request().method() === "POST" && node.status() === 200);
        publishedProjectId = await response.json();
        await this.publishBtn.waitFor({ state: "detached" });
        break;
      case "cancel":
        await this.cancelBtn.click();
        await this.page.waitForTimeout(1500);
        break;
    }
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    return publishedProjectId;
  }
}
