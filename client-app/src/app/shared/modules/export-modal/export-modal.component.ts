import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';

import { UserManagementApiEnum } from '../../../user-management/enums/user-management-api.enum';
import { UserDataService } from '../../../user-management/services/user.data.service';
import { ExportModule } from '../../enums/export-module.enum';
import { FeedbackService } from 'src/app/+admin/modules/+feedback-management/services/feedback.service';
import { InitiativeService } from 'src/app/+admin/modules/+initiatives-management/services/initiative.service';

@Component({
  selector: 'neo-export-modal',
  templateUrl: 'export-modal.component.html',
  styleUrls: ['export-modal.component.scss']
})
export class ExportModalComponent {
  @Input() recordsCount: number;
  @Input() title: string;
  @Input() subText: string;
  @Input() params: string;
  @Input() exportModule: ExportModule = ExportModule.UsersExport;
  @Input() itemsFoundDesc: string;
  @Output() closeModal = new EventEmitter<boolean>();
  fileIsReady: boolean;
  progress: boolean;
  apiRoutes = UserManagementApiEnum;
  fileData: string;
  fileName: string;
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(private userDataService: UserDataService, private feedbackService: FeedbackService,private initiativeService: InitiativeService) {}

  requestFile(): void {
    this.progress = true;

    if (this.exportModule === ExportModule.UsersExport || this.exportModule === ExportModule.CompanyUsersExport) {
      this.userDataService
        .exportUsers(this.exportModule === ExportModule.CompanyUsersExport)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(usersData => {
          this.fileIsReady = true;
          this.fileData = usersData.fileData;
          this.fileName = usersData.fileName;
          this.progress = false;
        });
    } else if (this.exportModule == ExportModule.FeedbackExport) {
      this.feedbackService
        .exportFeedbacks()
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(fbData => {
          this.fileIsReady = true;
          this.fileData = fbData.fileData;
          this.fileName = fbData.fileName;
          this.progress = false;
        });
    }
  else if (this.exportModule == ExportModule.InitiativeExport) {
    this.initiativeService
      .exportInitiatives()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(fbData => {
        this.fileIsReady = true;
        this.fileData = fbData.fileData;
        this.fileName = fbData.fileName;
        this.progress = false;
      });
  }
  }

  downloadFile(): void {
    const blob = new Blob([this.fileData]);
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = this.fileName;
    link.click();
    this.closeModal.emit(true);
  }
}
