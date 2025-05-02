import { BasePage } from "../shared/BasePage";

export class BatteryStoragePage extends BasePage {
  constructor(page, poManager) {
    super(page); // Extending BasePage
    this.page = page;
    this.poManager = poManager;
    // Direct access from POManager
    this.projectLibrary = this.poManager.getPage("projectLibrary");
    this.newProjectTypes = this.poManager.getProjectPage("New Project Types");
    this.newProjectAssocTech = this.poManager.getProjectPage("New Project Associated Technologies");
    this.newProjectRegions = this.poManager.getProjectPage("New Project Regions");
    this.newProjectDetails = this.poManager.getProjectPage("New Project Details");
    this.newProjectDescription = this.poManager.getProjectPage("New Project Description");
  }

  async createProject(projectData, role) {
    await this.verifyVisibilityAndClick(this.projectLibraryBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    await this.verifyVisibilityAndClick(this.projectLibrary.addProjectBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    // Select Project Type
    await this.newProjectTypes.clickOnProjectTypeByName(projectData.projectType);
    await this.verifyVisibilityEnabledAndClick(this.newProjectTypes.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    // Select Technologies
    await this.newProjectAssocTech.clickOnTechnologyByName(projectData.technology);
    await this.verifyVisibilityEnabledAndClick(this.newProjectAssocTech.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    // Select Regions and Countries
    await this.newProjectRegions.selectRegionAndCountriesCheckboxes(projectData.regions);
    await this.verifyVisibilityEnabledAndClick(this.newProjectRegions.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    // Project Details Page
    // Fill in Minimum Annual Peak
    await this.newProjectDetails.enterMinimumAnnualPeak(projectData.minimumAnnualPeak);
    //Select Contract Structures
    await this.newProjectDetails.selectContractStructuresCheckboxes(projectData.contractStructures);
    // Fill in Minimum Term Length Available (optional)
    await this.newProjectDetails.enterMinimumTermLength(projectData.minimumTermLength);
    // await this.page.waitForTimeout(500);
    // Select Value Provided
    await this.newProjectDetails.selectValueProvidedCheckboxes(projectData.valueProvided);
    // Fill in Time Urgency Considerations (optional)
    await this.newProjectDetails.enterTimeUrgencyConsiderations(projectData.timeUrgencyConsiderations);
    // await this.page.waitForTimeout(500);
    // Fill in Additional Comments (optional)
    await this.newProjectDetails.enterAdditionalComments(projectData.additionalComments);
    // await this.page.waitForTimeout(500);
    await this.verifyVisibilityEnabledAndClick(this.newProjectDetails.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    // Project Description Page
    // Fill in "Published By" (required) - search box field is exclusive to Admin and System Owner
    // if (["admin", "sysowner"].includes(role.toLowerCase())) {
    //   await this.createNewProjectDescription.selectPublishedByDropDownItem(); // take no parameters, instead defaults to name of login user
    // }
    // Store project title and Fill in Project Title field
    let { text: projectTitle } = await this.newProjectDescription.enterProjectTitleTextBox(projectData.projectTitle);
    projectData.projectTitle = projectTitle;
    // Fill in Project Subtitle
    await this.newProjectDescription.enterProjectSubTitleTextBox(projectData.subTitle);
    // Fill in Describe Opportunity
    await this.newProjectDescription.enterDescribeTheOpportunityTextBox(projectData.describeOpportunity);
    // Fill in About The Provider
    await this.newProjectDescription.enterAboutTheProviderTextBox(projectData.aboutTheProvider);
    // Perform last action by clicking on Cancel/Save as draft/ Publish
    const projectId = await this.newProjectDescription.performAction(projectData.action, projectTitle);
    // Search and verify Project Created/Cancelled/Saved as draft
    if (projectData.action === "Cancel") {
      await this.projectLibrary.searchAndVerifyProjectNotFound(projectTitle, this.projectLibraryBtn);
    } else {
      await this.projectLibrary.searchAndVerifyProjectFound(projectTitle, this.projectLibraryBtn);
    }
    return projectId;
  }
}
