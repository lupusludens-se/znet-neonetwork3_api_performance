import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { filter, Subject, switchMap, takeUntil } from 'rxjs';

import { HttpService } from '../core/services/http.service';
import { TitleService } from '../core/services/title.service';
import { CommonService } from '../core/services/common.service';
import { CoreService } from '../core/services/core.service';
import { TranslateService } from '@ngx-translate/core';

import { FilterStateInterface } from '../shared/modules/filter/interfaces/filter-state.interface';
import { PaginateResponseInterface } from '../shared/interfaces/common/pagination-response.interface';
import { ForumTopicInterface } from './interfaces/forum-topic.interface';
import { ForumRequestInterface } from './interfaces/forum-request.interface';
import { SearchResultInterface } from '../shared/interfaces/search-result.interface';

import { TaxonomyEnum } from '../core/enums/taxonomy.enum';
import { ForumFilterEnum } from './enums/forum-filter.enum';
import { ForumApiEnum } from './enums/forum-api.enum';
import { DEFAULT_PER_PAGE } from '../shared/modules/pagination/pagination.component';
import { ViewportScroller } from '@angular/common';

@Component({
  selector: 'neo-forum',
  templateUrl: './forum.component.html',
  styleUrls: ['./forum.component.scss']
})
export class ForumComponent implements OnInit, OnDestroy {
  title: string = this.translateService.instant('general.forYouLabel');
  postsTitle: string = this.translateService.instant('general.suggestedForYouLabel');
  defaultItemPerPage = DEFAULT_PER_PAGE;

  taxonomyEnum = TaxonomyEnum;
  parameterEnum = ForumFilterEnum;

  showClearButton: boolean;
  showSearchResults: boolean = false;

  forumData: PaginateResponseInterface<ForumTopicInterface>;
  searchResults: SearchResultInterface[];

  requestData: ForumRequestInterface = {
    filterBy: ForumFilterEnum.ForYou,
    search: '',
    expand: 'discussionusers.users,discussionusers.users.image,categories,regions,saved',
    skip: 0,
    take: DEFAULT_PER_PAGE,
    total: 0,
    includeCount: true
  };

  fetchData$: Subject<void> = new Subject<void>();

  private filterState: FilterStateInterface = null;
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly router: Router,
    private readonly commonService: CommonService,
    private readonly titleService: TitleService,
    private readonly httpService: HttpService,
    private readonly translateService: TranslateService,
    private readonly coreService: CoreService,    
    private viewPort: ViewportScroller
  ) {}

  ngOnInit(): void {
    this.coreService.elementNotFoundData$
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(data => !data)
      )
      .subscribe(() => {
        this.titleService.setTitle('title.forumLabel');

        this.listenForFiltersState();
        this.listedForDataFetch();

        this.fetchData$.next();
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    this.fetchData$.next();
    this.fetchData$.complete();

    this.commonService.clearFilters(this.filterState);
  }

  clearFilters(): void {
    this.commonService.clearFilters(this.filterState);
  }

  search(value: string): void {
    this.requestData.search = value;
    this.requestData.skip = 0;
    // this.showSearchResults = true;

    if (this.requestData.search) {
      this.setMainFilterCategory(this.translateService.instant('forum.allDiscussionsLabel'), null);
    } else {
      this.fetchData$.next();
    }
  }

  setMainFilterCategory(filterName: string, filterBy: ForumFilterEnum): void {
    this.title = filterName;
    this.requestData.filterBy = filterBy;
    switch (filterBy) {
      case ForumFilterEnum.ForYou:
        this.clearAll();
        this.postsTitle = 'general.suggestedForYouLabel';
        break;
      case ForumFilterEnum.Saved:
        this.clearAll();
        this.postsTitle = 'general.savedLabel';
        break;
      case ForumFilterEnum.YourDiscussion:
        this.clearAll();
        this.postsTitle = 'forum.yourDiscussionsLabel';
        break;
      default:
        this.commonService.clearFilters(this.filterState, true, undefined, true, '', true);
        if (filterBy) this.showSearchResults = false;
        this.postsTitle = 'forum.allDiscussionsLabel';
        break;
    }

    // this.listenForFiltersState();
  }

  changePage(page: number): void {
    this.requestData.skip = (page - 1) * this.defaultItemPerPage;
    this.showSearchResults = false;
    this.fetchData$.next();
  }

  navigateToDiscussion(discussion: Record<string, string | number>): void {
    this.router.navigate([`forum/topic/${discussion?.id}`]).then();
  }

  removeFromSaved(index: number): void {
    if (this.title.toLowerCase() === ForumFilterEnum.Saved) {
      this.forumData.dataList.splice(index, 1);
    }
  }

  private listenForFiltersState(): void {
    this.commonService
      .filterState()
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(state => !!state)
      )
      .subscribe(filterState => {
        this.filterState = filterState;
        this.requestData.skip = 0;

        this.showClearButton = this.commonService.isAnyFilterChecked(this.filterState);

        if (this.showClearButton || this.requestData.search) {
          this.requestData.filterBy = '';
        }

        if (this.showClearButton) {
          this.postsTitle = 'forum.allDiscussionsLabel';
          this.title = this.translateService.instant('forum.allDiscussionsLabel');
        }

        if (filterState?.parameter) {
          const regionIds: number[] = this.getIds('regions');
          if (regionIds.length) {
            this.requestData.filterBy += `&regionids=${regionIds.join(',')}`;
          }

          const solutionIds: number[] = this.getIds('solutions');
          if (solutionIds.length) {
            this.requestData.filterBy += `&solutionIds=${solutionIds.join(',')}`;
          }

          const categoryIds: number[] = this.getIds('categories');
          if (categoryIds.length) {
            this.requestData.filterBy += `&categoryIds=${categoryIds.join(',')}`;
          }
        }

        // if (this.requestData.filterBy !== '') {
        //   this.showSearchResults = false;
        // }

        this.fetchData$.next();
      });
  }

  private listedForDataFetch(): void {
    this.fetchData$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() =>
          this.httpService.get<PaginateResponseInterface<ForumTopicInterface>>(
            ForumApiEnum.Forum,
            this.coreService.deleteEmptyProps(this.requestData)
          )
        )
      )
      .subscribe(response => {
        this.forumData = response;
        this.searchResults = response.dataList.map(data => ({
          id: data?.id,
          name: data?.subject
        }));

        this.requestData = {
          ...this.requestData,
          total: response.count
        };

        this.viewPort.scrollToPosition([0, 0]);
      });
  }

  private getIds(propertyName: string): number[] {
    const idList: number[] = [];

    this.filterState.parameter[propertyName].map(item => {
      if (item.checked) idList.push(item.id);

      if (item.childElements?.length) {
        item.childElements.filter(i => !!i.checked && !i.disabled).map(i => idList.push(i.id));
      }
    });

    return idList;
  }

  private clearAll(): void {
    this.requestData.search = null;
    this.requestData.skip = 0;
    this.searchResults = [];
    this.clearFilters();
  }
}
