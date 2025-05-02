const { expect } = require("@playwright/test");
 

let spCompanyName = 'Tesla'
let Country ='India'
let randomEmail =''
let corporateCompanyName = 'Corporation Company'
let corpoText = " Corporation "
let internalText = " Internal SE "
let spText = " Solution Provider "
let charactersTxt = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'
let charactersNM = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'

//SPadminPage page methods 
class AdminPage {

    constructor(page) {
        this.page = page; // this belongs to current class
        this.adminLMbutton = page.getByRole('link', { name: 'Admin' });
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
        this.addUserbutton =page.getByRole('button', { name: 'Add User' })
        this.firstName = page.getByPlaceholder('First Name')
        this.lastName = page.getByPlaceholder('Last Name')
        this.email = page.getByPlaceholder('Email')
        this.companyName = page.getByLabel('Company', { exact: true });
        this.getSPCompanyName = page.getByText(spCompanyName);
        this.getCorpoCompanyName = page.getByText(corporateCompanyName);
        this.selectSProle = page.getByText('Solution Provider')
        this.selectCountry = page.getByLabel('Country');
        this.countrySelected = page.locator('div').filter({ hasText: /^India$/ });
        this.scrollTo = page.evaluate (() => { window.scrollBy (0, 500); })
        this.scrollToEnd = page.keyboard.press ('End')
        this.heardVia = page.locator('neo-dropdown').filter({ hasText: 'Heard via Conference/Event Co' }).getByPlaceholder('Select one');
        this.heardViaSelectedCE = page.getByText('Conference/Event')
        this.heardViaSelectedZNM = page.getByText('Zeigo Network Member')
        this.AddUser = page.getByRole('button', { name: 'Add User' })
        this.searchUser = page.locator('//div/neo-users-list/neo-search-bar/div/input')
        this.threeUsersDot = page.getByRole('table').getByRole('button').first()
        this.selectCorporateRole = page.locator('label').filter({ hasText: 'Corporation' })
        this.selectInternalSErole = page.locator('label').filter({ hasText: 'Internal SE' })
        this.roleCorporate = page.locator("neo-table-row:nth-of-type(1) .corporation")
        this.roleInterSE = page.locator("neo-table-row:nth-of-type(1) .internal")
        this.roleSP = page.locator("neo-table-row:nth-of-type(1) .provider")
    

        this.addCompanybtn = page.getByRole('button', { name: 'Add Company' })
        this.addcompanyName = page.locator('neo-text-input').filter({ hasText: 'Company Name' }).getByRole('textbox')
        

    }
    
    async verifyingAdminaccess(username, password) {
        // Verifying wether the user is SP admin or not
        await this.adminLMbutton.click()
        console.log("the logged in user is Admin");

    }

    async clickOnAdmin() {
        await this.adminLMbutton.click()
        console.log("Clicked on Admin");
        await this.page.waitForTimeout(800)
    }

    async clickOnuserManagement() {
        await this.userManagement.click()
        console.log("Clicked on userManagement");
        await this.page.waitForTimeout(800)
    }

    async clickOnaddUserbutton() {
        await this.addUserbutton.click()
        console.log("Clicked on addUserbutton");
        await this.page.waitForTimeout(800)
    }

    async enterFirstNamefunc(){
        function generateFirstName(length) {
            let result = '';
            let characters = charactersNM
            let charactersLength = characters.length;
            for (var i = 0; i < length; i++) {
              result += characters.charAt(Math.floor(Math.random() * charactersLength));
            }
            return result;
          }
          
          // Generate a firstName
          function generateRandomFirst() {
            let randomString = generateFirstName(5); 
            let firstname = 'First '+randomString ; 
            return firstname;
          }
          
          // Store the firstName in a variable
          let randomFirst = generateRandomFirst();
          console.log(randomFirst); 
          await this.firstName.fill('')
          await this.firstName.type(randomFirst)
    }
    
    async enterLastNamefunc(){
        
        // Function to generate lastName
        function generateLastName(length) {
            let result = '';
            let characters = charactersNM
            let charactersLength = characters.length;
            for (var i = 0; i < length; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
            }
            return result;
        }
        
        // Generate a lastName
        function generateRandomLast() {
            let randomString = generateLastName(5); 
            let lasttname = 'Last '+randomString+' Name' ; 
            return lasttname;
        }
        
        // Store the lastName in a variable
        let randomLast = generateRandomLast();
        console.log(randomLast); 
        await this.lastName.fill('')
        await this.lastName.type(randomLast)
    }

    async selectSProlefunc(){
        await this.scrollTo
        await this.selectSProle.click()
        await this.scrollToEnd
        await this.page.waitForTimeout(800)
    }

    async selectCorporaterolefunc(){
        await this.scrollTo
        await this.selectCorporateRole.click()
        await this.scrollToEnd
        await this.page.waitForTimeout(800)
    }

    async selectInternalSErolefunc(){
        await this.scrollTo
        await this.selectInternalSErole.click()
        await this.scrollToEnd
        await this.page.waitForTimeout(800)
    }

    async selectIndCountryfuc(){
        await this.selectCountry.type(Country)
        await this.page.waitForTimeout(500)
        await this.countrySelected.click()
    }

    async selectHeardViaCEfunc(){
        await this.heardVia.click()
        await this.heardViaSelectedCE.click()
        await this.page.waitForTimeout(800)
    }

    async selectHeardViaZNMfunc(){
        await this.heardVia.click()
        await this.heardViaSelectedZNM.click()
        await this.page.waitForTimeout(800)
    }

    async selectSPcompanyfunc(){
        await this.companyName.type(spCompanyName)
        await this.page.waitForTimeout(800)
        await this.getSPCompanyName.click()
    }

    async selectCorporatecompanyfunc(){
        await this.companyName.type(corporateCompanyName)
        await this.page.waitForTimeout(800)
        await this.getCorpoCompanyName.click()
    }

    async verifyingCorporateAddUser(){
        await this.page.waitForTimeout(500)
        await expect(this.roleCorporate).toContainText(corpoText)
        console.log('Corporate user has been added..... ')
    }

    async verifyingSPAddUser(){
        await this.page.waitForTimeout(500)
        await expect(this.roleSP).toContainText(spText)
        console.log('SP user has been added..... ')
    }

    async verifyingInternalSEAddUser(){
        await this.page.waitForTimeout(500)
        await expect(this.roleInterSE).toContainText(internalText)
        console.log('Internal SE user has been added..... ')
    }


//  Add new SP user function
    async addingNewSPuserandVerifying(){
        await this.clickOnAdmin()
        await this.clickOnuserManagement()
        await this.clickOnaddUserbutton()
        await this.enterFirstNamefunc()
        await this.enterLastNamefunc()
        await this.selectSPcompanyfunc()
        // Function to generate email
        function generateEmail(length) {
            let result = '';
            let characters = charactersTxt
            let charactersLength = characters.length;
            for (var i = 0; i < length; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
            }
            return result;
        }
        
        // Generate a random email ID
        function generateRandomEmail() {
            let randomString = generateEmail(8)
            let email = 'Test'+randomString + '@yopmail.com';
            return email;
        }
        
        // Store the random email ID in a variable
        randomEmail = generateRandomEmail();
        console.log(randomEmail); 
        await this.email.fill('')
        await this.email.type(randomEmail)
        await this.selectSProlefunc() // select SP profile 
        await this.selectIndCountryfuc() // select India Country
        await this.selectHeardViaCEfunc() //select heard via conference/event
        await this.AddUser.click()
        console.log('Verifying user ',randomEmail,' added or not.....'); 
        await this.page.waitForTimeout(1500)
        await this.searchMailIDgeneratefunc()
        await this.verifyingSPAddUser()
        
    }

    //  Add new Corporate user function
    async addingNewCorporateuserandVerifying(){
        await this.clickOnAdmin()
        await this.clickOnuserManagement()
        await this.clickOnaddUserbutton()
        await this.enterFirstNamefunc()
        await this.enterLastNamefunc()
        await this.selectCorporatecompanyfunc()
        // Function to generate email
        function generateEmail(length) {
            let result = '';
            let characters = charactersTxt
            let charactersLength = characters.length;
            for (var i = 0; i < length; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
            }
            return result;
        }
        
        // Generate a random email ID
        function generateRandomEmail() {
            let randomString = generateEmail(8)
            let email = 'Test'+randomString + '@yopmail.com';
            return email;
        }
        
        // Store the random email ID in a variable
        randomEmail = generateRandomEmail();
        console.log(randomEmail); 
        await this.email.fill('')
        await this.email.type(randomEmail)
        await this.selectCorporaterolefunc() // select SP profile 
        await this.selectIndCountryfuc() // select India Country
        await this.selectHeardViaZNMfunc() //select heard via ZNM
        await this.AddUser.click()
        console.log('Verifying user ',randomEmail,' added or not.....'); 
        await this.page.waitForTimeout(1500)
        await this.searchMailIDgeneratefunc()
        await this.verifyingCorporateAddUser()
        
    }



    // function to search and verify genearated mailID
    async searchMailIDgeneratefunc(){
        await this.searchUser.fill('')
        await this.searchUser.type(randomEmail)
        await this.page.waitForTimeout(1500)
        await expect(this.threeUsersDot).toBeEnabled()
        await this.threeUsersDot.click()
        console.log(randomEmail,' user Added Successfully.....'); 
    }

    //  Add new Internal SE user function
    async addingNewInternalSEandVerifying(){
        await this.clickOnAdmin()
        await this.clickOnuserManagement()
        await this.clickOnaddUserbutton()
        await this.enterFirstNamefunc()
        await this.enterLastNamefunc()
        // await this.selectCorporatecompanyfunc()
        // Function to generate email
        function generateEmail(length) {
            let result = '';
            let characters = charactersTxt
            let charactersLength = characters.length;
            for (var i = 0; i < length; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
            }
            return result;
        }
        
        // Generate a random email ID
        function generateRandomEmail() {
            let randomString = generateEmail(8)
            let email = 'Test'+randomString + '@yopmail.com';
            return email;
        }
        
        // Store the random email ID in a variable
        randomEmail = generateRandomEmail();
        console.log(randomEmail); 
        await this.email.fill('')
        await this.email.type(randomEmail)
        await this.selectInternalSErolefunc() // select InternalSE profile 
        await this.selectIndCountryfuc() // select India Country
        await this.scrollToEnd
        await this.page.waitForTimeout(800)
        await this.AddUser.click()
        console.log('Verifying user ',randomEmail,' added or not.....'); 
        await this.page.waitForTimeout(1500)
        await this.searchMailIDgeneratefunc()
        await this.verifyingInternalSEAddUser()
        
    }

    


}
module.exports = { AdminPage };