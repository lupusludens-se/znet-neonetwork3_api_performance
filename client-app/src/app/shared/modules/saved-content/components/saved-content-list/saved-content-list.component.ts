import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { PaginationInterface } from '../../../pagination/pagination.component';
import { ContentInterface } from 'src/app/shared/interfaces/content.interface';
import { Observable, Subject, map, switchMap, take } from 'rxjs';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { HttpService } from 'src/app/core/services/http.service';
import { CoreService } from 'src/app/core/services/core.service';
import { TitleService } from 'src/app/core/services/title.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { ArticleDescriptionService } from 'src/app/shared/services/article-description.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { SavedContentTypeEnum } from '../../enums/saved-content-type.enum';
import { SavedContentApiEnum } from '../../enums/saved-content-api.enum';
import { BadgeCountersInterface } from '../../interfaces/badge-counters.interface';

@UntilDestroy()
@Component({
  selector: 'neo-saved-content-list',
  templateUrl: './saved-content-list.component.html',
  styleUrls: ['./../../../../../../assets/styles/topics-and-saved-content.scss']
})
export class SavedContentListComponent implements OnInit, OnDestroy {
  @Input() shortVersion: boolean;
  loading: boolean;
  selectedType: SavedContentTypeEnum = null;
  savedContentType = SavedContentTypeEnum;
  type = TaxonomyTypeEnum;

  search: string;
  readonly defaultPerPage: number = 10;
  paging: PaginationInterface = {
    total: null,
    take: this.defaultPerPage,
    skip: 0
  };

  removeFromSaved$: Subject<ContentInterface<SavedContentTypeEnum>> = new Subject<
    ContentInterface<SavedContentTypeEnum>
  >();
  badgeData: BadgeCountersInterface;
  loadSavedContent$: Subject<void> = new Subject<void>();
  savedContent: PaginateResponseInterface<ContentInterface<SavedContentTypeEnum>>;

  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  currentUser: UserInterface;
  isCurrentUserIsSolutionProvider: boolean;
  roles = RolesEnum;

  constructor(
    private readonly httpService: HttpService,
    private readonly coreService: CoreService,
    private titleService: TitleService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // setting 20 records per page for saved widget
    this.paging.take = this.shortVersion ? 20 : this.defaultPerPage;

    this.getBadgeData();
    this.listenToLoadSavedContent();
    this.listenForRemoveFromSaved();

    this.loadSavedContent$.next();

    this.titleService.setTitle('saved.savedLabel');
    this.listenForCurrentUser();
  }

  private listenForCurrentUser(): void {
    this.authService.currentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.isCurrentUserIsSolutionProvider = this.currentUser?.roles?.some(
        r => r.id === this.roles.SolutionProvider || r.id === this.roles.SPAdmin
      );
    });
  }

  ngOnDestroy(): void {
    this.loadSavedContent$.next();
    this.loadSavedContent$.complete();
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.defaultPerPage;
    this.loadSavedContent$.next();
  }

  changeTab(type: SavedContentTypeEnum): void {
    this.savedContent = null;
    this.paging.skip = 0;
    this.selectedType = type;
    this.loadSavedContent$.next();
  }

  clearResults(): void {
    this.search = null;
    this.loadSavedContent$.next();
    this.getBadgeData();
  }

  searchResults(searchString: string): void {
    this.search = searchString;
    this.paging.skip = 0;
    this.loadSavedContent$.next();
    this.getBadgeData();
  }

  getLink(content: ContentInterface<SavedContentTypeEnum>): string {
    switch (content?.type) {
      case SavedContentTypeEnum.Article:
        return `/learn/${content?.id}`;
      case SavedContentTypeEnum.Forum:
        return `/forum/topic/${content?.id}`;
      case SavedContentTypeEnum.Project:
        return `/projects/${content?.id}`;
    }
  }

  getDescription(description: string): string {
    const modifiedDescription = ArticleDescriptionService.clearContent(this.coreService.convertToPlain(description));
    const paragraphs = modifiedDescription.split('<p>');

    if (paragraphs.some(item => item.includes('audio'))) {
      const index = paragraphs.findIndex(item => item.includes('audio') || item.includes('iframe'));

      paragraphs.splice(index, 1);
      return paragraphs.toString().slice(1, 150);
    } else {
      return modifiedDescription.length > 150 ? modifiedDescription.slice(0, 150) : modifiedDescription;
    }
  }

  private getRequest(content: ContentInterface<SavedContentTypeEnum>): Observable<unknown> {
    switch (content.type) {
      case SavedContentTypeEnum.Project:
        return this.httpService.delete(`${SavedContentApiEnum.SavedProject}/${content.id}`);
      case SavedContentTypeEnum.Article:
        return this.httpService.delete(`${SavedContentApiEnum.SavedArticles}/${content.id}`);
      case SavedContentTypeEnum.Forum:
        return this.httpService.delete(`${SavedContentApiEnum.SavedForums}/${content.id}`);
    }
  }

  private listenForRemoveFromSaved(): void {
    this.removeFromSaved$
      .pipe(
        untilDestroyed(this),
        switchMap(content => this.getRequest(content))
      )
      .subscribe(() => {
        this.loadSavedContent$.next();
        this.getBadgeData();
      });
  }

  private getBadgeData(): void {
    this.loading = true;
    this.httpService
      .get<BadgeCountersInterface>(
        SavedContentApiEnum.Counters,
        this.coreService.deleteEmptyProps({
          search: this.search
        })
      )
      .pipe(take(1))
      .subscribe((response: BadgeCountersInterface) => {
        this.badgeData = response;
      });
  }

  private listenToLoadSavedContent(): void {
    this.loadSavedContent$
      .pipe(
        untilDestroyed(this),
        switchMap(() =>
          this.httpService.get<PaginateResponseInterface<ContentInterface<SavedContentTypeEnum>>>(
            SavedContentApiEnum.SavedContent,
            this.coreService.deleteEmptyProps({
              ...this.paging,
              search: this.search,
              includeCount: true,
              type: this.selectedType
            })
          )
        ),
        map(response => this.mapSavedData(response))
      )
      .subscribe(response => {
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip ? this.paging?.skip : 0,
          total: response.count
        };

        this.savedContent = response;
        this.loading = false;
      });
  }

  private mapSavedData(
    data: PaginateResponseInterface<ContentInterface<SavedContentTypeEnum>>
  ): PaginateResponseInterface<ContentInterface<SavedContentTypeEnum>> {
    data.dataList = data.dataList.map(dataItem => {
      const categories = dataItem.categories.map(item => ({ ...item, type: TaxonomyTypeEnum.Category }));
      const technologies = dataItem.technologies.map(item => ({ ...item, type: TaxonomyTypeEnum.Technology }));
      const solutions = dataItem.solutions.map(item => ({ ...item, type: TaxonomyTypeEnum.Solution }));
      dataItem['tags'] = [...categories, ...technologies, ...solutions];

      return dataItem;
    });

    return data;
  }
}
