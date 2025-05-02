import { HttpContextToken, HttpEvent, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MsalInterceptor } from '@azure/msal-angular';
import { Observable } from 'rxjs';
export const BYPASS_MSAL = new HttpContextToken(() => false);

@Injectable()
export class MsalCustomInterceptor extends MsalInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<unknown>, next: any): Observable<HttpEvent<unknown>> {
    if (request.context.get(BYPASS_MSAL)) {
      return next.handle(request);
    }
    return super.intercept(request, next);
  }
}
