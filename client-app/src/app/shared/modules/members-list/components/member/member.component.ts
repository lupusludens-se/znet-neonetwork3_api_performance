import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { MemberInterface } from '../../interfaces/member.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'neo-member',
  templateUrl: './member.component.html',
  styleUrls: ['./member.component.scss']
})
export class MemberComponent {
  @Input() showActions: boolean;
  @Input() bordered: boolean;
  @Input() user: MemberInterface;
  @Input() isCompanyProfile: boolean = false;
  @Input() isCompanyFollowers: boolean = false;
  @Output() followClick: EventEmitter<MemberInterface> = new EventEmitter<MemberInterface>();
  @Output() profileClick: EventEmitter<boolean> = new EventEmitter<boolean>();

  readonly userStatuses = UserStatusEnum;

  constructor(private readonly router: Router) {}
  emitUserClick(): void {
    this.profileClick.emit(true);
    this.router.navigate(this.user.statusId === this.userStatuses.Deleted ? [] : ['/user-profile/', this.user.id]);
  }
}
