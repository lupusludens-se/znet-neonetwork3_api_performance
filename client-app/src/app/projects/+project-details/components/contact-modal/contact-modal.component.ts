import { Component, EventEmitter, Input, Output } from '@angular/core';

import { ProjectInterface } from '../../../../shared/interfaces/projects/project.interface';
import { MessageApiEnum } from '../../../../+messages/enums/message-api.enum';
import { NewConversationMessageInterface } from 'src/app/+messages/interfaces/conversation.interface';
import { HttpService } from 'src/app/core/services/http.service';
import { DiscussionSourceTypeEnum } from '../../../../shared/enums/discussion-source-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'neo-contact-modal',
  templateUrl: './contact-modal.component.html',
  styleUrls: ['contact-modal.component.scss']
})
export class ContactModalComponent {
  @Output() closeModal: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() project: ProjectInterface;
  apiRoutes = MessageApiEnum;
  message: string;
  showSuccess: boolean = false;

  constructor(
    private httpService: HttpService,
    private activityService: ActivityService,
    private translateService: TranslateService
  ) {}

  onClose(): void {
    this.closeModal.emit(this.showSuccess);
  }

  onNevermindButtonClick(): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ContactProviderNevermindButtonClick, {
        companyId: this.project.companyId
      })
      ?.subscribe();
    this.onClose();
  }

  sendMessage(): void {
    this.showSuccess = true;
    const payload: NewConversationMessageInterface = {
      subject: 'Project conversation',
      projectId: this.project.id,
      sourceTypeId: DiscussionSourceTypeEnum.ProviderContact,
      companyId: this.project.companyId, // for tracking activity
      message: {
        text:
          this.message ||
          this.translateService.instant('projects.projectDetails.contactSolutionProviderModal.messagePlaceholder')
      },
      users: [{ id: this.project.ownerId }]
    };
    this.httpService.post(this.apiRoutes.Conversations, payload).subscribe(() => (this.showSuccess = true));
  }
}
