import { Component } from '@angular/core';

import { RolesEnum } from '../../../shared/enums/roles.enum';

@Component({
  selector: 'neo-solutions',
  templateUrl: './solutions.component.html',
  styleUrls: ['./solutions.component.scss']
})
export class SolutionsComponent {
  userRoles = RolesEnum;
}
