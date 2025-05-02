// modules
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { MessagesComponent } from './messages.component';
import { NewMessageComponent } from './components/new-message/new-message.component';
import { MessageHistoryComponent } from './components/message-history/message-history.component';
import { CanDeactivateGuard } from '../shared/guards/can-deactivate.guard';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: MessagesComponent
  },
  {
    path: 'new-message',
    data: { breadcrumb: 'New Message' },
    component: NewMessageComponent,
    canDeactivate: [CanDeactivateGuard]
  },
  {
    path: ':id',
    data: { breadcrumb: 'Message Details' },
    component: MessageHistoryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MessagesRoutingModule {}
