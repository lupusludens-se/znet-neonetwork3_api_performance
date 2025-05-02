import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditInitiativeComponent } from './edit-initiative.component';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormFooterModule } from 'src/app/shared/modules/form-footer/form-footer.module';
import { DropdownModule } from 'src/app/shared/modules/controls/dropdown/dropdown.module';
import { TextInputModule } from 'src/app/shared/modules/controls/text-input/text-input.module';
import { InitiativeGeographicScaleModule } from '../shared/geographic-scale/initiative-geographic-scale/initiative-geographic-scale.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';
import { InitiativeSharedService } from '../shared/services/initiative-shared.service';
import { UserCollaboratorModule } from 'src/app/shared/modules/user-collaborator/user-collaborator.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: EditInitiativeComponent
  }
];

@NgModule({
  declarations: [EditInitiativeComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    FormFooterModule,
    DropdownModule,
    TextInputModule,
    InitiativeGeographicScaleModule,
    ModalModule,
    UserCollaboratorModule
  ],
  providers: [InitiativeSharedService]
})
export class EditInitiativeModule {}
