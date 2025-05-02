// modules
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { DotDecorModule } from '../../shared/modules/dot-decor/dot-decor.module';
import { PaginationModule } from '../../shared/modules/pagination/pagination.module';
import { UserAvatarModule } from '../../shared/modules/user-avatar/user-avatar.module';
import { FormFooterModule } from '../../shared/modules/form-footer/form-footer.module';
import { BlueCheckboxModule } from '../../shared/modules/blue-checkbox/blue-checkbox.module';
import { TextEditorModule } from '../../shared/modules/text-editor/text-editor.module';

// services
import { ModalModule } from '../../shared/modules/modal/modal.module';
import { InitiativeRoutingModule } from './initiative-routing.module';
import { InitiativeComponent } from './initiative.component';

@NgModule({
  declarations: [InitiativeComponent],
  imports: [
    SharedModule,
    InitiativeRoutingModule,
    DotDecorModule,
    PaginationModule,
    UserAvatarModule,
    FormsModule,
    FormFooterModule,
    ReactiveFormsModule,
    BlueCheckboxModule,
    TextEditorModule,
    ModalModule
  ],
  providers: []
})
export class InitiativeModule {}
