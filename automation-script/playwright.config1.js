// @ts-check
const { defineConfig, devices } = require('@playwright/test');

module.exports = defineConfig({
  testDir: './tests',
  retries: 0,        // re-executing the failed test cases
  workers:1,

  timeout: 35*10000,
  expect:{
    timeout:350000
  },

  reporter: 'html',
  projects: [

    {
      name: 'Safari',
      use: {

        browserName: 'webkit',
        headless: false,
        screenshot: 'only-on-failure',
        trace: 'on',
        ignoreHTTPSErrors: true,  // this will ignore all kind of http errors
        permissions:['geolocation'],
        viewport:{width:1536,height:730},
        // ...devices['iPad Pro 11'],  // running test on mobile devices
        launchOptions: {
          slowMo: 1000
        }
       
      }
    },
    {
      name: 'Chrome',
      use: {

        browserName: 'chromium',
        headless: false,
        screenshot: 'on',
        trace: 'on',
        video:'retain-on-failure', // it will take the video incase of not passing the test cases
        ignoreHTTPSErrors: true,  // this will ignore all kind of http errors
        permissions:['geolocation'],
        viewport:{width:1530,height:730},
        launchOptions: {
          slowMo: 1350
        }
       
      }
    }
  ]
  


 
 
});

