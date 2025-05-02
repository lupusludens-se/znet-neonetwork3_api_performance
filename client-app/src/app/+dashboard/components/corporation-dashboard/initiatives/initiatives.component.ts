import { Component, EventEmitter, Output } from '@angular/core';
import { FirstClickInfoActivityDetailsInterface } from 'src/app/core/interfaces/activity-details/first-click-info-activity-details.interface';
import { LocationStrategy } from '@angular/common';
import { InitiativeViewSource } from 'src/app/initiatives/+decarbonization-initiatives/enums/initiative-view-source';
import { Router } from '@angular/router';

@Component({
  selector: 'neo-initiatives',
  templateUrl: './initiatives.component.html',
  styleUrls: ['./initiatives.component.scss']
})
export class InitiativesComponent {
  @Output() elementClick: EventEmitter<FirstClickInfoActivityDetailsInterface> =
    new EventEmitter<FirstClickInfoActivityDetailsInterface>();
  initiativeSource: InitiativeViewSource = InitiativeViewSource['Dashboard'];
  constructor(private locationStrategy: LocationStrategy, private router: Router) {}

  openDecarbonizationInitiatives() {
    this.router.navigate(['/decarbonization-initiatives']);
  }
}
