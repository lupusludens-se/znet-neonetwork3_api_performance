// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { ForumComponent } from './forum.component';
import { ForumThreadComponent } from './components/forum-thread/forum-thread.component';
import { StartADiscussionComponent } from './components/start-a-discussion/start-a-discussion.component';
import { EditDiscussionComponent } from './components/edit-discussion/edit-discussion.component';

const routes: Routes = [
  { path: '', data: { breadcrumbSkip: true }, component: ForumComponent },
  {
    path: 'topic/:id',
    data: { breadcrumb: 'Discussion Details' },
    component: ForumThreadComponent,
    pathMatch: 'full'
  },
  {
    path: 'start-a-discussion',
    data: { breadcrumb: 'Start a Discussion' },
    component: StartADiscussionComponent,
    pathMatch: 'full'
  },
  {
    path: 'edit-discussion/:id',
    data: { breadcrumb: 'Edit Discussion' },
    component: EditDiscussionComponent,
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ForumRoutingModule {}
