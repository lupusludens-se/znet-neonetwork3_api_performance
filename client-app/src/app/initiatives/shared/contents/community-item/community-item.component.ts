import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { InitiativeCommunityInterface } from '../../models/initiative-resources.interface';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';

export enum InitiativeCommunityItemParentModuleEnum {
  Create = 'create',
  InitiativeDashboard = 'initiative-dashboard',
  ViewRecommended = 'view-recommended',
  ViewSaved = 'view-saved'
}

@Component({
  selector: 'neo-initiative-community-item',
  templateUrl: './community-item.component.html',
  styleUrls: ['./community-item.component.scss']
})
export class CommunityItemComponent {
  @Input() communityUser: InitiativeCommunityInterface;
  @Input() parentModule: string = InitiativeCommunityItemParentModuleEnum.Create;
  @Input() isAdminOrTeamMember: boolean = false;
  @Input() initiativeId: number;
  @Output() selectCheckbox = new EventEmitter<InitiativeCommunityInterface>();

  initiativeParentModuleEnum = InitiativeCommunityItemParentModuleEnum;
  rolesEnum = RolesEnum;
  spAdminRoleLabel = 'Solution Provider';
  type = TaxonomyTypeEnum;
  readonly userStatuses = UserStatusEnum;
  @Output() readonly selectedUser = new EventEmitter<number>();
  options: MenuOptionInterface[] = [
    {
      icon: 'trash-can-red',
      name: 'initiative.viewInitiative.deleteSavedContentLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];

  constructor(private activityService: ActivityService) {}

  get getUser(): UserInterface {
    const { image, firstName, lastName } = this.communityUser || {};
    return { image, imageName: image?.name, firstName, lastName } as UserInterface;
  }

  openUserProfile(event: Event) {
    this.trackUserProfileActivity();
    window.open(`user-profile/${this.communityUser.id}`, '_blank');
    event.stopPropagation();
  }

  trackUserProfileActivity(): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.UserProfileView, {
        userId: this.communityUser.id,
        initiativeId: this.initiativeId
      })
      ?.subscribe();
  }

  optionClick(): void {
    this.selectedUser.emit(this.communityUser.id);
  }
}
