// @ts-check
const { defineConfig, devices } = require('@playwright/test');
const path = require("path");
const dotenv = require("dotenv");

/**
 * Read environment variables from file.
 * https://github.com/motdotla/dotenv
 */

if (path.resolve("env")) {
  dotenv.config({
    path: process.cwd() + "/automation-script-new/.env",
  });
}

const resources = require("./utils/CommonTestResources.js");

/**
 * @see https://playwright.dev/docs/test-configuration
 */
module.exports = defineConfig({
  testDir: './',
  /* Run tests in files in parallel */
  fullyParallel: true,
  /* Fail the build on CI if you accidentally left test.only in the source code. */
  forbidOnly: !!process.env.CI,
  /* Retry on CI only */
  retries: process.env.CI ? 2 : 0,
  /* Opt out of parallel tests on CI. */
  workers: process.env.CI ? 1 : undefined,
  /* Maximum time one test can run for. */
  timeout: 35 * 10000,
  /* Reporter configuration */
  reporter: [
    ['html', { 
      outputFolder: './playwright-report',
      open: 'never' 
    }],
    ['list']  // Also show results in console
  ],
  outputDir: './test-results',
  /* Shared settings for all the projects below. See https://playwright.dev/docs/api/class-testoptions. */
  use: {
    launchOptions: {
      args: ['--remote-debugging-port=9222']
    },
    /* Maximum time each action such as `click()` can take. Defaults to 0 (no limit). */
    actionTimeout: 30000,
    navigationTimeout: 30000,
    ignoreHTTPSErrors: true,
    /* Base URL to use in actions like `await page.goto('/')`. */
    baseURL: resources.TEST_URL,
    /* Collect trace when retrying the failed test. See https://playwright.dev/docs/trace-viewer */
    trace: 'on-first-retry',
    video: "on-first-retry",
    screenshot: "only-on-failure",
    locale: "en-IN",
    timezoneId: "Asia/Kolkata",
    permissions: ["geolocation"],
    viewport: { width: 1280, height: 720 },
    headless: false,
  },
  projects: [
    {
      name: 'chromium',
      use: {
        browserName: 'chromium',
      },
      testMatch: [
        "automation-script-new/tests/**/*.spec.js",
        "automation-script-new/api-tests/specs/*.spec.js",
        "automation-script-new/performance-tests/*.spec.js"
      ],
      grepInvert: process.env.CI ? /@not_parallel/ : undefined,
    },
    {
      name: 'lighthouse',
      use: {
        browserName: 'chromium',
        channel: 'chrome',
        launchOptions: {
          args: ['--remote-debugging-port=9222']
        },
      },
    }
  ],
});
