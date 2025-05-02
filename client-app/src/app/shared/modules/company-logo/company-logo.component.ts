import { Component, Input } from '@angular/core';
import { CompanyLogoInterface } from '../../interfaces/company-logo.interface';
import { CompanyInterface } from '../../interfaces/user/company.interface';

@Component({
  selector: 'neo-company-logo',
  templateUrl: './company-logo.component.html',
  styleUrls: ['./company-logo.component.scss']
})
export class CompanyLogoComponent {
  @Input() company: CompanyInterface | CompanyLogoInterface;
  @Input() imageSize: 'small' | 'large';

  constructor() {}
}
