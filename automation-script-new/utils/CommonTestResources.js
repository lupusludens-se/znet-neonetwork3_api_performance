import "dotenv/config";

export class CommonTestResources {
  constructor() {
    // Store static Test Data
    this.TEST_URL = process.env.TEST_URL;
    this.PREPROD_URL = process.env.PREPROD_URL;
    if (
      !Object.hasOwn(process.env, "TARGET_ENV") ||
      (Object.hasOwn(process.env, "TARGET_ENV") && typeof process.env.TARGET_ENV === "string" && process.env.TARGET_ENV.trim().toLowerCase().startsWith("test"))
    ) {
      this.TARGET_ENV = "test";
    } else {
      this.TARGET_ENV = "preprod";
    }
    this.PASSWORD = process.env.PASSWORD;
    //LoginTestData
    this.LOGIN_TEST_DATA = {
      ADMIN_USERNAME: process.env.TEST_ADMIN_USERNAME,
      SPADMIN_USERNAME: process.env.TEST_SPADMIN_USERNAME,
      SPUSER_USERNAME: process.env.TEST_SPUSER_USERNAME,
      CORPORATEUSER_USERNAME: process.env.TEST_CORPORATE_USERNAME,
      INTERNALUSER_USERNAME: process.env.TEST_INTERNAL_USERNAME,
    };
    //LoginPreProdData
    this.LOGIN_PREPROD_DATA = {
      ADMIN_USERNAME: process.env.PREPROD_ADMIN_USERNAME,
      ADMIN_PASSWORD: process.env.PREPROD_ADMIN_PASSWORD,
      SPADMIN_USERNAME: process.env.PREPROD_SPADMIN_USERNAME,
      SPADMIN_PASSWORD: process.env.PREPROD_SPADMIN_PASSWORD,
      SPUSER_USERNAME: process.env.PREPROD_SPUSER_USERNAME,
      SPUSER_PASSWORD: process.env.PREPROD_SPUSER_PASSWORD,
      CORPORATEUSER_USERNAME: process.env.PREPROD_CORPORATE_USERNAME,
      CORPORATEUSER_PASSWORD: process.env.PREPROD_CORPORATE_PASSWORD,
      INTERNALUSER_USERNAME: process.env.PREPROD_INTERNAL_USERNAME,
      INTERNALUSER_PASSWORD: process.env.PREPROD_INTERNAL_PASSWORD,
    };
    //Grouping texts inside TEXT object
    this.TEXT = {
      DISCUSSION: "This discussion is created using Automation script",
      ABOUT_COMPANY: "This corporate company was added using Automation script",
      CORPORATION: "Corporation",
      CORPORATION_COMPANY: "Corporation Company",
      INTERNAL_COMPANY: "Internal SE",
      SOLUTION_PROVIDER: "Solution Provider",
      SP_COMPANY_NAME: "Tesla",
    };
    // Grouping country names inside COUNTRY object
    this.COUNTRY = {
      INDIA: "India",
      FRANCE: "France",
      USA: "United States",
    };
    (this.MDM_KEY_VALUE = "1234567890"), (this.PRIVATE_MEMBER = "Automation Sunny");
  }

  getUserCredentials(isTestEnv) {
    return {
      admin: {
        username: isTestEnv ? this.LOGIN_TEST_DATA.ADMIN_USERNAME : this.LOGIN_PREPROD_DATA.ADMIN_USERNAME,
        password: isTestEnv ? this.PASSWORD : this.LOGIN_PREPROD_DATA.ADMIN_PASSWORD,
      },
      spAdmin: {
        username: isTestEnv ? this.LOGIN_TEST_DATA.SPADMIN_USERNAME : this.LOGIN_PREPROD_DATA.SPADMIN_USERNAME,
        password: isTestEnv ? this.PASSWORD : this.LOGIN_PREPROD_DATA.SPADMIN_PASSWORD,
      },
      spUser: {
        username: isTestEnv ? this.LOGIN_TEST_DATA.SPUSER_USERNAME : this.LOGIN_PREPROD_DATA.SPUSER_USERNAME,
        password: isTestEnv ? this.PASSWORD : this.LOGIN_PREPROD_DATA.SPUSER_PASSWORD,
      },
      corporateUser: {
        username: isTestEnv ? this.LOGIN_TEST_DATA.CORPORATEUSER_USERNAME : this.LOGIN_PREPROD_DATA.CORPORATEUSER_USERNAME,
        password: isTestEnv ? this.PASSWORD : this.LOGIN_PREPROD_DATA.CORPORATEUSER_PASSWORD,
      },
      internalUser: {
        username: isTestEnv ? this.LOGIN_TEST_DATA.INTERNALUSER_USERNAME : this.LOGIN_PREPROD_DATA.INTERNALUSER_USERNAME,
        password: isTestEnv ? this.PASSWORD : this.LOGIN_PREPROD_DATA.INTERNALUSER_PASSWORD,
      },
    };
  }
}

// Export Singleton Instance
const resources = new CommonTestResources();
export default resources;
