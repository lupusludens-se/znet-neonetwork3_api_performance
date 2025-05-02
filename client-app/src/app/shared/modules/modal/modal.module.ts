// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

// components
import { ModalComponent } from './components/modal/modal.component';
import { ContactNeoNetworkComponent } from './components/contact-neo-network/contact-neo-network.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AttachToInitiativeComponent } from './components/attach-to-initiative/attach-to-initiative.component';
import { FileUploadComponent } from './components/file-upload/file-upload.component';

@NgModule({
  declarations: [ModalComponent, ContactNeoNetworkComponent, AttachToInitiativeComponent, FileUploadComponent],
  imports: [SharedModule, FormsModule, ReactiveFormsModule],
  exports: [ModalComponent, ContactNeoNetworkComponent, AttachToInitiativeComponent, FileUploadComponent]
})
export class ModalModule {}
