# Zeigo Network API Performance Testing

## Overview
This repository contains automated tests for evaluating the performance and functionality of Zeigo Network's API services. The tests include both API functionality verification and performance benchmarking using Lighthouse.

## Project Structure
- `automation-script-new/` - Main directory containing all test automation code
  - `api-tests/` - API functionality and performance tests
  - `performance-tests/` - Lighthouse-based web performance tests
  - `tests/` - End-to-end UI tests using Playwright

## Documentation
For detailed documentation on test setup and execution:
- [API Testing Documentation](automation-script-new/api-tests/README.md)
- [Performance Testing Documentation](automation-script-new/performance-tests/README.md)
- [Main Testing Documentation](automation-script-new/README.md)

## Quick Start
1. Navigate to `automation-script-new` directory
2. Install dependencies: `npm install`
3. Run API tests: `npx playwright test api-tests/specs/`
4. Run performance tests: `npx playwright test performance-tests/`

For more detailed instructions, see the documentation in the respective directories.
