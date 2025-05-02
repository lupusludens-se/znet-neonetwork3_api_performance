import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FileListViewComponent } from './components/file-list-view/file-list-view.component';

const routes: Routes = [
  {
    path: 'files',
    data: { breadcrumb: 'View All - Files' },
    component: FileListViewComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageFilesRoutingModule {}
