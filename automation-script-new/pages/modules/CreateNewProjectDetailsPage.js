import { BasePage } from "../shared/BasePage.js";
import { faker } from "@faker-js/faker";
import { expect } from "@playwright/test";

export class CreateNewProjectDetailsPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page;
    // Add Project -> naviation buttons
    this.nextBtn = page.getByRole("button", { name: "Next" });
    this.minAnnualPeakTxtBox = page.locator("#minimumAnnualPeakKW");
    this.minAnnualSiteTxtBox = page.locator("#minimumAnnualSiteKWh");
    this.minAnnualkWHTxtBox = page.locator("#minimumAnnualMWh");
    this.minimumChargingStationsTxtBox = page.locator("#minimumChargingStationsRequired");
    this.minTermLengthTxtBox = page.locator("#minimumTermLength");
    this.minVolumeRequiredTxtBox = page.locator("#minimumAnnualValue");
    this.minVolLoadRequiredTxtBoxPlcHldr = this.page.locator("input[placeholder='Select one']");
    this.minVolLoadRequiredTxtBoxDrpDwn = {
      KW: page.locator(".dropdown-item.ng-star-inserted").filter({ hasText: "KW" }),
      KWh: page.locator(".dropdown-item.ng-star-inserted").filter({ hasText: "KWh" }),
      MW: page.locator(".dropdown-item.ng-star-inserted'] div:nth-child(1)").filter({ hasText: "MW" }),
      MWh: page.locator(".dropdown-item.ng-star-inserted").filter({ hasText: "MWh" }),
      Gallons: page.locator(".dropdown-item.ng-star-inserted").filter({ hasText: "Gallons" }),
    };
    this.totalAnnualkWHTxtBox = page.locator("#totalAnnualMWh");
    this.utilityTerritoryTxtBox = page.locator("neo-text-input").filter({ hasText: "Utility Territory" }).getByRole("textbox");
    this.currentlyAvailableChkBox = page.locator("neo-blue-checkbox").getByText("Currently Available");
    this.calenderTxtBox = page.locator("input[formcontrolname=projectAvailabilityApproximateDate]");
    this.termLengthOptBox = {
      "12 months": page.locator("label").filter({ hasText: "12 months" }),
      "24 months": page.locator("label").filter({ hasText: "24 months" }),
      "36 months": page.locator("label").filter({ hasText: /^\s*36 months\s+$/ }),
      ">36 months": page.locator("label").filter({ hasText: ">36 months" }),
    };
    this.timeUrgencyField = page.locator("neo-text-input").filter({ hasText: "Time & Urgency Considerations" }).getByRole("textbox");
    this.additionalCommentsField = page.locator("textarea");
    this.contractStructures = {};
    const contractStructureNames = [
      "PPA",
      "Lease",
      "Cash Purchase",
      "As-a-Service or Alternative Financing",
      "Shared Savings",
      "Guaranteed Savings",
      "Other Financing Structures",
      "Other",
      "Discount to Tariff",
    ];
    const contractStructureContainer = page.locator("div.long-input.mb-24").filter({ hasText: "Contract Structures Available" });
    this.yesInvestmentGradeCreditButton = page.locator("label[for='YesisInvestmentGradeCreditOfOfftakerRequired0']");
    this.noInvestmentGradeCreditButton = page.locator("label[for='NoisInvestmentGradeCreditOfOfftakerRequired1']");
    // Store contract structures dynamically using correct locator pattern
    for (const structure of contractStructureNames) {
      this.contractStructures[structure] = contractStructureContainer.locator("label").filter({ hasText: structure }).locator("svg");
    }
    // Locators for Value Provided options
    this.valueProvided = {};
    const valueProvidedNames = [
      "Cost Savings",
      "Resiliency",
      "Energy Savings/Reduction",
      "Renewable Attributes",
      "Visible Commitment to Climate Action",
      "Visible Commitment to Mitigating Climate Action",
      "Greenhouse Gas Emission Reduction Offset",
      "Community Benefits",
      "Other",
    ];
    const valueProvidedContainer = page.locator("div.long-input.mb-24").filter({ hasText: "Value Provided" });
    // Store Value Provided dynamically using correct locator pattern
    for (const value of valueProvidedNames) {
      this.valueProvided[value] = valueProvidedContainer.locator("label").filter({ hasText: value }).locator("svg");
    }
    this.utilityNameTxtBox = page.locator("neo-text-input[formcontrolname=utilityName]").getByRole("textbox");
    this.progamWebsiteTxtBox = page.locator("neo-text-input[formcontrolname=programWebsite]").getByRole("textbox");
    this.minPurchaseVolume = page.locator("#minimumPurchaseVolume");
    this.stripLengths = {
      "12 months": page.locator("label").filter({ hasText: "12 months" }).locator("svg"),
      "24 months": page.locator("label").filter({ hasText: "24 months" }).locator("svg"),
      "36 months": page
        .locator("span")
        .filter({ hasText: /^36 months$/ })
        .locator("svg"),
      ">36 months": page.locator("label").filter({ hasText: ">36 months" }).locator("svg"),
    };
    // Locators for Purchase Options Additional Details options
    this.purchaseOptionsAdditionalDetails = {};
    const purchaseOptionsNames = ["Behind the Meter", "In Front of the Meter"];
    const purchaseOptionsContainer = page.locator("div").filter({ hasText: "Purchase Options - Additional Details" });
    for (const option of purchaseOptionsNames) {
      this.purchaseOptionsAdditionalDetails[option] = purchaseOptionsContainer.locator("label").filter({ hasText: option }).locator("svg");
    }
  }

  /**
   * Fill in "Minimum Annual kWH Purchase" textbox (required field)
   * @param {string} value - numbers in string format
   */
  async enterMinimumAnnualkWHPurchaseTextBox(value) {
    await this.enterGenericTextBox(this.minAnnualkWHTxtBox, "minimumAnnualKwhPurchase", value, true);
  }

  /**
   * Fill in "Total Annual kWH Available" textbox (required field)
   * @param {string} value - numbers in string format
   */
  async enterTotalAnnualkWHAvailableTextBox(value) {
    await this.enterGenericTextBox(this.totalAnnualkWHTxtBox, "totalAnnualKwhAvailable", value, true);
  }

  /**
   * Fill in "Utility Territory" textbox
   * @param {string} value - accepts alpha-numeric text
   */
  async enterUtilityTerritoryTextBox(value) {
    await this.enterGenericTextBox(this.utilityTerritoryTxtBox, "utilityTerritory", value, true);
  }

  // Fill Minimum Purchase Volume (kW) Field
  async enterMinimumPurchaseVolume(value) {
    await this.enterGenericTextBox(this.minPurchaseVolume, "minimumPurchaseVolume", value, true);
  }

  // Fill Minimum Annual Peak (kW) field
  async enterMinimumAnnualPeak(value) {
    await this.enterGenericTextBox(this.minAnnualPeakTxtBox, "minimumAnnualPeak", value, true);
  }

  /**
   * Fill the "Minimum Annual Site Load (MWh)" textbox
   * @param {string} value - text for the textbox
   */
  async enterMinimumAnnualSiteTextBox(value) {
    await this.enterGenericTextBox(this.minAnnualSiteTxtBox, "minimumAnnualSite", value, true);
  }

  // Fill Minimum Term Length Available field
  async enterMinimumTermLength(value) {
    if (value && value.trim().length > 0) {
      await this.enterGenericTextBox(this.minTermLengthTxtBox, "minimumTermLength", value);
    }
  }

  // Fill Minimum Volume/Load Required
  async enterMinimumVolumeRequired(value) {
    if (value && value.trim().length > 0) {
      await this.enterGenericTextBox(this.minVolumeRequiredTxtBox, "minimumVolumeRequired", value);
    }
  }

  /**
   * Fill the "Utility Name" textbox
   * @param {string} value - text
   */
  async enterUtilityNameTextBox(value) {
    await this.enterGenericTextBox(this.utilityNameTxtBox, "utilityName", value, true);
  }

  // Fill Minimum Charging Stations Required Field
  async enterMinimumChargingStationsRequired(value) {
    await this.enterGenericTextBox(this.minimumChargingStationsTxtBox, "minimumChargingStationsRequired", value, true);
  }

  /**
   * Fill in the Calendar textbox
   * @param {string} value - text for calendar in the format "yyyy-MM-dd" or "select any date" for random date
   */
  async enterCalendarTextBox(value) {
    if (value.trim().match(/select\s*(?:any|a)\s*date/i) || value.trim().match(/select\s*(?:the)?\s*any\s*date/i)) {
      value = new Date().toISOString().substring(0, 10);
    }
    await this.enterGenericTextBox(this.calenderTxtBox, "calendarDate", value, true);
  }

  /**
   * Either click on the "Currently Available" check box or fill in the Calendar textbox.
   * When "Currently Available" checkbox is checked, the Calender textbox should be "logically" disabled.
   * @param {string} value - text for calendar in the format "yyyy-MM-dd" or "select the currently available checkbox"
   */
  async selectEitherApproximateDateOfProjectAvailability(value) {
    if (value && value.trim().length > 0 && value.match(/select\s*the\s*currently\s*available\s*checkbox/i)) {
      await this.verifyVisibilityAndClick(this.currentlyAvailableChkBox);
      await this.page.waitForTimeout(1000);
      const isCalendarTextBoxDisabled = await this.calenderTxtBox.evaluate((node) => node.className.includes("disabled"));
      expect(isCalendarTextBoxDisabled, `After "Currently Available" checkbox is checked, then expect the calendar textbox to be disabled`).toBeTruthy();
    } else if (value && value.trim().length > 0) {
      await this.enterCalendarTextBox(value);
    } else {
      throw new Error("JSON's file (approxDate) property is not defined");
    }
  }

  /**
   * Fill in "Program Website" textbox in the required format to start-with "http://" or "https://"
   * @param {string} value - text must start with "http://" or "https://" to be valid
   */
  async enterProgramWebsiteTextBox(value) {
    value = value && typeof value === "string" && value.trim().length > 0 ? value.trim() : "";
    // expecting "enter a valid URL" or "enter value URL"
    if (value.match(/\s*enter\s*[a]?\s*valid\s*URL\s*/i)) {
      value = `https://www.${faker.internet.domainWord()}.co.in`;
    }
    // expecting string to begin with "http"
    else if (!value.startsWith("http") || value.length === 0) {
      throw new Error(`JSON file's (progamWebsite) property must start with "http://" text`);
    }
    await this.enterGenericTextBox(this.progamWebsiteTxtBox, "progamWebsite", value, true);
  }

  // Select value from dropdown menu Minimum Volume/Load Required Kw, Kwh, Mw, Mwh, Gallons
  async selectMinimumVolumeLoadRequiredDropdown(value) {
    const dropdown = this.minVolLoadRequiredTxtBoxDrpDwn[value];
    if (!dropdown) {
      throw new Error(`Value ${value} is not available in the dropdown`);
    }
    await this.selectGenericDropDownItem(this.minVolLoadRequiredTxtBoxPlcHldr, dropdown, "minVolLoadRequiredTxtBoxDrpDwn", value, true, true);
  }

  // Fill Time & Urgency Considerations field
  async enterTimeUrgencyConsiderations(value) {
    if (value && value.trim().length > 0) {
      await this.enterGenericTextBox(this.timeUrgencyField, "timeUrgencyConsiderations", value);
    }
  }

  // Fill Additional Comments field
  async enterAdditionalComments(value) {
    if (value && value.trim().length > 0) {
      await this.enterGenericTextBox(this.additionalCommentsField, "additionalComments", value);
    }
  }

  /**
   * Select a single Radio button
   * @param {string} value - expecting ONLY one of the following: "12 months", "24 months", "36 months", or ">36 months"
   */
  async selectTermLengthOptionBox(value) {
    if (value?.length > 0) {
      await this.verifyVisibilityAndClick(this.termLengthOptBox[value]);
    } else {
      throw new Error(
        `JSON file's (termLengthOption) property not defined; expecting ONLY one string. Use either "12 months", "24 months", "36 months", or ">36 months"`
      );
    }
  }

  // Select Strip Lengths Available
  async selectStripLengthsAvailable(stripLengths) {
    let lengthsToSelect = [];
    // Normalize input
    if (typeof stripLengths === "string") {
      lengthsToSelect = [stripLengths];
    } else if (Array.isArray(stripLengths)) {
      lengthsToSelect = stripLengths;
    }
    // If "=All" is specified, override with all keys
    if (lengthsToSelect.includes("=All")) {
      lengthsToSelect = Object.keys(this.stripLengths);
    }
    // Shuffle the selection order to optimize UI interactions
    lengthsToSelect = faker.helpers.shuffle(lengthsToSelect);
    for (const length of lengthsToSelect) {
      const lengthLocator = this.stripLengths[length];
      if (!lengthLocator) {
        continue;
      }
      // Skip if locator is not visible immediately
      if (!(await lengthLocator.isVisible({ timeout: 2000 }).catch(() => false))) {
        continue;
      }
      let clicked = false;
      let retries = 3;
      while (retries > 0) {
        await lengthLocator.waitFor({ state: "visible" });
        let isVisible = await lengthLocator.isVisible();
        if (!isVisible) {
          await this.scrollToStripLengths();
        }
        if (await lengthLocator.isVisible()) {
          await lengthLocator.scrollIntoViewIfNeeded();
          await lengthLocator.click();
          // Verify if it was actually clicked
          if (await this.isStripLengthSelected(length)) {
            clicked = true;
            break;
          }
        }
        retries--;
      }
    }
  }

  // Verify if the strip length checkbox is selected
  async isStripLengthSelected(length) {
    const lengthLocator = this.stripLengths[length];
    return lengthLocator ? await lengthLocator.isChecked() : false;
  }

  // Scroll to bring more strip length options into view if needed
  async scrollToStripLengths() {
    const scrollContainer = this.page.locator(".options-wrapper");
    let scrollAttempts = 0;

    while (scrollAttempts < 5) {
      await scrollContainer.evaluate((node) => {
        if (node.scrollWidth > node.clientWidth || node.scrollHeight > node.clientHeight) {
          node.scrollBy(0, 200);
        }
      });
      await this.page.waitForTimeout(300);
      scrollAttempts++;
    }
  }

  // Click on Investment Grade Credit of Offtaker
  async clickInvestmentGradeCreditBtn(option) {
    try {
      if (option.match(/(yes|select\s*yes\s*radio\s*button)/i)) {
        await this.verifyVisibilityAndClick(this.yesInvestmentGradeCreditButton);
      } else if (option.match(/(no|select\s*no\s*radio\s*button)\s*/i)) {
        await this.verifyVisibilityAndClick(this.noInvestmentGradeCreditButton);
      } else {
        throw new Error("JSON file's (requiresInvestmentGradeCreditofOfftaker) property is not defined");
      }
    } catch (error) {
      throw new Error(`Error verifying radio button clickability: ${error.message}`);
      throw error; // Rethrow the error for the test to fail
    }
  }

  // Select Contract Structures options
  async selectContractStructuresCheckboxes(contractStructures) {
    let structuresToSelect = [];
    if (!contractStructures) {
      throw new Error("JSON file's (contractStructures) property is not defined");
    }
    // Normalize input to array
    else if (typeof contractStructures === "string") {
      // handle "select the Renewable Attributes,Other checkboxes" or "select the Cost Savings,Renewable Attributes, and Other checkboxes"
      if (contractStructures.match(/select\s*the\s*(.+)\s+checkbox(?:es)?/i)) {
        const extractName = contractStructures.match(/select\s*the\s*(.+)\s+checkbox(?:es)?/i).at(1);
        if (extractName.includes(",")) {
          structuresToSelect = extractName.split(",").map((node) => {
            const x = node.trim();
            if (x.match(/^and\s+/i)) {
              // remove "and" if exists
              return x.slice("and").trim();
            }
            return x;
          });
        } else {
          structuresToSelect = [extractName];
        }
      } else if (contractStructures.includes(",")) {
        structuresToSelect = contractStructures
          .split(",")
          .map((node) => node.trim())
          .filter((node) => node !== ""); // remove empty array items
      } else {
        structuresToSelect = [contractStructures];
      }
    } else if (Array.isArray(contractStructures)) {
      // assuming a string contains "," commas in a few array elements
      if (contractStructures.some((node) => node.includes(","))) {
        contractStructures = contractStructures.flatMap((node) => {
          if (node.includes(",")) {
            return node
              .split(",")
              .map((x) => x.trim())
              .filter((y) => y !== ""); // remove empty array items
          }
          return node;
        });
      }
      structuresToSelect = contractStructures;
    }
    // Handle "=All" case
    if (structuresToSelect.includes("=All") || structuresToSelect.at(0).match(/select\s*all\s*(?:the)?\s*checkboxes/i)) {
      structuresToSelect = Object.keys(this.contractStructures);
    }
    // Handle "select any one of the checkboxes"
    else if (structuresToSelect.includes("select any one of the checkboxes")) {
      const allStructures = Object.keys(this.contractStructures);
      structuresToSelect = [faker.helpers.arrayElement(allStructures)];
    }
    // Shuffle the selection order to optimize UI interactions
    structuresToSelect = faker.helpers.shuffle(structuresToSelect);
    for (const structure of structuresToSelect) {
      const structureLocator = this.contractStructures[structure];
      if (!structureLocator) {
        continue;
      }
      // Skip if locator is not visible immediately
      if (!(await structureLocator.isVisible({ timeout: 2000 }).catch(() => false))) {
        continue;
      }
      let clicked = false;
      let retries = 3;
      while (retries > 0) {
        await structureLocator.waitFor({ state: "visible" });
        let isVisible = await structureLocator.isVisible();
        if (!isVisible) {
          await this.scrollToContractStructures();
        }
        if (await structureLocator.isVisible()) {
          await structureLocator.scrollIntoViewIfNeeded();
          await structureLocator.click();
          // Verify if it was actually clicked
          if (await this.isStructureSelected(structure)) {
            clicked = true;
            break;
          }
        }
        retries--;
      }
    }
  }

  // Scroll to bring more contract structures into view if needed
  async scrollToContractStructures() {
    const scrollContainer = this.page.locator(".options-wrapper");
    let scrollAttempts = 0;
    while (scrollAttempts < 5) {
      await scrollContainer.evaluate((node) => {
        if (node.scrollWidth > node.clientWidth || node.scrollHeight > node.clientHeight) {
          node.scrollBy(0, 200);
        }
      });
      await this.page.waitForTimeout(300); // Give time for UI adjustment
      scrollAttempts++;
    }
  }

  // Verify if the contract structure checkbox is selected
  async isStructureSelected(structure) {
    const structureLocator = this.contractStructures[structure];
    return await structureLocator.isChecked();
  }

  // Select Value Provided options
  async selectValueProvidedCheckboxes(valueProvidedOptions) {
    let valuesToSelect = [];
    if (!valueProvidedOptions) {
      throw new Error("JSON file's (valueProvided) property is not defined");
    }
    // Normalize input to an array
    else if (typeof valueProvidedOptions === "string") {
      // handle "select the Renewable Attributes,Other checkboxes" or "select the Cost Savings,Renewable Attributes, and Other checkboxes"
      if (valueProvidedOptions.match(/select\s*the\s*(.+)\s+checkbox(?:es)?/i)) {
        const extractName = valueProvidedOptions.match(/select\s*the\s*(.+)\s+checkbox(?:es)?/i).at(1);
        if (extractName.includes(",")) {
          valuesToSelect = extractName.split(",").map((node) => {
            const x = node.trim();
            if (x.match(/^and\s+/i)) {
              // remove "and" if exists
              return x.slice("and").trim();
            }
            return x;
          });
        } else {
          valuesToSelect = [extractName];
        }
      } else if (valueProvidedOptions.includes(",")) {
        valuesToSelect = valueProvidedOptions
          .split(",")
          .map((node) => node.trim())
          .filter((node) => node !== ""); // remove empty array items
      } else {
        valuesToSelect = [valueProvidedOptions];
      }
    } else if (Array.isArray(valueProvidedOptions)) {
      if (valueProvidedOptions.some((node) => node.includes(","))) {
        valueProvidedOptions = valueProvidedOptions.flatMap((node) => {
          if (node.includes(",")) {
            return node
              .split(",")
              .map((x) => x.trim())
              .filter((y) => y !== "");
          }
          return node;
        });
      }
      valuesToSelect = valueProvidedOptions;
    }
    // If "All" is specified, select all options
    if (valuesToSelect.includes("=All") || valuesToSelect.at(0).match(/select\s*all\s*(?:the)?\s*checkboxes/i)) {
      valuesToSelect = Object.keys(this.valueProvided);
    }
    // Shuffle selection order for better UI interaction
    valuesToSelect = faker.helpers.shuffle(valuesToSelect);
    for (const value of valuesToSelect) {
      const valueLocator = this.valueProvided[value];
      if (!valueLocator) {
        continue;
      }
      // Skip if locator is not visible immediately
      if (!(await valueLocator.isVisible({ timeout: 2000 }).catch(() => false))) {
        continue;
      }
      let retries = 3;
      while (retries > 0) {
        await valueLocator.waitFor({ state: "visible" });
        let isVisible = await valueLocator.isVisible();
        if (!isVisible) {
          await this.scrollToValueProvided();
        }
        if (await valueLocator.isVisible()) {
          await valueLocator.scrollIntoViewIfNeeded();
          await valueLocator.click();
          // Verify if it was actually clicked
          if (await this.isValueProvidedSelected(value)) {
            break;
          }
        }
        retries--;
      }
    }
  }

  // Scroll to bring more value provided options into view if needed
  async scrollToValueProvided() {
    const scrollContainer = this.page.locator(".options-wrapper");
    let scrollAttempts = 0;
    while (scrollAttempts < 5) {
      await scrollContainer.evaluate((node) => {
        if (node.scrollWidth > node.clientWidth || node.scrollHeight > node.clientHeight) {
          node.scrollBy(0, 200);
        }
      });
      await this.page.waitForTimeout(300);
      scrollAttempts++;
    }
  }

  // Verify if the value provided option checkbox is selected
  async isValueProvidedSelected(value) {
    const valueLocator = this.valueProvided[value];
    return await valueLocator.isChecked();
  }

  // Select Purchase Options Additional Details options
  async selectPurchaseOptionsAdditionalDetails(options) {
    if (!options) return;
    let optionsToSelect = [];
    // Normalize input
    if (typeof options === "string") {
      optionsToSelect = [options];
    } else if (Array.isArray(options)) {
      optionsToSelect = options;
    }
    // If "=All" is specified, override with all keys
    if (optionsToSelect.includes("=All")) {
      optionsToSelect = Object.keys(this.purchaseOptionsAdditionalDetails);
    }
    // Shuffle selection order
    optionsToSelect = faker.helpers.shuffle(optionsToSelect);
    for (const option of optionsToSelect) {
      const optionLocator = this.purchaseOptionsAdditionalDetails[option];
      if (!optionLocator) continue;
      // Skip if not visible at all
      if (!(await optionLocator.isVisible({ timeout: 2000 }).catch(() => false))) {
        continue;
      }
      let clicked = false;
      let retries = 3;
      while (retries > 0) {
        await optionLocator.waitFor({ state: "visible" });
        const isVisible = await optionLocator.isVisible();
        if (!isVisible) {
          await this.scrollToPurchaseOptions();
        }
        if (await optionLocator.isVisible()) {
          await optionLocator.scrollIntoViewIfNeeded();
          await optionLocator.click();
          // Verify it was clicked
          if (await this.isPurchaseOptionSelected(option)) {
            clicked = true;
            break;
          }
        }
        retries--;
      }
    }
  }

  async isPurchaseOptionSelected(option) {
    const optionLocator = this.purchaseOptionsAdditionalDetails[option];
    return optionLocator ? await optionLocator.isChecked() : false;
  }

  async scrollToPurchaseOptions() {
    const scrollContainer = this.page.locator(".options-wrapper");
    let scrollAttempts = 0;
    while (scrollAttempts < 5) {
      await scrollContainer.evaluate((node) => {
        if (node.scrollHeight > node.clientHeight || node.scrollWidth > node.clientWidth) {
          node.scrollBy(0, 200);
        }
      });
      await this.page.waitForTimeout(300);
      scrollAttempts++;
    }
  }
}
