import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { CoreService } from '../../../core/services/core.service';
import { HttpService } from '../../../core/services/http.service';
import { AuthService } from '../../../core/services/auth.service';
import { PinnedToolsRequestsService } from '../../services/pinned-tools-requests.service';
import { ActivityService } from '../../../core/services/activity.service';

import { ToolInterface } from '../../../shared/interfaces/tool.interface';
import { NewAndNoteworthyPostInterface } from '../../../+learn/interfaces/post.interface';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import { FirstClickInfoActivityDetailsInterface } from '../../../core/interfaces/activity-details/first-click-info-activity-details.interface';

import { CommonApiEnum } from '../../../core/enums/common-api.enum';
import { ToolsApiEnum } from '../../../shared/enums/api/tools-api.enum';
import { ActivityTypeEnum } from '../../../core/enums/activity/activity-type.enum';
import { DashboardClickElementActionTypeEnum } from '../../../core/enums/activity/dashboard-click-element-action-type.enum';
import { DashboardManagement } from '../../enums/dashboard-management.enum';
import { LocationStrategy } from '@angular/common';
import { SPDashboardProjectDetails } from '../../../shared/interfaces/projects/project.interface';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'neo-solution-provider-dashboard',
  templateUrl: './solution-provider-dashboard.component.html',
  styleUrls: ['./solution-provider-dashboard.component.scss', '../../styles/styles.scss']
})
export class SolutionProviderDashboardComponent implements OnInit {
  @Output() elementClick: EventEmitter<FirstClickInfoActivityDetailsInterface> =
    new EventEmitter<FirstClickInfoActivityDetailsInterface>();
  newAndNoteworthyArticles: NewAndNoteworthyPostInterface[] = [];
  loading: boolean = false;
  projects: SPDashboardProjectDetails[] = [];
  showToolsModal: boolean;
  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  pinnedTools$: Observable<ToolInterface[]> = this.pinnedToolsService.fetchPinnedTools();
  tools: ToolInterface[];
  isShowAddProject: boolean = true;
  trendingProgressPositions: number[] = [0];
  isNoProjects: boolean = false;
  projectCount: number;
  loadArticles = false;
  tools$: Observable<PaginateResponseInterface<ToolInterface>> = this.httpService
    .get<PaginateResponseInterface<ToolInterface>>(ToolsApiEnum.Tools, {
      filterBy: 'IsActive',
      expand: 'icon'
    })
    .pipe(
      map(response => {
        this.tools = response.dataList;
        return response;
      })
    );
  get selectedItems(): ToolInterface[] {
    return this.tools.filter(item => item.isPinned);
  }

  constructor(
    private readonly httpService: HttpService,
    public readonly router: Router,
    private readonly pinnedToolsService: PinnedToolsRequestsService,
    public readonly coreService: CoreService,
    private readonly authService: AuthService,
    private readonly activityService: ActivityService,
    private readonly locationStrategy: LocationStrategy
  ) {}

  ngOnInit(): void {
    this.loadNewAndTrendingArticles();
    this.loadActiveandDraftProjects();
  }

  loadNewAndTrendingArticles() {
    this.httpService
      .get(`${CommonApiEnum.Articles}/${DashboardManagement.NewAndNotworthy}`)
      .subscribe((response: any) => {
        this.newAndNoteworthyArticles = response.dataList as NewAndNoteworthyPostInterface[];
      });
    this.loadArticles = true;
  }

  closed(): void {
    this.tools.map(item => (item.isPinned = false));
    this.showToolsModal = false;
  }

  pinTool(index: number): void {
    this.pinnedToolsService.pinTool(this.tools, this.selectedItems, index);
  }

  savePinnedTools(): void {
    this.pinnedToolsService.savePinnedTools(this.tools).subscribe(() => {
      this.showToolsModal = false;
      this.pinnedTools$ = this.pinnedToolsService.fetchPinnedTools();
    });
  }

  onMessagesAllClick(): void {
    this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.MessagesView });
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ViewAllClick, { viewAllType: 'Message' })
      ?.subscribe();
  }

  onCustomizeToolsClick(): void {
    this.showToolsModal = true;
    this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.PinnedToolsCustomize });
  }

  onAddToolsClick(): void {
    this.showToolsModal = true;
    this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.PinnedToolAdd });
  }

  onToolClick(id: number): void {
    this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.PinnedToolView, id: id });
    this.activityService.trackElementInteractionActivity(ActivityTypeEnum.ToolClick, { toolId: id })?.subscribe();
  }

  onSaveArticleClick(articleId: number, isSaved: boolean): void {
    this.elementClick.emit({
      actionType: isSaved
        ? DashboardClickElementActionTypeEnum.LearnSave
        : DashboardClickElementActionTypeEnum.LearnUnsave,
      id: articleId
    });
  }

  onArticleTagClick(articleId: number): void {
    this.elementClick.emit({
      actionType: DashboardClickElementActionTypeEnum.LearnTagClick,
      id: articleId
    });
  }

  onArticleClick(articleId: number, articleTitle: string, url: string): void {
    this.elementClick.emit({
      actionType: DashboardClickElementActionTypeEnum.LearnView,
      id: articleId
    });

    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.LearnView, { id: articleId, title: articleTitle })
      ?.subscribe();

    const getBaseHref = location.origin + this.locationStrategy.getBaseHref();
    const learnUrl = getBaseHref + url;
    window.open(learnUrl, '_blank');
  }
  loadActiveandDraftProjects() {
    this.loading = false;
    this.httpService.get(DashboardManagement.SolutionProviderProjects).subscribe((response: any) => {
      this.projects = response as SPDashboardProjectDetails[];
      this.projects.forEach(project => {
        if (project.technologies.length > 0) {
          project.imageUrl = `${environment.baseAppUrl}assets/images/project-technologies-images/${project.technologies[0].slug}/${project.projectCategoryImage}.jpg`;
        } else {
          project.imageUrl = `${environment.baseAppUrl}assets/images/project-categories-images/${project.projectCategorySlug}/${project.projectCategoryImage}.jpg`;
        }
      });
      this.projectCount = this.projects?.length;
      if (this.projects?.length === 0) {
        this.isNoProjects = true;
        this.projects.push({ id: null });
      } else if (this.projects?.length < 3 && this.projects?.length > 0) {
        for (let i = 0; i <= 3 - this.projects?.length; i++) this.projects.push({ id: null });
      } else if (this.projects?.length > 3 && this.projects?.length <= 6) {
        this.trendingProgressPositions = [0, -3];
        for (let i = 0; i <= 6 - this.projects?.length; i++) this.projects.push({ id: null });
      } else if (this.projects?.length > 6 && this.projects?.length <= 9) {
        this.trendingProgressPositions = [0, -3, -6];
        for (let i = 0; i <= 9 - this.projects?.length; i++) this.projects.push({ id: null });
      } else {
        this.isShowAddProject = false;
      }
      this.loading = true;
    });
  }
}
