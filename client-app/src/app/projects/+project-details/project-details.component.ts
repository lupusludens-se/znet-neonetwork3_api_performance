import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ProjectDetailsService } from './services/project-details.service';
import { TitleService } from '../../core/services/title.service';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';
import { MessageApiEnum } from 'src/app/+messages/enums/message-api.enum';
import { HttpService } from 'src/app/core/services/http.service';
import { ResourceInterface } from 'src/app/core/interfaces/resource.interface';
import { ProjectTypesSteps } from '../+add-project/enums/project-types-name.enum';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { ProjectsViewTypeEnum } from '../+projects/services/project-catalog.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { catchError, filter, combineLatest, Subject, throwError } from 'rxjs';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { ProjectApiRoutes } from '../shared/constants/project-api-routes.const';
import { PermissionService } from 'src/app/core/services/permission.service';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CoreService } from 'src/app/core/services/core.service';
import { environment } from 'src/environments/environment';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CommonService } from 'src/app/core/services/common.service';
import { ProjectService } from '../+projects/services/project.service';
import { ProjectComponentEnum } from '../shared/enums/project-component.enum';
import { InitiativeAttachedContent } from 'src/app/initiatives/interfaces/initiative-attached.interface';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { takeUntil } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';


@UntilDestroy()
@Component({
  selector: 'neo-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.scss']
})
export class ProjectDetailsComponent implements OnInit, OnDestroy {
  roles = RolesEnum;
  showContactModal: boolean;
  showSolutionProviderModal: boolean;
  projectInfo: ProjectInterface;
  isOffsitePPAProject: boolean;
  conversationId: number;
  apiRoutes = MessageApiEnum;
  projectResources: ResourceInterface[];
  backgroundUrl: string;
  projectTypes = ProjectTypesSteps;

  currentUser: UserInterface;
  routesToNotClearFilters: string[] = [`${ProjectComponentEnum.ProjectsComponent}`];
  attachToInitiative: boolean = false;
  contentType: string = InitiativeModulesEnum[InitiativeModulesEnum.Projects];
  projectId: number;
  isCorporateUser: boolean = false;
  private unsubscribe$ = new Subject<void>();
  constructor(
    private httpService: HttpService,
    private projectDetailsService: ProjectDetailsService,
    private activatedRoute: ActivatedRoute,
    private titleService: TitleService,
    private permissionService: PermissionService,
    private authService: AuthService,
    private readonly activityService: ActivityService,
    private readonly router: Router,
    private readonly coreService: CoreService,
    private commonService: CommonService,
    private projectService: ProjectService,
    private translateService: TranslateService
  ) { }

  ngOnInit(): void {
    combineLatest([this.authService.currentUser(), this.activatedRoute.params])
      .pipe(untilDestroyed(this))
      .subscribe(([user, p]) => {
        if (!user) return;
        this.currentUser = user;
        this.isCorporateUser = user.roles.some(role => role.id === RolesEnum.Corporation);
        this.getProject(+p.id);
        this.projectId = +p.id;
      });
  }

  closeContactModal(isSuccess: boolean) {
    this.showSolutionProviderModal = false;
    if (isSuccess) this.getConversations();
  }

  modalClosed(): void {
    this.showContactModal = false;
  }

  getProject(id: number): void {
    this.projectDetailsService
      .getProjectDetails(
        id,
        this.currentUser?.roles?.map(r => r.id).includes(this.roles.SolutionProvider)
          ? `&projectsViewType=${ProjectsViewTypeEnum.Library}`
          : this.currentUser?.roles?.map(r => r.id).includes(this.roles.Corporation)
            ? `&projectsViewType=${ProjectsViewTypeEnum.Catalog}`
            : ''
      )
      .pipe(
        catchError((error: HttpErrorResponse) => {
          let buttonLink = '';
          if (error.status === 404) {
            if (
              this.currentUser.roles.find(x => x.id == RolesEnum.SPAdmin) ||
              this.currentUser.roles.find(x => x.id == RolesEnum.SolutionProvider)
            ) {
              buttonLink = '/projects-library';
            } else {
              buttonLink = '/projects';
            }
            this.coreService.elementNotFoundData$.next({
              iconKey: 'projects',
              mainTextTranslate: 'projects.notFoundText',
              buttonTextTranslate: 'projects.notFoundButton',
              buttonLink: buttonLink
            });
          }

          return throwError(error);
        })
      )
      .subscribe((res: ProjectInterface) => {
        this.projectInfo = res;
        this.titleService.setTitle(res.title);
        
        this.backgroundUrl = `${environment.baseAppUrl
          }assets/images/project-categories/${res.category?.slug.toLowerCase()}.jpg`;
        this.getProjectResourceDetails(res.id);
        this.getConversations();

        if (this.permissionService.userHasPermission(this.currentUser, PermissionTypeEnum.ProjectCatalogView))
          this.setProjectView();
      });
  }

  getConversations(): void {
    this.httpService
      .get<number>(`${this.apiRoutes.ContactProviderConversation}/${this.projectInfo.id}`)
      .subscribe(data => {
        this.conversationId = data;
      });
  }

  getProjectResourceDetails(id: number): void {
    this.projectDetailsService
      .getProjectResourceDetails(
        id,
        this.currentUser?.roles?.map(r => r.id).includes(this.roles.SolutionProvider)
          ? `&projectsViewType=${ProjectsViewTypeEnum.Library}`
          : this.currentUser?.roles?.map(r => r.id).includes(this.roles.Corporation)
            ? `&projectsViewType=${ProjectsViewTypeEnum.Catalog}`
            : ''
      ).subscribe(res => {
       const techResources = [].concat(...res.technologies.map(t => t.resources), res.category?.resources);

        const map = new Map();
        techResources.forEach(t => map.set(t.id, t));
        this.projectResources = Array.from(map.values());
      }); 
  }

  onProjectSidePanelUpdateProjectInfo(): void {
    this.projectInfo.isSaved = !this.projectInfo.isSaved;
  }

  goBack() {
    this.commonService.goBack();
  }

  onResourceClick(id: number): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ProjectResourceClick, { resourceId: id })
      ?.subscribe();
  }

  private setProjectView(): void {
    this.httpService
      .post<unknown>(`${ProjectApiRoutes.projectsList}/${this.projectInfo.id}/${ProjectApiRoutes.projectsView}`)
      .subscribe();
  }

  generateResourceLink(resource: ResourceInterface): string {
    if (resource.articleId) return 'learn/' + resource.articleId;

    if (resource.toolId) return 'tools/' + resource.toolId;

    return resource.referenceUrl;
  }

  onContactProviderClick(): void {
    this.showSolutionProviderModal = true;
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ContactProviderClick, { companyId: this.projectInfo.companyId })
      ?.subscribe();
  }

  trackAttachToInitiativeActivity() {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        buttonName: this.translateService.instant('initiative.attachContent.attachProjectLabel'),
        moduleName: InitiativeModulesEnum[InitiativeModulesEnum.Projects]
      })
      ?.subscribe();
  }

  ngOnDestroy(): void {
    const routesFound = this.routesToNotClearFilters.some(val => this.coreService.getOngoingRoute().includes(val));
    if (!routesFound) {
      this.commonService.clearFilters(this.commonService.filterState$.getValue(), false);
      this.projectService.clearPaging();

    }
  }
}
