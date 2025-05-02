import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CorporateInitiativePermissionGuard } from "src/app/shared/guards/permission-guards/corporate-initiative-permission.guard";
import { InitiativeToolsComponent } from "./components/initiative-tools-form/initiative-tools.component";



const routes: Routes = [
  {
    path: 'tools',
    component: InitiativeToolsComponent,
    canActivate: [CorporateInitiativePermissionGuard],
    data: { breadcrumb: 'View All - Tools' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InitiativeManageToolsRoutingModule { }
