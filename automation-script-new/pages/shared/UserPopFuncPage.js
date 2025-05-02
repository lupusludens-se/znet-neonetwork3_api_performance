import { expect } from "@playwright/test";
import { BasePage } from "./BasePage.js";
import fs from "fs";
import path from "path";
import { parse } from "csv-parse/sync";

//Searching user and deleting page methods
export class UserPopFuncPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page; // this belongs to current class
    this.viewUserBtn = page.getByRole("button", { name: "View Users" });
    this.searchInputBx = page.getByPlaceholder("Search");
    this.threeDotBtn = page.getByRole("table").getByRole("button");
    this.threeDotDelete = page.getByText("Delete");
    this.yesDeleteOpt = page.getByRole("button", { name: "Yes, Delete" });
    this.cancelDeleteOpt = page.getByRole("button", { name: "Cancel" });
    this.noResultFor = page.locator("//div/text()[1]");
  }

  async clickViewUserBtn() {
    await this.viewUserBtn.click();
    console.log("clicked on View User Button");
  }

  async clickSearchUser() {
    await this.searchInputBx.click();
  }

  async popUser() {
    const records = parse(
      fs.readFileSync(path.join(__dirname, "../miscellaneous/sampleCSV.csv")),
      {
        columns: true,
        skip_empty_lines: true,
      }
    );
    for (const record of records) {
      await this.searchInputBx.clear();
      await this.page.waitForTimeout(1000);
      await this.searchInputBx.type(record["Email"]);
      await this.page.waitForTimeout(2000);
      await this.threeDotBtn.first().click();
      await this.threeDotDelete.click();
      // await this.cancelDeleteOpt.click()
      await this.yesDeleteOpt.click();
      await this.page.waitForTimeout(2600);
      // console.log('Verify deleted users')
      // await this.searchInputBx.clear()
      // await this.page.waitForTimeout(2000)
      // await this.searchInputBx.type(record['Email'])
      // //await expect(this.threeDotBtn).toBeHidden()
      await this.page.waitForTimeout(2000);
    }
  }
}
