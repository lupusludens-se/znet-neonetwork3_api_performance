// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared.module';

// components
import { EmptyPageComponent } from './empty-page.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [EmptyPageComponent],
  exports: [EmptyPageComponent],
  imports: [CommonModule, SharedModule, RouterModule]
})
export class EmptyPageModule {}
