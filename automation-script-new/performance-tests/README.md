# Performance Testing with Lighthouse

## Overview
This directory contains automated performance tests using Playwright and Lighthouse for the Schneider Electric NEO application. These tests evaluate critical aspects of web performance, accessibility, best practices, and SEO.

## Test Structure
```
performance-tests/
├── test-reports/     # Generated test reports (HTML, JSON, CSV)
├── playwright-lighthouse-mcp.js    # MCP configuration for Lighthouse
└── ZeigoPerformanceLighthouse.spec.js    # Performance test specifications
```

## Performance Metrics & Thresholds
The tests evaluate four key areas with the following minimum thresholds:

| Metric         | Threshold | Description |
|----------------|-----------|-------------|
| Performance    | 50        | Overall performance score |
| Accessibility  | 80        | Web accessibility compliance |
| Best Practices | 80        | Web development best practices |
| SEO            | 75        | Search engine optimization |

### Key Metrics Explained
Each score is composed of several key metrics:
- Performance: First Contentful Paint, Time to Interactive, Speed Index
- Accessibility: ARIA attributes, color contrast, keyboard navigation
- Best Practices: HTTPS usage, error handling, browser compatibility
- SEO: Meta descriptions, crawlability, mobile friendliness

### Troubleshooting Failed Thresholds
If scores fall below thresholds:
1. Performance < 50: Check image optimization, caching, and server response times
2. Accessibility < 80: Review ARIA labels, heading structure, and color contrasts
3. Best Practices < 80: Verify security headers, console errors, and deprecated APIs
4. SEO < 75: Check meta tags, semantic HTML, and responsive design

## Configuration
The tests are configured in `playwright-lighthouse-mcp.js` with:
- Chrome debugging port: 9222
- Desktop viewport: 1350x940
- Network throttling:
  - RTT: 40ms
  - Throughput: 10240 Kbps
  - CPU slowdown: 1x

## Reports
Test results are automatically generated in `test-reports/` in three formats:
- HTML: Visual report with detailed metrics
- JSON: Raw data for programmatic analysis
- CSV: Tabular data for spreadsheet analysis

## Running Tests
To run the performance tests:

```powershell
cd C:\Users\SESA751855\Documents\GitHub\working_folder\znet-neonetwork3\automation-script-new\performance-tests
npx playwright test ZeigoPreProdtUrlPerformanceLighthouse.spec.js
npx playwright test ZeigoTestUrlPerformanceLighthouse.spec.js
```

## Interpreting Results
- Tests pass if all metrics meet or exceed their thresholds
- Review generated reports in `test-reports/` for detailed analysis
- HTML reports provide visual breakdowns and improvement suggestions

To view the test reports, use these commands:
```powershell
# Open HTML reports
cd 
C:\Users\SESA751855\Documents\GitHub\working_folder\znet-neonetwork3\automation-script-new\pe
rformance-tests
Invoke-Item "C:\Users\SESA751855\Documents\GitHub\working_folder\znet-neonetwork3_api_performance\automation-script-new\performance-tests\test-reports\lighthouse-test-report.html"
Invoke-Item "C:\Users\SESA751855\Documents\GitHub\working_folder\znet-neonetwork3_api_performance\automation-script-new\performance-tests\test-reports\lighthouse-preprod-report.html"

# Or from the performance-tests directory
cd "C:\Users\SESA751855\Documents\GitHub\working_folder\znet-neonetwork3_api_performance\automation-script-new\performance-tests"
Invoke-Item "test-reports\lighthouse-test-report.html"
Invoke-Item "test-reports\lighthouse-preprod-report.html"

# View JSON reports
Invoke-Item "C:\Users\SESA751855\Documents\GitHub\working_folder\znet-neonetwork3_api_performance\automation-script-new\performance-tests\test-reports\lighthouse-test-report.json"
```

## Maintenance
When updating tests:
1. Maintain thresholds in `playwright-lighthouse-mcp.js`
2. Keep reports organized in `test-reports/`
3. Follow existing patterns for new test cases 