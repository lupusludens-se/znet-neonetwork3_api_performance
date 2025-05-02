// modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ProfileSocialsControlsModule } from '../shared/modules/profile-socials-controls/profile-socials-controls.module';
import { TranslateModule } from '@ngx-translate/core';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { VerticalLineDecorModule } from '../shared/modules/vertical-line-decor/vertical-line-decor.module';
import { ContentTagModule } from '../shared/modules/content-tag/content-tag.module';
import { UserAvatarModule } from '../shared/modules/user-avatar/user-avatar.module';
import { TopPanelModule } from '../shared/modules/top-panel/top-panel.module';
import { ModalModule } from '../shared/modules/modal/modal.module';
import { SharedModule } from '../shared/shared.module';
import { MembersListModule } from '../shared/modules/members-list/members-list.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// components
import { CompanyProfileComponent } from './company-profile.component';
import { CompanyFilesSectionComponent } from './company-files-section/company-files-section.component';
import { MenuModule } from '../shared/modules/menu/menu.module';
import { FileService } from '../shared/services/file.service';
import { AddCompanyAnnouncementComponent } from './add-company-announcement/add-company-announcement.component';
import { TextInputModule } from '../shared/modules/controls/text-input/text-input.module';
import { DropdownModule } from '../shared/modules/controls/dropdown/dropdown.module';
import { FormFooterModule } from '../shared/modules/form-footer/form-footer.module';
import { InitiativeSharedService } from '../initiatives/shared/services/initiative-shared.service';
import { CreateInitiativeService } from '../initiatives/+create-initiative/services/create-initiative.service';
import { DotDecorModule } from '../shared/modules/dot-decor/dot-decor.module';
import { BlueCheckboxModule } from '../shared/modules/blue-checkbox/blue-checkbox.module';
import { TextEditorModule } from '../shared/modules/text-editor/text-editor.module';
import { FilterHeaderModule } from '../shared/modules/filter-header/filter-header.module';
import { EmptyPageModule } from '../shared/modules/empty-page/empty-page.module';
import { LearnItemModule } from '../initiatives/shared/contents/learn-item/learn-item.module';
import { InitiativeGeographicScaleModule } from '../initiatives/shared/geographic-scale/initiative-geographic-scale/initiative-geographic-scale.module';
import { MessageItemModule } from '../initiatives/shared/contents/message-item/message-item.module';
import { ToolItemModule } from '../initiatives/shared/contents/tool-item/tool-item.module';
import { CommunityItemModule } from '../initiatives/shared/contents/community-item/community-item.module';
import { ProjectItemModule } from '../initiatives/shared/contents/project-item/project-item.module';
import { UserCollaboratorModule } from '../shared/modules/user-collaborator/user-collaborator.module';
import { CompanyFileListViewComponent } from './+manage-files-content/components/company-file-list-view/company-file-list-view.component';
import { CompanyFileTableRowComponent } from './+manage-files-content/components/company-file-table-row/company-file-table-row.component';
import { CompanyProfileService } from './services/company-profile.service';
import { PaginationModule } from '../shared/modules/pagination/pagination.module';
import { SaveContentService } from '../shared/services/save-content.service';
import { ContentLocationModule } from '../shared/modules/content-location/content-location.module';
import { CompanyLiveProjectsSectionComponent } from './company-live-projects-section/company-live-projects-section.component';
import { CompanyAnnouncementsSectionComponent } from './company-announcements-section/company-announcements-section.component';
import { EditCompanyAnnouncementComponent } from './edit-company-announcement/edit-company-announcement.component';
import { NoResultsModule } from '../shared/modules/no-results/no-results.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: CompanyProfileComponent
  },
  {
    path: 'files',
    data: { breadcrumb: 'View Files' },
    component: CompanyFileListViewComponent
  }
];

@NgModule({
  declarations: [
    CompanyProfileComponent,
    CompanyFilesSectionComponent,
    CompanyFileTableRowComponent,
    CompanyFileListViewComponent,
    CompanyLiveProjectsSectionComponent,
    AddCompanyAnnouncementComponent,
    CompanyAnnouncementsSectionComponent,
    EditCompanyAnnouncementComponent
  ],
  imports: [
    CommonModule,
    TranslateModule,
    PaginationModule,
    RouterModule.forChild(routes),
    SvgIconsModule,
    ProfileSocialsControlsModule,
    VerticalLineDecorModule,
    ContentTagModule,
    UserAvatarModule,
    TopPanelModule,
    ModalModule,
    SharedModule,
    MembersListModule,
    MenuModule,
    TextInputModule,
    DropdownModule,
    FormFooterModule,
    ReactiveFormsModule,
    FormsModule,
    DotDecorModule,
    BlueCheckboxModule,
    TextEditorModule,
    FilterHeaderModule,
    InitiativeGeographicScaleModule,
    EmptyPageModule,
    LearnItemModule,
    MessageItemModule,
    ToolItemModule,
    ContentLocationModule,
    UserCollaboratorModule,
    ProjectItemModule,
    CommunityItemModule,
    NoResultsModule
  ],
  providers: [FileService, SaveContentService, InitiativeSharedService, CreateInitiativeService, CompanyProfileService]
})
export class CompanyProfileModule {}
