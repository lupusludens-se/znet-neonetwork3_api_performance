import { Component, Input } from '@angular/core';
import { InitiativeAdminResponse } from '../../interfaces/initiative-admin';
import { InitiativeStatusEnum } from '../../enums/initiative-status.enum';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';

@Component({
  selector: 'neo-initiative-table-row',
  templateUrl: './initiative-table-row.component.html',
  styleUrls: ['../../initiatives-management.component.scss', './initiative-table-row.component.scss']
})
export class InitiativeTableRowComponent {
  @Input() initiative: InitiativeAdminResponse;
  initiativeStatus = InitiativeStatusEnum;
  userStatus = UserStatusEnum;
  constructor() {}
}
