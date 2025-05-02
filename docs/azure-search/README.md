# Azure Cognitive Search Setup

The following document describes how to setup Azure Cognitive Search.

To setup Azure Search you are required to do next steps:
- make sure migration "AddAzureSearchViews" is applied on the target database
- create [Search Service](#markdown-header-search-service)
- configure [Index](#markdown-header-index)
- configure [DataSources](#markdown-header-datasources)
- configure [Indexers](#markdown-header-indexers)
- update [Key Vault Secrets](#markdown-header-key-vault-secrets)

***Note: all names provided in this document just a recommendation. You can name services as you want, but make sure you know what you are doing ;)***

## Search Service

- go to the [Azure Portal](https://portal.azure.com/)
- in the search bar type and find **"Cognitive search"**
- create new search service and fill necessary information:
  - choose subscription details
  - add service name (e.g. neo-{environment}-global-search)
  - select location (e.g. West US). Recommend to select same region as your database located to lower delay for data sync.
  - select Pricing tier. Make sure selected plan **supports at least 6 indexers**
  - setup other optional parameters - Scale, Networking, Tags

## Index

In Azure Cognitive Search, a search index is your searchable content, available to the search engine for indexing, full text search, and filtered queries.

- Go to the [Azure Portal](https://portal.azure.com/)
- In the search bar type and find **"Cognitive search"**
- in the list of services find created service from step #Search Service and open it
- find tab "Indexes" and create new one:
  - add index name (e.g. neo-{environment}-global-search-index)
  - other options leave as default values
- open newly created index
- go to the tab "Index Definition (Json)"
- add next fields to the `fields` array:
```json
{
  ...
  "fields": [
    {
      "name": "id",
      "type": "Edm.String",
      "facetable": false,
      "filterable": false,
      "key": true,
      "retrievable": true,
      "searchable": false,
      "sortable": false,
      "analyzer": null,
      "indexAnalyzer": null,
      "searchAnalyzer": null,
      "synonymMaps": [],
      "fields": []
    },
    {
      "name": "originalId",
      "type": "Edm.Int32",
      "facetable": false,
      "filterable": false,
      "retrievable": true,
      "sortable": false,
      "analyzer": null,
      "indexAnalyzer": null,
      "searchAnalyzer": null,
      "synonymMaps": [],
      "fields": []
    },
    {
      "name": "subject",
      "type": "Edm.String",
      "facetable": false,
      "filterable": false,
      "key": false,
      "retrievable": true,
      "searchable": true,
      "sortable": false,
      "analyzer": "standard.lucene",
      "indexAnalyzer": null,
      "searchAnalyzer": null,
      "synonymMaps": [],
      "fields": []
    },
    {
      "name": "description",
      "type": "Edm.String",
      "facetable": false,
      "filterable": false,
      "key": false,
      "retrievable": true,
      "searchable": true,
      "sortable": false,
      "analyzer": "standard.lucene",
      "indexAnalyzer": null,
      "searchAnalyzer": null,
      "synonymMaps": [],
      "fields": []
    },
    {
      "name": "updatedAt",
      "type": "Edm.DateTimeOffset",
      "facetable": false,
      "filterable": true,
      "retrievable": true,
      "sortable": true,
      "analyzer": null,
      "indexAnalyzer": null,
      "searchAnalyzer": null,
      "synonymMaps": [],
      "fields": []
    },
    {
      "name": "entityType",
      "type": "Edm.Int32",
      "facetable": true,
      "filterable": true,
      "retrievable": true,
      "sortable": false,
      "analyzer": null,
      "indexAnalyzer": null,
      "searchAnalyzer": null,
      "synonymMaps": [],
      "fields": []
    },
    {
      "name": "isDeleted",
      "type": "Edm.Boolean",
      "facetable": false,
      "filterable": true,
      "retrievable": true,
      "sortable": false,
      "analyzer": null,
      "indexAnalyzer": null,
      "searchAnalyzer": null,
      "synonymMaps": [],
      "fields": []
    },
    {
      "name": "categories",
      "type": "Collection(Edm.ComplexType)",
      "analyzer": null,
      "synonymMaps": [],
      "fields": [
        {
          "name": "categoryId",
          "type": "Edm.Int32",
          "facetable": false,
          "filterable": true,
          "retrievable": true,
          "sortable": false,
          "analyzer": null,
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        },
        {
          "name": "name",
          "type": "Edm.String",
          "facetable": false,
          "filterable": false,
          "key": false,
          "retrievable": true,
          "searchable": true,
          "sortable": false,
          "analyzer": "standard.lucene",
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        }
      ]
    },
    {
      "name": "solutions",
      "type": "Collection(Edm.ComplexType)",
      "analyzer": null,
      "synonymMaps": [],
      "fields": [
        {
          "name": "solutionId",
          "type": "Edm.Int32",
          "facetable": false,
          "filterable": true,
          "retrievable": true,
          "sortable": false,
          "analyzer": null,
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        },
        {
          "name": "name",
          "type": "Edm.String",
          "facetable": false,
          "filterable": false,
          "key": false,
          "retrievable": true,
          "searchable": true,
          "sortable": false,
          "analyzer": "standard.lucene",
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        }
      ]
    },
    {
      "name": "technologies",
      "type": "Collection(Edm.ComplexType)",
      "analyzer": null,
      "synonymMaps": [],
      "fields": [
        {
          "name": "technologyId",
          "type": "Edm.Int32",
          "facetable": false,
          "filterable": true,
          "key": false,
          "retrievable": true,
          "searchable": false,
          "sortable": false,
          "analyzer": null,
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        },
        {
          "name": "name",
          "type": "Edm.String",
          "facetable": false,
          "filterable": false,
          "key": false,
          "retrievable": true,
          "searchable": true,
          "sortable": false,
          "analyzer": "standard.lucene",
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        }
      ]
    },
    {
      "name": "regions",
      "type": "Collection(Edm.ComplexType)",
      "analyzer": null,
      "synonymMaps": [],
      "fields": [
        {
          "name": "regionId",
          "type": "Edm.Int32",
          "facetable": false,
          "filterable": true,
          "retrievable": true,
          "sortable": false,
          "analyzer": null,
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        },
        {
          "name": "name",
          "type": "Edm.String",
          "facetable": false,
          "filterable": false,
          "key": false,
          "retrievable": true,
          "searchable": true,
          "sortable": false,
          "analyzer": "standard.lucene",
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        }
      ]
    },
    {
      "name": "allowedRoles",
      "type": "Collection(Edm.ComplexType)",
      "analyzer": null,
      "synonymMaps": [],
      "fields": [
        {
          "name": "allowedRoleId",
          "type": "Edm.String",
          "facetable": false,
          "filterable": true,
          "retrievable": true,
          "sortable": false,
          "analyzer": null,
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        }
      ]
    },
    {
      "name": "allowedCompanies",
      "type": "Collection(Edm.ComplexType)",
      "analyzer": null,
      "synonymMaps": [],
      "fields": [
        {
          "name": "allowedCompanyId",
          "type": "Edm.String",
          "facetable": false,
          "filterable": true,
          "retrievable": true,
          "sortable": false,
          "analyzer": null,
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        }
      ]
    },
    {
      "name": "allowedRegions",
      "type": "Collection(Edm.ComplexType)",
      "analyzer": null,
      "synonymMaps": [],
      "fields": [
        {
          "name": "allowedRegionId",
          "type": "Edm.String",
          "facetable": false,
          "filterable": true,
          "retrievable": true,
          "sortable": false,
          "analyzer": null,
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        }
      ]
    },
    {
      "name": "allowedUsers",
      "type": "Collection(Edm.ComplexType)",
      "analyzer": null,
      "synonymMaps": [],
      "fields": [
        {
          "name": "allowedUserId",
          "type": "Edm.String",
          "facetable": false,
          "filterable": true,
          "retrievable": true,
          "sortable": false,
          "analyzer": null,
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        }
      ]
    },
    {
      "name": "allowedCategories",
      "type": "Collection(Edm.ComplexType)",
      "analyzer": null,
      "synonymMaps": [],
      "fields": [
        {
          "name": "allowedCategoryId",
          "type": "Edm.String",
          "facetable": false,
          "filterable": true,
          "retrievable": true,
          "sortable": false,
          "analyzer": null,
          "indexAnalyzer": null,
          "searchAnalyzer": null,
          "synonymMaps": [],
          "fields": []
        }
      ]
    },
	{
      "name": "isPrivate",
      "type": "Edm.Boolean",
      "facetable": false,
      "filterable": true,
      "retrievable": true,
      "sortable": false,
      "analyzer": null,
      "indexAnalyzer": null,
      "searchAnalyzer": null,
      "synonymMaps": [],
      "fields": []
    }
  ],
  ...
}
```
- save index

## DataSources

Indexers require a data source object that provides a connection string and possibly credentials.

- Go to the [Azure Portal](https://portal.azure.com/)
- In the search bar type and find **"Cognitive search"**
- in the list of services find created service from step #Search Service and open it
- find tab "Data sources" and create N of them:
  - select data source - Azure SQL Database
  - add name (value from table below)
  - provide credentials to access db (connection string, user/password etc.)
  - select table/view (value from table below)
  - Track Deletions - set as **true** (checkbox)
  - Soft delete column - set **Is_Deleted**
  - Delete marker value - set **true** (string without quotes)
  - Track changes - set as **true** (checkbox)
  - Data Change Detection Policy - select "High watermark column"
  - High watermark column - set **Last_Change_Ts**
- save data source
- repeat for each entry from the table 

| Name | Table/View |
| ------ | ------ |
| forums-{environment}-ds | [ForumSearchView] |
| articles-{environment}-ds | [ArticleSearchView] |
| projects-{environment}-ds | [ProjectSearchView] |
| companies-{environment}-ds | [CompanySearchView] |
| events-{environment}-ds | [EventSearchView] |
| tools-{environment}-ds | [ToolSearchView] |

## Indexers

An indexer in Azure Cognitive Search is a crawler that extracts searchable content from cloud data sources and populates a search index using field-to-field mappings between source data and a search index.

- Go to the [Azure Portal](https://portal.azure.com/)
- In the search bar type and find **"Cognitive search"**
- in the list of services find created service from step #Search Service and open it
- find tab "Indexers" and create 1 for each datasource from step #DataSources:
  - add indexer name - (value from table below)
  - Index - select index from step #Index
  - Datasource - select datasource from step #DataSources (see value from table below)
  - Schedule - select how often data from the datasource will be synchronized to the index
  - open tab "Indexer Definition (Json)"
  - in the json editor add next fieldMappings to the `fieldMappings` field:
```json
{
  ...
  "fieldMappings": [
	{
      "sourceFieldName": "Id",
      "targetFieldName": "id",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Original_Id",
      "targetFieldName": "originalId",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Subject",
      "targetFieldName": "subject",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Description",
      "targetFieldName": "description",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Entity_Type",
      "targetFieldName": "entityType",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Last_Change_Ts",
      "targetFieldName": "updatedAt",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Categories",
      "targetFieldName": "categories",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Solutions",
      "targetFieldName": "solutions",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Technologies",
      "targetFieldName": "technologies",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Regions",
      "targetFieldName": "regions",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Is_Deleted",
      "targetFieldName": "isDeleted",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Allowed_Roles",
      "targetFieldName": "allowedRoles",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Allowed_Categories",
      "targetFieldName": "allowedCategories",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Allowed_Regions",
      "targetFieldName": "allowedRegions",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Allowed_Companies",
      "targetFieldName": "allowedCompanies",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Allowed_Users",
      "targetFieldName": "allowedUsers",
      "mappingFunction": null
    },
    {
      "sourceFieldName": "Is_Private",
      "targetFieldName": "isPrivate",
      "mappingFunction": null
    }
  ],
  ...
}
```
***Note: `fieldMappings` has to be added for every indexer, and in this specific case they are the same for all of them.***

| Name | Index | Source |
| ------ | ------ | ------ |
| forums-{environment}-indexer | neo-{environment}-global-search-index | forums-{environment}-ds |
| articles-{environment}-indexer | neo-{environment}-global-search-index | articles-{environment}-ds |
| projects-{environment}-indexer | neo-{environment}-global-search-index | projects-{environment}-ds |
| companies-{environment}-indexer | neo-{environment}-global-search-index | companies-{environment}-ds |
| events-{environment}-indexer | neo-{environment}-global-search-index | events-{environment}-ds |
| tools-{environment}-indexer | neo-{environment}-global-search-index | tools-{environment}-ds |


## Key Vault Secrets
- Go to the [Azure Portal](https://portal.azure.com/)
- In the search bar type and find **"Key vaults"**
- select env target Key vault
- open tab "Secrets"
- add new secrets:
  - Name: **AzureCognitiveSearch--ApiKey**. Value: open search service from step #Search Service => tab "Keys" => select 1 of the primary admin keys or generate new one

Also update `appsettings.{environment}.json`:
  - Name: **AzureCognitiveSearch:ServiceName**. Value: name of the search service from step #Search Service (e.g. neo-{environment}-global-search)
  - Name: **AzureCognitiveSearch:IndexName**. Value: name of the index from step #Index (e.g. neo-{environment}-global-search-index)