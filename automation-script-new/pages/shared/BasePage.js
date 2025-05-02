import { expect } from "@playwright/test";
import { faker } from "@faker-js/faker";

export class BasePage {
  /**
   * @param {import('@playwright/test').Page} page
   */
  constructor(page) {
    this.page = page;
    this.projectLibraryBtn = page.getByRole("link", {
      name: "Project Library",
    });
    this.projectBtn = page.getByRole("link", { name: "Projects" });
    this.searchTxtBox = page.getByRole("textbox", { name: "Search" });
  }

  //Dynamic locator: returns the "No results for" message specific to the given project title.
  noResultsLabel(projectTitle) {
    return this.page.getByText(`No results for “${projectTitle}”`);
  }

  /**
   * Extracts the user's name located on the UI in the top-right corner of the user's partial profile container.
   * @returns users's name in the format "FirstName LastName" as a string
   */
  async verifyLoggedInUserDisplayedInProject() {
    // const userHeadingLocator = this.page.locator("h6.text-dark-gray-800");
    const userHeadingLocator = this.page.locator("h6[class^='text-dark-gray-800']");
    await expect(userHeadingLocator).toBeVisible();
    const userName = (await userHeadingLocator.textContent()).trim();
    const userTitleLocator = this.page.getByTitle(userName).first(); // the table may have multiple rows of project types with users with the same name
    await expect(userTitleLocator).toBeVisible();
    return userName;
  }

  /**
   * Common method to search for a project and verify that it is found.
   * Login as corporate user and make search in Project Page/Login as current role and make search in Project Library Page
   * @param {string} projectTitle - The title of the project to search for.
   * @param {Locator} navigationButton - The button to navigate to the correct page (e.g., Projects or Project Library).
   */
  async searchAndVerifyProjectFound(projectTitle, navigationButton) {
    if (!projectTitle) return;
    await this.waitForAndVerifyVisibility(navigationButton);
    await navigationButton.click();
    await this.page.waitForTimeout(2000);
    await this.waitForAndVerifyVisibility(this.searchTxtBox);
    await this.searchTxtBox.click();
    await this.searchTxtBox.fill(projectTitle);
    await this.page.keyboard.press("Enter");
    await this.page.waitForTimeout(2500);
    // Verify that the project appears in search results
    const projectLocator = this.page.getByText(projectTitle).first();
    await expect(projectLocator).toBeVisible();
    await this.page.waitForTimeout(500);
    await this.verifyLoggedInUserDisplayedInProject();
    await this.page.waitForTimeout(500);
  }

  /**
   * Common method to search for a project and verify that it is NOT found.
   * * Login as corporate user and make search in Project Page/Login as current role and make search in Project Library Page
   * @param {string} projectTitle - The title of the project to search for.
   * @param {Locator} navigationButton - The button to navigate to the correct page (e.g., Projects or Project Library).
   */
  async searchAndVerifyProjectNotFound(projectTitle, navigationButton) {
    if (!projectTitle) return;
    await this.waitForAndVerifyVisibility(navigationButton);
    await navigationButton.click();
    await this.page.waitForTimeout(3000);
    await this.waitForAndVerifyVisibility(this.searchTxtBox);
    await this.searchTxtBox.click();
    await this.searchTxtBox.fill(projectTitle);
    await this.page.keyboard.press("Enter");
    await this.page.waitForTimeout(2500);
    // Verify "No results for" message
    await this.waitForAndVerifyVisibility(this.noResultsLabel(projectTitle));
    await this.page.waitForTimeout(500);
  }

  /**
   * Click inside the textbox, then use Playwright specific methods to clear the field, then fill in the text
   * @param {page object} element - textbox page element
   * @param {string} propertyName - JSON property name
   * @param {string} text - replaces the existing text in the field
   * @param {boolean} [isRequired=false] - required means textbox is a required field, and text cannot be empty
   * @return {string} returns string that was filled into the textbox
   */
  async enterGenericTextBox(element, propertyName, text, isRequired = false) {
    text = text ? text.trim() : "";
    if (
      text.toLowerCase().match(/(enter [a]?\s*numeric(?:al)? value)/i) ||
      // check for string similar to "enter a numeric value 0 to 2147483647" or "enter a numeric value from (0 to 2147483647)"
      text.toLowerCase().match(/enter [a]?\s*numeric(?:al)?\s*value[s]?\s*(?:from)?\s*\(?(\d+)\s*to\s*(\d+)\s*\)?/i) ||
      // check for string similar to "0 to 2147483647" or "(0 to 2147483647)"
      text.toLowerCase().match(/\(?\s*(\d+)\s*to\s*(\d+)\s*\)?\s*(?:.*?)/i)
    ) {
      text = this.generateRandomShortValue();
    } else if (text.toLowerCase().match(/(enter [a]?\s*numeric(?:al)? value (\d+))/i)) {
      text = text.match(/\d+/).at(0);
    } else if (
      text.toLowerCase().match(/(enter [a]?\s*string value)/i) ||
      // check for string similar to
      //     "enter string value or alphanumeric value"
      //  or "enter a string value or alpha-numeric value"
      //  or "enter valid alpha-numerical value"
      //  or "enter a valid alpha numerical values"
      text
        .toLowerCase()
        .match(
          /enter\s*(a\s*)?(valid\s*)?(string|alphanumeric|alpha[\s-]*numeric(?:al)?)(\s*value[s]?)?(\s*or\s*(string|alphanumeric|alpha[\s-]*numeric(?:al)?)(\s*value[s]?)?)?/i
        )
    ) {
      text = faker.commerce.productAdjective();
    } else if (text.match(/(enter [a]?\s*alpha\s*numeric(?:al)? value)/i)) {
      text = faker.system.networkInterface();
    }
    // required element's text cannot be left blank
    if (isRequired && text.length === 0) {
      throw new Error(`JSON file's (${propertyName}) property not defined`);
    }
    await this.page.locator("body").click();
    await element.fill("");
    await element.fill(text, { force: true });
    const dElem = await element.evaluate((x) => {
      return { tagName: x.tagName, type: x.getAttribute("type") };
    });
    // private inner method
    async function _getSearchTextbox(element, elementTagName) {
      if (["input", "textarea"].includes(elementTagName)) {
        return await element.inputValue();
      } else {
        return await element.innerText();
      }
    }
    let fillText = await _getSearchTextbox(element, dElem.tagName.toLowerCase());
    if (!fillText.includes(text)) {
      await element.pressSequentially(text); // 2nd attempt to fill textbox
      fillText = await _getSearchTextbox(element, dElem.tagName.toLowerCase());
      if (!fillText.includes(text)) {
        await this.page.keyboard.type(text); // 3rd attempt to fill textbox
      }
    }
    // clicking outside the textbox allows the javascript to perform error checks
    await this.page.locator("body").click();
    return text;
  }

  /**
   * Click on Drop Down, then select item method
   * @param {locator} elementDropDn - drop down bar
   * @param {locator} elementDropDnListBox - drop down listing box
   * @param {string} propertyName - JSON property
   * @param {string} itemName - JSON value or "select 'some name' from the drop down"
   * @param {boolean} isExact - is case sensitive
   * @param {boolean} [isRequired=false] - true means dropdown item must be selected, so itemName cannot be blank or undefined
   */
  async selectGenericDropDownItem(elementDropDn, elementDropDnListBox, propertyName, itemName, isExact, isRequired = false) {
    itemName = itemName ? itemName.trim() : "";
    const strArray = itemName.match(/(?<=\bselect\s).+(?=\sfrom\b)/i);
    if (strArray && strArray.length > 0) {
      itemName = strArray.at(0).trim();
    }
    // required property itemName cannot be left blank
    if (isRequired && itemName.length === 0) {
      throw new Error(`JSON file's (${propertyName}) property is not defined`);
    } else if (itemName.length > 0) {
      await this.verifyVisibilityAndClick(elementDropDn);
      await this.verifyVisibilityAndClick(elementDropDnListBox.getByText(itemName, { exact: isExact }), 3060);
    }
  }

  /**
   * Verify element's visibility. When timeout is surpassed, test will fail
   * @param {locator} locator
   * @param {number} timeout
   */
  async waitForAndVerifyVisibility(locator, timeout = 30000) {
    try {
      await expect(locator).toBeVisible({ timeout: timeout });
    } catch (error) {
      throw error;
    }
  }

  /**
   * Verify element's visibility and click. When timeout is surpassed, test will fail
   * @param {locator} element
   * @param {number} waitTime
   */
  async verifyVisibilityAndClick(element, waitTime = 3000) {
    try {
      await this.waitForAndVerifyVisibility(element);
      await element.click();
      await this.page.waitForTimeout(waitTime);
    } catch (error) {
      throw error;
    }
  }

  /**
   * Verify element's visibility and element is enabled
   * @param {locator} element
   * @param {number} waitTime
   */
  async waitForAndVerifyEnabled(element, waitTime = 800) {
    await expect(element).toBeEnabled({ timeout: waitTime });
  }

  /**
   * Wait for page object to become visible, and transitioned for disabled to enabled, then click on it
   * Verify element's visibility, element is enabled and click on it
   * @param {page object} element - page element
   * @param {number} waitTime - time-out based in milliseconds
   */
  //Verify element's visibility, element is enabled and click on it
  async verifyVisibilityEnabledAndClick(element, waitTime = 800) {
    try {
      await this.waitForAndVerifyVisibility(element);
      await this.waitForAndVerifyEnabled(element, waitTime);
      await element.click();
      await this.page.waitForTimeout(waitTime);
    } catch (error) {
      throw error;
    }
  }

  // Scroll down by 500px
  async scrollDown() {
    await this.page.evaluate(() => window.scrollBy(0, 500));
  }

  // Scroll to the bottom of the page
  async scrollToEnd() {
    await this.page.keyboard.press("End");
  }

  //Generate random First name
  generateRandomFirstName() {
    const randomName = faker.person.firstName(); // Generate a random first name
    const randomLetter = faker.string.alpha({ length: 1, casing: "upper" });
    let firstName = `First${randomName}${randomLetter}Name`;
    return firstName;
  }

  //Generate random Last name
  generateRandomLastName() {
    const randomName = faker.person.lastName(); // Generate a random last name
    const randomLetter = faker.string.alpha({ length: 1, casing: "upper" });
    let lastName = `Last${randomName}${randomLetter}Name`;
    return lastName;
  }

  //Generate random Company name
  generateRandomCompanyName() {
    let generatedCompanyName = faker.company
      .name()
      .split(" ")[0]
      .replace(/[^a-zA-Z]/g, ""); // Remove special characters
    return generatedCompanyName;
  }

  //Generate random Forum name
  generateRandomForumName() {
    const randomCompany = this.generateRandomCompanyName();
    let forumName = `New Automation Forum ${randomCompany}`;
    return forumName;
  }

  //Generate random Corporate name
  generateRandomCorporateName() {
    const randomCompany = this.generateRandomCompanyName();
    const randomLetter = faker.string.alpha({ length: 1, casing: "upper" }); // Single uppercase letter
    let corporateName = `Corporate ${randomCompany}${randomLetter}Automation Company`;
    return corporateName;
  }

  //Generate random email address
  generateRandomEmail() {
    const randomString = faker.string.alphanumeric(8);
    let generatedEmail = `Test${randomString}@yopmail.com`;
    return generatedEmail;
  }

  generateRandomNumber() {
    return faker.number.int({ min: 1, max: 99999999 }).toString();
  }

  // Generate a single-digit year (1-9)
  generateRandomShortValue() {
    const randomYear = faker.number.int({ min: 1, max: 9 }).toString();
    return randomYear;
  }
}
