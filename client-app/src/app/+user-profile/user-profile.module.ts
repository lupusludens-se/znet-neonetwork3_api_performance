// modules
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { ContentTagModule } from '../shared/modules/content-tag/content-tag.module';
import { GeographicInterestModule } from '../shared/modules/geographic-interest/geographic-interest.module';
import { InterestsTopicModule } from '../shared/modules/interests-topic/interests-topic.module';
import { SaveCancelControlsModule } from '../shared/modules/save-cancel-controls/save-cancel-controls.module';
import { ProfileSocialsControlsModule } from '../shared/modules/profile-socials-controls/profile-socials-controls.module';
import { UserAvatarModule } from '../shared/modules/user-avatar/user-avatar.module';
import { BlueCheckboxModule } from '../shared/modules/blue-checkbox/blue-checkbox.module';
import { FilterHeaderModule } from '../shared/modules/filter-header/filter-header.module';

// components
import { UserProfileComponent } from './user-profile.component';
import { InterestsModalComponent } from './components/interests-modal/interests-modal.component';
import { RegionsModalComponent } from './components/regions-modal/regions-modal.component';
import { MsalGuard } from '@azure/msal-angular';
import { UserProfileEditPermissionGuard } from '../shared/guards/permission-guards/user-profile-edit-permission.guard';
import { ModalModule } from '../shared/modules/modal/modal.module';
import { InitiativeSharedService } from '../initiatives/shared/services/initiative-shared.service';
import { MembersListModule } from '../shared/modules/members-list/members-list.module';
import { SharedModule } from '../shared/shared.module';
import { NoResultsModule } from '../shared/modules/no-results/no-results.module';
import { SkillsModalComponent } from './components/skills-modal/skills-modal.component';
import { DropdownModule } from '../shared/modules/controls/dropdown/dropdown.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: UserProfileComponent
  },
  {
    path: 'edit',
    data: { breadcrumb: 'Edit Profile' },
    loadChildren: () => import('../+edit-profile/edit-profile.module').then(m => m.EditProfileModule),
    canActivate: [MsalGuard, UserProfileEditPermissionGuard]
  }
];

@NgModule({
  declarations: [UserProfileComponent, InterestsModalComponent, RegionsModalComponent, SkillsModalComponent],
  imports: [
    CommonModule,
    TranslateModule,
    RouterModule.forChild(routes),
    SvgIconsModule,
    ContentTagModule,
    GeographicInterestModule,
    InterestsTopicModule,
    SaveCancelControlsModule,
    ProfileSocialsControlsModule,
    UserAvatarModule,
    BlueCheckboxModule,
    ReactiveFormsModule,
    FilterHeaderModule,
    ModalModule,
    MembersListModule,
    SharedModule,
    NoResultsModule,
    DropdownModule
  ],
  providers: [InitiativeSharedService] 
})
export class UserProfileModule {}
