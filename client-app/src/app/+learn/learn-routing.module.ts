// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { LearnComponent } from './learn.component';
import { PostComponent } from './components/post/post.component';
import { ArticleDetailsViewOwnPermissionGuard } from '../shared/guards/permission-guards/article-details-view-own-permission.guard';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: LearnComponent
  },
  {
    path: ':id',
    data: { breadcrumb: 'Article Details' },
    component: PostComponent,
    canActivate: [ArticleDetailsViewOwnPermissionGuard],
  },
  {
    path: ':id/:src',
    data: { breadcrumb: 'Article Details' },
    component: PostComponent,
    canActivate: [ArticleDetailsViewOwnPermissionGuard],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LearnRoutingModule {}
