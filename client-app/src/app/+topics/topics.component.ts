import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { filter, Subject, switchMap } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';

import { HttpService } from '../core/services/http.service';
import { CoreService } from '../core/services/core.service';
import { TitleService } from '../core/services/title.service';
import { SnackbarService } from '../core/services/snackbar.service';
import { AuthService } from '../core/services/auth.service';

import { TopicInterface } from '../shared/interfaces/topic.interface';
import { ContentInterface } from '../shared/interfaces/content.interface';
import { PaginationInterface } from '../shared/modules/pagination/pagination.component';

import { TopicTypeEnum } from '../shared/enums/topic-type.enum';
import { TopicApiEnum } from '../shared/enums/topic-api.enum';
import { TaxonomyTypeEnum } from '../shared/enums/taxonomy-type.enum';
import { RolesEnum } from '../shared/enums/roles.enum';
import { ArticleDescriptionService } from '../shared/services/article-description.service';
import { TranslateService } from '@ngx-translate/core';
import { SearchValidatorService } from '../shared/services/search-validator.service';
import { TagInterface } from '../core/interfaces/tag.interface';

@Component({
  selector: 'neo-topics',
  templateUrl: './topics.component.html',
  styleUrls: ['../../assets/styles/topics-and-saved-content.scss']
})
export class TopicsComponent implements OnInit, OnDestroy {
  // TODO: Set value depending on selected tag
  selectedTopic: string;
  selectedType: TopicTypeEnum = null;
  topicType = TopicTypeEnum;
  topicsPaginateResponse: TopicInterface;
  topicsPaginateAllResponse: TopicInterface;
  taxonomyId: number;
  taxonomyType: TaxonomyTypeEnum;
  type = TaxonomyTypeEnum;
  totalSearch: number = 0;
  totalProject: number = 0;
  totalLearn: number = 0;
  totalEvent: number = 0;
  totalForum: number = 0;
  totalCompany: number = 0;
  newSearch: boolean = true;

  isGlobalSearch: boolean;
  isSolutionProvider: boolean;
  searchValue: string;
  emptyStateLabel: string;

  readonly defaultPerPage: number = 10;
  paging: PaginationInterface = {
    total: null,
    take: this.defaultPerPage,
    skip: 0
  };

  loadTopic$: Subject<void> = new Subject<void>();

  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly httpService: HttpService,
    private readonly coreService: CoreService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly titleService: TitleService,
    private readonly router: Router,
    private readonly snackbarService: SnackbarService,
    private readonly authService: AuthService,
    private readonly translateService: TranslateService
  ) {}

  ngOnInit(): void {
    if (location.pathname.includes('search')) {
      this.isGlobalSearch = true;
    }

    this.emptyStateLabel = this.getEmptyStateLabel();
    this.listenToLoadTopic();
    this.listenToQueryParamsChange();
    this.listenToCurrentUserChange();

    this.titleService.setTitle('forum.topicsLabel');
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    this.loadTopic$.next();
    this.loadTopic$.complete();
  }

  getEmptyStateLabel(): string {
    return this.isGlobalSearch
      ? this.translateService.instant('topics.emptySearchLabel')
      : `${this.translateService.instant('general.emptyStateLabel')}`;
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.defaultPerPage;
    this.loadTopic$.next();
  }

  changeTab(type: TopicTypeEnum): void {
    this.paging.skip = 0;
    this.selectedType = type;
    this.loadTopic$.next();
  }

  regionTagClicked(): void {
    this.newSearch = true;
    this.paging.skip = 0; 
    this.selectedType = null;
  }

  categoryTagClicked(tag: TagInterface): void {
    
    this.newSearch = true;
    this.paging.skip = 0; 
    this.selectedType = null;
    this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: { id: tag.id, name: tag.name, type: tag.type },
      queryParamsHandling: 'merge'
    });
  }
  

  getLink(content: ContentInterface<TopicTypeEnum>): string {
    switch (content.type) {
      case TopicTypeEnum.Learn:
        return `/learn/${content?.id}`;
      case TopicTypeEnum.Forum:
        return `/forum/topic/${content.id}`;
      case TopicTypeEnum.Project:
        return `/projects/${content.id}`;
      case TopicTypeEnum.Event:
        return `/events/${content.id}`;
      case TopicTypeEnum.Company:
        return `/company-profile/${content.id}`;
    }
  }

  getDescription(description: string): string {
    const modifiedDescription = ArticleDescriptionService.clearContent(description);
    const paragraphs = modifiedDescription.split('<p>');

    if (paragraphs.some(item => item.includes('audio'))) {
      const index = paragraphs.findIndex(item => item.includes('audio') || item.includes('iframe'));

      paragraphs.splice(index, 1);
      return paragraphs.toString().slice(1, 150);
    } else {
      return modifiedDescription.length > 150 ? modifiedDescription.slice(0, 150) : modifiedDescription;
    }
  }

  searchFor(search: string): void {
    this.selectedType = null;

    if (!search?.length) {
      this.router.navigate(['/search'], { replaceUrl: true }).then();
    } else if (search.length < 3) {
      this.snackbarService.showError('general.searchMinCharactersLabel');
    } else {
      this.newSearch = true;
      this.router.navigate(['/search'], { replaceUrl: true, queryParams: { data: search } }).then();
    }
  }

  private listenToLoadTopic(): void {
    this.loadTopic$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          const paging = this.coreService.deleteEmptyProps({
            ...this.paging,
            includeCount: true,
            entityType: this.selectedType
          });
          if (!SearchValidatorService.validateSearch(this.searchValue)) {
            this.snackbarService.showError('general.searchErrorLabel');
            return;
          }
          return this.httpService.get<TopicInterface>(
            TopicApiEnum.Topics,
            this.coreService.deleteEmptyProps({
              ...paging,
              taxonomyType: this.taxonomyType,
              taxonomyId: this.taxonomyId,
              search: this.isGlobalSearch ? this.searchValue : null
            })
          );
        }),
        map(response => this.mapData(response))
      )
      .subscribe(response => {
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip ? this.paging?.skip : 0,
          total: response.count
        };

        this.topicsPaginateResponse = response;
        this.topicsPaginateAllResponse = response;
        if (this.newSearch) {
          this.totalSearch = response.count;
          this.totalProject = response.counters.projectsCount;
          this.totalLearn = response.counters.articlesCount;
          this.totalForum = response.counters.forumsCount;
          this.totalEvent = response.counters.eventsCount;
          this.totalCompany = response.counters.companiesCount;
          this.newSearch = false;
        }
      });
  }

  private listenToQueryParamsChange(): void {
    this.activatedRoute.queryParamMap.pipe(takeUntil(this.unsubscribe$)).subscribe(params => {
      if (!params) {
        this.router.navigate(['..']).then();
      }

      if (this.isGlobalSearch) {
        this.searchValue = params.get('data');
      } else {
        this.selectedTopic = params.get('name');
        this.taxonomyId = parseInt(params.get('id'));
        this.taxonomyType = parseInt(params.get('type'));
      }

      this.loadTopic$.next();
    });
  }

  private listenToCurrentUserChange() {
    this.authService
      .currentUser()
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(user => !!user)
      )
      .subscribe(
        user =>
          (this.isSolutionProvider = user?.roles.some(
            role => role.id === RolesEnum.SolutionProvider || role.id == RolesEnum.SPAdmin
          ))
      );
  }

  private mapData(data: TopicInterface): TopicInterface {
    data.dataList = data.dataList.map(dataItem => {
      const categories = dataItem.categories.map(item => ({ ...item, type: TaxonomyTypeEnum.Category }));
      const technologies = dataItem.technologies.map(item => ({ ...item, type: TaxonomyTypeEnum.Technology }));
      const solutions = dataItem.solutions.map(item => ({ ...item, type: TaxonomyTypeEnum.Solution }));
      const contentTags = dataItem.contentTags.map(item => ({ ...item, type: TaxonomyTypeEnum.ContentTag }));
      dataItem['tags'] = [...categories, ...technologies, ...solutions, ...contentTags];

      return dataItem;
    });

    return data;
  }
}
