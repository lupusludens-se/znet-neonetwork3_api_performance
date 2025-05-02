// create node new project command:
npm init playwright

// "playwright.config.js" is a test runner , all the configration of the test are declered heare
// "package.json" contains playwright dependencies and this file should be available 
// "package-lock.json" ignorable file for us it is related to package.json file 
// All the playwright related jars are installed in node_module 


Inside that directory, you can run several commands:
//  Runs the end-to-end tests.(npx is path which will point to the path of playwright module in node_module)
  npx playwright test
  npx playwright test --headed 
   
// Starts the interactive UI mode.
  npx playwright test --ui
    
//  Runs the tests only on Desktop Chrome.
  npx playwright test --project=chromium
   
// Runs the tests in a specific file.
  npx playwright test example
    
// Runs the tests in debug mode.
  npx playwright test --debug
    
// Auto generate tests with Codegen.
  npx playwright codegen

// .
  npm install packageName --savedev
    
