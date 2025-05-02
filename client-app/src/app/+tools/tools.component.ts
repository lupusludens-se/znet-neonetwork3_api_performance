import { Component, Input, OnChanges, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { combineLatest, Subject, switchMap, takeUntil } from 'rxjs';

import { HttpService } from '../core/services/http.service';
import { TitleService } from '../core/services/title.service';
import { CoreService } from '../core/services/core.service';

import { ToolInterface } from '../shared/interfaces/tool.interface';
import { PaginateResponseInterface } from '../shared/interfaces/common/pagination-response.interface';
import { PaginationInterface } from '../shared/modules/pagination/pagination.component';
import { ToolsApiEnum } from '../shared/enums/api/tools-api.enum';
import { AuthService } from '../core/services/auth.service';
import { PermissionService } from '../core/services/permission.service';
import { PermissionTypeEnum } from '../core/enums/permission-type.enum';
import { SOLAR_QUOTE_NAME } from '../shared/constants/solar-quote-name.const';
import { ActivityTypeEnum } from '../core/enums/activity/activity-type.enum';
import { ActivityService } from '../core/services/activity.service';
import { SignTrackingSourceEnum } from '../core/enums/sign-tracking-source-enum';
import { UserInterface } from '../shared/interfaces/user/user.interface';
import { ViewportScroller } from '@angular/common';
@Component({
  templateUrl: './tools.component.html',
  styleUrls: ['./tools.component.scss']
})
export class ToolsComponent implements OnInit, OnDestroy {
  search: string;
  defaultItemPerPage = 24;
  paging: PaginationInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null
  }; 
  @Input() public PageNumber: number;
  tools: PaginateResponseInterface<ToolInterface>;
  loadTools$: Subject<void> = new Subject<void>();
  loadMore$: Subject<void> = new Subject<void>();
  private pinnedTools: ToolInterface[];
  currentUser: UserInterface;
  isPublicUser: boolean = false;

  private pinListChange$: Subject<{ toolId: number }[]> = new Subject<{ toolId: number }[]>();
  private unsubscribe$: Subject<void> = new Subject<void>();
  auth = AuthService;
  signTrackingSourceEnum = SignTrackingSourceEnum.ZeigoNetwork;
  constructor(
    private readonly httpService: HttpService,
    private readonly titleService: TitleService,
    private readonly coreService: CoreService,
    public readonly router: Router,
    private readonly authService: AuthService,
    private readonly permissionService: PermissionService,
    private readonly activityService: ActivityService,
    private viewPort: ViewportScroller
  ) {}

  get pinnedToolIds(): number[] {
    return this.pinnedTools.map(tool => tool.id);
  }

  ngOnInit(): void {
    this.titleService.setTitle('toolsManagement.toolsLabel');
    if (!(this.auth.isLoggedIn() || this.auth.needSilentLogIn())) {
      this.defaultItemPerPage = 8;
      this.paging.take = this.defaultItemPerPage;
      this.isPublicUser = true;
    }
    this.listenToLoadTools();
    this.loadTools$.next();
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    this.pinListChange$.next(null);
    this.pinListChange$.complete();
  }
  onLoadMoreData(page: number) {
    this.defaultItemPerPage = 8;
    this.paging.skip = page * this.defaultItemPerPage;
    var isLastPage = this.isLastPage(page, Math.floor(this.paging.total / this.defaultItemPerPage));
    if (!isLastPage) this.loadNext();
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.defaultItemPerPage;
    this.loadTools$.next();
  }

  pinClick(toolPin: { toolId: number }): void {
    const pinnedTools = this.pinnedTools.map(tool => ({ toolId: tool.id }));

    const toolIdIndex = pinnedTools.findIndex(tool => tool.toolId === toolPin.toolId);

    toolIdIndex >= 0 ? pinnedTools.splice(toolIdIndex, 1) : pinnedTools.push(toolPin);

    this.pinListChange$.next(pinnedTools);
  }

  onToolClick(id: number): void {
    this.activityService.trackElementInteractionActivity(ActivityTypeEnum.ToolClick, { toolId: id })?.subscribe();
  }

  private listenToLoadTools(): void {
    if (this.isPublicUser) {
      this.LoadPublicData();
    } else {
      this.authService.currentUser().subscribe(val => {
        if (val != null) {
          this.currentUser = val;
          this.LoadPrivateData();
          this.listenToPinListChange();
          this.loadTools$.next();
        }
      });
    }
  }

  private listenToPinListChange(): void {
    this.pinListChange$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(pinList => this.httpService.post(ToolsApiEnum.PinTools, pinList))
      )
      .subscribe(() => this.loadTools$.next());
  }

  private loadNext(): void {
    const paging = this.coreService.deleteEmptyProps({
      ...this.paging,
      search: this.search,
      includeCount: true,
      filterBy: 'isactive',
      expand: 'icon'
    });
    this.httpService.get<PaginateResponseInterface<ToolInterface>>(ToolsApiEnum.Tools, paging).subscribe(
      data => {
        this.tools.dataList = this.tools.dataList.concat(data.dataList);
      },
      error => {
        console.error('Error fetching data:', error);
      }
    );
  }
  private isLastPage(page: number, total: number): boolean {
    return page > total ? true : false;
  }
  private LoadPrivateData(): void {
    this.loadTools$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          const paging = this.coreService.deleteEmptyProps({
            ...this.paging,
            search: this.search,
            includeCount: true,
            filterBy: 'isactive',
            expand: 'pinned,icon',
            orderBy: 'pinned.desc'
          });

          return combineLatest([
            this.httpService.get<PaginateResponseInterface<ToolInterface>>(ToolsApiEnum.Tools, paging),
            this.httpService.get<ToolInterface[]>(ToolsApiEnum.PinTools, {
              filterBy: 'isactive'
            })
          ]);
        })
      )
      .subscribe(([toolsData, toolsPinned]) => {
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip ? this.paging?.skip : 0,
          total: toolsData.count
        };

        //TODO: after rewritting logic of solarquote(mark it as other tool type - need to put this into filter by that type)

        if (!this.permissionService.userHasPermission(this.currentUser, PermissionTypeEnum.SendQuote)) {
          const solarQuote = toolsData.dataList.findIndex(t => t.title === SOLAR_QUOTE_NAME);

          if (solarQuote > -1) {
            toolsData.dataList.splice(solarQuote, 1);
          }
        }

        this.tools = toolsData;
        this.pinnedTools = toolsPinned; 
        this.viewPort.scrollToPosition([0, 0]);
      });
  }
  private LoadPublicData(): void {
    this.loadTools$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          const paging = this.coreService.deleteEmptyProps({
            ...this.paging,
            search: this.search,
            includeCount: true,
            filterBy: 'isactive',
            expand: 'icon'
          });

          return combineLatest([
            this.httpService.get<PaginateResponseInterface<ToolInterface>>(ToolsApiEnum.Tools, paging)
          ]);
        })
      )
      .subscribe(([toolsData]) => {
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip ? this.paging?.skip : 0,
          total: toolsData.count
        };
        this.tools = toolsData;
      });
  }
}
