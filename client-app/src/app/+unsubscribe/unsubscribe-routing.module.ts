// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { UnsubscribeComponent } from './unsubscribe.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: UnsubscribeComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UnsubscribeRoutingModule {}
