const { expect } = require("@playwright/test");

let charactersNM = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'
let discusiontext = ' This discussion is created using Automation script'
let gendForum =''
let mdmInputVal ='1234567890'
let privateMember = 'Automation Sunny'

class AddForum{
    constructor(page) {
        this.page = page; // this belongs to current class
        this.forumBtn = page.getByRole('link', { name: '  Forum  ' });
        this.addStartDiscussionBtn= page.getByRole('button',{name:' Start a Discussion  '})
        this.startTypingDiscussion = page.getByPlaceholder('Start typing your discussion title to search...');
        this.createNewDiscussion = page.getByRole('button',{name:'  Create New Discussion  '})
        this.discusionInput = page.locator('//div[@id="editor"]')
        this.forumTopicsAggBtn = page.getByText('Aggregated PPAs')
        this.forumTopicsBatBtn = page.getByText(' Battery Storage')
        this.forumTopicsCarbBtn = page.getByText('Carbon Offset Purchasing')
        this.forumTopiccommunitysolarBtn = page.getByText('Community Solar', { exact: true })
        this.forumTopicDecarbonizationBtn = page.getByText('Decarbonization')
        this.forumTopicEACPurchasingBtn = page.getByText(' EAC Purchasing')
        this.forumTopicEfficiencyAuditConsultuingBtn = page.locator("//div[4]/div[2]/neo-select-item[7]") //page.getByText(' EAC  Efficiency Audits & Consulting',{ exact: true })
        this.forumTopicEfficiencyEquipmentMeasuresBtn = page.getByText('Efficiency Equipment Measures')
        this.forumTopicEmergingTechnologiesBtn = page.getByText(' Emerging Technologies')
        this.forumTopicEnergyNewsBtn = page.getByText('Energy News')
        this.forumTopicEVChargingFleetElectrificationBtn = page.locator('neo-select-item').filter({ hasText: 'EV Charging & Fleet' }).locator('svg')
        this.forumTopicFuelCellsBtn = page.getByText('Fuel Cells')
        this.forumTopicNewCommunitySolarBtn = page.getByText('New Community Solar')
        this.forumTopicNewOffsitePowerPurchaseAgreementBtn = page.getByText(' Offsite Power Purchase Agreement')
        this.forumTopicOnsiteSolarBtn = page.getByText('Onsite Solar')
        this.forumTopicRenewableRetailElectricityBtn = page.getByText('Renewable Retail Electricity')
        this.forumTopicResponsibleRenewablesBtn = page.getByText('Responsible Renewables')
        this.forumTopicUtilityGreenTariffBtn = page.getByText(' Utility Green Tariff')
        this.locationSpecificBtnNo = page.getByRole('button',{name:'No'})
        this.locationSpecificBtnYes = page.getByText('Yes')
        this.loactionSpecificBtnAfrica = page.getByText('Africa')
        this.loactionSpecificBtnAsia = page.getByText('Asia')
        this.loactionSpecificBtnEurope = page.getByText('Europe')
        this.loactionSpecificBtnMexico = page.getByText(' Mexico & Central America')
        this.loactionSpecificBtnOceania = page.getByText('Oceania')
        this.loactionSpecificBtnSouthAmerica= page.getByText('South America')
        this.loactionSpecificBtnUSAandCannada= page.getByText('USA & Canada')
        this.forumPostDiscussionBtn = page.getByRole('button',{name:' Post Discussion '})
        this.forumBackBtn = page.getByRole('button',{name:' Back '})
        this.searchForum = page.getByPlaceholder('Search');
        this.deleteBtn = page.locator("neo-forum-topic:nth-of-type(1) .remove-item-icon > [width='100%']")
        this.deleteConfirmBtn = page.getByRole('button',{name:'Yes, Delete '})
        this.scrollTo = page.evaluate (() => { window.scrollBy (0, 500); })
        this.scrollToEnd = page.keyboard.press ('End')
        this.privateMembersSearch = page.getByPlaceholder('Search Users')
        this.selectPrivateMember = page.getByText('corporate Automation Sunny,')   //this detail will vary in future
        this.pinForum = page.getByRole('heading', { name: 'Pin the discussion' })
   }
  
       // generate and enter new corporate name 
       async enterForumNamefunc(){
        await this.startTypingDiscussion.click()
        await this.page.waitForTimeout(500)
        function generateForumName(length) {
            let result = '';
            let characters = charactersNM
            let charactersLength = characters.length;
            for (var i = 0; i < length; i++) {
              result += characters.charAt(Math.floor(Math.random() * charactersLength));
            }
            return result;
          }
          
          function generaterandomForumName() {
            let randomString = generateForumName(4); 
            let corponame = 'New Automation Forum '+ [randomString ]; 
            return corponame;
          }
          
          gendForum = generaterandomForumName();
          console.log(gendForum); 
          await this.startTypingDiscussion.fill('')
          await this.startTypingDiscussion.type(gendForum)
          }


    
    async clickOnForumModule(){
      await  this.forumBtn.click();
    }

    async clickOnStartDiscussionBtn(){
      await  this.addStartDiscussionBtn.click();
      await this.page.waitForTimeout(500)
    }

    async clickOnCreateNewDiscussionBtn(){
      await this.createNewDiscussion.click();
      await this.page.waitForTimeout(500)
    }

    async enterDiscussionFunc(){
      await this.discusionInput.type(discusiontext);
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicAggtPPA(){
      await this.forumTopicsAggBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicBattryStrge(){
      await this.forumTopicsBatBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicCarbonOffset(){
      await this.forumTopicsCarbBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicCommunitySolar(){
      await this.forumTopiccommunitysolarBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicDecarbonization(){
      await this.forumTopicDecarbonizationBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicEACPurchasing(){
      await this.forumTopicEACPurchasingBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicEfficiencyEquipmentMeasures(){
      await this.forumTopicEfficiencyEquipmentMeasuresBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicEfficiencyAuditConsulting(){
      await this.forumTopicEfficiencyAuditConsultuingBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicEmergingTechnology(){
      await this.forumTopicEmergingTechnologiesBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicEnergyNews(){
      await this.forumTopicEnergyNewsBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicEVChargingFleetElectrification(){
      await this.forumTopicEVChargingFleetElectrificationBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicFuelCell(){
      await this.forumTopicFuelCellsBtn.click();
      await this.page.waitForTimeout(500)
    }

    
    async selectForumTopicNewCommunitySolar(){
      await this.forumTopicNewCommunitySolarBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicOffsitePPA(){
      await this.forumTopicNewOffsitePowerPurchaseAgreementBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicOnsiteSolar(){
      await this.forumTopicOnsiteSolarBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicReNewableRetailElectricity(){
      await this.forumTopicRenewableRetailElectricityBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicResponsibleReNewables(){
      await this.forumTopicResponsibleRenewablesBtn.click();
      await this.page.waitForTimeout(500)
    }

    async selectForumTopicUtilityGreenTariff(){
      await this.forumTopicUtilityGreenTariffBtn.click();
      await this.page.waitForTimeout(500)
    }


    async selectLocationASIA(){
      await this.loactionSpecificBtnAsia.click();
      await this.page.waitForTimeout(500)
    }

    async selectLocationAFRICA(){
      await this.loactionSpecificBtnAfrica.click();
      await this.page.waitForTimeout(500)
    }

    async selectLocationEurope(){
      await this.loactionSpecificBtnEurope.click();
      await this.page.waitForTimeout(500)
    }

    async selectLocationMexico(){
      await this.loactionSpecificBtnMexico.click();
      await this.page.waitForTimeout(500)
    }
    
    async selectLocationOceania(){
      await this.loactionSpecificBtnOceania.click();
      await this.page.waitForTimeout(500)
    }

    async selectLocationSouthAmerica(){
      await this.loactionSpecificBtnSouthAmerica.click();
      await this.page.waitForTimeout(500)
    }

    async selectLocationUSAandCanada(){
      await this.loactionSpecificBtnUSAandCannada.click();
      await this.page.waitForTimeout(500)
    }

    async selectLocationYES(){
      await this.scrollToEnd
      await this.locationSpecificBtnYes.click();
      await this.page.waitForTimeout(500)
    }

    async clickOnPostForum(){
      await this.forumPostDiscussionBtn.click();
    }

      // search newly generated forum func..
      async searchNewgeneratedForum(){
        await this.searchForum.fill('')
        await this.searchForum.type(gendForum)
        await this.page.waitForTimeout(1500)
        var titleName = await this.page.locator('h4').innerText() // get element text
        console.log(titleName, ' h4 s innerText')
        expect(titleName).toContain(gendForum)
        console.log(gendForum,' Successfully Added Forum.....'); 
    }

    async deletingForumfunc(){
      await this.deleteBtn.click()
      await this.page.waitForTimeout(500)
      await this.deleteConfirmBtn.click()
      console.log(' Forum deleted Successfully.....'); 
    }

    async privateForumAssigntoMemberfunc(){
      await this.privateMembersSearch.type(privateMember)
      await this.page.waitForTimeout(500)
      await this.selectPrivateMember.click()
      console.log(' Private Member selected Successfully.....'); 
    }

    async pintheForumfunc(){
      await this.pinForum.click()
      await this.page.waitForTimeout(500)
      console.log(' Forum pinned Successfully.....'); 
    }


  
}

module.exports = { AddForum };
