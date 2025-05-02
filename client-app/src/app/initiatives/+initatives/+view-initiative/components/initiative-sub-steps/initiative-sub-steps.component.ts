import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { InitiativeStep, InitiativeSubStep } from 'src/app/initiatives/interfaces/initiative-progress.interface';

@Component({
  selector: 'neo-initiative-sub-steps',
  templateUrl: './initiative-sub-steps.component.html',
  styleUrls: ['./initiative-sub-steps.component.scss']
})
export class InitiativeSubStepsComponent implements OnInit {
  @Input() initiativeStep: InitiativeStep;
  @Input() currentStep: number;
  @Input() isAdmin: boolean = false;
  isContentAvailable: boolean = false;
  @Output() updateSubStep: EventEmitter<InitiativeSubStep> = new EventEmitter<InitiativeSubStep>();
  constructor() {}

  ngOnInit(): void {
    this.isContentAvailable = this.initiativeStep.subSteps.find(x => x.content !== null && x.title === '')
      ? true
      : false;
  }

  subStepClick(subStep) {
    this.updateSubStep.emit(subStep);
  }
}
