import { Component, EventEmitter, Input, Output } from '@angular/core';

import { CommunityInterface } from '../../interfaces/community.interface';

import { CommunityDataService } from '../../services/community.data.service';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { ROLES_DATA } from 'src/app/+admin/modules/+company-management/constants/parameter.const';

@Component({
  selector: 'neo-community-user',
  templateUrl: './community-user.component.html',
  styleUrls: ['./community-user.component.scss']
})
export class CommunityUserComponent {
  @Input() communityUser: CommunityInterface;
  rolesData: TagInterface[] = ROLES_DATA;

  @Output() userClick: EventEmitter<void> = new EventEmitter<void>();
  type = TaxonomyTypeEnum;
  readonly userStatuses = UserStatusEnum;
  rolesEnum = RolesEnum;
  @Output() followClick: EventEmitter<void> = new EventEmitter<void>();

  constructor(private readonly communityDataService: CommunityDataService) {}

  get getUser(): UserInterface {
    return {
      image: this.communityUser?.image,
      imageName: this.communityUser?.image?.name,
      firstName: this.communityUser?.firstName,
      lastName: this.communityUser?.lastName
    } as UserInterface;
  }

  getRoleLabel(communityUserRoleId: number): string {
    let roleLabel: string;
    switch (communityUserRoleId) {
      case this.rolesEnum.Admin:
        roleLabel = this.rolesData.find(role => role.id === 3)?.name;
        break;
      case this.rolesEnum.SPAdmin:
        roleLabel = this.rolesData.find(role => role.id === 2)?.name;
        break;
      case this.rolesEnum.Internal:
        roleLabel = this.rolesData.find(role => role.id === 1)?.name;
        break;
      case this.rolesEnum.SystemOwner:
        roleLabel = this.rolesData.find(role => role.id === 7).name;
        break;
      default:
        roleLabel = this.communityUser?.role.name;
        break;
    }
    return roleLabel;
  }

  follow(httpMethod: string): void {
    this.communityDataService.followUser(this.communityUser.id, httpMethod).subscribe(res => {
      if (!res.errorMessages) this.communityUser.isFollowed = !this.communityUser.isFollowed;
      this.followClick.emit();
    });
  }
}
