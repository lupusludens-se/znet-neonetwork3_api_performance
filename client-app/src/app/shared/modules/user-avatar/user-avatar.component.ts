import { Component, Input } from '@angular/core';
import { UserAvatarInterface } from '../../interfaces/user-avatar.interface';
import { UserInterface } from '../../interfaces/user/user.interface';
import { AVATAR_SIZES } from './constants/avatar-sizes.const';

@Component({
  selector: 'neo-user-avatar',
  templateUrl: 'user-avatar.component.html',
  styleUrls: ['user-avatar.component.scss']
})
export class UserAvatarComponent {
  @Input() user: UserInterface | UserAvatarInterface;
  @Input() imageSize: 'size16' | 'size20' | 'size24' | 'size32' | 'size48' | 'size56' | 'size96';

  avatarSizes = AVATAR_SIZES;
}
