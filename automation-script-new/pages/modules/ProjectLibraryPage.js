import { BasePage } from "../shared/BasePage.js";
import { expect } from "@playwright/test";
import { faker } from "@faker-js/faker";

export class ProjectLibraryPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page;
    this.addProjectBtn = page.getByRole("button", { name: " Add Project " }).first();
    this.exportProjectsBtn = page.getByRole("button", {
      name: " Export Projects ",
    });
    this.resultsTbl = page.locator("table>tbody"); // traverse each row using .locator("neo-projects-table-row:nth-of-type(1), {hasText: 'table cell text'}")
    this.resultsTblOptListBox = page.locator("ul[class='p-absolute ng-star-inserted'] li");
    this.areYouSureOverLayBox = page.locator("neo-modal div").first();
    this.threeDotsBtn = page.getByRole("table").getByRole("button");
    this.threeDotsDeleteOpt = page.getByText("Delete");
    this.yesDeleteOpt = page.getByRole("button", { name: "Yes, Delete" });
  }

  /**
   * Find the text in the Table Results, and click on the "..." icon in that row and click on a specific menu item
   * @param {string} text - text located in any table cell
   * @param {string} itemName - menu item name ["Preview", "Edit", "Duplicate", "Deactivate", "Activate", "Delete"]
   * @param {string} areYouSureButtonName - Are You Sure Popup confirmation button name ["Cancel", "Yes, save as draft", "Yes, Delete"]
   */
  async clickTableRowOptionItem(text, itemName, areYouSureButtonName) {
    await this.waitForAndVerifyVisibility(this.resultsTbl);
    const element = await this.resultsTbl.evaluateHandle((x) => x.childElementCount);
    const numRows = element._preview;
    const findText = text.toLowerCase();
    // find the text in every row and every column
    for (const rowNumber in numRows) {
      const titleColumnText = (await this.resultsTbl.locator(`neo-projects-table-row:nth-of-type(${rowNumber + 1}) td:nth-of-type(1)`).innerText()).trim().toLowerCase();
      const typeColumnText = (await this.resultsTbl.locator(`neo-projects-table-row:nth-of-type(${rowNumber + 1}) td:nth-of-type(2)`).innerText()).trim().toLowerCase();
      const locationColumnText = (await this.resultsTbl.locator(`neo-projects-table-row:nth-of-type(${rowNumber + 1}) td:nth-of-type(3)`).innerText())
        .trim()
        .toLowerCase();
      const modifiedColumnText = (await this.resultsTbl.locator(`neo-projects-table-row:nth-of-type(${rowNumber + 1}) td:nth-of-type(4)`).innerText())
        .trim()
        .toLowerCase();
      const publishedByColumnText = (await this.resultsTbl.locator(`neo-projects-table-row:nth-of-type(${rowNumber + 1}) td:nth-of-type(5)`).innerText())
        .trim()
        .toLowerCase();

      if (
        findText.includes(titleColumnText) ||
        findText.includes(typeColumnText) ||
        findText.includes(locationColumnText) ||
        findText.includes(modifiedColumnText) ||
        findText.includes(publishedByColumnText)
      ) {
        // Within the row that contains the text, click on the "..." icon
        await this.verifyVisibilityAndClick(this.resultsTbl.locator(`neo-projects-table-row:nth-of-type(${rowNumber + 1}) button`));
        await this.verifyVisibilityAndClick(this.resultsTblOptListBox.getByText(itemName));
        // only "Duplicate" and "Delete" menu items will reveal a confirmation overlay
        if (["Duplicate", "Delete"].includes(itemName)) {
          await this.verifyVisibilityAndClick(
            this.areYouSureOverLayBox.getByRole("button", {
              name: areYouSureButtonName,
            })
          );
          await this.page.waitForTimeout(880);
        }
        break;
      }
    }
  }

  async verifyTableContainsTitle(text) {
    await this.page.getByText("Loading...Loading, please").waitFor({ state: "detached" });
    const element = await this.resultsTbl.evaluateHandle((x) => x.childElementCount);
    const numRows = element._preview;
    let found = false;
    for (const rowNumber in numRows) {
      const elementInnerText = await this.resultsTbl.locator(`neo-projects-table-row:nth-of-type(${rowNumber + 1}) td:nth-of-type(1)`).innerText();
      if (text.toLowerCase().includes(elementInnerText.trim().toLowerCase())) {
        found = true;
        break;
      }
    }
    expect(found === true, `Expected Project Library Page -> Results Table Title column to contain the text, ${text}`).toBeTruthy();
  }

  async clickOnAddProjectBtn() {
    await this.verifyVisibilityAndClick(this.addProjectBtn);
  }

  async deleteRandomProjectInTable() {
    await this.page.waitForTimeout(1000);
    // Get all project rows
    const projectRows = this.page.locator("neo-projects-table-row");
    const rowCount = await projectRows.count();
    if (rowCount === 0) {
      throw new Error("No projects available to delete.");
    }
    // Pick random row
    const safeMax = Math.min(rowCount - 1, 9); // Limit random index
    const randomIndex = faker.number.int({ min: 0, max: safeMax });
    const randomProjectRow = projectRows.nth(randomIndex);
    // Extract text of project title
    const projectTitleElement = randomProjectRow.locator("span.title-col.ellipsis.draftTitle");
    await projectTitleElement.waitFor({ state: "visible", timeout: 5000 });
    const projectTitle = (await projectTitleElement.innerText()).trim();
    // Locate the three dots for this project
    const threeDotsBtn = randomProjectRow.locator('[class*="three-dots"]');
    // Click three dots
    await this.verifyVisibilityAndClick(threeDotsBtn);
    // Click 'Delete'
    await this.verifyVisibilityAndClick(this.threeDotsDeleteOpt);
    await this.verifyVisibilityAndClick(this.yesDeleteOpt);
    await this.page.waitForTimeout(3000);
    await this.searchAndVerifyProjectNotFound(projectTitle, this.projectLibraryBtn);
    return projectTitle;
  }
}
