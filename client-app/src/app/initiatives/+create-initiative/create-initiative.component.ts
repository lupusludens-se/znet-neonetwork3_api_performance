import { Component, HostListener, OnInit } from '@angular/core';
import { TitleService } from 'src/app/core/services/title.service';
import { InitiativeContentDetails } from '../shared/models/initiative-content.interface';
import { CanComponentDeactivate } from 'src/app/shared/guards/can-deactivate.guard';

@Component({
  selector: 'neo-create-initiative',
  templateUrl: './create-initiative.component.html',
  styleUrls: ['./create-initiative.component.scss']
})
export class CreateInitiativeComponent implements OnInit, CanComponentDeactivate {
  initialData: InitiativeContentDetails = { id: 0 };
  showLeaveConfirmation = false;
  nextUrl: string;
  isBrowserBack = false;
  isSubmitAction = false;

  constructor(
    private titleService: TitleService
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle('initiative.createInitiative.createNewInitiativeLabel');
  }

  setInitiativeDetails(initativeDetails: InitiativeContentDetails): void {
    this.initialData = { ...initativeDetails };
  }

  @HostListener('window:popstate', ['$event'])
  onPopState($event: PopStateEvent): boolean {
    this.isBrowserBack = true;
    if (this.initialData.id !== 0) {
      this.showLeaveConfirmation = false;
      $event.preventDefault();
    }
    return true;
  }
  onSubmitAction(isSubmit: boolean): void {
    this.isSubmitAction = isSubmit;
  }
  canDeactivate(nextUrl?: string): boolean {
    // Show warning only for side menu navigation and browser back
    if (this.initialData.id !== 0 && !this.showLeaveConfirmation && !this.isSubmitAction) {
      this.showLeaveConfirmation = true;
      this.nextUrl = nextUrl;
      return false;
    }

    return true;
  }
}
