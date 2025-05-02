import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { SharedModule } from '../../../../shared/shared.module';
import { InitiativeContentComponent } from './initiative-content.component';
import { RouterModule, Routes } from '@angular/router';
const routes: Routes = [
  {
    path: '',
    component: InitiativeContentComponent
  }
];
@NgModule({
  declarations: [],
  imports: [CommonModule, TranslateModule, SharedModule, RouterModule.forChild(routes)]
})
export class InitiativeContentModule { }
