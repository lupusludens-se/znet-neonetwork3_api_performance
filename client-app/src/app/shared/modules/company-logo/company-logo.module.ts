import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompanyLogoComponent } from './company-logo.component';
import { SharedModule } from '../../shared.module';

@NgModule({
  declarations: [CompanyLogoComponent],
  imports: [CommonModule, SharedModule],
  exports: [CompanyLogoComponent]
})
export class CompanyLogoModule {}
