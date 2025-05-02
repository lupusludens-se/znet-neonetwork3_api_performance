import { Component, Input, OnChanges, OnInit, ViewChild } from '@angular/core';
import { MessageInterface } from '../../interfaces/message.interface';
import { TextEditorComponent } from 'src/app/shared/modules/text-editor/text-editor.component';
import { TranslateService } from '@ngx-translate/core';
import { catchError, of } from 'rxjs';
import { CoreService } from 'src/app/core/services/core.service';
import { HttpService } from 'src/app/core/services/http.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { MessageApiEnum } from '../../enums/message-api.enum';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';

@Component({
  selector: 'neo-edit-message',
  templateUrl: './edit-message.component.html',
  styleUrls: ['./edit-message.component.scss']
})
export class EditMessageComponent implements OnInit, OnChanges {
  @ViewChild('editorComponent') editorComponent: TextEditorComponent;
  @Input() message: MessageInterface;
  @Input() currentUser: UserInterface;
  showDiscardModal: boolean;
  orginalText: string;

  constructor(
    private readonly httpService: HttpService,
    private readonly snackbarService: SnackbarService,
    private readonly coreService: CoreService,
    private translateService: TranslateService) { }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
  }


  setMessage($event) {
    const value = this.editorComponent?.editor?.nativeElement?.innerText.replace('/n', '').trim();
    if (!value && !value?.length) {
      this.message.showRequiredMessage = true;
      return;
    }
    this.orginalText = this.message.text;
    this.message.text = $event;
    this.message.showRequiredMessage = false;
  }

  saveMessage(message: MessageInterface) {
    const value = this.editorComponent?.editor?.nativeElement?.innerText.replace('/n', '').trim();
    if (!value && !value?.length) {
      this.message.showRequiredMessage = true;
      return;
    }
    const messageText = this.coreService.convertToPlain(message.text);
    if (messageText?.trim() == '' || messageText?.trim() == null) {
      message.showRequiredMessage = true;
      return;
    }
    message.showEdit = false;
    const formData = {
      Text: message.text,
      Attachments: []
    };
    this.httpService
      .patch(MessageApiEnum.UpdateMessage + `/${this.message.id}`, formData)
      .pipe(
        catchError(error => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(error);
        })
      )
      .subscribe((result) => {
        if (result == true) {
          this.snackbarService.showSuccess(
            this.translateService.instant('messages.editModal.successMessage')
          );
          this.message.text = message.text;
          message.modifiedOn = new Date();
        }
        else {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      });
  }

  cancelEditMessage(message: MessageInterface) {
    this.showDiscardModal = true;
    message.showRequiredMessage = false;
  }

  confirmDiscard() {
    
    this.message.text = this.orginalText ? this.orginalText : this.message.text;
    this.message.showEdit = false;
    this.showDiscardModal = false;
  }
}
