import { Component, Input } from '@angular/core';

import { SolarRequestTypeEnum } from '../../../enums/solar-request-type.enum';
import { Router } from '@angular/router';

@Component({
  selector: 'neo-indicative-quote-request-form',
  templateUrl: './indicative-quote-request-form.component.html',
  styleUrls: ['./indicative-quote-request-form.component.scss']
})
export class IndicativeQuoteRequestFormComponent {
  @Input() toolId: number;

  solarRequestType = SolarRequestTypeEnum;
  selectedFormType: SolarRequestTypeEnum;

  constructor(private readonly router: Router) {}

  cancel() {
    this.router.navigate(['/tools']);
  }
}
