import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { InitiativeProgress } from 'src/app/initiatives/interfaces/initiative-progress.interface';

@Component({
  selector: 'neo-progress-stepper',
  templateUrl: './progress-stepper.component.html',
  styleUrls: ['./progress-stepper.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProgressStepperComponent implements OnInit {
  @Input() initiativeprogressdetails: InitiativeProgress;
  @Input() currentStep: number;
  @Input() canHideStepDescription: boolean = false;
  constructor(private changeDetector: ChangeDetectorRef) {}

  ngOnInit(): void {}

  getStepProgressColor(step): string {
    
    if (step.stepId == this.currentStep) {
      return 'is-active';
    }
    if (step.completed == 100) {
      return 'is-complete';
    }
    
    if (
      ((step.stepId < this.initiativeprogressdetails.currentStepId || step.stepId < this.currentStep) &&
        step.completed < 100) ||
      (step.completed > 0 && step.completed < 100)
    ) {
      return 'is-partial';
    }
    return 'is-inactive';
  }
}
