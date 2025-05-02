import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { BreadcrumbInterface } from '../../interfaces/breadcrumb.interface';
import { BreadcrumbService } from '../../services/breadcrumb.service';
import { Router } from '@angular/router';
import { PermissionTypeEnum } from '../../enums/permission-type.enum';
import { AuthService } from '../../services/auth.service';
import { PermissionService } from '../../services/permission.service';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';

@Component({
  selector: 'neo-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BreadcrumbComponent implements OnInit {
  breadcrumbs$: Observable<BreadcrumbInterface[]>;
  currentUser: UserInterface;
  disableRedirection = false;
  private readonly excludedRoutes: string[] = ['topics'];
  private readonly excludedRoutesByPattern: RegExp[] = [
    /^initiatives\/\d+\/learn$/,
    /^initiatives\/\d+\/projects$/,
    /^initiatives\/\d+\/messages$/,
    /^initiatives\/\d+\/community$/,
    /^initiatives\/\d+\/tools$/];
  private readonly excludeAdminOnlyRoutesByPattern: RegExp[] = [/^decarbonization-initiatives$/]


  constructor(
    private readonly breadcrumbService: BreadcrumbService,
    private router: Router,
    private readonly authService: AuthService,
    private readonly permissionService: PermissionService
  ) { }

  ngOnInit(): void {
    this.breadcrumbs$ = this.breadcrumbService.getBreadcrumbs();
    this.authService.currentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
    });
  }

  navigationHandling(route: string) {
    if (!this.isClickable(route) || this.excludedRoutes.includes(route) || this.isExcludedRouteByPattern(route) || (this.isExcludedRouteByPatternForAdmin())) {
      return;
    }
    this.router.navigate([route]);
  }


  private isExcludedRouteByPattern(route: string): boolean {
    return this.excludedRoutesByPattern.some(pattern => pattern.test(route));
  }
  private isExcludedRouteByPatternForAdmin(): boolean {
    return this.disableRedirection;
  }

  isClickable(route: string): boolean {
    if (this.currentUser !== null) {
      this.disableRedirection = (this.currentUser.roles.some(role => role.id === RolesEnum.Admin) && this.excludeAdminOnlyRoutesByPattern.some(pattern => pattern.test(route))) ? true : false;
    }
    return !(
      route === 'projects' &&
      !this.permissionService.userHasPermission(this.currentUser, PermissionTypeEnum.ProjectCatalogView) &&
      !this.permissionService.userHasPermission(this.currentUser, PermissionTypeEnum.ProjectsManageAll)
    );
  }
}
