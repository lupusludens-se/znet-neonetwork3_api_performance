import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { AuthService } from '../../../../../core/services/auth.service';
@Component({
  selector: 'neo-tool',
  templateUrl: './tool.component.html',
  styleUrls: ['./tool.component.scss']
})
export class ToolComponent implements OnInit {
  @Input() id: number;
  @Input() pinned: boolean;
  @Input() wrapTitle: boolean = false;
  @Input() titleCenter: boolean = false;
  @Input() titleSize;
  @Input() icon: string;
  @Input() title: string;
  @Input() description: string;
  @Input() tileHeight: string;
  @Input() size: 'wide' | 'medium' | 'fixed';
  @Input() showButton: boolean;
  @Input() showFavoriteButton: boolean;

  @Output() pinClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() toolClick: EventEmitter<boolean> = new EventEmitter<boolean>();
  auth = AuthService;
  activityTypeEnum: any;
  ngOnInit() {
    this.activityTypeEnum = ActivityTypeEnum.ToolClick;
  }
}
