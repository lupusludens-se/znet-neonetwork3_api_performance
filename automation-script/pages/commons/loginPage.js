//login page methods 
const dataset = JSON.parse(JSON.stringify(require("../commons/testdata/logIntestData.json")));
const datasetpreprod = JSON.parse(JSON.stringify(require("../commons/testdata/logInPreProdData.json")));

class LoginPage {

    constructor(page) {
        this.page = page; // this belongs to current class
        this.logInbutton = page.getByRole('button', { name: 'Log In' });
        this.userName = page.getByPlaceholder('Email Address')
        this.password = page.getByPlaceholder('Password');
        this.signInbutton = page.getByRole("button", { name: "Sign in" })
        this.profileDropdown = page.locator('neo-menu').getByRole('button');
        this.logOutbutton = page.getByText('Log Out');

    }

    async goToTest() {
        await this.page.goto("https://network-tst.zeigo.com/zeigonetwork/tst/dashboard");
    }

    async goToProd() {
        await this.page.goto("https://network.zeigo.com/zeigonetwork/dashboard");
    }

    async goToPreProd(){
        await this.page.goto("https://network-pre.zeigo.com/zeigonetwork/preprod/dashboard")
    }

    // Master Admin login func..
    async tstAdminUserLogin() {
        await this.testAdminUserLogin()
        // await this.preprodAdminUserLogin();
        
    }

    //Master corporate login func..
    async tstCorporateUserLogin() {
        await this.testCorporateUserLogin()
        // await this.preprodCorporateUserLogin();

    }

    //Master SP Login func..
    async tstSPUserLogin() {
         await this.testSPUserLogin()
        // await this.preprodSPUserLogin()
    
        }


    //Master SP Admin Login func..
    async tstSPadminUserLogin() {
        await this.testSPadminUserLogin()
        // await this.preprodSPadminUserLogin();
      

    }

    // test admin login func..
    async testAdminUserLogin() {
        await this.goToTest()
        await this.logInbutton.click();
        await this.userName.type(dataset[0].username);
        await this.password.type(dataset[0].password);
        await this.signInbutton.click();

    }

      //Test SP Admin Login func..
      async testSPadminUserLogin() {
        await this.goToTest()
        await this.logInbutton.click();
        await this.userName.type(dataset[1].username);
        await this.password.type(dataset[1].password);
        await this.signInbutton.click();

    }

     //Preprod SP Admin Login func..
     async preprodSPadminUserLogin() {
        await this.goToPreProd()
        await this.logInbutton.click();
        await this.userName.type(dataset[1].username);
        await this.password.type(dataset[1].password);
        await this.signInbutton.click();

    }


    //test Sp login
    async testSPUserLogin() {
        await this.goToTest()
        await this.logInbutton.click();
        await this.userName.type(dataset[2].username);
        await this.password.type(dataset[2].password);
        await this.signInbutton.click();

    }

     //preprod Sp login
     async preprodSPUserLogin() {
        await this.goToPreProd()
        await this.logInbutton.click();
        await this.userName.type(dataset[2].username);
        await this.password.type(dataset[2].password);
        await this.signInbutton.click();

    }


    //test corporate login
    async testCorporateUserLogin() {
        await this.goToTest()
        await this.logInbutton.click();
        await this.userName.type(dataset[3].username);
        await this.password.type(dataset[3].password);
        await this.signInbutton.click();

    }

      //preprod corporate login
      async preprodCorporateUserLogin() {
        await this.goToPreProd() // preprodURL
        await this.logInbutton.click();
        await this.userName.type(dataset[3].username);
        await this.password.type(dataset[3].password);
        await this.signInbutton.click();

    }

    //preprod admin login
    async preprodAdminUserLogin() {
        await this.goToPreProd() // preprodURL
        await this.logInbutton.click();
        await this.userName.type(datasetpreprod[0].username);
        await this.password.type(datasetpreprod[0].password);
        await this.signInbutton.click();

    }

    async logOut(){
        await this.profileDropdown.click();
        await this.page.waitForTimeout(500)
        console.log('click on Dropdown')
        await this.logOutbutton.click();
        await this.page.waitForTimeout(500)
        console.log('click on LogOut button')
    }

}
module.exports = { LoginPage };