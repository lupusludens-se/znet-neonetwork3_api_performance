import { BasePage } from "../shared/BasePage.js";
import { faker } from "@faker-js/faker";

export class CreateNewProjectRegionsPage extends BasePage {
  constructor(page) {
    super(page);
    this.page = page;
    // Add Project -> Region buttons
    const regionNames = ["Africa", "Asia", "Europe", "Mexico & Central America", "Oceania", "South America", "USA & Canada"];
    this.regions = {};
    for (const name of regionNames) {
      this.regions[name] = page.getByRole("button", { name, exact: true });
    }
    // Add Project -> Geography search textbox
    this.SearchCountriesStatesTxtBox = page.getByRole("textbox", {
      name: "Search Countries / States",
    });
    this.countries = {};
    const countryNames = [
      "US - All",
      "Afghanistan",
      "Albania",
      "Algeria",
      "Angola",
      "Anguilla",
      "Antigua and Barbuda",
      "Argentina",
      "Armenia",
      "Australia",
      "Austria",
      "Azerbaijan",
      "Bahamas",
      "Bahrain",
      "Bangladesh",
      "Barbados",
      "Belarus",
      "Belgium",
      "Belize",
      "Benin",
      "Bhutan",
      "Bolivia",
      "Bosnia and Herzegovina",
      "Botswana",
      "Brazil",
      "Bulgaria",
      "Burkina Faso",
      "Burundi",
      "Cambodia",
      "Cameroon",
      "Canada",
      "Cayman Islands",
      "Central African Republic",
      "Chad",
      "Chile",
      "China",
      "Colombia",
      "Congo",
      "Costa Rica",
      "Côte d'Ivoire",
      "Croatia",
      "Cuba",
      "Curaçao",
      "Cyprus",
      "Czech Republic",
      "Democratic Republic of the Congo",
      "Denmark",
      "Djibouti",
      "Dominica",
      "Dominican Republic",
      "Ecuador",
      "Egypt",
      "El Salvador",
      "Equatorial Guinea",
      "Eritrea",
      "Estonia",
      "Ethiopia",
      "Fiji",
      "Finland",
      "France",
      "French Guiana",
      "Gabon",
      "Gambia",
      "Georgia",
      "Germany",
      "Ghana",
      "Greece",
      "Greenland",
      "Grenada",
      "Guadeloupe",
      "Guatemala",
      "Guinea",
      "Guinea-Bissau",
      "Guyana",
      "Haiti",
      "Honduras",
      "Hong Kong",
      "Hungary",
      "Iceland",
      "India",
      "Indonesia",
      "Iran",
      "Iraq",
      "Ireland",
      "Israel",
      "Italy",
      "Jamaica",
      "Japan",
      "Jordan",
      "Kazakhstan",
      "Kenya",
      "Kuwait",
      "Kyrgyzstan",
      "Laos",
      "Latvia",
      "Lebanon",
      "Lesotho",
      "Liberia",
      "Libya",
      "Liechtenstein",
      "Lithuania",
      "Luxembourg",
      "Macedonia",
      "Madagascar",
      "Malawi",
      "Malaysia",
      "Maldives",
      "Mali",
      "Martinique",
      "Mauritania",
      "Mauritius",
      "Mexico",
      "Moldova",
      "Monaco",
      "Mongolia",
      "Montenegro",
      "Montserrat",
      "Morocco",
      "Mozambique",
      "Myanmar",
      "Namibia",
      "Nepal",
      "Netherlands",
      "New Caledonia",
      "New Zealand",
      "Nicaragua",
      "Niger",
      "Nigeria",
      "North Korea",
      "Norway",
      "Oman",
      "Pakistan",
      "Panama",
      "Papua New Guinea",
      "Paraguay",
      "Peru",
      "Philippines",
      "Poland",
      "Portugal",
      "Puerto Rico",
      "Qatar",
      "Romania",
      "Russia",
      "Rwanda",
      "Saint Kitts and Nevis",
      "Saint Martin, FR",
      "Saint Vincent and the Grenadines",
      "Saudi Arabia",
      "Senegal",
      "Serbia",
      "Sierra Leone",
      "Singapore",
      "Slovakia",
      "Slovenia",
      "Solomon Islands",
      "Somalia",
      "South Africa",
      "South Korea",
      "South Sudan",
      "Spain",
      "Sri Lanka",
      "Sudan",
      "Suriname",
      "Svalbard and Jan Mayen Islands",
      "Swaziland",
      "Sweden",
      "Switzerland",
      "Syria",
      "Taiwan",
      "Tajikistan",
      "Tanzania",
      "Thailand",
      "Timor-Leste",
      "Togo",
      "Trinidad and Tobago",
      "Tunisia",
      "Turkey",
      "Turkmenistan",
      "Turks and Caicos Islands",
      "Uganda",
      "Ukraine",
      "United Arab Emirates",
      "United Kingdom",
      "Uruguay",
      "US - Alabama",
      "US - Alaska",
      "US - Arizona",
      "US - Arkansas",
      "US - California",
      "US - Colorado",
      "US - Connecticut",
      "US - D.C.",
      "US - Delaware",
      "US - Florida",
      "US - Georgia",
      "US - Hawaii",
      "US - Idaho",
      "US - Illinois",
      "US - Indiana",
      "US - Iowa",
      "US - Kansas",
      "US - Kentucky",
      "US - Louisiana",
      "US - Maine",
      "US - Maryland",
      "US - Massachusetts",
      "US - Michigan",
      "US - Minnesota",
      "US - Mississippi",
      "US - Missouri",
      "US - Montana",
      "US - Nebraska",
      "US - Nevada",
      "US - New Hampshire",
      "US - New Jersey",
      "US - New Mexico",
      "US - New York",
      "US - North Carolina",
      "US - North Dakota",
      "US - Ohio",
      "US - Oklahoma",
      "US - Oregon",
      "US - Pennsylvania",
      "US - Rhode Island",
      "US - South Carolina",
      "US - South Dakota",
      "US - Tennessee",
      "US - Texas",
      "US - Utah",
      "US - Vermont",
      "US - Virginia",
      "US - Washington",
      "US - West Virginia",
      "US - Wisconsin",
      "US - Wyoming",
      "Uzbekistan",
      "Vanuatu",
      "Venezuela",
      "Vietnam",
      "Virgin Islands, British",
      "Virgin Islands, U.S.",
      "Western Sahara",
      "Yemen",
      "Zambia",
      "Zimbabwe",
    ];
    // Initialize all country checkboxes dynamically
    for (const country of countryNames) {
      this.countries[country] = page
        .locator("span")
        .filter({ hasText: new RegExp(`^${country}$`, "i") }) // Exact match
        .locator("svg")
        .first();
    }
    // Navigation buttons
    this.backBtn = page.getByRole("button", { name: "Back" });
    this.cancelBtn = page.getByRole("button", { name: "Cancel" });
    this.nextBtn = page.getByRole("button", { name: "Next" });
  }

  /**
   * Click to remove the countries/states
   * @param {string} geographyName - name of the element to be found
   * @param {boolean} exact - default false means geographyName is NOT case-sensitive, true otherwise
   */
  async clickRemoveCountriesOrStates(geographyName, exact = false) {
    // UI X (close) button is dynamically created
    const xBtn = this.page
      .locator("xpath=//div[starts-with(@class, 'option') and contains(@class, 'selected-list')]/div")
      .getByText(geographyName, { exact: exact })
      .getByRole("button");
    await this.verifyVisibilityAndClick(xBtn);
  }

  /**
   * Assert that the button is visible
   * @param {string} geographyName - name of the element to be found
   * @param {boolean} exact - default false means geographyName is NOT case-sensitive, true otherwise
   */
  async verifyVisibilityOfRemoveCountriesOrStatesButton(geographyName, exact = false) {
    const xBtn = this.page
      .locator("xpath=//div[starts-with(@class, 'option') and contains(@class, 'selected-list')]/div")
      .getByText(geographyName, { exact: exact })
      .getByRole("button");
    await expect(xBtn, `ERROR: Countries State Removal button, [ ${geographyName} ] is NOT visible.`).toBeVisible();
  }

  async scrollToEnd(container) {
    await container.evaluate((node) => node.scrollTo(0, node.scrollHeight));
  }

  /**
   * Normalize country names by removing accents, special characters,
   * dashes, and converting to lowercase for easy comparison.
   * @param {string} name
   * @returns {string}
   */
  normalizeCountryName(name) {
    return name
      .normalize("NFD") // Split accents
      .replace(/[\u0300-\u036f]/g, "") // Remove accents
      .replace(/['’]/g, "") // Remove special apostrophes
      .replace(/-/g, " ") // Normalize dashes to spaces (optional)
      .trim()
      .toLowerCase();
  }

  /**
   * Find the correct country name from the list by comparing
   * normalized versions of the input and stored names.
   * @param {string} inputCountry
   * @returns {string|undefined}
   */
  findMatchingCountry(inputCountry) {
    const normalizedInput = this.normalizeCountryName(inputCountry);
    const allCountries = Object.keys(this.countries);
    return allCountries.find((c) => this.normalizeCountryName(c) === normalizedInput);
  }

  /**
   * Choose specific Region and Countries
   * @param {string[]} regionsName - Each string item needs to be in the format of "top-level (sub-level)"
   *     ["Africa (=All)"] means "Africa" will be selected, and every single sub-countries checkbox will be selected.
   *     ["USA & Canada (US-Hawaii, Puerto Rico)", "Europe", "Asia (Bangladesh)"] means that "USA & Canada", "Europe", "Asia" buttons will first be selected
   *          then the Display Selected Geographies: "US - Hawaii", "Puerto Rico", and "Bangladesh" checkboxes will be selected.
   */
  async selectRegionAndCountriesCheckboxes(regionsName) {
    if (regionsName && typeof regionsName === "string" && regionsName.trim().length > 0) {
      regionsName = [regionsName.trim()];
    } else if (
      // check not empty string
      (regionsName && typeof regionsName === "string" && regionsName.trim().length === 0) ||
      // check not empty array
      (regionsName && Array.isArray(regionsName) && regionsName.map((node) => node.trim()).filter((node) => node !== "").length === 0) ||
      // check if undefined or null
      !regionsName
    ) {
      throw new Error(`JSON file's (regions) property not defined`);
    }

    for (const region of regionsName) {
      const match = region.match(/(.+?)\s?\((.+?)\)/); // Extract region and country details
      let regionName = match ? match[1].trim() : region.trim();
      let countrySelection = match ? match[2].trim() : "";
      // Click the region button if it exists
      const regionLocator = this.regions[regionName];
      if (!regionLocator) {
        continue;
      }
      // Click region and wait for country checkboxes to load
      await this.verifyVisibilityAndClick(regionLocator);
      await this.page.locator(".options-wrapper").first().waitFor({ state: "visible", timeout: 3000 });
      await this.page.waitForTimeout(500);
      // Define list of countries to select
      let countriesToSelect;
      const countriesWithCommas = ["Virgin Islands, British", "Virgin Islands, U.S.", "Saint Martin, FR"];
      if (countrySelection === "=All") {
        countriesToSelect = Object.keys(this.countries);
      }
      // handle countries with commas to avoid mistakenly split into array
      else if (countriesWithCommas.some((node) => node.toLowerCase().includes(countrySelection.toLowerCase()))) {
        for (const name of countriesWithCommas) {
          // temporarily replace specific names from commas to semiconlons
          countrySelection = countrySelection.replace(new RegExp(name, "i"), name.replace(/,/i, ";"));
        }
        // after split into an array by commas, then restore the semicolons back to commas
        countriesToSelect = countrySelection.split(",").map((node) => node.trim().replace(/;/i, ",").replace(/-/g, " - "));
      } else {
        countriesToSelect = countrySelection.split(",").map((node) => node.trim().replace(/-/g, " - "));
      }
      // Shuffle order to optimize visibility
      countriesToSelect = faker.helpers.shuffle(countriesToSelect);
      for (const country of countriesToSelect) {
        const matchedCountry = this.findMatchingCountry(country); // Normalize input
        if (!matchedCountry) {
          continue;
        }
        const countryLocator = this.countries[matchedCountry];
        if (!countryLocator) {
          continue;
        }
        let count = await countryLocator.count();
        if (count === 0 || (await this.isCountrySelected(country))) {
          continue;
        }

        let clicked = false;
        let retries = 3;
        while (retries > 0) {
          // Scroll to country
          await countryLocator.scrollIntoViewIfNeeded();
          await this.page.waitForTimeout(80);
          // Ensure visible and enabled before click
          await this.waitForAndVerifyVisibility(countryLocator);
          await this.waitForAndVerifyEnabled(countryLocator);
          // Attempt click
          try {
            await countryLocator.click({ timeout: 8000 });
            await this.page.waitForTimeout(30);
            // Verify selection
            if (await this.isCountrySelected(country)) {
              clicked = true;
              break;
            }
          } catch (error) {
            throw error;
          }
          retries--;
        }
      }
      // Add delay before moving to next region
      await this.page.waitForTimeout(1000);
    }
  }

  // Smarter Scroll function: scroll until country is visible or max attempts
  async scrollToCountry(countryLocator) {
    const scrollContainer = this.page.locator(".options-wrapper").first().waitFor({ state: "visible", timeout: 3000 });
    await this.page.waitForTimeout(500);
    let scrollAttempts = 0;
    const maxScrollAttempts = 6;
    while (!(await countryLocator.isVisible()) && scrollAttempts < maxScrollAttempts) {
      await scrollContainer.evaluate((node) => node.scrollBy(0, 300));
      await this.page.waitForTimeout(300);
      scrollAttempts++;
    }
  }

  // Verify if the country checkbox was actually selected
  async isCountrySelected(country) {
    const countryLocator = this.page
      .locator("neo-blue-checkbox")
      .getByText(new RegExp(`^${country}$`, "i")) // Exact match
      .first();
    return await countryLocator.isChecked(); // Ensure it's actually selected
  }
}
