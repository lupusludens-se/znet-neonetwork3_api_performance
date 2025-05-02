const { expect } = require("@playwright/test");
import fs from 'fs';
import path from 'path';
import { parse } from 'csv-parse/sync';

let enterIndCountry ='India'
let enterFranCountry ='France'
let enterUSCountry ='United States'
let charactersNM = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'
let aboutCompanytext = ' This corporate company added using Automation script'
let randomComp =''
let mdmInputVal ='1234567890'

//SPadminPage page methods 
class CreateCompanyPage {

    constructor(page) {
        this.page = page; // this belongs to current class
        this.enterCompanyName = page.locator('neo-text-input').filter({ hasText: 'Company Name' }).getByRole('textbox')
        this.enterURL = page.locator('neo-text-input:nth-child(3) > .ng-untouched')
        this.selectCountry = page.getByPlaceholder('Search')
        this.selIndiaCountry = page.locator('div').filter({ hasText: /^India$/ })
        this.selFranceCountry = page.locator('div').filter({ hasText: /^France$/ }).nth(3)
        this.selUSCountry = page.locator('div').filter({ hasText: /^United States$/ })
        this.selectIndustry = page.locator('neo-dropdown').filter({ hasText: 'Industry Consumer Goods' }).getByPlaceholder('Select one')
        this.consumerGoodsIndustry = page.getByText('Consumer Goods')
        this.extraMineralsIndustry = page.getByText('Extractives and Minerals')
        this.financialsIndustry = page.getByText('Financials')
        this.foodBeverageIndustry = page.getByText('Food and Beverage')
        this.govtIndustry = page.getByText('Government and Municipality')
        this.healthCareIndustry = page.getByText('Health Care')
        this.infrasIndustry = page.getByText('Infrastructure')
        this.renewableIndustry = page.getByText('Renewable Resources and')
        this.resourceIndustry = page.getByText('Resource Transformation')
        this.serviceIndustry = page.getByText('Services')
        this.technologyIndustry = page.getByText('Technology and Communications')
        this.transportIndustry = page.getByText('Transportation')
        this.aboutCompantTxt = page.locator('#editor')
        this.coporateCompanyType = page.locator('label').filter({ hasText: 'Corporation' })
        this.spCompanyType = page.locator('label').filter({ hasText: 'Solution Provider' })
        this.addComoanyButton = page.getByRole('button', { name: 'Add Company' })
        this.scrollTo = page.evaluate (() => { window.scrollBy (0, 500); })
        this.scrollToEnd = page.keyboard.press ('End')
        this.searchTxt = page.getByPlaceholder('Search')
        this.mdmKeyDD = page.locator('neo-dropdown').filter({ hasText: 'MDM Key ORG- MDM-' }).getByPlaceholder('Select one')
        this.mdmSelORG = page.getByText('ORG-')
        this.mdmSelMDM = page.getByText('MDM-')
        this.mdmInput  = page.getByPlaceholder('Maximum 10 digits')
        this.linkNameInput = page.getByPlaceholder('Link Name')
        this.linkNameURLInput = page.getByPlaceholder('URL')
        // this.threeUsersDot = page.getByRole('table').getByRole('button').first()

    }

    //parsing excelsheet data
    async addcompNamefrmCSVfunc(){
        const comprecords = parse(
            fs.readFileSync(path.join(__dirname,"../miscellaneous/CompanyDatatobeAddedToPreProdCSV.csv")),
            {
                columns: true,
                skip_empty_lines: true
        
            }
        )
        for(const comprecord of comprecords){
            await this.enterCompanyName.click()
            await this.enterCompanyName.clear()
            await this.page.waitForTimeout(500)
            await this.enterCompanyName.type(comprecord['Company_Name'])
            console.log(comprecord['Company_Name'])
            await this.enterValidURL()
            await this.selectUSCountry()
            await this.renewableresourcesIndustryType()
            await this.addCompanyText()
            await this.selectCorporateCompanyType()
            await this.clickOnAddCompany()
            // await this.searchTxt.fill('')
            // await this.searchTxt.type(comprecord['Company_Name'])
            // await this.page.waitForTimeout(1500)
            // await expect(this.threeUsersDot).toBeEnabled()
            console.log(comprecord['Company_Name'],' Company Added Successfully......');
            await this.page.waitForTimeout(900)
            await this.clickOnAddCompany()

        }
    }

    // add About company text func..
    async addCompanyText(){
        await this.scrollTo
        await this.page.waitForTimeout(500)
        await this.aboutCompantTxt.type(aboutCompanytext)
    }

    // slecet corporate company type
    async enterLinkDetails(){
        await this.scrollTo
        await this.linkNameInput.type('Test Link Name')
        await this.linkNameURLInput.type('www.testURL.co.in')
        
    }

    // slecet corporate company type
    async selectCorporateCompanyType(){
        await this.scrollTo
        await this.coporateCompanyType.click()
    }

      // slecet Solution Provider company type
      async selectSPCompanyType(){
        await this.scrollTo
        await this.spCompanyType.click()
    }

      // slecet enter URL 
      async enterValidURL(){ 
        await this.enterURL.click()
        await this.enterURL.type('www.testdomai.co.in')
    }

       // slecet ORG MDM 
       async selectORGmdm(){ 
        await this.mdmKeyDD.click()
        await this.mdmSelORG.click()
        await this.mdmInput.type(mdmInputVal)
    }

       // slecet  MDM mdm
       async selectMDMmdm(){ 
        await this.mdmKeyDD.click()
        await this.mdmSelMDM.click()
        await this.mdmInput.type(mdmInputVal)
    }

    // generate and enter new corporate name 
    async enterCorporateNamefunc(){
        await this.enterCompanyName.click()
        await this.page.waitForTimeout(500)
        function generateCorpoName(length) {
            let result = '';
            let characters = charactersNM
            let charactersLength = characters.length;
            for (var i = 0; i < length; i++) {
              result += characters.charAt(Math.floor(Math.random() * charactersLength));
            }
            return result;
          }
          
          function generateCorporateName() {
            let randomString = generateCorpoName(4); 
            let corponame = 'Corporate '+ randomString + ' Automation Company' ; 
            return corponame;
          }
          
          randomComp = generateCorporateName();
          console.log(randomComp); 
          await this.enterCompanyName.fill('')
          await this.enterCompanyName.type(randomComp)
    }
    
    // Select United States country
    async selectUSCountry(){
        await this.selectCountry.type(enterUSCountry)
        await this.page.waitForTimeout(1000)
        await this.selUSCountry.click()
        console.log("Country United States has been selected")
    }
   
    // Select India country
    async selectINDCountry(){
        await this.selectCountry.type(enterIndCountry)
        await this.page.waitForTimeout(1000)
        await this.selIndiaCountry.click()
        console.log("Country India has been selected")
    }

     // Select France country
     async selectFRANCECountry(){
        await this.selectCountry.type(enterFranCountry)
        await this.page.waitForTimeout(1000)
        await this.selFranceCountry.click()
        console.log("Country France has been selected")
    }

    // Select Consumer Industry type
    async consumerGoodsIndustryType() {
        await this.selectIndustry.click()
        await this.page.waitForTimeout(1000)
        await this.consumerGoodsIndustry.click()
        
    }

     // Select extraMineralsIndustry type
     async extraMineralsIndustryType() {
        await this.selectIndustry.click()
        await this.page.waitForTimeout(1000)
        await this.extraMineralsIndustry.click()
        
    }

     // Select Financial Industry type
     async financialsIndustryType() {
        await this.selectIndustry.click()
        await this.page.waitForTimeout(1000)
        await this.financialsIndustry.click()
        
    }

    //Select food and Beverage Industry type 
    async foodBeverageIndustryType(){
        await this.selectIndustry.click()
        await this.page.waitForTimeout(1000)
        await this.foodBeverageIndustry.click()
    }

    //Select resource transfer Industry type 
    async resourceIndustryType(){
        await this.selectIndustry.click()
        await this.page.waitForTimeout(1000)
        await this.resourceIndustry.click()
    }

    //Select Govt. municipal Industry type 
    async govtandmunicipalIndustryType(){
        await this.selectIndustry.click()
        await this.page.waitForTimeout(1000)
        await this.govtIndustry.click()
    }

     //Select Health care Industry type 
     async healthcareIndustryType(){
        await this.selectIndustry.click()
        await this.page.waitForTimeout(1000)
        await this.healthCareIndustry.click()
    }

     //Select infrastructure Industry type 
     async infrastructureIndustryType(){
        await this.selectIndustry.click()
        await this.page.waitForTimeout(1000)
        await this.infrasIndustry.click()
    }

    //Select Renewable Resources and Alternative Energy Industry type 
    async renewableresourcesIndustryType(){
        await this.selectIndustry.click()
        await this.page.waitForTimeout(1000)
        await this.renewableIndustry.click()
    }

     //Select Service Industry type 
     async serviceIndustryType(){
        await this.selectIndustry.click()
        await this.page.waitForTimeout(1000)
        await this.serviceIndustry.click()
    }

    //Select Technology and Communications Industry type
    async technologyandcommIndustryType(){
        await this.selectIndustry.click()
        await this.page.waitForTimeout(1000)
        await this.technologyIndustry.click()
    }

     //Select Transportation Industry type 
     async transportationIndustryType(){
        await this.selectIndustry.click()
        await this.page.waitForTimeout(1000)
        await this.transportIndustry.click()
    }

    // click on Add company button func..
    async clickOnAddCompany(){
        await this.addComoanyButton.click()
        await this.page.waitForTimeout(500)
    }

    // search newly generated company func..
    async searchNewgeneratefunc(){
        await this.searchTxt.fill('')
        await this.searchTxt.type(randomComp)
        await this.page.waitForTimeout(1500)
        console.log(randomComp,' Company Added Successfully.....'); 
    }


}
module.exports = { CreateCompanyPage };