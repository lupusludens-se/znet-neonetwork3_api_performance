const { expect } = require("@playwright/test");

//SPadminPage page methods 
class CorporateUserPage {

    constructor(page) {
        this.page = page; // this belongs to current class
        this.dasboardLMbutton = page.getByRole('link', { name: 'Dashboard' });
        this.projectLMbutton = page.getByRole('link', { name: 'Projects' })
        this.learnLMbutton = page.getByRole('link', { name: 'Learn' });
        this.eventLMbutton = page.getByRole('link', { name: 'Events' });
        this.toolLMbutton = page.getByRole('link', { name: 'Tools' });
        this.communityLMbutton = page.getByRole('link', { name: 'Community' });
        this.forumLMbutton = page.getByRole('link', { name: 'Forum' });
        this.messageLMbutton = page.getByRole('link', { name: 'Messages' });

    }

    async verifyingCorporateUser() {
        console.log("Verifying the loggedIn user is Corpoarte or not.........");
        await expect(this.projectLMbutton).toBeEnabled()
        await this.projectLMbutton.click()
        console.log("clicked on project")
        await this.page.waitForTimeout(800)
        console.log("the logged in user is Corporate user");

    }


}
module.exports = { CorporateUserPage };