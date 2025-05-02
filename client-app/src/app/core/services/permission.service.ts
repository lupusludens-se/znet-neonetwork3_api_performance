import { Injectable } from '@angular/core';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  userHasPermission(user: UserInterface, permissionType: number): boolean {
    const allPermissions = [].concat(...(user?.roles.map(r => r?.permissions) ?? []), user?.permissions).map(p => p.id);

    return this.hasPermission(allPermissions, permissionType);
  }

  hasPermission(userPermissions: number[], permissionType: number): boolean {
    return userPermissions?.includes(permissionType);
  }
}
