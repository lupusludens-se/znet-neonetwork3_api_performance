import { AuthenticationResult } from '@azure/msal-browser';

export interface PayloadInterface extends AuthenticationResult {
  idTokenClaims: {
    tfp?: string;
    exp?: number;
  };
}
