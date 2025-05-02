/* eslint-disable prettier/prettier */
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

import { CreateInitiativeComponent } from './create-initiative.component';
import { InitiativeFormComponent } from './components/initiative-form/initiative-form.component';
import { InitiativeContentComponent } from './components/initiative-content/initiative-content.component';
import { LearnComponent } from './components/initiative-resources/learn/learn.component';
import { CommunityComponent } from './components/initiative-resources/community/community.component';
import { ToolsComponent } from './components/initiative-resources/tools/tools.component';
import { MessageComponent } from './components/initiative-resources/message/message.component';
import { ProjectComponent } from './components/initiative-resources/project/project.component';

import { SharedModule } from 'src/app/shared/shared.module';
import { DotDecorModule } from 'src/app/shared/modules/dot-decor/dot-decor.module';
import { FormFooterModule } from 'src/app/shared/modules/form-footer/form-footer.module';
import { BlueCheckboxModule } from 'src/app/shared/modules/blue-checkbox/blue-checkbox.module';
import { TextEditorModule } from 'src/app/shared/modules/text-editor/text-editor.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';
import { TextInputModule } from 'src/app/shared/modules/controls/text-input/text-input.module';
import { DropdownModule } from 'src/app/shared/modules/controls/dropdown/dropdown.module';
import { FilterHeaderModule } from 'src/app/shared/modules/filter-header/filter-header.module';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { EmptyPageModule } from 'src/app/shared/modules/empty-page/empty-page.module';

import { LearnItemModule } from '../shared/contents/learn-item/learn-item.module';
import { InitiativeGeographicScaleModule } from '../shared/geographic-scale/initiative-geographic-scale/initiative-geographic-scale.module';
import { ToolItemModule } from '../shared/contents/tool-item/tool-item.module';
import { MessageItemModule } from '../shared/contents/message-item/message-item.module';
import { CommunityItemModule } from '../shared/contents/community-item/community-item.module';
import { ProjectItemModule } from '../shared/contents/project-item/project-item.module';

import { CanDeactivateGuard } from 'src/app/shared/guards/can-deactivate.guard';
import { InitiativeSharedService } from '../shared/services/initiative-shared.service';
import { UserCollaboratorModule } from 'src/app/shared/modules/user-collaborator/user-collaborator.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: CreateInitiativeComponent,
    canDeactivate: [CanDeactivateGuard]
  }
];

@NgModule({
  declarations: [
    CreateInitiativeComponent,
    InitiativeFormComponent,
    InitiativeContentComponent,
    LearnComponent,
    CommunityComponent,
    ToolsComponent,
    MessageComponent,
    ProjectComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    TranslateModule,
    SharedModule,
    DotDecorModule,
    FormFooterModule,
    BlueCheckboxModule,
    TextEditorModule,
    ModalModule,
    TextInputModule,
    DropdownModule,
    FilterHeaderModule,
    PaginationModule,
    EmptyPageModule,
    LearnItemModule,
    InitiativeGeographicScaleModule,
    ToolItemModule,
    MessageItemModule,
    CommunityItemModule,
    ProjectItemModule,
    UserCollaboratorModule
  ],
  providers: [InitiativeSharedService]
})
export class CreateInitiativeModule { }
