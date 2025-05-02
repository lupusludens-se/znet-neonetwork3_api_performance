// @ts-check
const { chromium } = require('playwright');

/**
 * MCP server for running Lighthouse performance tests
 */
async function runLighthouseTest(url, options = {}) {
  const browser = await chromium.launch({
    args: ['--remote-debugging-port=9222']
  });
  
  const context = await browser.newContext();
  const page = await context.newPage();
  
  try {
    await page.goto(url);
    
    const { playAudit } = await import('playwright-lighthouse');
    const results = await playAudit({
      page,
      thresholds: {
        performance: options.performance || 50,
        accessibility: options.accessibility || 80,
        'best-practices': options.bestPractices || 80,
        seo: options.seo || 75
      },
      port: 9222,
      reports: {
        formats: {
          html: true,
          json: true,
          csv: true
        },
        name: options.reportName || 'lighthouse-report',
        directory: './lighthouse-reports'
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
    
    return results;
  } finally {
    await browser.close();
  }
}

module.exports = {
  runLighthouseTest
}; 