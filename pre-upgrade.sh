#!/bin/bash

set -euo pipefail

# Restore the dotnet tools
echo "Restoring dotnet tools"
dotnet tool restore

# Run the migrations
echo "Running the migrations"
dotnet ef database update --verbose

echo "Pre-upgrade script completed successfully"