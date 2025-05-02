# What are we using

The Zeigo Network application uses Playwright to write end-to-end (E2E) tests.
E2E tests should be written from the end user's perspective to simulate real user scenarios.

# End-to-End Testing by Playwright

Playwright UI test framework that enables reliable end-to-end testing for modern web apps.

Features:

- Supports cross-browser testing.
- Supports cross-platform testing.
- Support for cross-languages(TS, JS, Python, .NET, Java)
- Test Mobile Web.

# Playwright docs

- [Playwright](https://playwright.dev/docs/intro)
- [How to run Playwright tests](https://playwright.dev/docs/running-tests)

# Dependencies and instructions on how to execute tests

## Node

Download and install **NodeJS v20+**

### Dependencies

From `automation-script-new` directory, install Playwright locally via command-line (generally done once)

```
npm install -D @playwright/test
```

Install new browsers via Playwright command-line (generally done once)

```
npx playwright install
```

Install node modules in `automation-script-new` directory. Re-install `node_modules` anytime a dependency is updated in `package.json`.

```
npm install
```

Create .env file in the `automation-script-new` directory and set certain variables as dependencies (reach out to QA team for the content of .env file)
The .env file should should contain the following information:

TEST_URL=\
PREPROD_URL=\
PASSWORD=

### LoginTestData

TEST_ADMIN_USERNAME=\
TEST_SPADMIN_USERNAME=\
TEST_SPUSER_USERNAME=\
TEST_CORPORATE_USERNAME=\
TEST_INTERNAL_USERNAME=

### LoginPreProdData

PREPROD_ADMIN_USERNAME=\
PREPROD_SPADMIN_USERNAME=\
PREPROD_CORPORATE_USERNAME=

TARGET_ENV=

The test environment is determined dynamically based on TARGET_ENV in the .env file, which is set in the config file by baseURL:
To run in TEST env., set: TARGET_ENV=test (it can be "test" or left undefined (empty)—TEST is the default if not set.)
To run in PREPROD env., set: TARGET_ENV=preprod (it can be any value other than "test", like "t", "staging", etc.)
No need to change baseURL manually—Playwright automatically selects the correct environment!
also, run from `automation-script-new` directory

```
npm install dotenv
```

## Run specific test

```
// notice the test title is "name of my test"
test("name of my test", async ({ page }) => {});
```

### <code style="color :rgb(38, 141, 24)">Windows PowerShell</code> (capitalization doesn't matter)

```
$env:target_env="test"; npx playwright test -g "name of my test" --project=chromium; remove-item env:\target_env
```

```
$env:target_env="preprod"; npx playwright test -g "name of my test" --project=chromium; remove-item env:\target_env
```

### <code style="color : rgb(38, 141, 24)">macOS bash</code>

```
target_env="test" npx playwright test -g "name of my test" --project=chromium
```

```
target_env="preprod" npx playwright test -g "name of my test" --project=chromium
```

---

### To execute tests locally in headed mode with test results

From comamand-line, export `CI` environment variable to run retries and additional workers

```
Mac: export CI=1 
Windows: set CI=1
```

Full UI suite in headless

```
npm run regression
```

### In headless or headed mode

To execute all Project creation spec files in headless mode:

```
$env:CI="true"; $env:target_env='test'; npx playwright test --grep "@all_projects_parallel" --reporter=list
```
OR in preprod environmend:
```
$env:CI="true"; $env:target_env='preprod'; npx playwright test --grep "@all_projects_parallel" --reporter=list
```

To execute Project creation spec files tagged by @core_projects in headless mode:

```
$env:CI="true"; $env:target_env='test'; npx playwright test --grep "@core_projects" --reporter=list
```
OR in preprod environmend:
```
$env:CI="true"; $env:target_env='preprod'; npx playwright test --grep "@core_projects" --reporter=list
```

To execute Project creation spec files tagged by @secondary_projects in headless mode:

```
$env:CI="true"; $env:target_env='test'; npx playwright test --grep "@secondary_projects" --reporter=list
```
OR in preprod environmend:
```
$env:CI="true"; $env:target_env='preprod'; npx playwright test --grep "@secondary_projects" --reporter=list
```

To execute Project creation spec files tagged by @small_projects in headless mode:

```
$env:CI="true"; $env:target_env='test'; npx playwright test --grep "@small_projects" --reporter=list
```
OR in preprod environmend:
```
$env:CI="true"; $env:target_env='preprod'; npx playwright test --grep "@small_projects" --reporter=list
```

To execute Project creation spec files tagged by @core_projects and @secondary_projects together in headless mode:

```
$env:CI="true"; $env:target_env='test'; npx playwright test --grep "@core_projects|@secondary_projects" --reporter=list
```
OR in preprod environmend:
```
$env:CI="true"; $env:target_env='preprod'; npx playwright test --grep "@core_projects|@secondary_projects" --reporter=list
```

To execute a specific Project creation spec file in headless mode:

```
$env:CI="true"; $env:target_env='test'; npx playwright test tests/project-types-tests/BatteryStorage.spec.js --reporter=list
```
OR in preprod environmend:
```
$env:CI="true"; $env:target_env='preprod'; npx playwright test tests/project-types-tests/BatteryStorage.spec.js --reporter=list
```

To execute tests locally in headed mode against an environment defined in `.env`

```
npm run regression:headed
```

In Chrome

```
npm run regression:chromium
```

In Safari

```
npm run regression:safari
```

For all other specific browser and mobile device execution, please see `package.json` for a list of commands.

---

// create node new project command:

```
npm init playwright
```

// "playwright.config.js" is a test runner , all the configuration of the test are declared here
// "package.json" contains playwright dependencies and this file should be available
// "package-lock.json" ignorable file for us it is related to package.json file
// All the playwright related jars are installed in node_module

Inside that directory, you can run several commands:
// Runs the end-to-end tests.(npx is path which will point to the path of playwright module in node_module)

```
npx playwright test
npx playwright test --headed
```

// Runs the tests only on Desktop Chrome.

```
npx playwright test --project=chromium
```

// Runs the tests in a specific file.

```
npx playwright test example
```

// Runs the tests in debug mode.

```
npx playwright test --debug
```

// Auto generate tests with Codegen.

```
npx playwright codegen
```

// .

```
npm install packageName --savedev
```

# MCP Functions

## Playwright MCP Server Functions (29 total)
1. `playwright_navigate` - Navigate to a URL
2. `playwright_close` - Close browser
3. `playwright_custom_user_agent` - Set custom User Agent
4. `playwright_go_back` - Go back in history
5. `playwright_go_forward` - Go forward in history
6. `playwright_click` - Click elements
7. `playwright_iframe_click` - Click in iframes
8. `playwright_fill` - Fill form fields
9. `playwright_select` - Select from dropdowns
10. `playwright_hover` - Hover over elements
11. `playwright_drag` - Drag and drop
12. `playwright_press_key` - Keyboard input
13. `playwright_screenshot` - Take screenshots
14. `playwright_save_as_pdf` - Save as PDF
15. `playwright_get_visible_text` - Get page text
16. `playwright_get_visible_html` - Get page HTML
17. `playwright_evaluate` - Run JavaScript
18. `playwright_console_logs` - Get console logs
19. `playwright_get` - HTTP GET
20. `playwright_post` - HTTP POST
21. `playwright_put` - HTTP PUT
22. `playwright_patch` - HTTP PATCH
23. `playwright_delete` - HTTP DELETE
24. `playwright_expect_response` - Wait for response
25. `playwright_assert_response` - Validate response
26. `playwright_start_codegen_session` - Start recording
27. `playwright_end_codegen_session` - End recording
28. `playwright_get_codegen_session` - Get session info
29. `playwright_clear_codegen_session` - Clear session

## Context7 MCP Server Functions (2 total)
1. `mcp_context7_resolve-library-id` - Resolves a package name into Context7-compatible library ID
2. `mcp_context7_get-library-docs` - Fetches documentation for a library using the resolved ID

# GitHub MCP Server Integration

The project now includes GitHub MCP Server integration for streamlined development workflow and test automation.

Features:

- Direct repository management from your IDE
- Automated pull request workflows
- Integrated issue tracking
- Code search and management
- Automated file operations

## GitHub MCP Server Commands

### Repository Operations

```
// Create a new repository
npx playwright test --grep "@github-create-repo"

// Fork an existing repository
npx playwright test --grep "@github-fork-repo"
```

### Pull Request Management

```
// Create a pull request
$env:CI="true"; npx playwright test --grep "@github-create-pr"

// Review and merge pull requests
$env:CI="true"; npx playwright test --grep "@github-manage-pr"
```

### Issue Tracking

```
// Create and manage issues
npx playwright test --grep "@github-issue-management"

// Search through issues
npx playwright test --grep "@github-search-issues"
```

### File Operations

```
// Update multiple files in a single commit
npx playwright test --grep "@github-batch-update"

// Get repository contents
npx playwright test --grep "@github-get-contents"
```

For more detailed information about GitHub MCP Server integration, refer to the official documentation or contact the development team.

## Sample Prompts for GitHub MCP Server

### Repository Management Prompts

```
"Create a new private repository named 'my-test-automation' with README"
"Fork repository 'username/existing-repo' to my account"
"Create a new branch called 'feature/automated-tests' in 'owner/repo'"
"Get the contents of the test configuration file"
```

### Pull Request Workflow Prompts

```
"Create a PR from 'feature/automated-tests' to 'main' branch"
"Show all files changed in PR #123"
"Review and approve pull request #456 with comment 'LGTM'"
"Merge PR #789 using squash merge strategy"
```

### Issue Management Prompts

```
"Create issue 'Fix flaky test in login suite'"
"Add comment to issue #123 with latest test results"
"List all open issues with label 'automation'"
"Update issue #456 to add QA team as assignees"
```

### Code Search and File Operations Prompts

```
"Search for test files using 'describe' block"
"Find code examples using 'test.beforeAll' hook"
"Update test configuration files in batch"
"Push new test files to 'test/e2e' directory"
```

### Combined Workflow Prompts

```
"Create branch, add new test files, and create PR for review"
"Search for existing test issues, create new if none exist"
"Fork repo, add automation scripts, and submit PR"
```

Note: These prompts can be used directly with the GitHub MCP Server integration to automate your workflow. Adjust repository names, branch names, and other parameters as needed for your specific use case.
