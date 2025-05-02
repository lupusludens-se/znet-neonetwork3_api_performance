const { expect } = require("@playwright/test");

//SPadminPage page methods 
class SPadminPage {

    constructor(page) {
        this.page = page; // this belongs to current class
        this.mangeLMbutton = page.getByRole('link', { name: 'Manage' });
        this.viewMyuserbutton = page.getByRole('button', { name: 'View My Users' });
        this.editCompanybutton = page.getByRole('button', { name: 'Edit Company Profile' });
        this.viewallPM = page.locator("//div[3]/button[@class='btn-m green-frame flex-center']")
        this.viewallMM = page.locator("//div[4]/button[@class='btn-m green-frame flex-center']")
        this.projrctMangment = page.locator("//h4[.='Project Management']");
        this.messageManagement = page.locator("//h4[.='Messages Management']");
        this.companyManagement = page.locator("//h4[.='Company Management']");
        this.userManagement = page.locator("//h4[.='User Management']");
        this.dasboardLMbutton = page.getByRole('link', { name: 'Dashboard' });
        this.projectLibraryLMbutton = page.getByRole('link', { name: 'Project Library' });
        this.learnLMbutton = page.getByRole('link', { name: 'Learn' });
        this.eventLMbutton = page.getByRole('link', { name: 'Events' });
        this.toolLMbutton = page.getByRole('link', { name: 'Tools' });
        this.communityLMbutton = page.getByRole('link', { name: 'Community' });
        this.forumLMbutton = page.getByRole('link', { name: 'Forum' });
        this.messageLMbutton = page.getByRole('link', { name: 'Messages' });
        this.addProjectbutton = page.getByRole('button', { name: 'Add Project' });
        this.exportProjectbutton = page.getByRole('button', { name: 'Export Projects' });

    }

    async verifyingSPadminaccess(username, password) {
        console.log("Verifying the loggedIn user is SP Admin or not.........");
        await expect(this.mangeLMbutton).toContainText('Manage')
        // console.log(await expect(this.mangeLMbutton).toBeVisible('Manage'))
        console.log("the logged in user is SP Admin");
        this.mangeLMbutton.click()
        console.log("clicked on manage");

    }


}
module.exports = { SPadminPage };