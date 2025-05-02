import { Component, OnDestroy, OnInit } from '@angular/core';

import { FilterStateInterface } from '../../shared/modules/filter/interfaces/filter-state.interface';
import { ProjectInterface } from '../../shared/interfaces/projects/project.interface';
import { ProjectsViewTypeEnum } from './services/project-catalog.service';
import { CommonService } from '../../core/services/common.service';
import { TitleService } from '../../core/services/title.service';
import { TaxonomyEnum } from '../../core/enums/taxonomy.enum';
import { DEFAULT_PER_PAGE, PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { filter, Subject, switchMap, takeUntil } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { HttpService } from 'src/app/core/services/http.service';
import { FilterDataInterface } from 'src/app/shared/modules/filter/interfaces/filter-data.interface';
import { FilterChildDataInterface } from 'src/app/shared/modules/filter/interfaces/filter-child-data.interface';
import { CoreService } from 'src/app/core/services/core.service';
import { ProjectApiRoutes } from '../shared/constants/project-api-routes.const';
import { ProjectTypesSteps } from '../+add-project/enums/project-types-name.enum';
import { Router } from '@angular/router';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ProjectService } from './services/project.service';
import { ProjectComponentEnum } from '../shared/enums/project-component.enum';
import { InitialDataInterface } from 'src/app/core/interfaces/initial-data.interface';
import { ViewportScroller } from '@angular/common';

export enum SearchType {
  ForYou,
  Saved,
  AllProjects,
  Filter,
  Search
}

@Component({
  selector: 'neo-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectsComponent implements OnInit, OnDestroy {
  showMap: boolean;
  taxonomyEnum = TaxonomyEnum;
  showClearButton: boolean;
  projectsList: ProjectInterface[];
  mapProjectList: ProjectInterface[];
  ppaSolutionIndices: number[];
  ppaCategoryIndicies: number[];
  ppaTechnologyIndicies: number[];
  searchType: SearchType = SearchType.ForYou;
  title: string = '';
  forYouFilterTitle: string = '';
  filterState: FilterStateInterface;
  projectTypes = ProjectTypesSteps;
  defaultItemPerPage = DEFAULT_PER_PAGE;
  skipFiltersLoad: boolean;
  hoveredProject: ProjectInterface;
  paging: PaginationInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null
  };

  loadForYou$: Subject<void> = new Subject<void>();
  loadSaved$: Subject<void> = new Subject<void>();
  loadProjects$: Subject<void> = new Subject<void>();

  private readonly routes = ProjectApiRoutes;
  private unsubscribe$: Subject<void> = new Subject<void>();
  routesToNotClearFilters: string[] = [`${ProjectComponentEnum.ProjectDetailsComponent}`];
  initialLoad: boolean = true;
  initialData: InitialDataInterface;
  constructor(
    public commonService: CommonService,
    private readonly httpService: HttpService,
    private readonly coreService: CoreService,
    private readonly titleService: TitleService,
    private readonly translateService: TranslateService,
    private router: Router,
    private readonly activityService: ActivityService,
    private readonly projectService: ProjectService,
    private viewPort: ViewportScroller
  ) { }

  ngOnInit() {
    this.projectService
      .getSkip$()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(val => {
        if (val != this.paging.skip) this.paging.skip = val;
      });
    this.projectService
      .getSearchType$()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(val => {
        this.searchType = val;
      });
    this.projectService
      .getshowMap()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(val => {
        this.showMap = val;
      });
    this.projectService
      .getMapProjectList$()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(val => {
        this.mapProjectList = val;
      });
    this.coreService.elementNotFoundData$
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(data => !data)
      )
      .subscribe(() => {
        this.loadInitialData();
        this.listenForYou();
        this.listenSaved();
        this.listenToLoadProjects();
        this.listenForFilterState();

        this.titleService.setTitle('title.projectsLabel');
        if (this.showMap) {
          this.showMap = false;
          this.switchViewMode();
        }
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    const routesFound = this.routesToNotClearFilters.some(val => this.coreService.getOngoingRoute().includes(val));
    if (!routesFound) {
      this.commonService.clearFilters(this.filterState, false);
      this.projectService.clearPaging();
    }
  }

  onHover(project: ProjectInterface): void {
    this.projectService.setHoverProject(project);
  }

  clearFilters(): void {
    this.commonService.clearFilters(this.filterState);
  }

  searchProjects(searchStr: string): void {
    this.filterState.search = searchStr;
    this.projectService.setSkip(0);
    this.projectsList = [];
    this.projectService.setMapProjectList(this.projectsList);
    this.projectService.setSearchType(SearchType.AllProjects);
    this.title = this.translateService.instant('projects.filterResultsLabel');
    this.forYouFilterTitle = '';
    this.commonService.filterState$.next(this.filterState);
  }

  clearSearch(): void {
    if (!this.showMap) {
      this.commonService.clearFilters(this.filterState, false, false, true, '', true);
      if (this.commonService.isAnyFilterChecked(this.filterState) || this.filterState.search) {
        this.updateSearchType(SearchType.Filter);
      } else {
        this.updateSearchType(SearchType.ForYou);
      }
    } else {
      this.commonService.clearFilters(this.filterState, false, true, true, '', true);
      this.updateSearchType(SearchType.Filter);
    }
  }

  applyFilter(filterName: string): void {
    switch (filterName) {
      case this.translateService.instant('general.forYouLabel'): {
        this.updateSearchType(SearchType.ForYou);
        break;
      }
      case this.translateService.instant('general.savedLabel'): {
        this.title = this.translateService.instant('general.savedLabel');
        this.forYouFilterTitle = '';
        this.showClearButton = false;
        this.projectService.setSearchType(SearchType.Saved);
        break;
      }
      case this.translateService.instant('projects.allProjectsLabel'): {
        this.title = this.translateService.instant('projects.allProjectsLabel');
        this.forYouFilterTitle = '';
        this.showClearButton = false;
        this.projectService.setSearchType(SearchType.AllProjects);
        break;
      }
    }
    this.clearFilters();
  }

  switchViewMode(): void {
    this.projectService.setMapProjectList(this.projectsList);
    this.showMap = !this.showMap;
    this.projectService.setshowMap(this.showMap);
    if (this.showMap) {
      const solutions = this.filterState.parameter[TaxonomyEnum.solutions] as FilterDataInterface[];
      const categories = this.filterState.parameter[TaxonomyEnum.categories] as FilterDataInterface[];
      const technologies = this.filterState.parameter[TaxonomyEnum.technologies] as FilterDataInterface[];

      solutions
        .filter(s => !this.ppaSolutionIndices.includes(s.id))
        .forEach(s => {
          (s.checked = false), (s.hide = true);
        });
      categories
        .filter(c => !this.ppaCategoryIndicies.includes(c.id))
        .forEach(c => {
          (c.checked = false), (c.hide = true);
        });
      technologies
        .filter(c => !this.ppaTechnologyIndicies.includes(c.id))
        .forEach(c => {
          (c.checked = false), (c.hide = true);
        });

      this.commonService.checkCategories(this.filterState, this.initialData);
      this.commonService.checkTechnologies(this.filterState, this.initialData);
      this.projectService.setSearchType(SearchType.Filter);
      this.commonService.filterState$.next(this.filterState);
      this.activityService.trackElementInteractionActivity(ActivityTypeEnum.ViewMapClick)?.subscribe();
    } else {
      const solutions = this.filterState.parameter[TaxonomyEnum.solutions] as FilterDataInterface[];
      const categories = this.filterState.parameter[TaxonomyEnum.categories] as FilterDataInterface[];
      const technologies = this.filterState.parameter[TaxonomyEnum.technologies] as FilterDataInterface[];

      solutions.filter(s => !this.ppaSolutionIndices.includes(s.id)).forEach(s => (s.hide = false));
      categories.filter(c => !this.ppaCategoryIndicies.includes(c.id)).forEach(c => (c.hide = false));
      technologies.filter(c => !this.ppaTechnologyIndicies.includes(c.id)).forEach(c => (c.hide = false));

      if (this.commonService.isAnyFilterChecked(this.filterState) || this.filterState?.search) {
        this.updateSearchType(this.searchType);
      } else {
        this.updateSearchType(SearchType.ForYou);
      }
    }
  }

  updatePaging(page: number): void {
    this.paging.skip = (page - 1) * this.defaultItemPerPage;
    this.projectService.setSkip((page - 1) * this.defaultItemPerPage);
    this.updateSearchType(this.searchType);
  }

  updateSearchType(type: SearchType) {
    if (this.searchType !== type) {
      this.projectService.setSkip(0);
    }
    this.searchType = type;
    this.projectService.setSearchType(type);
    switch (this.searchType) {
      case SearchType.ForYou: {
        this.title = this.translateService.instant('projects.suggestedForYouLabel');
        this.forYouFilterTitle = this.translateService.instant('general.forYouLabel');
        this.showClearButton = false;
        this.loadForYou$.next();
        break;
      }
      case SearchType.Saved: {
        this.title = this.translateService.instant('general.savedLabel');
        this.forYouFilterTitle = '';
        this.loadSaved$.next();
        break;
      }
      case SearchType.AllProjects: {
        this.title = this.translateService.instant('general.allProjectsLabel');
        this.forYouFilterTitle = '';
        this.loadProjects$.next();
        break;
      }
      case SearchType.Filter: {
        this.title = this.translateService.instant('projects.allProjectsLabel');
        this.forYouFilterTitle = '';
        this.loadProjects$.next();
        break;
      }
      case SearchType.Search: {
        this.title = this.translateService.instant('projects.filterResultsLabel');
        this.forYouFilterTitle = '';
        this.loadProjects$.next();
        break;
      }
    }
  }

  private loadInitialData(): void {
    this.commonService
      .initialData()
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(response => !!response)
      )
      .subscribe(d => {
        this.initialData = d;
        const ppaCategories = d.categories.filter(
          c => c.slug === this.projectTypes.AggregatedPpa || c.slug === this.projectTypes.OffsitePpa
        );

        this.ppaCategoryIndicies = ppaCategories.map(c => c.id);
        this.ppaSolutionIndices = d.solutions
          .filter(
            s =>
              s.categories.map(c => c.slug).includes(this.projectTypes.AggregatedPpa) ||
              s.categories.map(c => c.slug).includes(this.projectTypes.OffsitePpa)
          )
          .map(c => c.id);
        this.ppaTechnologyIndicies = [].concat(...ppaCategories.map(s => s.technologies)).map(c => c.id);
      });
  }

  private listenForYou(): void {
    this.loadForYou$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          return this.httpService.get<PaginateResponseInterface<ProjectInterface>>(
            `${this.routes.projectsList}?orderby=ispinned.desc,title.asc&expand=category,owner,company,company.image,regions,saved,projectdetails`,
            this.getApiBody('foryou')
          );
        })
      )
      .subscribe(projList => {
        this.initialLoad = false;
        this.fillProjectData(projList);
      });
  }

  private listenSaved(): void {
    this.loadSaved$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          return this.httpService.get<PaginateResponseInterface<ProjectInterface>>(
            `${this.routes.projectsList}?orderby=ispinned.desc,title.asc&expand=category,owner,company,company.image,regions,saved,projectdetails`,
            this.getApiBody('saved')
          );
        })
      )
      .subscribe(projList => {
        this.initialLoad = false;
        this.fillProjectData(projList);
      });
  }

  private listenForFilterState(): void {
    this.commonService
      .filterState()
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(initialFiltersState => !!initialFiltersState)
      )
      .subscribe(filterState => {
        let searchStr = filterState ? filterState?.search : '';
        this.filterState = filterState;
        if (this.filterState) this.filterState.search = searchStr;
        this.paging.skip = this.getLastAppliedSkip();
        if (this.commonService.isAnyFilterChecked(this.filterState) || this.filterState.search) {
          this.updateSearchType(SearchType.Filter);
        } else {
          this.updateSearchType(this.searchType);
        }
      });
  }

  private listenToLoadProjects(): void {
    let searchStr = '';
    this.loadProjects$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => {
          searchStr = this.filterState.search;
          const parameters = this.getParams(this.filterState);
          this.showClearButton = this.commonService.isAnyFilterChecked(this.filterState);

          const requestInfo: Record<string, unknown> = {
            search: parameters.search,
            filterBy: parameters.parameter,
            projectsViewType: ProjectsViewTypeEnum.Catalog,
            includeCount: true,
            skip: this.paging?.skip ? this.paging?.skip : 0,
            take: this.paging?.take ? this.paging?.take : this.defaultItemPerPage
          };

          if (this.searchType === SearchType.AllProjects && !this.showClearButton) {
            requestInfo.filterBy += `allprojects`;
          }

          if (this.showMap) {
            delete requestInfo.take;
            if (
              !(this.filterState.parameter[this.taxonomyEnum.categories] as FilterDataInterface[]).filter(
                c => c.checked
              ).length
            ) {
              requestInfo.filterBy += `&categoryids=${this.ppaCategoryIndicies.join(',')}`;
            }
            return this.httpService.get<PaginateResponseInterface<ProjectInterface>>(
              `${this.routes.projectsList}?orderby=ispinned.desc,changedon.desc&expand=category,owner,company,company.image,regions,saved,projectdetails`,
              this.coreService.deleteEmptyProps(requestInfo)
            );
          }
          return this.httpService.get<PaginateResponseInterface<ProjectInterface>>(
            `${this.routes.projectsList}?orderby=ispinned.desc,title.asc&expand=category,owner,company,company.image,regions,saved,projectdetails`,
            this.coreService.deleteEmptyProps(requestInfo)
          );
        })
      )
      .subscribe(projList => {
        if (!this.filterState.search) this.filterState.search = searchStr;
        this.initialLoad = false;
        this.fillProjectData(projList);
      });
  }

  private getApiBody(filter: string): object {
    return this.coreService.deleteEmptyProps({
      filterBy: filter,
      projectsViewType: ProjectsViewTypeEnum.Catalog,
      includeCount: true,
      skip: this.paging?.skip ? this.paging?.skip : 0,
      take: this.paging?.take ? this.paging?.take : this.defaultItemPerPage
    });
  }

  private getLastAppliedSkip(): number {
    return this.initialLoad && this.paging.skip > 0 ? this.paging.skip : 0;
  }

  private fillProjectData(projList: PaginateResponseInterface<ProjectInterface>): void {
    this.projectsList = projList.dataList;
    this.projectService.setMapProjectList(projList.dataList);
    this.paging = {
      ...this.paging,
      skip: this.paging?.skip ? this.paging?.skip : 0,
      total: projList.count
    };

    this.viewPort.scrollToPosition([0, 0]);
  }

  private getParams(params: FilterStateInterface): FilterStateInterface {
    return {
      ...params,
      parameter: this.getQueryString(params?.parameter)
        .replace('solutions', 'solutionids')
        .replace('categories', 'categoryids')
        .replace('technologies', 'technologyids')
        .replace('regions', 'regionids')
    };
  }

  private getQueryString(params): string {
    return Object.keys(params)
      .map(key => {
        const items = params[key].filter(item => !!item.checked || item?.childElements?.length);

        if (items.some(item => item?.childElements)) {
          return ProjectsComponent.getFilterChildDataQuery(key, items);
        } else if (items.length) {
          return ProjectsComponent.getFilterDataInterfaceQuery(key, items);
        }
      })
      .filter(item => !!item)
      .join('&');
  }

  private static getFilterDataInterfaceQuery(key: string, items: FilterDataInterface[]): string {
    const query = `${key}=${items.map(item => {
      if (item.checked) {
        return item.id;
      }
    })}`;

    return query.match(/[0-9]/g) ? query : '';
  }

  private static getFilterChildDataQuery(key: string, items: FilterChildDataInterface[]): string {
    const ids = items.map(item => {
      if (item.checked && item?.childElements.some(item => item.checked)) {
        return (
          `${item.id},` +
          `${item.childElements
            .filter(child => !!child.checked && !child.disabled)
            .map(child => child.id)
            .join(',')}`
        );
      } else if (item.checked && !item?.childElements.some(item => item.checked && !item.disabled)) {
        return `${item.id},`;
      } else if (item?.childElements.some(item => item.checked && !item.disabled)) {
        return `${item.childElements
          .filter(child => !!child.checked)
          .map(child => child.id)
          .join(',')}`;
      }
    });
    const query = `${key}=${ids.filter(id => !!id)}`;
    return query.match(/[0-9]/g) ? query : '';
  }

  clearAll() {
    if (this.showMap) {
      this.commonService.clearFilters(this.filterState, true, true);
    } else {
      this.commonService.clearFilters(this.filterState, true);
    }
  }

  removeProjectFromSavedList(project: ProjectInterface) {
    this.mapProjectList = this.mapProjectList.filter(x => x.id != project.id);
  }
}
