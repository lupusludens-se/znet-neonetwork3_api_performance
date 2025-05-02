import { BasePage } from "../shared/BasePage";

export class OffsitePowerPurchaseAgreementPage extends BasePage {
  constructor(page, poManager) {
    super(page);
    this.page = page;
    this.poManager = poManager;
    // Direct access from POManager
    this.projectLibrary = this.poManager.getPage("projectLibrary");
    this.newProjectTypes = this.poManager.getProjectPage("New Project Types");
    this.newProjectAssocTech = this.poManager.getProjectPage("New Project Associated Technologies");
    this.newProjectRegions = this.poManager.getProjectPage("New Project Regions");
    this.newProjectDetailPublic = this.poManager.getProjectPage("Aggregated PPAs Public Details");
    this.newProjectDetailPrivate = this.poManager.getProjectPage("Aggregated PPAs Private Details");
    this.newProjectDescription = this.poManager.getProjectPage("New Project Description");
  }

  // scenario based on sequential steps
  async createProject(projectData, role) {
    // Left Margin Project Libray button
    await this.verifyVisibilityAndClick(this.projectLibraryBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    // Add Project button
    await this.verifyVisibilityAndClick(this.projectLibrary.addProjectBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    // Select Project Type
    await this.newProjectTypes.clickOnProjectTypeByName(projectData.projectType);
    await this.verifyVisibilityAndClick(this.newProjectTypes.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    // Select Technologies
    await this.newProjectAssocTech.clickOnTechnologyByName(projectData.technology);
    await this.verifyVisibilityAndClick(this.newProjectTypes.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    // Select Regions and Countries
    await this.newProjectRegions.selectRegionAndCountriesCheckboxes(projectData.regions);

    await this.verifyVisibilityAndClick(this.newProjectTypes.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });

    // Project Details - Public Page

    // search map textbox, and search list results
    await this.newProjectDetailPublic.searchMapForValue(projectData.mapLocation);
    // click on ISO/RTO drop down, and select item
    await this.newProjectDetailPublic.selectIsoRtoDropDownItem(projectData.isoRto);
    // click on Product Type drop down, and select item
    await this.newProjectDetailPublic.selectProductTypeDropDownItem(projectData.productType);
    // Commercial Operation Calendar date in format "yyyy-MM-dd"
    await this.newProjectDetailPublic.enterCalenderDateTextBox(projectData.calendarDate);

    // select Value to Offtaker checkboxes
    await this.newProjectDetailPublic.selectValueToOfftakerCheckBox(projectData.valueToOfftaker);

    // PPA Term Length (required)
    await this.newProjectDetailPublic.enterPPATermLengthTextBox(projectData.ppaTermLength);
    // Total Project Nameplate Capacity (required)
    await this.newProjectDetailPublic.enterTotalProjectNameplateCapacityTextBox(projectData.totalProjectNameplateCapacity);
    // Total Project Expected Annual Production - P50 (required)
    await this.newProjectDetailPublic.enterTotalProjectExpectedAnnualProductionP50TextBox(projectData.totalProjectExpectedAnnualProdP50);
    // Minimum Offtake Volume Required (required)
    await this.newProjectDetailPublic.enterMinimumOfftakeVolumeRequiredTextBox(projectData.minOfftakeVolumeRequired);
    // Notes for Potential Offtakers (required)
    await this.newProjectDetailPublic.enterNotesForPotentialOfftakersTextBox(projectData.notesForPotentialOfftakers);
    // click either Back, Cancel, Save As Draft, or Next button
    await this.verifyVisibilityEnabledAndClick(this.newProjectDetailPublic.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });

    // Project Details - Private Page

    // Settlement Type (optional)
    await this.newProjectDetailPrivate.selectSettlementTypeDropDownItem(projectData.settlementType);

    if (
      Object.hasOwn(projectData, "settlementType") &&
      projectData.settlementType &&
      typeof projectData.settlementType === "string" &&
      projectData.settlementType.trim().toLowerCase().length > 0 &&
      (projectData.settlementType.trim().toLowerCase() === "hub" || projectData.settlementType.trim().toLowerCase() === "loadzone")
    ) {
      // Settlement Hub / Load Zone (optional)
      await this.newProjectDetailPrivate.selectSettlementHubLoadZoneDropDownItem(projectData.settlementHub);
    }
    // Currency for all Price Entries (required)
    await this.newProjectDetailPrivate.selectCurrencyForAllPriceEntriesDropDownItem(projectData.currencyForAllPriceEntries);
    // Pricing Structure (required)
    await this.newProjectDetailPrivate.selectPricingStructureDropDownItem(projectData.pricingStructure);
    // EAC Type drop down (required)
    await this.newProjectDetailPrivate.selectEACTypeDropDownItem(projectData.eacType);
    // Settlement Price Interval (required)
    await this.newProjectDetailPrivate.selectSettlementPriceIntervalDropDownItem(projectData.settlementPriceInterval);
    // Settlement Calculation Interval (required)
    await this.newProjectDetailPrivate.selectSettlementCalcIntervalDropDownItem(projectData.settlementCalculationInterval);

    // Contract Price (required)
    await this.newProjectDetailPrivate.enterContractPriceTextBox(projectData.contractPrice);
    // Floating Market Swap (Index, Discount) (optional)
    await this.newProjectDetailPrivate.enterFloatingMarketSwapIndexTextBox(projectData.floatIndex);
    // Floating Market Swap (Floor) (optional)
    await this.newProjectDetailPrivate.enterFloatingMarketSwapFloorTextBox(projectData.floatFloor);
    // Floating Market Swap (Cap) (optional)
    await this.newProjectDetailPrivate.enterFloatingMarketSwapCapTextBox(projectData.floatCap);

    // selecting a specific dropdown item will reveal new hidden textboxes
    if (projectData.pricingStructure.trim().toLowerCase().includes("upside share")) {
      // once a drop down item is selected, these become (required) fields
      await this.newProjectDetailPrivate.enterUpsidePercentageToDeveloperTextBox(projectData.upsidePercentageToDeveloper);
      await this.newProjectDetailPrivate.enterUpsidePercentageToOfftakerTextBox(projectData.upsidePercentageToOfftaker);
    } else if (projectData.pricingStructure.trim().toLowerCase().includes("fixed discount to market")) {
      // once a drop down item is selected, this becomes a (required) field
      await this.newProjectDetailPrivate.enterDiscountAmountTextBox(projectData.discountAmount);
    }

    // selecting specific dropdown item will reveal new hidden textbox
    if (projectData.settlementPriceInterval.trim().toLowerCase() === "other") {
      // Custom Settlement Price Interval (required)
      await this.newProjectDetailPrivate.enterCustomSettlementPriceIntervalTextBox(projectData.customSettlementPriceInterval);
    }

    if (projectData.eacType.trim().toLowerCase() === "other") {
      // Custom EAC Type (required)
      await this.newProjectDetailPrivate.enterCustomEacTypeTextBox(projectData.customEacType);
    }
    // EAC Value (optional)
    await this.newProjectDetailPrivate.enterEacValueTextBox(projectData.eacValue);

    // Project MW Currently Available (required)
    await this.newProjectDetailPrivate.enterProjectMWCurrentlyAvailTextBox(projectData.projectMWCurrentlyAvailable);
    // Notes for SE Operations Team (optional)
    await this.newProjectDetailPrivate.enterNotesForSEOperationsTeamTextBox(projectData.notesForSEOpsTeam);
    // click Next button
    await this.verifyVisibilityEnabledAndClick(this.newProjectDetailPrivate.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    // Project Description Page

    // Fill in "Published By" (required) - search box field is exclusive to Admin and System Owner
    // if (["admin", "sysowner"].includes(role.toLowerCase())) {
    //   await this.newProjectDescription.selectPublishedByDropDownItem(); // take no parameters, instead defaults to name of login user
    // }
    // Project Title (required)
    const textObj = await this.newProjectDescription.enterProjectTitleTextBox(projectData.projectTitle);
    // Sub-Title (required)
    await this.newProjectDescription.enterProjectSubTitleTextBox(textObj.isGeneratedText ? textObj.text : projectData.subTitle);
    // Describe the Opportunity (required)
    await this.newProjectDescription.enterDescribeTheOpportunityTextBox(projectData.describeOpportunity);
    // About the Provider (optional)
    await this.newProjectDescription.enterAboutTheProviderTextBox(projectData.aboutTheProvider);

    // Click Save As Draft
    projectData.projectTitle = textObj.isGeneratedText ? textObj.text : "TC-" + projectData.testCaseId + "-SubTitle";
    const projectId = await this.newProjectDescription.performAction(projectData.action, projectData.projectTitle);

    // Check for Project Title within Project Library
    if (projectData.action.trim().toLowerCase() === "cancel") {
      await this.projectLibrary.searchAndVerifyProjectNotFound(projectData.projectTitle, this.projectLibraryBtn);
    } else {
      await this.projectLibrary.searchAndVerifyProjectFound(projectData.projectTitle, this.projectLibraryBtn);
    }
    return projectId;
  }
}
