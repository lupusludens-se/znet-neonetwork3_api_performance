import { expect } from "@playwright/test";
import { BasePage } from "../shared/BasePage.js";

export class SPAdminPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page; // this belongs to current class
    this.manageLMBtn = page.getByRole("link", { name: "Manage" });
    this.viewMyuserBtn = page.getByRole("button", { name: "View My Users" });
    this.editCompanyBtn = page.getByRole("button", { name: "Edit Company Profile" });
    this.viewallPM = page.locator("//div[3]/button[@class='btn-m green-frame flex-center']");
    this.viewallMM = page.locator("//div[4]/button[@class='btn-m green-frame flex-center']");
    this.projrctMangment = page.locator("//h4[.='Project Management']");
    this.messageManagement = page.locator("//h4[.='Messages Management']");
    this.companyManagement = page.locator("//h4[.='Company Management']");
    this.userManagement = page.locator("//h4[.='User Management']");
    this.dasboardLMBtn = page.getByRole("link", { name: "Dashboard" });
    this.projectLibraryLMBtn = page.getByRole("link", { name: " Project Library " });
    this.learnLMBtn = page.getByRole("link", { name: "Learn" });
    this.eventLMBtn = page.getByRole("link", { name: "Events" });
    this.toolLMBtn = page.getByRole("link", { name: "Tools" });
    this.communityLMBtn = page.getByRole("link", { name: "Community" });
    this.forumLMBtn = page.getByRole("link", { name: "Forum" });
    this.messageLMBtn = page.getByRole("link", { name: "Messages" });
    this.addProjectBtn = page.getByRole("button", { name: "Add Project" });
    this.exportProjectBtn = page.getByRole("button", { name: "Export Projects" });
  }

  async clickProjectLibraryLeftMarginBtn() {
    await this.verifyVisibilityAndClick(this.projectLibraryLMBtn);
  }

  async verifyingSPadminaccess(username, password) {
    await expect(this.manageLMBtn).toContainText("Manage");
    // console.log(await expect(this.manageLMBtn).toBeVisible('Manage'))
    this.manageLMBtn.click();
  }
}
