import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'neo-initiative-progress-tracker',
  templateUrl: './initiative-progress-tracker.component.html',
  styleUrls: ['./initiative-progress-tracker.component.scss']
})
export class InitiativeProgressTrackerComponent implements OnInit {
  stepData = [
    { id: 'STEP 1', name: 'Planning, Data collection & Education' },
    { id: 'STEP 2', name: 'Internal Alignment' },
    { id: 'STEP 3', name: 'Identity Providers & Collect Offers' },
    { id: 'STEP 4', name: 'Evaluate Offers & Choose Preliminary Partner' },
    { id: 'STEP 5', name: 'Negotiate, Refine terms & Contract' },
    { id: 'STEP 6', name: 'Execute' },
  ];
  constructor() { }

  ngOnInit():void {
}
}
