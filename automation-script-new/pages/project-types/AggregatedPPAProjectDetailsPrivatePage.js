import { BasePage } from "../shared/BasePage";

export class AggregatedPPAProjectDetailsPrivatePage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page;

    this.settlementTypeDropDn = page.locator("neo-dropdown[formcontrolname=settlementType] input");
    this.settlementTypeDropDnListBox = page.locator("neo-dropdown[formcontrolname=settlementType]").locator("div[class^=dropdown-item]");

    // only visible when Settlement Type dropdown items ["Hub", "Loadzone"] is selected
    this.settlementHubLoadZoneDropDn = page.locator("neo-dropdown[formcontrolname=settlementHubOrLoadZone] input");
    this.settlementHubLoadZoneDropDnListBox = page.locator("neo-dropdown[formcontrolname=settlementHubOrLoadZone]").locator("div[class^=dropdown-item]");

    this.currencyForAllPriceEntriesDropDn = page.locator("neo-dropdown[formcontrolname=forAllPriceEntriesCurrency] input");

    // choose drop-down item with getByText("USD")
    this.currencyForAllPriceEntriesDropDnListBox = page.locator("neo-dropdown[formcontrolname=forAllPriceEntriesCurrency]").locator("div[class^=dropdown-item]");

    this.contractPriceTxtBox = page.locator("#contractPricePerMWh");

    this.floatingMarketSwapIndexDiscountTxtBox = page.locator("neo-text-input").filter({ hasText: "Floating Market Swap (Index, Discount)" }).getByRole("textbox");
    this.floatingMarketSwapFloorTxtBox = page.locator("neo-text-input").filter({ hasText: "Floating Market Swap (Floor)" }).getByPlaceholder("Text");
    this.floatingMarketSwapCapTxtBox = page.locator("neo-text-input").filter({ hasText: "Floating Market Swap (Cap)" }).getByPlaceholder("Text");

    this.pricingStructureDropDn = page.locator("neo-dropdown[formcontrolname=pricingStructure] input");
    this.pricingStructureDropDnListBox = page.locator("neo-dropdown[formcontrolname=pricingStructure]").locator("div[class^=dropdown-item]");

    this.upsidePercentageToDeveloperTxtBox = page.locator("#upsidePercentageToDeveloper");
    this.upsidePercentageToOfftakerTxtBox = page.locator("#upsidePercentageToOfftaker");
    this.discountAmountTxtBox = page.locator("#discountAmount");

    this.eacTypeDropDn = page.locator("neo-dropdown[formcontrolname=eac] input");
    this.eacTypeDropDnListBox = page.locator("neo-dropdown[formcontrolname=eac]").locator("div[class^=dropdown-item]");
    this.eacValueTxtBox = page.locator("neo-text-input").filter({ hasText: "EAC Value" }).getByPlaceholder("Text");

    // only visible when EAC Type dropdown item "Other" is selected
    this.customEACValueTxtBox = page.locator("neo-text-input").filter({ hasText: "EAC Value Type" }).getByPlaceholder("Custom EAC Type");

    this.settlementPriceIntervalDropDn = page.locator("neo-dropdown[formcontrolname=settlementPriceInterval] input");
    this.settlementPriceIntervalDropDnListBox = page.locator("neo-dropdown[formcontrolname=settlementPriceInterval]").locator("div[class^=dropdown-item]");

    // only visible when Settlement Price Interval dropdown item "Other" is selected
    this.customSettlementPriceIntervalTxtBox = page.getByRole("textbox", { name: "Custom Settlement Price Interval" });

    this.settlementCalcIntervalDropDn = page.locator("neo-dropdown[formcontrolname=settlementCalculationInterval] input");
    this.settlementCalcIntervalDropDnListBox = page.locator("neo-dropdown[formcontrolname=settlementCalculationInterval]").locator("div[class^=dropdown-item]");
    this.projectMWCurrentlyAvailTxtBox = page.locator("#projectMWCurrentlyAvailable");

    this.notesForSEOperationsTeamTxtBox = page.getByRole("textbox", { name: "Notes" });

    this.backBtn = page.getByRole("button", { name: "Back" });
    this.cancelBtn = page.getByRole("button", { name: "Cancel" });
    this.saveAsDraftBtn = page.getByRole("button", { name: "Save as Draft" });
    this.nextBtn = page.getByRole("button", { name: "Next" });
  }

  /**
   * Click on the "Settlement Type" drop down box and select the drop down item
   * @param {string | undefined} itemName - drop down text to select or "" to skip as optional
   * @param {boolean} exact - is itemName case sensitive
   */
  async selectSettlementTypeDropDownItem(itemName, exact = false) {
    if (itemName && itemName.trim().length > 0) {
      await this.selectGenericDropDownItem(this.settlementTypeDropDn, this.settlementTypeDropDnListBox, "settlementType", itemName, exact);
    }
  }

  /**
   * Click on the "Settlement Hub / Load Zone" drop down box and select the drop down item
   * @param {string | undefined} itemName - drop down item name or "" to skip as optional
   * @param {boolean} exact - is itemName case sensitive
   */
  async selectSettlementHubLoadZoneDropDownItem(itemName, exact = false) {
    if (itemName && itemName.trim().length > 0) {
      await this.selectGenericDropDownItem(this.settlementHubLoadZoneDropDn, this.settlementHubLoadZoneDropDnListBox, "settlementHub", itemName, exact);
    }
  }

  /**
   * Click on the "Currency for all Price Entries" drop down, and select the item
   * @param {string | undefined} itemName - list item name to be clicked on
   * @param {boolean} exact - default false means itemName is NOT case-sensitive, true otherwise
   */
  async selectCurrencyForAllPriceEntriesDropDownItem(itemName, exact = false) {
    await this.selectGenericDropDownItem(
      this.currencyForAllPriceEntriesDropDn,
      this.currencyForAllPriceEntriesDropDnListBox,
      "currencyForAllPriceEntries",
      itemName,
      exact,
      true
    );
  }

  /**
   * Enter text into the Contract Price (per MWh) textbox
   * @param {string | undefined} text - text to fill or "enter numeric value" for generated string
   */
  async enterContractPriceTextBox(text) {
    await this.enterGenericTextBox(this.contractPriceTxtBox, "contractPrice", text, true);
  }

  /**
   * Enter text into the Floating Market Swap (Index, Discount) textbox
   * @param {string} text
   */
  async enterFloatingMarketSwapIndexTextBox(text) {
    if (text && text.trim().length > 0) {
      await this.enterGenericTextBox(this.floatingMarketSwapIndexDiscountTxtBox, "floatIndex", text);
    }
  }

  /**
   * Enter text into the Floating Market Swap (Floor) textbox
   * @param {string} text
   */
  async enterFloatingMarketSwapFloorTextBox(text) {
    if (text && text.trim().length > 0) {
      await this.enterGenericTextBox(this.floatingMarketSwapFloorTxtBox, "floatFloor", text);
    }
  }

  /**
   * Enter text into the Floating Market Swap (Cap) textbox
   * @param {string} text
   */
  async enterFloatingMarketSwapCapTextBox(text) {
    if (text && text.trim().length > 0) {
      await this.enterGenericTextBox(this.floatingMarketSwapCapTxtBox, "floatCap", text);
    }
  }

  /**
   * Enter text into the Upside Percentage To Developer textbox
   * @param {string} text
   */
  async enterUpsidePercentageToDeveloperTextBox(text) {
    await this.enterGenericTextBox(this.upsidePercentageToDeveloperTxtBox, "upsidePercentageToDeveloper", text, true);
  }

  /**
   * Enter text into the Upside Percentage To Offtaker textbox
   * @param {string} text
   */
  async enterUpsidePercentageToOfftakerTextBox(text) {
    await this.enterGenericTextBox(this.upsidePercentageToOfftakerTxtBox, "upsidePercentageToOfftaker", text, true);
  }

  /**
   * Enter text into the Discount Amount textbox
   * @param {string} text
   */
  async enterDiscountAmountTextBox(text) {
    await this.enterGenericTextBox(this.upsidePercentageToOfftakerTxtBox, "upsidePercentageToOfftaker", text, true);
  }

  /**
   * Enter text into the EAC Value textbox
   * @param {string} text
   */
  async enterEacValueTextBox(text) {
    if (text && text.trim().length > 0) {
      await this.enterGenericTextBox(this.eacValueTxtBox, "eacValue", text);
    }
  }

  /**
   * Enter text into the Custom EAC Type textbox
   * @param {string} text
   */
  async enterCustomEacTypeTextBox(text) {
    await this.enterGenericTextBox(this.customEACValueTxtBox, "customEacType", text, true);
  }

  /**
   * Click on the "Pricing Structure" drop dow, and select the item
   * @param {string | undefined} itemName - list item name to be clicked on
   * @param {boolean} exact - default false means itemName is NOT case-sensitive, true otherwise
   */
  async selectPricingStructureDropDownItem(itemName, exact = false) {
    await this.selectGenericDropDownItem(this.pricingStructureDropDn, this.pricingStructureDropDnListBox, "pricingStructure", itemName, exact, true);
  }

  /**
   * Click on the "EAC Type" drop down, and select the item
   * @param {string | undefined} itemName - list item name to be clicked on
   */
  async selectEACTypeDropDownItem(itemName) {
    await this.selectGenericDropDownItem(this.eacTypeDropDn, this.eacTypeDropDnListBox, "eacType", itemName, true);
  }

  /**
   * Fill the "Custom Settlement Price Interval" text box
   * This textbox only appears when "Settlement Price Interval" drop down item is "Other"
   * @param {string | undefined} text - text to fill, cannot be empty
   */
  async enterCustomSettlementPriceIntervalTextBox(text) {
    this.enterGenericTextBox(this.customSettlementPriceIntervalTxtBox, "customSettlementPriceInterval", text, true);
  }

  /**
   * Click on "Settlement Price Interval" drop down and select the item
   * @param {string | undefined} itemName - list item name to be clicked on
   * @param {boolean} exact - default false means itemName is NOT case-sensitive, true otherwise
   */
  async selectSettlementPriceIntervalDropDownItem(itemName, exact = false) {
    await this.selectGenericDropDownItem(this.settlementPriceIntervalDropDn, this.settlementPriceIntervalDropDnListBox, "settlementPriceInterval", itemName, exact, true);
  }

  /**
   * Click on "Settlement Calcuation Interval" drop down and select the item
   * @param {string | undefined} itemName - list item name to be clicked on
   * @param {boolean} exact - default false means itemName is NOT case-sensitive, true otherwise
   */
  async selectSettlementCalcIntervalDropDownItem(itemName, exact = false) {
    await this.selectGenericDropDownItem(
      this.settlementCalcIntervalDropDn,
      this.settlementCalcIntervalDropDnListBox,
      "settlementCalculationInterval",
      itemName,
      exact,
      true
    );
  }

  /**
   * Enter numbers into the "Project MW Currently Available" textbox
   * @param {string | undefined} text - numbers or "enter a numeric value" will generate a random number
   * - "enter a numeric value 12345" will enter in the number value specified
   */
  async enterProjectMWCurrentlyAvailTextBox(text) {
    this.enterGenericTextBox(this.projectMWCurrentlyAvailTxtBox, "projectMWCurrentlyAvailable", text, true);
    await this.page.waitForTimeout(1020);
  }

  /**
   * Enter numbers into the "Notes for SE Operations Team" textbox
   * @param {string} text - text or "enter a alphanumeric value" for a random generated text
   */
  async enterNotesForSEOperationsTeamTextBox(text) {
    if (text && text.trim().length > 0) {
      await this.enterGenericTextBox(this.notesForSEOperationsTeamTxtBox, "notesForSEOpsTeam", text);
    }
  }
}
