import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FeedbackInterface } from '../../interfaces/feedback.interface';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { UserRoleInterface } from 'src/app/shared/interfaces/user/user-role.interface';

@Component({
  selector: 'neo-feedback-table-row',
  templateUrl: './feedback-table-row.component.html',
  styleUrls: ['../../feedback-management.component.scss', './feedback-table-row.component.scss']
})
export class FeedbackTableRowComponent {
  @Input() feedback: FeedbackInterface;
  userStatuses = UserStatusEnum;
  @Output() updateFeedback: EventEmitter<boolean> = new EventEmitter<boolean>();
  constructor() {}

  getClassNamesBasedonRole(role: UserRoleInterface) {
    return role.id == RolesEnum.SPAdmin ? role.name.toLowerCase().replace(/\s/g, '') : role.name.toLowerCase();
  }
}
