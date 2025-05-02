import { expect } from "@playwright/test";
import { BasePage } from "../shared/BasePage.js";

export class InternalUserPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page;
  }
}
