import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { HttpService } from 'src/app/core/services/http.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { CreateInitiativeService } from 'src/app/initiatives/+create-initiative/services/create-initiative.service';
import { InitiativeApiEnum } from 'src/app/initiatives/enums/initiative-api.enum';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import {
  InitiativeAttachedContent,
  InitiativeAutoAttachedDetails
} from 'src/app/initiatives/interfaces/initiative-attached.interface';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';

@Component({
  selector: 'neo-attach-to-initiative',
  templateUrl: './attach-to-initiative.component.html',
  styleUrls: ['./attach-to-initiative.component.scss']
})
export class AttachToInitiativeComponent implements OnInit {
  @Output() readonly closed = new EventEmitter<void>();
  @Output() readonly initiativeLoadedStatus = new EventEmitter<void>();
  @Input() initiativeContentType: InitiativeModulesEnum;
  @Input() contentId: number;
  @Input() typeName: string;

  initiatives: InitiativeAttachedContent[];
  saveButtonDisable: boolean;
  hasError = false;

  constructor(
    private readonly httpService: HttpService,
    private readonly router: Router,
    private readonly snackbarService: SnackbarService,
    private readonly activityService: ActivityService,
    private readonly createInitiativeService: CreateInitiativeService,
    private readonly translateService: TranslateService,
    private readonly initiativeSharedService: InitiativeSharedService
  ) {}

  ngOnInit(): void {
    this.loadInitiatives();
  }

  private loadInitiatives = (): void => {
    this.initiativeSharedService
      .getInitiativeAttachedDetails(this.contentId.toString(), this.initiativeContentType.toString())
      .subscribe(response => {
        this.initiatives = response.map(initiative => ({
          ...initiative,
          isChecked: initiative.isAttached
        }));
        this.saveButtonDisable = this.initiatives.every(initiative => initiative.isAttached);
        this.initiativeLoadedStatus.emit();
      });
  };

  createInitiative = (): void => {
    this.createInitiativeService.autoAttached = {
      contentId: this.contentId,
      contentType: this.initiativeContentType,
      isAdded: false
    } as InitiativeAutoAttachedDetails;
    this.activityService.trackElementInteractionActivity(ActivityTypeEnum.InitiativeCreate)?.subscribe();
    this.router.navigate(['/create-initiative']);
  };

  saveContent = (): void => {
    const checkedInitiatives = this.initiatives.filter(initiative => initiative.isChecked !== initiative.isAttached);
    if (checkedInitiatives.length > 0) {
      this.activityService
        .trackElementInteractionActivity(ActivityTypeEnum.SaveContentFromAttachContentPopUp, {
          initiativeIds: checkedInitiatives.map(initiative => initiative.initiativeId),
          moduleName: this.initiativeContentType,
          contentId: this.contentId
        })
        ?.subscribe();
      this.hasError = false;
      this.httpService
        .put(InitiativeApiEnum.AttachContentToInitiative, {
          contentId: this.contentId,
          contentType: this.initiativeContentType,
          initiativeIds: checkedInitiatives.map(initiative => initiative.initiativeId)
        })
        .subscribe(response => {
          if (response) {
            this.initiatives.forEach(initiative => (initiative.isAttached = initiative.isChecked));
            const messageKey =
              checkedInitiatives.length > 1
                ? 'initiative.attachContent.successMultipleInitiativeLabel'
                : 'initiative.attachContent.successSingleInitiativeLabel';
            this.snackbarService.showSuccess(`${this.typeName} ${this.translateService.instant(messageKey)}`);
            this.closed.emit();
          } else {
            this.snackbarService.showError(
              this.translateService.instant('initiative.attachContent.initiativeIsInvalidToAttach')
            );
          }
        });
    } else {
      this.hasError = true;
    }
  };
}
