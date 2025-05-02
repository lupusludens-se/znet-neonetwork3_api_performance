import { Component, EventEmitter, Input, Output } from '@angular/core';
import { EventInterface } from 'src/app/shared/interfaces/event/event.interface';
import { UserInterface } from '../../../../shared/interfaces/user/user.interface';
import { PermissionTypeEnum } from '../../../../core/enums/permission-type.enum';
import { PermissionService } from '../../../../core/services/permission.service';

@Component({
  selector: 'neo-event-item',
  templateUrl: './event-item.component.html',
  styleUrls: ['./event-item.component.scss']
})
export class EventItemComponent {
  @Input() eventData: EventInterface;
  @Input() currentUser: UserInterface;
  @Input() attendanceEnable: boolean = true;
  @Input() publicReadMore: boolean = false;
  @Output() changeAttendance: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private readonly permissionService: PermissionService) {}

  hasPermission(): boolean {
    return this.permissionService.userHasPermission(this.currentUser, PermissionTypeEnum.EventManagement);
  }
}
