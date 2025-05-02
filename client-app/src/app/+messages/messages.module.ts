// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { MessagesRoutingModule } from './messages-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from '../shared/modules/modal/modal.module';
import { MultiselectModule } from '../shared/modules/controls/multiselect/multiselect.module';
import { PaginationModule } from '../shared/modules/pagination/pagination.module';
import { NoResultsModule } from '../shared/modules/no-results/no-results.module';
import { UserAvatarModule } from '../shared/modules/user-avatar/user-avatar.module';
import { ImageViewModule } from '../shared/modules/image-view/image-view.module';
import { EmptyPageModule } from '../shared/modules/empty-page/empty-page.module';
import { MessageControlModule } from '../shared/modules/message-control/message-control.module';
import { ControlErrorModule } from '../shared/modules/controls/control-error/control-error.module';
import { TextInputModule } from '../shared/modules/controls/text-input/text-input.module';
import { TextEditorModule } from '../shared/modules/text-editor/text-editor.module';

// components
import { MessagesComponent } from './messages.component';
import { NewMessageComponent } from './components/new-message/new-message.component';
import { MessageHistoryComponent } from './components/message-history/message-history.component';
import { LeaveConfirmationComponent } from './components/leave-confirmation/leave-confirmation.component';

// pipes
import { DatePipe } from '@angular/common';
import { MessagesService } from './services/messages.service';
import { MessageUserDetailComponent } from './components/message-user-detail/message-user-detail.component';
import { StaticDropdownModule } from '../shared/modules/controls/static-dropdown/static-dropdown.module';
import { SortDropdownModule } from '../shared/modules/sort-dropdown/sort-dropdown.module';
import { FilterModule } from '../shared/modules/filter/filter.module';
import { EditMessageComponent } from './components/edit-message/edit-message.component';
import { InitiativeSharedService } from '../initiatives/shared/services/initiative-shared.service';
import { MessageTabService } from '../shared/services/message-tab.service';

@NgModule({
  declarations: [
    MessagesComponent,
    NewMessageComponent,
    MessageHistoryComponent,
    LeaveConfirmationComponent,
    MessageUserDetailComponent,
    EditMessageComponent
  ],
  imports: [
    SharedModule,
    MessagesRoutingModule,
    FormsModule,
    ModalModule,
    ReactiveFormsModule,
    MultiselectModule,
    PaginationModule,
    NoResultsModule,
    UserAvatarModule,
    ImageViewModule,
    EmptyPageModule,
    ControlErrorModule,
    TextInputModule,
    MessageControlModule,
    TextEditorModule,
    SortDropdownModule,
    StaticDropdownModule,
    FilterModule
  ],
  providers: [DatePipe, MessagesService, InitiativeSharedService, MessageTabService]
})
export class MessagesModule { }
