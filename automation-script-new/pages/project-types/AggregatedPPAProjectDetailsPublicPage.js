import { BasePage } from "../shared/BasePage";

export class AggregatedPPAProjectDetailsPublicPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page;
    this.mapSearchTxtBox = page.getByRole("textbox", { name: "Search" });
    this.mapSearchListBox = page.locator("ul[class=suggestions]"); // use li.innerText to get the list item

    this.isoRtoDropDnBox = page.locator('neo-dropdown[formcontrolname="isoRto"] input');
    this.isoRtoDropDnListBox = page.locator('neo-dropdown[formcontrolname="isoRto"]').locator("div[class^=dropdown-item]");

    this.productTypeDropDnBox = page.locator("neo-dropdown[formcontrolname=productType] input");
    this.productTypeDropDnListBox = page.locator("neo-dropdown[formcontrolname=productType]").locator("div[class^=dropdown-item]");

    this.commercialOpDateCalendarBox = page.locator("neo-date-input[formcontrolname=commercialOperationDate]").locator("input[type=date]"); //.fill('2025-02-07');

    this.renewableAttributesChkBox = page.locator("div").filter({ hasText: /^Renewable Attributes$/ });
    this.energyArbitrageChkBox = page.locator("div").filter({ hasText: /^Energy Arbitrage$/ });
    this.visibleCommitmentToClimateActionChkBox = page.locator("div").filter({ hasText: /^Visible Commitment to Climate Action$/ });
    this.communityBenefitsChkBox = page.locator("div").filter({ hasText: /^Community Benefits$/ });
    this.ppaTermLengthTxtBox = page.locator("#ppaTermYearsLength");
    this.totalProjectNameplateCapacityTxtBox = page.locator("#totalProjectNameplateMWACCapacity");
    this.totalProjectExpectedAnnualProductionP50TxtBox = page.locator("#totalProjectExpectedAnnualMWhProductionP50");
    this.minimumOfftakeVolumeRequiredTxtBox = page.locator("#minimumOfftakeMWhVolumeRequired");
    this.notesForPotentialOfftakersTxtBox = page.locator("textarea[placeholder=Notes]");
    // Navigation buttons
    this.backBtn = page.getByRole("button", { name: "Back" });
    this.cancelBtn = page.getByRole("button", { name: "Cancel" });
    this.saveAsDraftBtn = page.getByRole("button", { name: "Save as Draft" });
    this.nextBtn = page.getByRole("button", { name: "Next" });
  }

  /**
   * Click into Map's search textbox, fill it with text, then wait for the dropdown list to appear and click on the specific item
   * @param {Object {searchText: string, dropdownText: string}} searchObj
   * @param {string} searchObj.searchText - Map search text
   * @param {string} searchObj.dropdownText - innerText of the dropdown item
   */
  async searchMapForValue(searchObj) {
    // check that searchObj.searchText is valid and not empty
    if (searchObj && Object.hasOwn(searchObj, "searchText") && typeof searchObj.searchText === "string" && searchObj.searchText.trim().length > 0) {
      searchObj.searchText = searchObj.searchText.trim();
      // check for QA request for generated data
      const regEx = searchObj.searchText.toLowerCase().match(/(enter [a]?\s*zip\s*code and select (?:the)?\s*value from (?:the)?\s*suggestions)/i);
      if (regEx) {
        searchObj.searchText = "02115";
        searchObj.dropdownText = "Boston";
      }
      // searchObj.searchText is already validated, make sure that searchObj.dropdownText is valid
      if (Object.hasOwn(searchObj, "dropdownText") && typeof searchObj.dropdownText === "string" && searchObj.dropdownText.trim().length > 0) {
        await this.enterGenericTextBox(this.mapSearchTxtBox, "mapLocation", searchObj.searchText, true);
        await this.verifyVisibilityAndClick(this.mapSearchListBox.locator("a").filter({ hasText: searchObj.dropdownText }));
        return;
      }
      else {
        throw new Error(`JSON file's (mapLocation.dropdownText) property is not defined`);    
      }
    }
    throw new Error(`JSON file's (mapLocation.searchText) property is not defined`);
  }

  /**
   * Populate the "Commercial Operation Date" textbox with the text date format yyyy-MM-dd
   * @param {string} dateString - Required string format as "yyyy-MM-dd"
   */
  async enterCalenderDateTextBox(dateString) {
    if (dateString.trim().toLowerCase() === "select any date") {
      dateString = new Date().toISOString().substring(0, 10);
    }
    await this.enterGenericTextBox(this.commercialOpDateCalendarBox, "calendarDate", dateString, true);
  }

  /**
   * Click on "ISO / RTO" drop down, and select the item
   * @param {string} text - list item name to be clicked on - if text is empty, click nothing
   * @param {boolean} exact - default false means text is NOT case-sensitive, true otherwise
   */
  async selectIsoRtoDropDownItem(text, exact = false) {
    if (text && text.trim().length > 0) {
      await this.selectGenericDropDownItem(this.isoRtoDropDnBox, this.isoRtoDropDnListBox, "isoRto", text, exact, true);
    }
  }

  /**
   * Click on the "Product Type" drop down, and select the item
   * @param {string} text - list item name to be clicked on
   * @param {boolean} exact - default false means text is NOT case-sensitive, true otherwise
   */
  async selectProductTypeDropDownItem(text, exact = false) {
    await this.selectGenericDropDownItem(this.productTypeDropDnBox, this.productTypeDropDnListBox, "productType", text, exact, true);
  }

  /**
   * Click on the "Value To Offer" list of checkboxes
   * @param {string | Array<string>} stringArray - list of checkbox names or "select all the checkboxes" to select all of them
   */
  async selectValueToOfftakerCheckBox(stringArray) {
    // assume stringArray is a single string of comma separated items "Renewable Attributes", "Energy Arbitrage"
    if (stringArray && typeof stringArray === "string" && stringArray.includes(",")) {
      stringArray = stringArray.split(",").map((node) => node.trim());
    }
    // assuming that stringArray is a string "select all checkboxes" or "select all the checkboxes"
    else if (
      (stringArray &&
        typeof stringArray === "string" &&
        stringArray
          .trim()
          .toLowerCase()
          .match(/select all (the)?\s?checkboxes/i)) ||
      // assuming that stringArray is an array with a single string element that contains
      //  either "select all checkboxes" or "select all the checkboxes"
      (stringArray &&
        Array.isArray(stringArray) &&
        stringArray
          .at(0)
          .trim()
          .toLowerCase()
          .match(/select all (the)?\s?checkboxes/i))
    ) {
      stringArray = ["Renewable Attributes", "Energy Arbitrage", "Visible Commitment to Climate Action", "Community Benefits"];
    }
    // assuming stringArray is a single string "Energy Arbitrage"
    else if (stringArray && typeof stringArray === "string" && stringArray.trim().length > 0) {
      const regExtract = stringArray.match(/(?<=select the\b\s).+(?=\s\bcheckbox)/i);
      if (regExtract && regExtract.length > 0) {
        if (regExtract.at(0).includes(" and ")) {
          stringArray = regExtract
            .at(0)
            .split(" and ")
            .map((node) => node.trim());
        } else {
          stringArray = [regExtract.at(0).trim()];
        }
      } else {
        stringArray = [stringArray.trim()];
      }
    }

    for (const text of stringArray) {
      const chkBoxElement = this.page.locator("div").filter({ hasText: new RegExp(`^${text}$`, "i") });
      await this.verifyVisibilityAndClick(chkBoxElement);
    }
  }

  /**
   * Enter text into the "PPA Term Length" textbox
   * @param {string | undefined} text - text to fill textbox or "enter a numeric value" to use random generated number
   */
  async enterPPATermLengthTextBox(text) {
    await this.enterGenericTextBox(this.ppaTermLengthTxtBox, "ppaTermLength", text, true);
  }

  /**
   * Enter text into the "Total Project Nameplate Capacity" textbox
   * @param {string} text
   */
  async enterTotalProjectNameplateCapacityTextBox(text) {
    await this.enterGenericTextBox(this.totalProjectNameplateCapacityTxtBox, "totalProjectNameplateCapacity", text, true);
  }

  /**
   * Enter text into the "Total Project Expect Annual Production - P50" textbox
   * @param {string} text
   */
  async enterTotalProjectExpectedAnnualProductionP50TextBox(text) {
    await this.enterGenericTextBox(this.totalProjectExpectedAnnualProductionP50TxtBox, "totalProjectExpectedAnnualProdP50", text, true);
  }

  /**
   * Enter text into the "Minimum Offtake Value Required" textbox
   * @param {string} text
   */
  async enterMinimumOfftakeVolumeRequiredTextBox(text) {
    await this.enterGenericTextBox(this.minimumOfftakeVolumeRequiredTxtBox, "minOfftakeVolumeRequired", text, true);
  }

  /**
   * Enter text into the "Notes For Potential Offtakers" textbox
   * @param {string} text
   */
  async enterNotesForPotentialOfftakersTextBox(text) {
    await this.enterGenericTextBox(this.notesForPotentialOfftakersTxtBox, "notesForPotentialOfftakers", text, false);
  }
}
