import { BasePage } from "../shared/BasePage.js";

export class CreateNewProjectAssocTechPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page;
    // Add Project -> Select Associated Technologies Apply All
    this.applyAllBtnBtn = page.getByRole("button", { name: "Apply All" });
    // Add Project -> Select Associated Technologies
    const technologyNames = [
      "Groundmount Solar",
      "Offshore Wind",
      "Onshore Wind",
      "Battery Storage",
      "Rooftop Solar",
      "Carport Solar",
      "Building Controls",
      "Building Envelope",
      "HVAC",
      "Lighting",
      "Green Hydrogen",
      "Renewable Thermal",
      "Emerging Technology",
      "EV Charging",
      "Fuel Cell",
      "Apply All",
    ];
    this.technologyButtons = {};
    for (const name of technologyNames) {
      this.technologyButtons[name.toLowerCase()] = page.getByRole("button", {
        name,
        exact: true,
      });
    }
    // Navigation buttons
    this.backBtn = page.getByRole("button", { name: "Back" });
    this.cancelBtn = page.getByRole("button", { name: "Cancel" });
    this.nextBtn = page.getByRole("button", { name: "Next" });
  }

  /**
   * Click on one or multiple Associated Technologies
   * @param {string | Array<string>} technologyNames
   */
  async clickOnTechnologyByName(technologyNames) {
    let technologies = [];
    // Normalize input to an array of trimmed lowercase values
    if (typeof technologyNames === "string") {
      if (technologyNames.match(/(select|apply)\s+all/i)) {
        this.verifyVisibilityAndClick(this.applyAllBtnBtn);
        await this.page.waitForTimeout(500);
        technologies = [];
      } else {
        technologies = [technologyNames.trim().toLowerCase()];
      }
    } else if (Array.isArray(technologyNames)) {
      technologies = technologyNames.map((t) => t.trim().toLowerCase());
    }
    // If "=All" is specified, override with all keys
    if (technologies.includes("=all")) {
      technologies = Object.keys(this.technologyButtons);
    }
    for (const tech of technologies) {
      const techLocator = this.technologyButtons[tech];
      if (!techLocator) {
        throw new Error(`Technology "${tech}" is not defined in technology buttons.`);
      }
      // Skip if locator is not visible within short timeout
      const isVisible = await techLocator.isVisible({ timeout: 2000 }).catch(() => false);
      if (!isVisible) {
        continue;
      }
      await this.verifyVisibilityAndClick(techLocator);
      await this.page.waitForTimeout(500);
    }
  }
}
