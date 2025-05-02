import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { InitiativeSavedContentListRequestInterface } from '../../interfaces/initiative-saved-content-list-request.interface';
import { PostTypeEnum } from 'src/app/core/enums/post-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { LocationStrategy } from '@angular/common';
import { Router } from '@angular/router';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { catchError, interval, of, startWith, Subject, switchMap, takeUntil } from 'rxjs';
import { ViewInitiativeService } from '../../services/view-initiative.service';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { TranslateService } from '@ngx-translate/core';
import { InitiativeContentTabEnum } from 'src/app/initiatives/shared/enums/initiative-content-tabs.enum';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { InitiativeArticleInterface } from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { CoreService } from 'src/app/core/services/core.service';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';

@Component({
  selector: 'neo-learn-section',
  templateUrl: './learn-section.component.html',
  styleUrls: ['./learn-section.component.scss']
})
export class LearnSectionComponent implements OnInit, OnDestroy {
  showDeleteModal = false;
  savedPosts: PaginateResponseInterface<InitiativeArticleInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: 0
  };
  readonly defaultItemPerPage = 4;
  page = 1;
  paging: PaginationInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null
  };
  requestData: InitiativeSavedContentListRequestInterface = {
    skip: 0,
    take: 6,
    total: 0,
    includeCount: true
  };
  isLoading = true;
  options: MenuOptionInterface[] = [
    {
      icon: 'trash-can-red',
      name: 'initiative.viewInitiative.deleteSavedContentLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];

  clickedArticleId: number;
  @Input() initiativeId: number;
  @Input() initiativeTitle: string;
  @Input() isAdminOrTeamMember: boolean = false;
  @Output() postClick = new EventEmitter<void>();
  @Output() tagClick = new EventEmitter<void>();
  public loadInitiativeSavedArticles$ = new Subject<void>();
  private unsubscribe$ = new Subject<void>();
  articleRecommendationsCounter = '';

  constructor(
    private readonly activityService: ActivityService,
    private readonly locationStrategy: LocationStrategy,
    public readonly router: Router,
    private readonly viewInitiativeService: ViewInitiativeService,
    private readonly snackbarService: SnackbarService,
    private readonly initiativeSharedService: InitiativeSharedService,
    private readonly translateService: TranslateService,
    private coreService: CoreService
  ) {}

  ngOnInit() {
    this.loadRecommendationCount();
    this.loadSavedArticles();
    this.loadInitiativeSavedArticles$.next();
  }

  getNeoType(postTypeEnum: PostTypeEnum): string {
    if (!postTypeEnum) return '';
    return Object.keys(PostTypeEnum)[Object.values(PostTypeEnum).indexOf(postTypeEnum)].toLowerCase() || '';
  }

  routeToLearnPage(ctrlKeyPressed: boolean, path: string, id: number, title: string) {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.LearnView, {
        id: id,
        title: title,
        initiativeId: this.initiativeId
      })
      ?.subscribe();
    const getBaseHref = location.origin + this.locationStrategy.getBaseHref();
    const serializedUrl = getBaseHref + this.router.serializeUrl(this.router.createUrlTree([`${path}`]));
    if (ctrlKeyPressed) {
      window.open(serializedUrl, '_blank');
    } else {
      this.router.navigate([path]);
    }
    this.postClick.emit();
  }

  loadSavedArticles() {
    this.loadInitiativeSavedArticles$
      .pipe(
        switchMap(() => {
          const { skip, take, ...requestData } = this.requestData;
          return this.viewInitiativeService.getSavedArticlesForInitiative(this.initiativeId, this.requestData);
        })
      )
      .subscribe(data => {
        this.savedPosts.count = data.count;
        this.savedPosts.dataList.push(...data.dataList);
        if (this.isLoading) this.isLoading = false;
      });
  }

  onLoadMoreData() {
    this.page++;
    this.paging.skip = this.savedPosts.dataList.length;
    if (this.savedPosts.dataList.length < this.savedPosts.count) {
      this.loadMoreInitiativeSavedArticles();
    }
  }

  loadMoreInitiativeSavedArticles() {
    this.requestData.includeCount = true;
    this.requestData.skip = this.paging.skip;
    this.requestData.take = this.defaultItemPerPage;
    this.loadInitiativeSavedArticles$.next();
  }

  optionClick(id: number): void {
    this.showDeleteModal = true;
    this.clickedArticleId = id;
  }

  closeDeletePopup() {
    this.showDeleteModal = false;
  }

  confirmDelete() {
    this.showDeleteModal = false;
    this.initiativeSharedService
      .deleteSavedContent(this.initiativeId, this.clickedArticleId, InitiativeModulesEnum.Learn)
      .pipe(
        catchError(error => {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
          return of(error);
        })
      )
      .subscribe(result => {
        if (result === true) {
          this.snackbarService.showSuccess(
            this.translateService.instant('initiative.viewInitiative.successContentRemoveMessage')
          );
          this.savedPosts.dataList = this.savedPosts.dataList.filter(p => p.id !== this.clickedArticleId);
          this.savedPosts.count--;
          if (this.savedPosts.dataList.length < 4 && this.savedPosts.count >= 4) {
            this.page = 0;
            this.onLoadMoreData();
          }
        } else {
          this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
        }
      });
  }

  viewAllArticleRecommendations(): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativeModuleViewAllClick, {
        id: this.initiativeId,
        title: this.initiativeTitle,
        resourceType: InitiativeModulesEnum[InitiativeModulesEnum.Learn]
      })
      ?.subscribe();
    this.router.navigate(['decarbonization-initiatives', this.initiativeId, 'learn'], {
      queryParams: {
        tab:
          this.savedPosts.dataList.length && this.articleRecommendationsCounter === ''
            ? InitiativeContentTabEnum.Saved
            : InitiativeContentTabEnum.Recommended
      }
    });
  }

  loadRecommendationCount(): void {
    interval(60000)
      .pipe(
        startWith(0),
        switchMap(() =>
          this.initiativeSharedService.getNewRecommendationsCount(this.initiativeId, InitiativeModulesEnum.Learn)
        ),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(data => {
        this.articleRecommendationsCounter = data
          ? this.initiativeSharedService.displayCounter(data[0].articlesCount)
          : '';
      });
  }

  openTopics(category: TagInterface) {
    this.coreService.goToTopics(category.id, category.name, TaxonomyTypeEnum.Category, true);
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this.loadInitiativeSavedArticles$.complete();
  }
}
