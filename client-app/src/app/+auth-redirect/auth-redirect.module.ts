import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
// components
import { AuthRedirectRoutingModule } from './auth-redirect-routing.module';
import { AuthRedirectComponent } from './auth-redirect.component';

@NgModule({
  declarations: [AuthRedirectComponent],
  imports: [SharedModule, AuthRedirectRoutingModule]
})
export class AuthRedirectModule {}
