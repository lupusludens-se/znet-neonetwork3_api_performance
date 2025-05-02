import { Component, EventEmitter, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { ProjectApiRoutes } from '../../../projects/shared/constants/project-api-routes.const';
import { HttpService } from '../../../core/services/http.service';
import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import { ForumTopicInterface } from '../../../+forum/interfaces/forum-topic.interface';
import { ForumApiEnum } from '../../../+forum/enums/forum-api.enum';
import { NewTrendingProjectResponse, ProjectInterface } from '../../../shared/interfaces/projects/project.interface';
import { NewAndNoteworthyPostInterface, PostInterface } from '../../../+learn/interfaces/post.interface';
import { CommonApiEnum } from '../../../core/enums/common-api.enum';
import { PinnedToolsRequestsService } from '../../services/pinned-tools-requests.service';
import { AuthService } from '../../../core/services/auth.service';
import { PermissionService } from '../../../core/services/permission.service';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { PermissionTypeEnum } from '../../../core/enums/permission-type.enum';
import { ActivityService } from '../../../core/services/activity.service';
import { ActivityTypeEnum } from '../../../core/enums/activity/activity-type.enum';
import { FirstClickInfoActivityDetailsInterface } from '../../../core/interfaces/activity-details/first-click-info-activity-details.interface';
import { DashboardClickElementActionTypeEnum } from '../../../core/enums/activity/dashboard-click-element-action-type.enum';
import { ForumComponentClickType } from '../../../shared/enums/forum-component-click-type.enum';
import { map } from 'rxjs/operators';
import { DashboardManagement } from '../../enums/dashboard-management.enum';
import { environment } from 'src/environments/environment';
import { LocationStrategy } from '@angular/common';

@Component({
  selector: 'neo-corporation-dashboard',
  templateUrl: './corporation-dashboard.component.html',
  styleUrls: ['./corporation-dashboard.component.scss']
})
export class CorporationDashboardComponent {
  newAndNoteworthyArticles: NewAndNoteworthyPostInterface[] = [];
  posts: NewTrendingProjectResponse[] = [];

  recentlyViewedProjects: NewTrendingProjectResponse[] = [];
  technologies: string[];
  loading: boolean = false;
  progressPositions: number[] = [0];
  trendingProgressPositions: number[] = [0];
  loadArticles = false;
  @Output() elementClick: EventEmitter<FirstClickInfoActivityDetailsInterface> =
    new EventEmitter<FirstClickInfoActivityDetailsInterface>();

  currentUser$: Observable<UserInterface> = this.authService.currentUser();

  forums$: Observable<PaginateResponseInterface<ForumTopicInterface>> = this.httpService.get<
    PaginateResponseInterface<ForumTopicInterface>
  >(ForumApiEnum.Forum, {
    filterBy: 'foryou.popular',
    take: 4,
    expand: 'discussionusers.users,discussionusers.users.image,categories,regions,saved'
  });
  posts$: Observable<PaginateResponseInterface<PostInterface>> = this.httpService
    .get<PaginateResponseInterface<PostInterface>>(CommonApiEnum.Articles, {
      filterby: 'foryou',
      expand: 'categories,regions,solutions,technologies',
      random: 5
    })
    .pipe(
      map(posts => {
        posts.dataList.forEach(post => this.pinnedToolsService.setTags(post));
        return posts;
      })
    );

  constructor(
    private readonly httpService: HttpService,
    private readonly pinnedToolsService: PinnedToolsRequestsService,
    private readonly authService: AuthService,
    private readonly permissionService: PermissionService,
    private readonly activityService: ActivityService,
    private locationStrategy: LocationStrategy
  ) {}

  ngOnInit(): void {
    this.loadNewAndTrendingProjects();
    this.loadRecentlyViewedProjects();
    this.loadNewAndTrendingArticles();
  }

  loadRecentlyViewedProjects() {
    this.httpService
      .get<ProjectInterface[]>(`${ProjectApiRoutes.projectsList}/${ProjectApiRoutes.projectsView}`, {
        skipActivities: true
      })
      .subscribe(val => {
        if (val) {
          let recentlyViewedProjects: NewTrendingProjectResponse[] = val.map(v => {
            return {
              id: v.id,
              title: v.title,
              subTitle: v.subTitle,
              description: v.description,
              tag: '',
              companyImage: v.company?.image?.uri,
              imageUrl: '',
              projectCategory: v.category,
              projectCategoryImage: v.projectCategoryImage,
              projectCategorySlug: v.category.slug,
              technologies: v.technologies.map(t => t.slug)
            };
          });
          this.recentlyViewedProjects = recentlyViewedProjects;
          this.setImageValueForNewTrendingProject(this.recentlyViewedProjects);
          if (this.recentlyViewedProjects?.length <= 2) {
            this.progressPositions = [];
          } else if (this.recentlyViewedProjects?.length > 2 && this.recentlyViewedProjects?.length <= 4) {
            this.progressPositions = [0, -2];
          } else if (this.recentlyViewedProjects?.length > 4 && this.recentlyViewedProjects?.length <= 6) {
            this.progressPositions = [0, -2, -4];
          }
        }
      });
  }

  setImageValueForNewTrendingProject(posts: NewTrendingProjectResponse[]) {
    posts.forEach(post => {
      const baseUrl = `${environment.baseAppUrl}assets/images/`;
      if (post.technologies.length > 0) {
        post.imageUrl = `${baseUrl}project-technologies-images/${post.technologies[0]}/${post.projectCategoryImage}.jpg`;
      } else {
        post.imageUrl = `${baseUrl}project-categories-images/${post.projectCategorySlug}/${post.projectCategoryImage}.jpg`;
      }
    });
  }

  loadNewAndTrendingProjects() {
    this.loading = false;
    this.httpService.get(DashboardManagement.NewTrending).subscribe((response: any) => {
      this.posts = response.dataList as NewTrendingProjectResponse[];
      this.setImageValueForNewTrendingProject(this.posts);
      this.loading = true;
      if (this.posts.length > 3 && this.posts.length <= 6) {
        this.trendingProgressPositions = [0, -3];
      } else if (this.posts.length > 6 && this.posts.length <= 9) {
        this.trendingProgressPositions = [0, -3, -6];
      }
    });
  }

  loadNewAndTrendingArticles() {
    this.loadArticles = false;
    this.httpService
      .get(`${CommonApiEnum.Articles}/${DashboardManagement.NewAndNotworthy}`)
      .subscribe((response: any) => {
        this.newAndNoteworthyArticles = response.dataList as NewAndNoteworthyPostInterface[];
        this.loadArticles = true;
      });
  }

  hasPermission(user: UserInterface): boolean {
    return this.permissionService.userHasPermission(user, PermissionTypeEnum.ProjectCatalogView);
  }

  onProjectBrowseClick(): void {
    this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.ProjectCatalogBrowse });
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ViewAllClick, { viewAllType: 'Project' })
      ?.subscribe();
  }

  onSolutionsClick(): void {
    this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.AboutSolutionsClick });
  }

  onTechnologiesClick(): void {
    this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.AboutTechnologiesClick });
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

  onForumLikeClick(forum: ForumTopicInterface, isLiked: boolean): void {
    const forumId = forum.id;
    forum.firstMessage.isLiked = isLiked;
    this.elementClick.emit({
      actionType: isLiked
        ? DashboardClickElementActionTypeEnum.ForumLike
        : DashboardClickElementActionTypeEnum.ForumUnLike,
      id: forumId
    });
  }

  onForumSaveClick(forum: ForumTopicInterface, isSaved: boolean): void {
    const forumId = forum.id;
    forum.isSaved = isSaved;
    this.elementClick.emit({
      actionType: isSaved
        ? DashboardClickElementActionTypeEnum.ForumSave
        : DashboardClickElementActionTypeEnum.ForumUnsave,
      id: forumId
    });
  }

  onForumUserFollowClick(forum: ForumTopicInterface, isFollowed: boolean): void {
    const forumId = forum.id;
    forum.firstMessage.user.isFollowed = isFollowed;
    this.elementClick.emit({
      actionType: isFollowed
        ? DashboardClickElementActionTypeEnum.ForumOwnerFollow
        : DashboardClickElementActionTypeEnum.ForumOwnerUnfollow,
      id: forumId
    });
  }

  onForumElementClick(forumId: number, element: ForumComponentClickType): void {
    switch (element) {
      case ForumComponentClickType.User: {
        this.elementClick.emit({
          actionType: DashboardClickElementActionTypeEnum.ForumOwnerView,
          id: forumId
        });
        break;
      }
      case ForumComponentClickType.Title: {
        this.elementClick.emit({
          actionType: DashboardClickElementActionTypeEnum.ForumView,
          id: forumId
        });
        break;
      }
      case ForumComponentClickType.Comment: {
        this.elementClick.emit({
          actionType: DashboardClickElementActionTypeEnum.ForumComment,
          id: forumId
        });
        break;
      }
      case ForumComponentClickType.Category: {
        this.elementClick.emit({
          actionType: DashboardClickElementActionTypeEnum.ForumCategoryClick,
          id: forumId
        });
        break;
      }
      case ForumComponentClickType.Region: {
        this.elementClick.emit({
          actionType: DashboardClickElementActionTypeEnum.ForumRegionClick,
          id: forumId
        });
        break;
      }
    }
  }

  onSaveProjectClick(projectId: number, isSaved: boolean): void {
    this.elementClick.emit({
      actionType: isSaved
        ? DashboardClickElementActionTypeEnum.ProjectSave
        : DashboardClickElementActionTypeEnum.ProjectUnsave,
      id: projectId
    });
  }

  onProjectClick(projectId: number): void {
    this.elementClick.emit({
      actionType: DashboardClickElementActionTypeEnum.ProjectView,
      id: projectId
    });
  }

  onProjectTagClick(projectId: number): void {
    this.elementClick.emit({
      actionType: DashboardClickElementActionTypeEnum.ProjectTagClick,
      id: projectId
    });
  }
}
