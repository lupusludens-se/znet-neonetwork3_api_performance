import { BasePage } from "../shared/BasePage.js";
import { expect } from "@playwright/test";

export class ProjectPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page;
  }
}
