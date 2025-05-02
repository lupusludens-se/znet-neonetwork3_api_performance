import { LoginPage } from "../pages/shared/LoginPage.js";
import { SPAdminPage } from "../pages/roles/SPAdminPage.js";
import { AdminPage } from "../pages/roles/AdminPage.js";
import { CorporateUserPage } from "../pages/roles/CorporateUserPage.js";
import { CreateNewCompanyPage } from "../pages/modules/CreateNewCompanyPage.js";
import { AddForumPage } from "../pages/modules/AddForumPage.js";
import { UserPopFuncPage } from "../pages/shared/UserPopFuncPage.js";
import { InternalUserPage } from "../pages/roles/InternalUserPage.js";
import { ProjectLibraryPage } from "../pages/modules/ProjectLibraryPage.js";
import { CreateNewProjectAssocTechPage } from "../pages/modules/CreateNewProjectAssocTechPage.js";
import { CreateNewProjectTypesPage } from "../pages/modules/CreateNewProjectTypesPage.js";
import { CreateNewProjectRegionsPage } from "../pages/modules/CreateNewProjectRegionsPage.js";
import { CreateNewProjectDetailsPage } from "../pages/modules/CreateNewProjectDetailsPage.js";
import { CreateNewProjectDescriptionPage } from "../pages/modules/CreateNewProjectDescriptionPage.js";
import { AggregatedPPAPage } from "../pages/project-types/AggregatedPPAPage.js";
import { OffsitePowerPurchaseAgreementPage } from "../pages/project-types/OffsitePowerPurchaseAgreementPage.js";
import { AggregatedPPAProjectDetailsPublicPage } from "../pages/project-types/AggregatedPPAProjectDetailsPublicPage.js";
import { AggregatedPPAProjectDetailsPrivatePage } from "../pages/project-types/AggregatedPPAProjectDetailsPrivatePage.js";
import { BatteryStoragePage } from "../pages/project-types/BatteryStoragePage.js";
import { OnsiteSolarPage } from "../pages/project-types/OnsiteSolarPage.js";
import { CarbonOffsetPurchasingPage } from "../pages/project-types/CarbonOffsetPurchasingPage.js";
import { ProjectPage } from "../pages/modules/ProjectPage.js";
import { FuelCellsPage } from "../pages/project-types/FuelCellsPage.js";
import { EfficiencyEquipmentMeasuresPage } from "../pages/project-types/EfficiencyEquipmentMeasuresPage.js";
import { RenewableRetailElectricityPage } from "../pages/project-types/RenewableRetailElectricityPage.js";
import { EacPurchasingPage } from "../pages/project-types/EacPurchasingPage.js";
import { UtilityGreenTariffPage } from "../pages/project-types/UtilityGreenTariffPage.js";
import { EfficiencyAuditsConsultingPage } from "../pages/project-types/EfficiencyAuditsConsultingPage.js";
import { CommunitySolarPage } from "../pages/project-types/CommunitySolarPage.js";
import { EmergingTechnologiesPage } from "../pages/project-types/EmergingTechnologiesPage.js";
import { EVChargingAndFleetElectrificationPage } from "../pages/project-types/EVChargingAndFleetElectrificationPage.js";

// Mapping of project types to their corresponding page classes
const PROJECT_TYPE_CLASSES = {
  "New Project Associated Technologies": CreateNewProjectAssocTechPage,
  "New Project Types": CreateNewProjectTypesPage,
  "New Project Regions": CreateNewProjectRegionsPage,
  "New Project Details": CreateNewProjectDetailsPage,
  "New Project Description": CreateNewProjectDescriptionPage,
  "Aggregated PPAs Private Details": AggregatedPPAProjectDetailsPrivatePage,
  "Aggregated PPAs Public Details": AggregatedPPAProjectDetailsPublicPage,
  "Carbon Offset Purchasing": CarbonOffsetPurchasingPage,
  "Aggregated PPAs": AggregatedPPAPage,
  "Offsite Power Purchase Agreement": OffsitePowerPurchaseAgreementPage,
  "Battery Storage": BatteryStoragePage,
  "Fuel Cells": FuelCellsPage,
  "Efficiency Equipment Measures": EfficiencyEquipmentMeasuresPage,
  "Onsite Solar": OnsiteSolarPage,
  "Renewable Retail Electricity": RenewableRetailElectricityPage,
  "EAC Purchasing": EacPurchasingPage,
  "Utility Green Tariff": UtilityGreenTariffPage,
  "Efficiency Audits & Consulting": EfficiencyAuditsConsultingPage,
  "Community Solar": CommunitySolarPage,
  "Emerging Technologies": EmergingTechnologiesPage,
  "EV Charging & Fleet Electrification": EVChargingAndFleetElectrificationPage,
};

export class POManager {
  constructor(page) {
    this.page = page;
    // Dynamically storing all page objects inside an object
    this.pages = {
      login: new LoginPage(this.page),
      spAdmin: new SPAdminPage(this.page),
      admin: new AdminPage(this.page),
      userPopFunc: new UserPopFuncPage(this.page),
      corporateUser: new CorporateUserPage(this.page),
      createCompany: new CreateNewCompanyPage(this.page),
      addForum: new AddForumPage(this.page),
      internalUser: new InternalUserPage(this.page),
      projectLibrary: new ProjectLibraryPage(this.page),
      project: new ProjectPage(this.page),
    };
  }

  // Dynamically initialize project-type pages ONLY when they are needed
  getProjectPage(projectType) {
    if (!PROJECT_TYPE_CLASSES[projectType]) {
      throw new Error(`Project type "${projectType}" does not exist in POManager.`);
    }
    if (!this.pages[projectType]) {
      this.pages[projectType] = new PROJECT_TYPE_CLASSES[projectType](this.page, this);
    }
    return this.pages[projectType];
  }

  getPage(pageName) {
    if (!this.pages[pageName]) {
      throw new Error(`Page object "${pageName}" does not exist in POManager.`);
    }
    return this.pages[pageName];
  }
}
