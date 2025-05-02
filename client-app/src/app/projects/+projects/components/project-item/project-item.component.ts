import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable, Subject, takeUntil } from 'rxjs';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { PermissionService } from 'src/app/core/services/permission.service';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';

import { ProjectInterface } from '../../../../shared/interfaces/projects/project.interface';
import { SaveContentService } from '../../../../shared/services/save-content.service';
import { ProjectCatalogService } from '../../services/project-catalog.service';
import { UserRoleInterface } from '../../../../shared/interfaces/user/user-role.interface';
import { RolesEnum } from '../../../../shared/enums/roles.enum';
import { ProjectService } from '../../services/project.service';
import { SearchType } from '../../projects.component';

@Component({
  selector: 'neo-project-item',
  templateUrl: './project-item.component.html',
  styleUrls: ['./project-item.component.scss']
})
export class ProjectItemComponent {
  @Input() project: ProjectInterface;
  @Output() hoverProject: EventEmitter<ProjectInterface> = new EventEmitter<ProjectInterface>();
  @Output() removeProjectFromSaved: EventEmitter<ProjectInterface> = new EventEmitter<ProjectInterface>();

  type = TaxonomyTypeEnum;
  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  showMap: boolean;
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private saveContentService: SaveContentService,
    private projectCatalogService: ProjectCatalogService,
    private readonly authService: AuthService,
    private readonly permissionService: PermissionService,
    private readonly projectService: ProjectService
  ) {}

  ngOnInit() {
    this.projectService
      .getshowMap()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(val => {
        this.showMap = val;
      });
  }

  saveProject(): void {
    this.saveContentService.saveProject(this.project.id).subscribe(() => (this.project.isSaved = true));
  }

  deleteSavedProject(): void {
    this.saveContentService.deleteProject(this.project.id).subscribe(() => {
      this.project.isSaved = false;
      if (this.projectService.getSearchTypeValue() === SearchType.Saved) {
        this.removeProjectFromSaved.emit(this.project);
      }
    });
  }

  pinProject(): void {
    this.projectCatalogService
      .changeProjectPinned(this.project.id, true)
      .subscribe(() => (this.project.isPinned = true));
  }

  unpinProject(): void {
    this.projectCatalogService
      .changeProjectPinned(this.project.id, false)
      .subscribe(() => (this.project.isPinned = false));
  }

  hasPermissionToPin(user: UserInterface): boolean {
    return this.permissionService.userHasPermission(user, PermissionTypeEnum.ProjectsManageAll);
  }

  hasPermissionToSave(user: UserInterface): boolean {
    return this.permissionService.userHasPermission(user, PermissionTypeEnum.ProjectCatalogView);
  }

  isAdmin(roles: UserRoleInterface[]): boolean {
    return roles?.some(role => role.id === RolesEnum.Admin && role.isSpecial);
  }
  onHover(project: ProjectInterface) {
    this.hoverProject.emit(project);
  }
}
