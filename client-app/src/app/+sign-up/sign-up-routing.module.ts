// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { SignUpComponent } from './sign-up.component';
import { CompleteComponent } from './components/complete/complete.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: SignUpComponent
  },
  {
    path: 'complete',
    pathMatch: 'full',
    component: CompleteComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SignUpRoutingModule {}
