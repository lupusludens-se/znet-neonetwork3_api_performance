import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TopPanelModule } from "../../shared/modules/top-panel/top-panel.module";
import { RouterModule, Routes } from '@angular/router';
import { InitiativeDetailsComponent } from './initiative-details.component';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: InitiativeDetailsComponent
  }
];
@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    TopPanelModule,
    RouterModule.forChild(routes)
  ]
})
export class InitiativeDetailsModule { }
