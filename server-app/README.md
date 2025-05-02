# Schneider Electric NEO Server App

## Setup local development environment	
	
- request access to Abto Azure org (ask Oleh Kachmar)
	- will need secrets list permission on key vault 'neo-poc'
- request access to Bitbucket repo (ask Kevin Price)
- clone repo and switch to development branch
- add ASPNETCORE_ENVIRONMENT and APP_ENV both as a windows environment variable with the value of Local
- open VS
	- make sure the Azure account that was given access to Abto Azure org is selected as the account to use for development (Tools -> Options -> Azure Service Authentication)
	- open Neo SLN
- create local DB instance
	- make sure the LocalConnection connection string in appsettings.local.json has the correct Server name for your instance of SQL Server
	- open Package Manager Console
		- select "SE.Neo.WebAPI" in Default Project dropdown
		- execute the following statements:
			- Update-Database
- execute SQL scripts in SE.Neo.Core\Data\SQL
	- 1. init scripts.sql
		- some statements will error; this is OK
	- 2. Init Contries_States insert.sql
- create user account in DB
	- modify and execute the last 3 INSERT's in the 1. init scripts.sql script as-needed to create an Admin account
- sign up for user account here (using same email as previous step): https://app-dev.neonetworkexchange.com/neonetwork/dev/
	- have Doug approve sign up request
	- following approval, check email to set password and finish setting up the account
- initialize the timezones
	- load the timezones using EF DB intiializer (via the Web API)
		- uncomment the following line in SE.Neo.WebAPI.Program.cs
			// DbInitializer.Initialize(context);
		- run the web API and make sure the call to the DBInitializer is hit
		- stop API and re-comment the line
	- execute SQL script in SE.Neo.Core\Data\SQL
		- 5. set timezone abbreviation.sql
- initialize the taxonomies
	- Go to the Admin page, and click WP DataSync
	- execute SQL script in SE.Neo.Core\Data\SQL
		- 3. Taxonomy_dependency_creation_script.sql

## How to call the API directly from Swagger or Postman

Run and log in to the web app to get an auth token from one of the API calls (use F12 tools and copy Bearer token from one of the API network requests).

Go to this URL to access swagger, https://localhost:7203/neonetwork/local/api/index.html
Once you login, you will be able to call any of the APIs

There is also a Postman collection in GIT at docs\local-development
