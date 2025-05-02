/* eslint-disable no-undef */
/**
 * This file contains authentication parameters. Contents of this file
 * is roughly the same across other MSAL.js libraries. These parameters
 * are used to initialize Angular and MSAL Angular configurations in
 * in app.module.ts file.
 */

import { BrowserCacheLocation, Configuration, LogLevel } from '@azure/msal-browser';
import { environment } from 'src/environments/environment';
import { CommonApiEnum } from '../enums/common-api.enum';
import { AdmitUserEnum } from '../../admit/emuns/admit-user.enum';
import { UnsubscribeApiEnum } from 'src/app/shared/enums/api/general-api.enum';
import { CommunityComponentEnum } from 'src/app/+community/enums/community-component.enum';

const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

/**
 * Enter here the user flows and custom policies for your B2C application
 * To learn more about user flows, visit: https://docs.microsoft.com/en-us/azure/active-directory-b2c/user-flow-overview
 * To learn more about custom policies, visit: https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-overview
 */
export const b2cPolicies = {
  names: {
    signUpSignIn: environment.signUpSignIn.name
  },
  authorities: {
    signUpSignIn: {
      authority: environment.signUpSignIn.authority
    },
    passwordReset: {
      authority: environment.passwordReset.authority
    },
    changePassword: {
      authority: environment.changePassword.authority,
      redirectUri: environment.changePassword.redirectUri
    }
  },
  authorityDomain: environment.authorityDomain
};

/**
 * Configuration object to be passed to MSAL instance on creation.
 * For a full list of MSAL.js configuration parameters, visit:
 * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/configuration.md
 */
export const msalConfig: Configuration = {
  auth: {
    clientId: environment.authClientId, // This is the ONLY mandatory field that you need to supply.
    authority: b2cPolicies.authorities.signUpSignIn.authority,
    knownAuthorities: [b2cPolicies.authorityDomain], // Mark your B2C tenant's domain as trusted.
    redirectUri: environment.redirect, // Points to window.location.origin. You must register this URI on Azure portal/App Registration.
    postLogoutRedirectUri: environment.postLogoutRedirect
  },
  cache: {
    cacheLocation: BrowserCacheLocation.LocalStorage, // Configures cache location. "sessionStorage" is more secure, but "localStorage" gives you SSO between tabs.
    storeAuthStateInCookie: isIE // Set this to "true" if you are having issues on IE11 or Edge
  },
  system: {
    loggerOptions: {
      loggerCallback: (level: LogLevel, message: string): void => {
        if (environment.disableLog) {
          return;
        }

        switch (level) {
          case LogLevel.Error:
            console.error(message);
            return;
          case LogLevel.Info:
            // TODO: do not generate excessive information e.g. date and version hundred times
            // console.info(message);
            return;
          case LogLevel.Verbose:
            console.debug(message);
            return;
          case LogLevel.Warning:
            console.warn(message);
            return;
        }
      },
      piiLoggingEnabled: false
    }
  }
};

/**
 * Add here the endpoints and scopes when obtaining an access token for protected web APIs. For more information, see:
 * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/resources-and-scopes.md
 */
export const protectedResources = {
  getApi: {
    endpoint: environment.apiUrl,
    scopes: environment.scopes
  },
  pendingApi: {
    endpoint: `${environment.apiUrl}/${AdmitUserEnum.PendingCounter}`,
    scopes: environment.scopes
  }
};

export const unprotectedResources = {
  heardViaApi: {
    endpoint: `${environment.apiUrl}/${CommonApiEnum.HeardVia}`,
    scopes: null
  },
  countriesApi: {
    endpoint: `${environment.apiUrl}/${CommonApiEnum.Countries}`,
    scopes: null
  },
  userPendingApi: {
    endpoint: `${environment.apiUrl}/${CommonApiEnum.UserPending}`,
    scopes: null
  },
  contactUsApi: {
    endpoint: `${environment.apiUrl}/${CommonApiEnum.ContactUs}`,
    scopes: null
  },
  unSubscribeGetApi: {
    endpoint: `${environment.apiUrl}/${UnsubscribeApiEnum.UnsubscribeGetDetailsApi}`,
    scopes: null
  },
  unSubscribePostApi: {
    endpoint: `${environment.apiUrl}/${UnsubscribeApiEnum.UnsubscribeEmailsApi}`,
    scopes: null
  },
  networkStatsCountersApi: {
    endpoint: `${environment.apiUrl}/${CommunityComponentEnum.NetworkStats}`,
    scopes: null
  }
};

/**
 * Scopes you add here will be prompted for user consent during sign-in.
 * By default, MSAL.js will add OIDC scopes (openid, profile, email) to any login request.
 * For more information about OIDC scopes, visit:
 * https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-permissions-and-consent#openid-connect-scopes
 */
export const loginRequest = {
  scopes: [] // If you would like the admin-user to explicitly consent via "Admin" page, instead of being prompted for admin consent during initial login, remove this scope.
};
