import { Component, Input, OnInit } from '@angular/core';
import { ViewInitiativeService } from '../../services/view-initiative.service';
import {
  InitiativeProgress,
  InitiativeStep,
  InitiativeSubStep
} from 'src/app/initiatives/interfaces/initiative-progress.interface';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';

@Component({
  selector: 'neo-progress-tracker',
  templateUrl: './progress-tracker.component.html',
  styleUrls: ['./progress-tracker.component.scss']
})
export class ProgressTrackerComponent implements OnInit {
  @Input() initiativeProgress: InitiativeProgress;
  @Input() isAdminOrTeamMember: boolean = false;
  initiativeStep: InitiativeStep;
  currentStepNumber: number;
  constructor(
    private viewInitiativeService: ViewInitiativeService,
    private snackbarService: SnackbarService,
    private readonly activityService: ActivityService
  ) {}

  ngOnInit(): void {
    
    this.currentStepNumber = this.initiativeProgress.currentStepId;
    this.initiativeProgress.steps.forEach(st => {
      
      st.isActive = st.stepId == this.initiativeProgress.currentStepId ? true : false;
      st.completed = (st.subSteps.filter(x => x.isChecked == true).length * 100) / st.subSteps.length;
    });
    this.initiativeStep = this.initiativeProgress.steps.find(x => x.isActive == true);
  }

  goNext(stepId, isNext: boolean): void {
    
    let currentIndex = this.initiativeProgress.steps.findIndex(x => x.stepId == stepId);
    if (currentIndex > -1) {
      if (isNext) {
        this.initiativeProgress.steps[currentIndex].isActive = false;
        this.initiativeProgress.steps[currentIndex + 1].isActive = true;
        this.currentStepNumber = stepId + 1;
      } else {
        this.initiativeProgress.steps[currentIndex].isActive = false;
        this.initiativeProgress.steps[currentIndex - 1].isActive = true;
        this.currentStepNumber = stepId - 1;
      }
    }
    this.initiativeStep = this.initiativeProgress.steps.find(x => x.isActive == true);
  }

  updateSubStep(subStep: InitiativeSubStep) {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativeSubstepClick, {
        id: this.initiativeProgress.initiativeId,
        stepDesc: this.initiativeStep.description,
        subStepId: subStep.subStepId
      })
      ?.subscribe();
    this.updateSubStepContent(subStep);
    
    const payload = {
      InitiativeId: this.initiativeProgress.initiativeId,
      SubStepId: subStep.subStepId,
      StepId: this.initiativeStep.stepId,
      IsChecked: subStep.isChecked,
      CurrentStep: this.initiativeProgress.currentStepId
    };
    this.viewInitiativeService
      .updateInitiativeSubStep(payload)
      .pipe(
        catchError(error => {
          this.snackbarService.showError('general.defaultErrorLabel');
          this.updateSubStepContent(subStep);
          return throwError(error);
        })
      )
      .subscribe(data => {
        if (data == false) this.snackbarService.showError('general.defaultErrorLabel');
      });
  }

  updateSubStepContent(subStep: InitiativeSubStep) {
    subStep.isChecked = !subStep.isChecked;
    
    let stepIndex = this.initiativeProgress.steps.findIndex(x => x.stepId == this.initiativeStep.stepId);

    let completePercentage =
      (this.initiativeProgress.steps[stepIndex].subSteps.filter(x => x.isChecked == true).length * 100) /
      this.initiativeProgress.steps[stepIndex].subSteps.length;
    this.initiativeProgress.steps[stepIndex].completed = completePercentage;

    if (this.initiativeProgress.currentStepId <= this.initiativeProgress.steps[stepIndex].stepId) {
      let newStep = this.getCurrentStep(stepIndex, this.initiativeProgress.steps.length);
      if (newStep != undefined && newStep != this.initiativeProgress.currentStepId) {
        this.initiativeProgress.currentStepId = newStep;
      }
    } else {
      let newStep = this.getCurrentStep(this.initiativeProgress.steps.length - 1, this.initiativeProgress.steps.length);
      if (newStep != undefined && newStep != this.initiativeProgress.currentStepId) {
        this.initiativeProgress.currentStepId = newStep;
      }
    }
    this.initiativeProgress = { ...this.initiativeProgress };
  }

  getCurrentStep(stepIndex, length): number {
    if (this.initiativeProgress.steps[stepIndex].completed == 100) {
      if (this.initiativeProgress.steps[stepIndex].stepId < this.initiativeProgress.steps[length - 1].stepId) {
        return this.initiativeProgress.steps[stepIndex + 1].stepId;
      } else {
        return this.initiativeProgress.steps[stepIndex].stepId;
      }
    } else if (
      this.initiativeProgress.steps[stepIndex].completed > 0 &&
      this.initiativeProgress.steps[stepIndex].completed < 100
    ) {
      return this.initiativeProgress.steps[stepIndex].stepId;
    } else {
      if (stepIndex > 0) {
        return this.getCurrentStep(stepIndex - 1, length);
      } else {
        return this.initiativeProgress.steps[stepIndex].stepId;
      }
    }
  }
}
