// @ts-check
import { defineConfig } from '@playwright/test';

export default defineConfig({
  testDir: './',
  use: {
    baseURL: 'https://example.com',
    headless: false,
  },
  projects: [
    {
      name: 'lighthouse',
      use: {
        // Chrome-only
        browserName: 'chromium',
        channel: 'chrome',
        launchOptions: {
          args: ['--remote-debugging-port=9222']
        },
      },
    },
  ],
  reporter: 'html',
}); 