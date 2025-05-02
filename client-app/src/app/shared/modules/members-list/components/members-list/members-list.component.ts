import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';

import { Observable } from 'rxjs';

import { AuthService } from '../../../../../core/services/auth.service';

import { UserInterface } from '../../../../interfaces/user/user.interface';
import { MemberInterface } from '../../interfaces/member.interface';

@Component({
  selector: 'neo-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.scss']
})
export class MembersListComponent implements OnChanges {
  @Input() title: string;
  @Input() noMembersMessage: string;
  @Input() iconKey: string = 'user-unavailable';
  @Input() members: MemberInterface[];
  @Input() isCompanyProfile: boolean = false;
  @Output() followClick: EventEmitter<MemberInterface> = new EventEmitter<MemberInterface>();

  membersDuplicate: MemberInterface[];
  membersModal: boolean;
  search: string;

  currentUser$: Observable<UserInterface> = this.authService.currentUser();

  constructor(private readonly authService: AuthService) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.members?.currentValue) {
      this.membersDuplicate = [...this.members];
    }
  }

  searchMembers(search: string): void {
    const searchToLoverCase = search.toLowerCase();
    this.search = search;
    this.members = this.membersDuplicate.filter(
      user =>
        `${user.firstName} ${user.lastName}`.toLowerCase().includes(searchToLoverCase) ||
        user?.company?.toLowerCase()?.includes(searchToLoverCase)
    );
  }

  clear(): void {
    this.members = this.membersDuplicate;
  }
}
