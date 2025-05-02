# API Tests Documentation

## Overview
This directory contains API tests for the NEO Network application using Playwright test framework.

## Project Structure
```
api-tests/
├── core/
│   └── ApiCore.js         # Core API testing functionality
├── specs/
│   ├── ArticlesApiTest.spec.js
│   ├── EventApiTest.spec.js
│   ├── NetworkStatsTest.spec.js
│   └── SampleApiTest.spec.js
├── data/
│   └── builder/
│       ├── compareTokens.js
│       ├── ContactUsDataBuilder.js
│       └── EventsDataBuilder.js
└── README.md
```

## Prerequisites
- Node.js (v14 or higher)
- npm (v6 or higher)
- Visual Studio Code with Playwright Test Explorer

## Setup and Installation
1. Navigate to the project root:
   ```bash
   cd automation-script-new
   ```

2. Install dependencies:
   ```bash
   npm install playwright @playwright/test mocha chai axios --save-dev
   ```

## Running Tests

### Via Command Line
```bash
# Run all API tests
cd api-tests
npx playwright test

# Run specific test files
npx playwright test specs/ArticlesApiTest.spec.js
npx playwright test specs/EventApiTest.spec.js
npx playwright test specs/NetworkStatsTest.spec.js
npx playwright test specs/SampleApiTest.spec.js
```

### Via Visual Studio Code
1. Open the Testing sidebar (beaker icon)
2. Expand the API Tests section
3. Click the play button next to the test you want to run

## Authentication
- Bearer tokens are automatically managed by ApiCore
- To validate tokens: Use [jwt.io](https://jwt.io/)
- To compare tokens:
  ```bash
  cd api-tests
  node compareTokens.js
  ```

## Best Practices
1. Each test file should:
   - Have clear test descriptions
   - Include proper setup and cleanup
   - Handle token refresh automatically
   - Log important test steps

2. API requests should:
   - Include proper error handling
   - Use retry mechanisms for flaky calls
   - Log request/response details when needed

## Troubleshooting
1. Authentication Issues
   - Verify your credentials
   - Check token expiration
   - Ensure proper permissions

2. Test Failures
   - Check network connectivity
   - Review test logs in test-results/
   - Verify API endpoints are accessible

3. Common Solutions
   - Clear browser cache and cookies
   - Update bearer token
   - Check API service status

## Reports
Test reports are generated in:
- HTML: `playwright-report/index.html`
- Test results: `test-results/`
