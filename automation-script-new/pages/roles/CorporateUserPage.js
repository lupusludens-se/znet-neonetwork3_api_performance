import { expect } from "@playwright/test";
import { BasePage } from "../shared/BasePage.js";

export class CorporateUserPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page; // this belongs to current class
    this.dasboardLMBtn = page.getByRole("link", { name: "Dashboard" });
    this.projectLMbtn = page.getByRole("link", { name: "Projects" });
    this.learnLMBtn = page.getByRole("link", { name: "Learn" });
    this.eventLMBtn = page.getByRole("link", { name: "Events" });
    this.toolLMBtn = page.getByRole("link", { name: "Tools" });
    this.communityLMBtn = page.getByRole("link", { name: "Community" });
    this.forumLMBtn = page.getByRole("link", { name: "Forum" });
    this.messageLMBtn = page.getByRole("link", { name: "Messages" });
  }

  async verifyingCorporateUser() {
    await expect(this.projectLMbtn).toBeEnabled();
    await this.projectLMbtn.click();
    await this.page.waitForTimeout(800);
  }
}
