import { BasePage } from "../shared/BasePage";

export class UtilityGreenTariffPage extends BasePage {
  constructor(page, poManager) {
    super(page);
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

    // Project Type Page

    // Select "Project Type" (required)
    await this.newProjectTypes.clickOnProjectTypeByName(projectData.projectType);
    await this.verifyVisibilityAndClick(this.newProjectTypes.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });

    // Technologies Page

    // Select Technologies
    await this.newProjectAssocTech.clickOnTechnologyByName(projectData.technology);
    await this.verifyVisibilityAndClick(this.newProjectAssocTech.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });

    // Project Geography Page

    // Select "Regions and Countries" (required)
    await this.newProjectRegions.selectRegionAndCountriesCheckboxes(projectData.regions);
    await this.verifyVisibilityAndClick(this.newProjectRegions.nextBtn);
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });

    // Project Details Page

    // Fill in "Utility Name" (required)
    await this.newProjectDetails.enterUtilityNameTextBox(projectData.utilityName);
    // Fill in "Program Website" (required)
    await this.newProjectDetails.enterProgramWebsiteTextBox(projectData.progamWebsite);
    // Fill in "Minimum Purchase Volume Available (1 EAC = 1 MWh)" (required)
    await this.newProjectDetails.enterMinimumPurchaseVolume(projectData.minimumPurchaseVolume);
    // Select "Term Length" (required)
    await this.newProjectDetails.selectTermLengthOptionBox(projectData.termLengthOption);
    // Select "Value Provided" (required)
    await this.newProjectDetails.selectValueProvidedCheckboxes(projectData.valueProvided);
    // Fill in "Time Urgency Considerations" (optional)
    await this.newProjectDetails.enterTimeUrgencyConsiderations(projectData.timeUrgencyConsiderations);
    // Fill in "Additional Comments" (optional)
    await this.newProjectDetails.enterAdditionalComments(projectData.additionalComments);
    await this.waitForAndVerifyVisibility(this.newProjectDetails.nextBtn);
    await this.newProjectDetails.nextBtn.click();
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });

    // Project Description Page

    // Fill in "Published By" (required) - search box field is exclusive to Admin and System Owner
    // if (["admin", "sysowner"].includes(role.toLowerCase())) {
    //   await this.createNewProjectDescription.selectPublishedByDropDownItem(); // take no parameters, instead defaults to name of login user
    // }
    // Store project title and Fill in "Project Title" field (required)
    let { text: projectTitle } = await this.newProjectDescription.enterProjectTitleTextBox(projectData.projectTitle);
    projectData.projectTitle = projectTitle;
    // Fill in Project "Subtitle" (required)
    await this.newProjectDescription.enterProjectSubTitleTextBox(projectData.subTitle);
    // Fill in "Describe Opportunity" (required)
    await this.newProjectDescription.enterDescribeTheOpportunityTextBox(projectData.describeOpportunity);
    // Fill in "About The Provider" (optional)
    await this.newProjectDescription.enterAboutTheProviderTextBox(projectData.aboutTheProvider);
    //Perform last action by clicking on Cancel/Save as draft/ Publish
    const projectId = await this.newProjectDescription.performAction(projectData.action, projectTitle);
    //Searh and verify Project Created/Cancelled/Saved as draft
    if (projectData.action === "Cancel") {
      await this.projectLibrary.searchAndVerifyProjectNotFound(projectTitle, this.projectLibraryBtn);
    } else {
      await this.projectLibrary.searchAndVerifyProjectFound(projectTitle, this.projectLibraryBtn);
    }
    return projectId;
  }
}
