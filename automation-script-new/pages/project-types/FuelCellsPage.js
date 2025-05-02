import { BasePage } from "../shared/BasePage";

export class FuelCellsPage extends BasePage {
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
    // Click left-margin Project Library button
    await this.verifyVisibilityAndClick(this.projectLibraryBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    // Click "Add Project" button
    await this.verifyVisibilityAndClick(this.projectLibrary.addProjectBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });

    // Project Type Page

    // Select Project Type (required)
    await this.newProjectTypes.clickOnProjectTypeByName(projectData.projectType);
    await this.verifyVisibilityAndClick(this.newProjectTypes.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });

    // Technology Page

    // Select Technologies (required)
    await this.newProjectAssocTech.clickOnTechnologyByName(projectData.technology);
    await this.verifyVisibilityAndClick(this.newProjectAssocTech.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });

    // Project Geography Page

    // Select Regions and Countries (required)
    await this.newProjectRegions.selectRegionAndCountriesCheckboxes(projectData.regions);
    await this.verifyVisibilityAndClick(this.newProjectRegions.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });

    // Project Details Page

    // Fill in Minimum Annual Peak (required)
    await this.newProjectDetails.enterMinimumAnnualSiteTextBox(projectData.minimumAnnualSite);
    // Select Contract Structures (required)
    await this.newProjectDetails.selectContractStructuresCheckboxes(projectData.contractStructures);
    // Fill in Minimum Term Length Available (optional)
    await this.newProjectDetails.enterMinimumTermLength(projectData.minimumTermLength);
    // Select Value Provided (required)
    await this.newProjectDetails.selectValueProvidedCheckboxes(projectData.valueProvided);
    // Fill in Time Urgency Considerations (optional)
    await this.newProjectDetails.enterTimeUrgencyConsiderations(projectData.timeUrgencyConsiderations);
    // Fill in Additional Comments (optional)
    await this.newProjectDetails.enterAdditionalComments(projectData.additionalComments);
    await this.waitForAndVerifyVisibility(this.newProjectDetails.nextBtn);
    await this.newProjectDetails.nextBtn.click();
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });

    // Project Description Page

    // Fill in "Published By" (required) - search box field is exclusive to Admin and System Owner
    // if (["admin", "sysowner"].includes(role.toLowerCase())) {
    //   await this.newProjectDescription.selectPublishedByDropDownItem(); // take no parameters, instead defaults to name of login user
    // }
    // Store project title and Fill in Project Title field
    let { text: projectTitle } = await this.newProjectDescription.enterProjectTitleTextBox(projectData.projectTitle);
    projectData.projectTitle = projectTitle;
    // Fill in Project Subtitle
    await this.newProjectDescription.enterProjectSubTitleTextBox(projectData.subTitle);
    // Fill in Describe Opportunity
    await this.newProjectDescription.enterDescribeTheOpportunityTextBox(projectData.describeOpportunity);
    await this.page.click("body");
    // Fill in About The Provider
    await this.newProjectDescription.enterAboutTheProviderTextBox(projectData.aboutTheProvider);
    //Perform last action by clicking on Cancel, "Save As Draft", or Publish button
    const projectId = await this.newProjectDescription.performAction(projectData.action, projectTitle);
    //Searh and verify Project Created using either Cancelled or "Saved As Draft"
    if (projectData.action === "Cancel") {
      await this.projectLibrary.searchAndVerifyProjectNotFound(projectTitle, this.projectLibraryBtn);
    } else {
      await this.projectLibrary.searchAndVerifyProjectFound(projectTitle, this.projectLibraryBtn);
    }
    return projectId;
  }
}
