// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared.module';
import { ModalModule } from '../modal/modal.module';
import { RouterModule } from '@angular/router';
import { UserAvatarModule } from '../user-avatar/user-avatar.module';
import { NoResultsModule } from '../no-results/no-results.module';

// components
import { MembersListComponent } from './components/members-list/members-list.component';
import { MemberComponent } from './components/member/member.component';

@NgModule({
  declarations: [MembersListComponent, MemberComponent],
  exports: [MembersListComponent,MemberComponent],
  imports: [CommonModule, ModalModule, SharedModule, RouterModule, UserAvatarModule, NoResultsModule]
})
export class MembersListModule {}
