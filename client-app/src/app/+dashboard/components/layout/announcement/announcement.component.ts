import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AnnouncementInterface } from '../../../../+admin/modules/announcements/interfaces/announcement.interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';

@Component({
  selector: 'neo-announcement',
  templateUrl: './announcement.component.html',
  styleUrls: ['./announcement.component.scss']
})
export class AnnouncementComponent {
  @Input() cssClasses: string;
  @Input() announcement: AnnouncementInterface;
  @Input() isPublicUser : boolean = false;

  @Output() announcementClick: EventEmitter<void> = new EventEmitter<void>();

  activityEnum = ActivityTypeEnum.AnnouncementButtonClick
}
