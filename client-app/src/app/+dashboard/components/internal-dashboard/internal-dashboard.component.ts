import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { CoreService } from '../../../core/services/core.service';
import { HttpService } from '../../../core/services/http.service';
import { PinnedToolsRequestsService } from '../../services/pinned-tools-requests.service';
import { ActivityService } from '../../../core/services/activity.service';

import { ToolInterface } from '../../../shared/interfaces/tool.interface';
import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import { CompanyInterface } from '../../../shared/interfaces/user/company.interface';
import { ForumTopicInterface } from '../../../+forum/interfaces/forum-topic.interface';
import { NewAndNoteworthyPostInterface } from '../../../+learn/interfaces/post.interface';
import { FirstClickInfoActivityDetailsInterface } from '../../../core/interfaces/activity-details/first-click-info-activity-details.interface';

import { ToolsApiEnum } from '../../../shared/enums/api/tools-api.enum';
import { CompanyApiEnum } from '../../../user-management/enums/company-api.enum';
import { ForumApiEnum } from '../../../+forum/enums/forum-api.enum';
import { CommonApiEnum } from '../../../core/enums/common-api.enum';
import { ActivityTypeEnum } from '../../../core/enums/activity/activity-type.enum';
import { DashboardClickElementActionTypeEnum } from '../../../core/enums/activity/dashboard-click-element-action-type.enum';
import { ForumComponentClickType } from '../../../shared/enums/forum-component-click-type.enum';
import { DashboardManagement } from '../../enums/dashboard-management.enum';
import { LocationStrategy } from '@angular/common';

@Component({
  selector: 'neo-internal-dashboard',
  templateUrl: './internal-dashboard.component.html',
  styleUrls: ['./internal-dashboard.component.scss', '../../styles/styles.scss']
})
export class InternalDashboardComponent implements OnInit {
  @Output() elementClick: EventEmitter<FirstClickInfoActivityDetailsInterface> =
    new EventEmitter<FirstClickInfoActivityDetailsInterface>();

  showToolsModal: boolean;
  pinnedTools$: Observable<ToolInterface[]> = this.pinnedToolsService.fetchPinnedTools();
  tools: ToolInterface[];
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
  companies$: Observable<PaginateResponseInterface<CompanyInterface>> = this.httpService.get<
    PaginateResponseInterface<CompanyInterface>
  >(CompanyApiEnum.FollowedCompanies, {
    skip: 0,
    take: 8
  });
  forums$: Observable<PaginateResponseInterface<ForumTopicInterface>> = this.httpService.get<
    PaginateResponseInterface<ForumTopicInterface>
  >(ForumApiEnum.Forum, {
    filterBy: 'foryou.popular',
    take: 4,
    expand: 'discussionusers.users,discussionusers.users.image,categories,regions,saved'
  });
  // posts$: Observable<PaginateResponseInterface<PostInterface>> = this.httpService
  //   .get<PaginateResponseInterface<PostInterface>>(CommonApiEnum.Articles, {
  //     filterby: 'foryou',
  //     expand: 'categories,regions,solutions,technologies',
  //     random: 5
  //   })
  //   .pipe(
  //     tap(posts => {
  //       posts.dataList.forEach(post => this.pinnedToolsService.setTags(post));
  //       return posts;
  //     })
  //   );
  newAndNoteworthyArticles: NewAndNoteworthyPostInterface[] = [];
  get selectedItems(): ToolInterface[] {
    return this.tools.filter(item => item.isPinned);
  }

  constructor(
    private readonly httpService: HttpService,
    public readonly router: Router,
    private readonly pinnedToolsService: PinnedToolsRequestsService,
    public readonly coreService: CoreService,
    private readonly activityService: ActivityService,
    private locationStrategy: LocationStrategy
  ) {}

  ngOnInit(): void {
    this.loadNewAndTrendingArticles();
  }
  closed(): void {
    this.tools.map(item => (item.isPinned = false));
    this.showToolsModal = false;
  }

  redirectToCompany(id: number): void {
    this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.CompanyView, id: id });
    this.pinnedToolsService.redirectToPage(`/company-profile/${id}`);
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
  loadNewAndTrendingArticles() {
    this.httpService
      .get(`${CommonApiEnum.Articles}/${DashboardManagement.NewAndNotworthy}`)
      .subscribe((response: any) => {
        this.newAndNoteworthyArticles = response.dataList as NewAndNoteworthyPostInterface[];
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

  onForumPinClick(forum: ForumTopicInterface, isPinned: boolean): void {
    const forumId = forum.id;
    forum.isPinned = isPinned;
    this.elementClick.emit({
      actionType: isPinned
        ? DashboardClickElementActionTypeEnum.ForumPin
        : DashboardClickElementActionTypeEnum.ForumUnpin,
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
      case ForumComponentClickType.Delete: {
        this.elementClick.emit({ actionType: DashboardClickElementActionTypeEnum.ForumDelete, id: forumId });
        break;
      }
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

  onBrowseCompaniesClick(): void {
    this.elementClick.emit({
      actionType: DashboardClickElementActionTypeEnum.CompaniesBrowse
    });
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
    this.activityService.trackElementInteractionActivity(ActivityTypeEnum.LearnView, { id: articleId, title: articleTitle })?.subscribe();

    const getBaseHref = location.origin + this.locationStrategy.getBaseHref();
    const learnUrl = getBaseHref + url;
    window.open(learnUrl, '_blank');
  }
}
