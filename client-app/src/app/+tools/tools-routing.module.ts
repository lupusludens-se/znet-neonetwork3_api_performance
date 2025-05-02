// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { ToolsComponent } from './tools.component';
import { ToolContentComponent } from './components/tool-content/tool-content.component';
import { ThankYouComponent } from './components/solar-quote/thank-you/thank-you.component';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: ToolsComponent
  },
  {
    path: ':id',
    data: { breadcrumb: 'Tool Details' },
    component: ToolContentComponent,
    pathMatch: 'full'
  },
  {
    path: 'thank-you/:id',
    data: { breadcrumb: 'Thank You' },
    component: ThankYouComponent,
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ToolsRoutingModule {}
