import { Component, OnDestroy, OnInit } from '@angular/core';

import { Observable, Subject, filter, switchMap, takeUntil } from 'rxjs';

import { TitleService } from '../core/services/title.service';
import { HttpService } from '../core/services/http.service';
import { CoreService } from '../core/services/core.service';
import { AuthService } from '../core/services/auth.service';
import { PermissionService } from '../core/services/permission.service';

import { PaginationInterface } from '../shared/modules/pagination/pagination.component';
import { PaginateResponseInterface } from '../shared/interfaces/common/pagination-response.interface';
import { ConversationInterface } from './interfaces/conversation.interface';
import { UserInterface } from '../shared/interfaces/user/user.interface';
import { ConversationUserInterface } from './interfaces/conversation-user.interface';

import { MessageApiEnum } from './enums/message-api.enum';
import { PermissionTypeEnum } from '../core/enums/permission-type.enum';
import { DiscussionSourceTypeEnum } from '../shared/enums/discussion-source-type.enum';
import { Router } from '@angular/router';
import { UserStatusEnum } from '../user-management/enums/user-status.enum';
import { SortingOptionsKeyValuePair } from '../shared/modules/sort-dropdown/sort-dropdown.component';
import { MessagesService } from './services/messages.service';
import { MessageRoutesEnum } from './enums/message-routes.enum';
import { FilterDataInterface } from '../shared/modules/filter/interfaces/filter-data.interface';
import { TaxonomyEnum } from '../core/enums/taxonomy.enum';
import { FilterStateInterface } from '../shared/modules/filter/interfaces/filter-state.interface';
import { CommonService } from '../core/services/common.service';
import { RolesEnum } from '../shared/enums/roles.enum';
import { MessageTabService } from '../shared/services/message-tab.service';

@Component({
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit, OnDestroy {
  selectedTab: 'inbox' | 'network' = 'inbox';
  defaultItemPerPage: number = 20;
  search: string;
  includeAll: boolean;
  inboxSortOrder: string;
  networkSortOrder: string;
  taxonomyEnum = TaxonomyEnum;
  userStatuses = UserStatusEnum;
  discussionSourceType = DiscussionSourceTypeEnum;
  pageData: PaginateResponseInterface<ConversationInterface>;
  pageDataLoad$: Subject<void> = new Subject<void>();
  currentUser: UserInterface;
  routesToNotClearFilters: string[] = [`${MessageRoutesEnum.MessageHistoryComponent}`];
  private unsubscribe$: Subject<void> = new Subject<void>();
  selectedConversationOptionIds: number[];
  private filterState: FilterStateInterface = null;
  isFirstTimeLoad: boolean = false;
  isUserAdmin: boolean = false;
  isAllowedOnDetailPage: boolean = true;
  userRoles = RolesEnum;
  paging: PaginationInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null
  };

  messageSortingOptions: SortingOptionsKeyValuePair[] = [
    { key: 'recent', value: 'Most Recent' },
    { key: 'unread', value: 'Unread' },
    { key: 'leads', value: 'Leads' }
  ];

  messageNetworkSortingOptions: SortingOptionsKeyValuePair[] = [
    { key: 'recent', value: 'Most Recent' },
    { key: 'leads', value: 'Leads' }
  ];

  conversationBtwnType: FilterDataInterface[] = [
    { id: -1, name: 'All', checked: false, disabled: false, hide: true },
    { id: 0, name: 'Corporation - Corporation', checked: false, disabled: false, hide: false },
    { id: 1, name: 'Corporation - Solution Provider', checked: false, disabled: false, hide: false },
    { id: 2, name: 'Solution Provider - Corporation', checked: false, disabled: false, hide: false },
    { id: 3, name: 'Solution Provider - Solution Provider', checked: false, disabled: false, hide: false }
  ];

  corpTocorpEntity = this.conversationBtwnType.filter(x => x.name == 'Corporation - Corporation')?.[0];

  constructor(
    private readonly titleService: TitleService,
    private readonly httpService: HttpService,
    private readonly authService: AuthService,
    private readonly coreService: CoreService,
    private readonly router: Router,
    private readonly permissionService: PermissionService,
    private readonly commonService: CommonService,
    public messagesService: MessagesService,
    private readonly messageTabService: MessageTabService
  ) {
    this.messagesService
      .getInboxSortBy()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(value => {
        this.inboxSortOrder = value;
      });
    this.messagesService
      .getNetworkSortBy()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(sortValue => {
        this.networkSortOrder = sortValue;
      });
    this.messageTabService
      .getTabState()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(tabState => {
        this.includeAll = tabState === 'network';
        this.selectedTab = tabState;
      });
    this.messagesService
      .getPaging()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(page => {
        this.paging = page;
      });
    this.messagesService
      .getSelectedConvTypeIdsValue()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(ids => {
        this.selectedConversationOptionIds = ids;
        this.conversationBtwnType.forEach(element => {
          if (ids.findIndex(x => x == element.id) > -1) {
            element.checked = true;
          }
        });
      });
  }

  ngOnInit(): void {
    this.titleService.setTitle('messages.messagesLabel');
    this.coreService.previousRoute$.asObservable().subscribe(route => {
      if (route !== null) {
        if (route.includes('manage')) {
          this.selectedTab = 'network';
          this.messageTabService.setTabState(this.selectedTab);
        }
      }
    });

    this.listenToLoadMessages();
    this.pageDataLoad$.next();

    this.authService.currentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      if (currentUser) {
        this.isUserAdmin = this.currentUser.roles.some(r => r.id === this.userRoles.Admin);
      }
    });

    this.isFirstTimeLoad = true;

    this.commonService.filterState$
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(initialFiltersState => !!initialFiltersState)
      )
      .subscribe(entity => {
        this.filterState = entity;
        const categoriesFilter = entity.parameter['categories'] as FilterDataInterface[];
        const c2cEntityIndexInselectedConvOptionIds = this.selectedConversationOptionIds.findIndex(
          y => y == this.corpTocorpEntity.id
        );
        if (categoriesFilter.findIndex(x => x.checked) > -1) {
          if (c2cEntityIndexInselectedConvOptionIds > -1) {
            this.selectedConversationOptionIds.splice(c2cEntityIndexInselectedConvOptionIds, 1);
          }
          this.conversationBtwnType
            .filter(y => y.id == this.corpTocorpEntity.id)
            .map(element => {
              element.disabled = true;
              element.checked = false;
            });
        } else {
          this.conversationBtwnType
            .filter(y => y.id == this.corpTocorpEntity.id)
            .map(element => {
              element.disabled = false;
              element.checked = element.checked;
            });
        }
        if (!this.isFirstTimeLoad) {
          this.changePage(1);
        }
        this.pageDataLoad$.next();
        this.isFirstTimeLoad = false;
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    const routesFound = this.routesToNotClearFilters.some(val => this.coreService.getOngoingRoute().includes(val));
    if (!routesFound) {
      this.messagesService.clearPreferencesData();
      this.commonService.clearFilters(this.filterState);
    }
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.defaultItemPerPage;
    this.messagesService.setPaging(this.paging);
    this.pageDataLoad$.next();
  }

  changeTab(tabName: 'inbox' | 'network'): void {
    this.paging.skip = 0;
    this.messagesService.setPaging(this.paging);
    this.selectedTab = tabName;
    this.includeAll = tabName === 'network';
    this.messageTabService.setTabState(this.selectedTab);
    this.pageDataLoad$.next();
  }

  hasPermission(user: UserInterface): boolean {
    return (
      this.permissionService.userHasPermission(user, PermissionTypeEnum.MessagesAll) ||
      this.permissionService.userHasPermission(user, PermissionTypeEnum.ViewCompanyMessages)
    );
  }

  openUserProfile(user?: ConversationUserInterface): void {
    if (!user || user.statusId === UserStatusEnum.Deleted) {
      return;
    }

    this.router.navigateByUrl('/user-profile/' + user.id);
  }

  conversationSearch(searchString: string) {
    this.search = searchString;
    this.paging.skip = 0;
    this.pageDataLoad$.next();
  }

  private listenToLoadMessages(): void {
    this.pageDataLoad$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          this.isAllowedOnDetailPage = !this.isUserAdmin && this.selectedTab == 'network' ? false : true;
          const paging = this.coreService.deleteEmptyProps({
            ...this.paging,
            search: this.search,
            includeAll: this.includeAll,
            includeCount: true,
            expand:
              'discussionusers,discussionusers.users,discussionusers.users.image,project,discussionusers.users.company',
            orderby: this.selectedTab === 'inbox' ? this.inboxSortOrder : this.networkSortOrder,
            ConversationType: this.selectedTab === 'network' ? this.selectedConversationOptionIds : null,
            filterby: this.selectedTab === 'network' ? this.getSelectedCategories() : null,
          });
          return this.httpService.get<PaginateResponseInterface<ConversationInterface>>(
            MessageApiEnum.Conversations,
            paging
          );
        })
      )
      .subscribe(response => {
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip ? this.paging?.skip : 0,
          total: response.count
        };
        this.messagesService.setPaging(this.paging);

        this.pageData = response;
      });
  }

  private getSelectedCategories() {
    const categoriesFilter = this.commonService.filterState$.value.parameter['categories'] as FilterDataInterface[];
    const categoryIds = categoriesFilter.filter(x => x.checked).map(x => x.id);
    if (this.selectedConversationOptionIds?.findIndex(x => x == this.corpTocorpEntity.id) > -1) {
      return '';
    }
    return categoryIds.length > 0 ? `categoryIds=${categoryIds.join(',')}` : '';
  }

  getLastMessageUser(currentUser: UserInterface, conversation: ConversationInterface): string {
    const lastConversationUserId: number = conversation?.lastMessage?.user?.id;
    if (conversation?.lastMessage?.user?.statusId === this.userStatuses.Deleted) {
      return 'Deleted User';
    } else if (currentUser?.id === lastConversationUserId) return 'You';
    else return conversation?.lastMessage?.user?.name;
  }

  onInboxSortChange(sortKey: string) {
    this.inboxSortOrder = sortKey;
    this.messagesService.setSortValue(sortKey, 'inbox');
    this.pageDataLoad$.next();
  }

  OnNetworkSortChange(sortKey: string) {
    this.networkSortOrder = sortKey;
    this.messagesService.setSortValue(sortKey, 'network');
    this.pageDataLoad$.next();
  }

  ConversationTypeChange(ids: number[]) {
    this.selectedConversationOptionIds = ids.length > 0 ? ids : [];
    this.messagesService.setSelectedConvTypeIdsValue(this.selectedConversationOptionIds);
    const categoriesFilter = this.filterState.parameter['categories'] as FilterDataInterface[];
    if (ids?.length == 1 && ids.findIndex(x => x == this.corpTocorpEntity.id) > -1) {
      categoriesFilter.forEach(element => {
        element.disabled = true;
        element.checked = false;
      });
    } else {
      categoriesFilter.forEach(element => {
        element.disabled = false;
        element.checked = element.checked;
      });
    }
    this.commonService.filterState$.next(this.filterState);
    this.pageDataLoad$.next();
  }
}
