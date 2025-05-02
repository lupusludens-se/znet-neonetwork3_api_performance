# NEO Network 3.0 Database 

# MySQL (Wordpress)
---

Leveraging VPN the IP address of the Azure Flexible MySQL server instance can be leveraged. 

## Development
---

* __KeyVault Connection String__: TBD
* __IP Address__: 172.22.164.132
* __DatabaseName__: neonetwork_wp_dev
* __UserName__: neonetwork_dev

## Test
---

* __KeyVault Connection String__: TBD
* __IP Address__: 172.22.164.132
* __DatabaseName__: neonetwork_wp_tst
* __UserName__: neonetwork_tst

## Pre-Production (UAT)
---

TBD

## Production
---

TBD

# MSSQL
---

The MSSQL instance leverages a private link to ensure communication to the instance remains private.  Due to how this functions in Azure through an interface Gateway the domain resolution for the DNS record is abstracted and cannot be updated in local DNS.  Because of this an update to your local `hosts` file is required to ensure resolution to the appropriate private address is set appropriately when connecting locally.

__Host Update for DEV / TST Environments__

```
172.22.167.133 sbmssql.database.windows.net
```

__NOTE__: If you are using a Windows workstation you will need to open the file as an administrator.  The file is located at `C:\windows\system32\drivers\etc\hosts`

## Development
---

* __KeyVault Connection String__: This is stored in Azure KeyVault with the configuration `ConnectionStrings--DefaultConnection` and will be automatically injected into the application.
* __IP Address__: 172.22.167.133
* __DatabaseName__: sbsolutions
* __SchemaName__: neonetwork_dev

## Test
---

* __KeyVault Connection String__: This is stored in Azure KeyVault with the configuration `ConnectionStrings--DefaultConnection` and will be automatically injected into the application.
* __IP Address__: 172.22.167.133
* __DatabaseName__: sbsolutions
* __SchemaName__: neonetwork_tst

## Pre-Production (UAT)

TBD

## Production

TBD

# Migrations

To run migrations on the environments you can leverage the `DevOps` pipeline created.  This pipeline job will execute and run Entity migrations leveraging the `dotnet` tool kit.

[Migrations Job](https://dev.azure.com/SchneiderElectric-ESS/NEO/_build?definitionId=544)

The migrations job is `branch` driven.  If you would like to run migrations in the development environment run the job by selecting the `development` branch.