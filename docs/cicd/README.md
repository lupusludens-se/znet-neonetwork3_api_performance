# CI/CD Alignment and Strategy

## Branch Strategy
---

### Branches

* `feat/<feature name>`: Feature development branch as part of planned software development lifecycle.
* `hotfix/<hotfix name>`: Unplanned development due to a break fix or required deployment outside the scope of the typical flow.
* `development` - Consolidated branch to support development of features.  Will trigger release to development environment.
* `tst` - Consolidated branch to support QA testing.  Will trigger release to tst environment.
* `release` - Consolidated branch from development of features ready for business pre-production / uat testing.
* `master` - Production branch.  Will represent what is currently deployed in the production environment.

The Software Development Lifecycle (SDLC) will be branch driven and automated based on source code management events.  Below represents the expected `SDLC` flow and the expected events.

```
<feature branch> - Local Development based on a feature enhancement - created from `development`
     -> Pull Request 
            -> development - Merge to development will trigger a release to the `dev` environment
     -> Pull Request
            -> tst - Merge to test will trigger a release to the `tst` environment

            -> Pull Request development -> release
                -> release - Merge to release from development will trigger a deployment to the `preprod` environment.
                -> Pull Request - release -> master
                    -> master - Merge to master from release will trigger a production deployment.

    rebase release, development...
                             

<hotfix branch> - Local Development based on a hot fix requirement - created from `master`
    -> Pull Request <hotfix branch> -> release
        -> release - Merge to release from hot fix will trigger a deployment to the `preprod` environment.
        -> Pull Request - release -> master
            -> master - Merge to master from release will trigger a production deployment.

    rebase release, development...            

```

## Environments
---

### Development

* __Client Application__: https://app-dev.neonetworkexchange.com/neonetwork/dev/
* __Server Application__: https://app-dev.neonetworkexchange.com/neonetwork/dev/api/

### Test (TST)

* __Client Application__: https://app-tst.neonetworkexchange.com/neonetwork/tst/
* __Server Application__: https://app-tst.neonetworkexchange.com/neonetwork/tst/api/

### Pro-Production (PREPROD)

* __Client Application__: https://app-preprod.neonetworkexchange.com/neonetwork/preprod/
* __Server Application__: https://app-preprod.neonetworkexchange.com/neonetwork/preprod/api/

### Production

This setup is currently being designed by Marketing and the domain could change.

* __Client Application__: https://app.neonetworkexchange.com/neonetwork/prod/
* __Server Application__: https://app.neonetworkexchange.com/neonetwork/prod/api/

## Server App (CICD)
---

The following components in the application drive alignment with the branching and environment strategy defined above.

### Properties 

__Project__: SE.Neo.WEbAPI
__Application Settings__: `appsettings.json`: Global Settings, `appsettings.<env>.json`: Environment specific settings (Environments: `dev`, `tst`, `preprod`, `prod`)

### URI Pattern

The application URI pattern drives our internal gateway and ingress routing to support fault tolerance, scalability, and security in depth configurations.  Each api environment follows the pattern:

```
/neonetwork/<env>/api
```

This is required to remain consistent in the application to support ingress routing requirements.

## Client App
---

The following components in the application drive alignment with the branching and environment strategy defined above.

### Properties 

__Project__: src/environments
__Application Settings__: `environment.ts`: Global Settings, `environment.<env>.ts`: Environment specific settings (Environments: `dev`, `tst`, `preprod`, `prod`)


### URI Pattern

The application URI pattern drives our internal gateway and ingress routing to support fault tolerance, scalability, and security in depth configurations.  Each application environment follows the pattern:

```
/neonetwork/<env>/
```

This is required to remain consistent in the application to support ingress routing requirements.