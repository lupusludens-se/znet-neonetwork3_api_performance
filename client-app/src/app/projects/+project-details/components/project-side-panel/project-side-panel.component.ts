import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import * as mapboxgl from 'mapbox-gl';

import { SaveContentService } from '../../../../shared/services/save-content.service';
import { environment } from '../../../../../environments/environment';
import { AuthService } from 'src/app/core/services/auth.service';
import { PermissionService } from 'src/app/core/services/permission.service';
import { Observable } from 'rxjs';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { TaxonomyTypeEnum } from '../../../../shared/enums/taxonomy-type.enum';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';
import { ProjectOffsitePpaInterface } from 'src/app/shared/interfaces/projects/project-creation.interface';
import { ProjectTypesSteps } from 'src/app/projects/+add-project/enums/project-types-name.enum';
import { ProjectStatusEnum } from '../../../../shared/enums/projects/project-status.enum';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { Router } from '@angular/router';

@Component({
  selector: 'neo-project-side-panel',
  templateUrl: './project-side-panel.component.html',
  styleUrls: ['./project-side-panel.component.scss']
})
export class ProjectSidePanelComponent implements OnInit {
  @Input() projectDetails: ProjectInterface;
  @Input() conversationId: number;

  @Output() updateProjectInfo: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() contactProvider: EventEmitter<boolean> = new EventEmitter<boolean>();

  saved: boolean;
  map: mapboxgl.Map;
  largeMap: mapboxgl.Map;
  showLargeMap: boolean;
  type = TaxonomyTypeEnum;
  isOffsitePPAProject: boolean;
  projectTypes = ProjectTypesSteps;
  projectStatusEnum = ProjectStatusEnum;

  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  currentUser: UserInterface = this.authService.currentUser$.getValue();
  isCurrentUserIsSolutionProvider: boolean;
  isCurrentUserHasEditPermission: boolean;
  roles = RolesEnum;

  constructor(
    private saveContentService: SaveContentService,
    private authService: AuthService,
    private permissionService: PermissionService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.isCurrentUserHasEditPermission =
      (this.currentUser?.roles?.map(r => r.id).includes(this.roles.SPAdmin) &&
        this.currentUser.companyId === this.projectDetails.companyId) ||
      this.currentUser.id === this.projectDetails.ownerId ||
      this.currentUser?.roles?.map(r => r.id).includes(this.roles.Admin);
    this.isCurrentUserIsSolutionProvider = this.currentUser?.roles?.some(
      r => r.id === this.roles.SolutionProvider || r.id === this.roles.SPAdmin
    );
    this.isOffsitePPAProject =
      (this.projectDetails?.category?.slug.toLowerCase() === this.projectTypes.OffsitePpa ||
        this.projectDetails?.category.slug === this.projectTypes.AggregatedPpa) &&
      this.projectDetails.projectDetails != null;

    if (this.isOffsitePPAProject) {
      this.setupMap();
    }
  }

  saveProject(): void {
    if (!this.projectDetails.isSaved) {
      this.saveContentService.saveProject(this.projectDetails.id).subscribe(() => this.updateProjectInfo.emit(true));
    } else {
      this.saveContentService.deleteProject(this.projectDetails.id).subscribe(() => this.updateProjectInfo.emit(true));
    }
  }

  hasPermission(user: UserInterface): boolean {
    return this.permissionService.userHasPermission(user, PermissionTypeEnum.ProjectCatalogView);
  }

  expandMap(): void {
    this.showLargeMap = true;
    setTimeout(() => this.setupLargeMap(), 100);
  }

  goToEditProject(): void {
    this.router
      .navigate([`projects-library/edit-project/${this.projectDetails.id}`], {
        state: {
          isCallFromProjectDetailsPage: true
        }
      })
      .then();
  }

  private setupMap(): void {
    const details = this.projectDetails.projectDetails as ProjectOffsitePpaInterface;

    setTimeout(function () {
      this.map = new mapboxgl.Map({
        accessToken: environment.mapBox.accessToken,
        container: 'map',
        style: environment.mapBox.styles,
        zoom: 13,
        center: [details.longitude, details.latitude]
      });

      new mapboxgl.Marker().setLngLat([details.longitude, details.latitude]).addTo(this.map);
    }, 100);
  }

  private setupLargeMap(): void {
    const details = this.projectDetails.projectDetails as ProjectOffsitePpaInterface;

    this.largeMap = new mapboxgl.Map({
      accessToken: environment.mapBox.accessToken,
      container: 'largeMap',
      style: environment.mapBox.styles,
      zoom: 13,
      center: [details.longitude, details.latitude]
    });

    // Add zoom and rotation controls to the map.
    this.largeMap.addControl(new mapboxgl.NavigationControl());

    new mapboxgl.Marker().setLngLat([details.longitude, details.latitude]).addTo(this.largeMap);
  }
}
