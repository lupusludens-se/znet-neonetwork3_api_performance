import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { HttpService } from 'src/app/core/services/http.service';
import { NewTrendingProjectResponse } from 'src/app/shared/interfaces/projects/project.interface';
import { environment } from 'src/environments/environment';
import { DashboardManagement } from '../../enums/dashboard-management.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { Observable, Subject, map } from 'rxjs';
import { CommunityDataService } from 'src/app/+community/services/community.data.service';
import { NetwrokStatsInterface } from 'src/app/shared/interfaces/netwrok-stats.interface';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { DashboardClickElementActionTypeEnum } from 'src/app/core/enums/activity/dashboard-click-element-action-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { FirstClickInfoActivityDetailsInterface } from 'src/app/core/interfaces/activity-details/first-click-info-activity-details.interface';
import { ApiRoutes } from 'src/app/+admin/modules/announcements/enums/announcement.enum';
import { AnnouncementInterface } from 'src/app/+admin/modules/announcements/interfaces/announcement.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { EventInterface } from 'src/app/shared/interfaces/event/event.interface';
import * as dayjs from 'dayjs';
import { EventService } from 'src/app/+events/services/event.service';
import { EventsApiEnum } from 'src/app/shared/enums/api/events-api.enum';
import { CommonApiEnum } from '../../../core/enums/common-api.enum';
import { NewAndNoteworthyPostInterface } from '../../../+learn/interfaces/post.interface';
import { LocationStrategy } from '@angular/common';

@Component({
  selector: 'neo-public-corporation-dashboard',
  templateUrl: './public-corporation-dashboard.component.html',
  styleUrls: ['./public-corporation-dashboard.component.scss']
})
export class PublicCorporationDashboardComponent implements OnInit {
  loading: boolean;
  loadingDiscoverKeyContent: boolean;
  isLocked: boolean = false;
  posts: NewTrendingProjectResponse[] = [];
  trendingProgressPositions: number[] = [0];
  auth: AuthService;
  networknetworkStats: Observable<NetwrokStatsInterface> = null;
  elementClick$: Subject<FirstClickInfoActivityDetailsInterface> =
    new Subject<FirstClickInfoActivityDetailsInterface>();

  announcement$: Observable<AnnouncementInterface> = this.httpService.get<AnnouncementInterface>(
    ApiRoutes.AnnouncementLatest,
    {
      expand: 'backgroundimage,audience'
    }
  );

  events$: Observable<PaginateResponseInterface<EventInterface>> = this.httpService
    .get<PaginateResponseInterface<EventInterface>>(EventsApiEnum.Events, {
      expand: 'occurrences',
      skip: 0,
      take: 3,
      from: dayjs().toISOString()
    })
    .pipe(
      map((events: PaginateResponseInterface<EventInterface>) => {
        events.dataList.forEach(event => this.eventService.convertEvent(event));
        return events;
      })
    );
  newAndNoteworthyArticles: NewAndNoteworthyPostInterface[] = [];
  publicArticlesLimit = 3;
  @Output() elementClick: EventEmitter<FirstClickInfoActivityDetailsInterface> =
    new EventEmitter<FirstClickInfoActivityDetailsInterface>();
  constructor(
    private httpService: HttpService,
    private readonly communityDataService: CommunityDataService,
    private readonly activityService: ActivityService,
    private readonly eventService: EventService,
    private locationStrategy: LocationStrategy
  ) {}

  ngOnInit(): void {
    this.loadDiscoverabilityProjectsData();
    this.loadDiscoverKeyContent();
    this.networknetworkStats = this.communityDataService.getNetorkStats();
  }

  loadDiscoverabilityProjectsData() {
    this.loading = false;
    this.httpService.get(DashboardManagement.PublicDiscoverabilityData).subscribe((response: any) => {
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

  setImageValueForNewTrendingProject(posts: NewTrendingProjectResponse[]) {
    posts.forEach(post => {
      if (post.technologies.length > 0) {
        post.imageUrl = `${environment.baseAppUrl}assets/images/project-technologies-images/${post.technologyImageSlug}/${post.projectCategoryImage}.jpg`;
      } else
        post.imageUrl = `${environment.baseAppUrl}assets/images/project-categories-images/${post.projectCategorySlug}/${post.projectCategoryImage}.jpg`;
    });
  }

  onEventAllClick(): void {
    this.elementClick$.next({ actionType: DashboardClickElementActionTypeEnum.EventsViewAll });
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.ViewAllClick, { viewAllType: 'Event' })
      ?.subscribe();
  }

  onEventClick(id: number): void {
    this.elementClick$.next({ actionType: DashboardClickElementActionTypeEnum.EventView, id: id });
  }
  onAnnouncementClick(id: number): void {
    this.elementClick$.next({ actionType: DashboardClickElementActionTypeEnum.AnnouncementButtonClick, id: id });
  }
  loadDiscoverKeyContent() {
    this.loadingDiscoverKeyContent = false;
    this.httpService
      .get(`${CommonApiEnum.Articles}/${DashboardManagement.NewAndNotworthy}`)
      .subscribe((response: any) => {
        this.newAndNoteworthyArticles = response.dataList as NewAndNoteworthyPostInterface[];
      });
    this.loadingDiscoverKeyContent = true;
  }
  onArticleClick(articleId: number, url: string, articleName: string, isPublic: boolean): void {
    if (isPublic) {
      this.activityService
        .trackElementInteractionActivity(ActivityTypeEnum.LearnView, { id: articleId, title: articleName })
        ?.subscribe();
      const getBaseHref = location.origin + this.locationStrategy.getBaseHref();
      const learnUrl = getBaseHref + url;
      window.open(learnUrl, '_blank');
    } else {
      this.activityService
        .trackElementInteractionActivity(ActivityTypeEnum.PrivateLearnClick, { id: articleId, title: articleName })
        ?.subscribe();
    }
  }
}
