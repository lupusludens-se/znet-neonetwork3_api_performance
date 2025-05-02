// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared.module';
import { ModalModule } from '../modal/modal.module';

// component
import { PrivacyPolicyComponent } from './privacy-policy.component';

@NgModule({
  declarations: [PrivacyPolicyComponent],
  exports: [PrivacyPolicyComponent],
  imports: [CommonModule, ModalModule, SharedModule]
})
export class PrivacyPolicyModule {}
