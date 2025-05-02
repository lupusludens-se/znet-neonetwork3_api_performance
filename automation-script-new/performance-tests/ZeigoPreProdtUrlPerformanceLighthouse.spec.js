// @ts-check
const { test } = require('@playwright/test');
const resources = require('../utils/CommonTestResources.js');

test('Zeigo Network PreProd Performance Audit', async ({ page }) => {
  // Navigate to the page you want to test using the preprod URL
  const baseUrl = process.env.PREPROD_URL || 'https://network-pre.zeigo.com';
  const targetUrl = new URL('/zeigonetwork/preprod/dashboard', baseUrl).toString();
  await page.goto(targetUrl);

  // Import playwright-lighthouse
  const { playAudit } = await import('playwright-lighthouse');
  
  // Run Lighthouse audit
  const results = await playAudit({
    page: page,
    thresholds: {
      performance: 50,
      accessibility: 80,
      'best-practices': 80,
      seo: 75  // Adjusted based on current baseline
    },
    port: 9222,
    reports: {
      formats: {
        html: true,
        json: true,
        csv: true
      },
      name: 'lighthouse-preprod-report',
      directory: 'C:\\Users\\SESA751855\\Documents\\GitHub\\working_folder\\znet-neonetwork3_api_performance\\automation-script-new\\performance-tests\\test-reports'  // Windows path with escaped backslashes
    },
    config: {
      extends: 'lighthouse:default',
      settings: {
        formFactor: 'desktop',
        screenEmulation: {
          mobile: false,
          width: 1350,
          height: 940,
          deviceScaleFactor: 1,
          disabled: false
        },
        throttling: {
          rttMs: 40,
          throughputKbps: 10240,
          cpuSlowdownMultiplier: 1,
          requestLatencyMs: 0,
          downloadThroughputKbps: 0,
          uploadThroughputKbps: 0
        }
      }
    }
  });

  // Log the results
  console.log('Lighthouse PreProd Results:', results);
}); 