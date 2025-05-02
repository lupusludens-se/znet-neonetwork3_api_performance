// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared.module';
import { ModalModule } from '../modal/modal.module';

// component
import { TermOfUseComponent } from './term-of-use.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [TermOfUseComponent],
  exports: [TermOfUseComponent],
  imports: [CommonModule, ModalModule, SharedModule, RouterModule]
})
export class TermOfUseModule {}
