import { BasePage } from "../shared/BasePage.js";
import resources from "../../utils/CommonTestResources.js";

export class AddForumPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page;
    this.timeout = 500;
    this.initializeSelectors();
  }

  initializeSelectors() {
    this.forumBtn = this.page.getByRole("link", { name: "  Forum  " });
    this.addStartDiscussionBtn = this.page.getByRole("button", { name: " Start a Discussion  " });
    this.startTypingDiscussion = this.page.getByPlaceholder("Start typing your discussion title to search...");
    this.createNewDiscussion = this.page.getByRole("button", { name: "  Create New Discussion  " });
    this.discusionInput = this.page.locator('//div[@id="editor"]');

    this.forumTopics = {
      AggtPPA: this.page.getByText("Aggregated PPAs"),
      BattryStrge: this.page.getByText(" Battery Storage"),
      CarbonOffset: this.page.getByText("Carbon Offset Purchasing"),
      CommunitySolar: this.page.getByText("Community Solar", { exact: true }),
      Decarbonization: this.page.getByText("Decarbonization"),
      EACPurchasing: this.page.getByText(" EAC Purchasing"),
      EfficiencyAuditConsulting: this.page.locator("//div[4]/div[2]/neo-select-item[7]"),
      EfficiencyEquipmentMeasures: this.page.getByText("Efficiency Equipment Measures"),
      EmergingTechnology: this.page.getByText(" Emerging Technologies"),
      EnergyNews: this.page.getByText("Energy News"),
      EVChargingFleetElectrification: this.page.locator("neo-select-item").filter({ hasText: "EV Charging & Fleet" }).locator("svg"),
      FuelCell: this.page.getByText("Fuel Cells"),
      NewCommunitySolar: this.page.getByText("New Community Solar"),
      OffsitePPA: this.page.getByText(" Offsite Power Purchase Agreement"),
      OnsiteSolar: this.page.getByText("Onsite Solar"),
      ReNewableRetailElectricity: this.page.getByText("Renewable Retail Electricity"),
      ResponsibleReNewables: this.page.getByText("Responsible Renewables"),
      UtilityGreenTariff: this.page.getByText(" Utility Green Tariff"),
    };

    this.locationBtns = {
      No: this.page.getByRole("button", { name: "No" }),
      Yes: this.page.getByText("Yes"),
      Africa: this.page.getByText("Africa"),
      Asia: this.page.getByText("Asia"),
      Europe: this.page.getByText("Europe"),
      Mexico: this.page.getByText(" Mexico & Central America"),
      Oceania: this.page.getByText("Oceania"),
      SouthAmerica: this.page.getByText("South America"),
      USAandCanada: this.page.getByText("USA & Canada"),
    };

    this.forumPostDiscussionBtn = this.page.getByRole("button", { name: " Post Discussion " });
    this.forumBackBtn = this.page.getByRole("button", { name: " Back " });
    this.searchForum = this.page.getByPlaceholder("Search");
    this.deleteBtn = this.page.locator("neo-forum-topic:nth-of-type(1) .remove-item-icon > [width='100%']");
    this.deleteConfirmBtn = this.page.getByRole("button", { name: "Yes, Delete " });
    this.privateMembersSearch = this.page.getByPlaceholder("Search Users");
    this.selectPrivateMember = this.page.getByText("corporate Automation Sunny,");
    this.pinForum = this.page.getByRole("heading", { name: "Pin the discussion" });
  }

  async enterForumName(nameLength) {
    await this.startTypingDiscussion.click();
    await this.page.waitForTimeout(this.timeout);
    let generatedForumName = this.generateRandomForumName(nameLength);
    await this.startTypingDiscussion.fill("");
    await this.startTypingDiscussion.type(generatedForumName);
    this.generatedForumName = generatedForumName;
    await this.page.waitForTimeout(this.timeout * 2);
    return this.generatedForumName;
  }

  async clickOnForumModule() {
    await this.waitForAndVerifyVisibility(this.forumBtn);
    await this.forumBtn.click();
    await this.page.waitForTimeout(this.timeout * 12);
  }

  async clickOnStartDiscussionBtn() {
    await this.addStartDiscussionBtn.click();
    await this.page.waitForTimeout(this.timeout);
  }

  async clickOnCreateNewDiscussionBtn() {
    await this.createNewDiscussion.click();
    await this.page.waitForTimeout(this.timeout);
  }

  async enterDiscussion() {
    await this.discusionInput.type(resources.TEXT.DISCUSSION);
    await this.page.waitForTimeout(this.timeout);
  }

  async selectForumTopic(topic) {
    let elForumTopic = await this.forumTopics[topic];
    await this.waitForAndVerifyVisibility(elForumTopic);
    await elForumTopic.click();
    await this.page.waitForTimeout(this.timeout);
  }

  async selectLocation(location) {
    await this.scrollToEnd();
    let elSelectLocator = await this.locationBtns[location];
    await this.waitForAndVerifyVisibility(elSelectLocator);
    await elSelectLocator.click();
    await this.page.waitForTimeout(this.timeout);
  }

  async clickOnPostForum() {
    await this.forumPostDiscussionBtn.click();
    await this.page.waitForTimeout(this.timeout);
  }

  async searchNewgeneratedForum() {
    await this.searchForum.click();
    await this.searchForum.fill("");
    await this.searchForum.fill(this.generatedForumName);
    await this.searchForum.press("Enter");
    await this.page.waitForTimeout(this.timeout * 6);
    const matchingForum = await this.page.locator("div:has(h4)", { hasText: this.generatedForumName }).first();
    await this.waitForAndVerifyVisibility(matchingForum);
    const forumName = await matchingForum.innerText();
    if (!forumName.includes(this.generatedForumName)) {
      throw new Error(`Forum "${this.generatedForumName}" was not found in the search results.`);
    }
  }

  async deleteForum() {
    await this.deleteBtn.click();
    await this.page.waitForTimeout(this.timeout);
    await this.deleteConfirmBtn.click();
    await this.page.waitForTimeout(this.timeout);
  }

  async privateForumAssigntoMember() {
    await this.privateMembersSearch.type(resources.PRIVATE_MEMBER);
    await this.page.waitForTimeout(this.timeout);
    await this.selectPrivateMember.click();
    await this.page.waitForTimeout(this.timeout);
  }

  async pintheForum() {
    await this.pinForum.click();
    await this.page.waitForTimeout(this.timeout);
  }
}
