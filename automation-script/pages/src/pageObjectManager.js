// pageObjectManger file which handle all the object file created,
// consolidate and keep the all the objects
const { LoginPage } = require('../commons/loginPage');
const {SPadminPage} = require('../roles/spAdminPage')
const {AdminPage} = require('../roles/adminPage')
const {UserPopPage} = require('../miscellaneous/userPopFunc')
const {CorporateUserPage} = require('../roles/corporateUserPage')
const {CreateCompanyPage} = require('../modules/createNewCompany')
const {AddForum} = require('../modules/forumPage')


class POManager {
    constructor(page) {
        this.page = page;
        this.loginPage = new LoginPage(this.page);
        this.spadminPage = new SPadminPage(this.page)
        this.adminPage = new AdminPage(this.page)
        this.userPopPage = new UserPopPage(this.page)
        this.corporatePage = new CorporateUserPage(this.page)
        this.createCompanyPage = new CreateCompanyPage(this.page)
        this.addForum = new AddForum(this.page)

    }

    getLoginPage() {
        return this.loginPage;
    }

    getSPadminPage(){
        return this.spadminPage;
    }

    getAdminPage(){
        return this.adminPage;
    }

    getUserPopPage(){
        return this.userPopPage;
    }

    getCorporatePage(){
        return this.corporatePage;
    }
    
    getCreateCompanyPage(){
        return this.createCompanyPage;
    }
    getAddForum(){
        return this.addForum;
    }

}
module.exports = { POManager };