import { BasePage } from "../shared/BasePage.js";

export class CreateNewProjectTypesPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page;
    // Add Project -> project types
    const projectTypes = [
      "Aggregated PPAs",
      "Battery Storage",
      "Carbon Offset Purchasing",
      "Community Solar",
      "EAC Purchasing",
      "Efficiency Audits & Consulting",
      "Efficiency Equipment Measures",
      "Emerging Technologies",
      "EV Charging & Fleet Electrification",
      "Fuel Cells",
      "Offsite Power Purchase Agreement",
      "Onsite Solar",
      "Renewable Retail Electricity",
      "Utility Green Tariff",
    ];
    this.projectTypeButtons = {};
    for (const type of projectTypes) {
      this.projectTypeButtons[type.toLowerCase()] = page.getByText(type, { exact: true });
    }
    // Add Project -> naviation buttons
    this.cancelBtn = page.getByRole("button", { name: "Cancel" });
    this.nextBtn = page.getByRole("button", { name: "Next" });
  }

  /**
   * Click on the specific "Project Type" button by name
   * @param {string} projectTypeName - project type name (not case sensitive)
   */
  async clickOnProjectTypeByName(projectTypeName) {
    projectTypeName = projectTypeName && typeof projectTypeName === "string" ? projectTypeName.trim().toLowerCase() : "";
    //confirm name - select or click
    if (!this.projectTypeButtons[projectTypeName]) {
      throw new Error(`Project type "${projectTypeName}" is not defined in project type buttons.`);
    }
    // wait for the project types container height to be expanded
    await this.page
      .locator("div[class^=content]")
      .locator("div[class^=controls-wrapper]")
      .evaluate((node) => node.clientHeight > 30);
    await this.verifyVisibilityAndClick(this.projectTypeButtons[projectTypeName]);
    await this.page.waitForTimeout(500);
  }

  /**
   * From Create New Project Types page, click on footer button by name
   * @param {string | undefined} buttonName - name of the button
   */
  async clickOnFooterButtonByName(buttonName) {
    buttonName = buttonName && typeof buttonName === "string" ? buttonName.trim().toLowerCase() : "";
    if (buttonName.length === 0) {
      throw new Error(`Create New Project Type footer navigation button name is not defined\n
        Expected: either "cancel" or "next", but received ${buttonName}`);
    }
    if (buttonName === "next") {
      await this.verifyVisibilityAndClick(this.nextBtn);
    } else if (buttonName === "cancel") {
      await this.verifyVisibilityAndClick(this.cancelBtn);
    }
    await this.page.waitForTimeout(1000);
  }
}
