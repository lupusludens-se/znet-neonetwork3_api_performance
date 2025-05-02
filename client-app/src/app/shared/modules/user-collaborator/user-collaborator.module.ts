import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserCollaboratorComponent } from './user-collaborator.component';
import { FilterHeaderModule } from '../filter-header/filter-header.module';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { BlueCheckboxModule } from '../blue-checkbox/blue-checkbox.module';
import { SharedModule } from '../../shared.module';

@NgModule({
  declarations: [UserCollaboratorComponent],
  exports: [UserCollaboratorComponent],
  imports: [CommonModule, ReactiveFormsModule, TranslateModule, BlueCheckboxModule, SharedModule, FilterHeaderModule]
})
export class UserCollaboratorModule {}
