import { Injectable } from '@angular/core';

import { BehaviorSubject, forkJoin, Observable } from 'rxjs';
import { share } from 'rxjs/operators';

import { HttpService } from './http.service';

import { InitialDataInterface } from '../interfaces/initial-data.interface';
import { TagInterface } from '../interfaces/tag.interface';
import { TagParentInterface } from '../interfaces/tag-parent.interface';
import { FilterStateInterface } from '../../shared/modules/filter/interfaces/filter-state.interface';
import { FilterChildDataInterface } from '../../shared/modules/filter/interfaces/filter-child-data.interface';

import { CommonApiEnum } from '../enums/common-api.enum';
import { TaxonomyEnum } from '../enums/taxonomy.enum';
import { ExpandStateEnum } from '../../shared/modules/filter/enums/expand-state.enum';

import { USER_STATUS_CONST } from '../../shared/constants/user-status.const';
import { ActivatedRoute, Router } from '@angular/router';
import { FilterDataInterface } from 'src/app/shared/modules/filter/interfaces/filter-data.interface';

@Injectable()
export class CommonService {
  US_ALL_OPTION = 'US - All';
  statusesList = USER_STATUS_CONST;
  initialData$: BehaviorSubject<InitialDataInterface> = new BehaviorSubject<InitialDataInterface>(null);
  isLearnFilter$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  filterState$: BehaviorSubject<FilterStateInterface> = new BehaviorSubject<FilterStateInterface>(null);

  initialDataRequestsList: Observable<TagParentInterface[] | TagInterface[]>[] = [
    this.httpService.get<TagParentInterface[]>(CommonApiEnum.Categories),
    this.httpService.get<TagInterface[]>(CommonApiEnum.Technologies),
    this.httpService.get<TagParentInterface[]>(CommonApiEnum.Regions),
    this.httpService.get<TagInterface[]>(CommonApiEnum.Solutions)
  ];

  hideSkeleton$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  private isSessionStorageAvailable: boolean | null = null;
  private previousFilterStateKey: string = null;

  constructor(private readonly httpService: HttpService, private readonly router: Router) {}

  initialData(): Observable<InitialDataInterface> {
    return this.initialData$.pipe(share());
  }

  // savedStateKey: string | null = null
  filterState(): Observable<FilterStateInterface> {
    // if (savedStateKey) {
    //   const savedFilterState = this.loadFilterState(savedStateKey);
    //
    //   if (savedFilterState) {
    //     this.filterState$.next(savedFilterState);
    //   }
    // }

    return this.filterState$.pipe(share());
  }

  emitFilterState(savedStateKey: string): void {
    if (savedStateKey) {
      const savedFilterState = this.loadFilterState(savedStateKey);
      if (savedFilterState) {
        this.filterState$.next(savedFilterState);
        return;
      }
    }

    this.filterState$.next(null);
  }

  goBack() {
    if (history.length > 1) history.back();
    else this.router.navigate(['./dashboard']);
  }

  hasFilterSessionData(
    activatedRoute: ActivatedRoute,
    filterStateKey: string,
    defaultTitle: string,
    isFilteredorSearched?: boolean
  ): boolean {
    const sessionData = sessionStorage.getItem(filterStateKey);
    const hasFilterQueryParam = !!activatedRoute.snapshot.queryParams.filtered;
    return (
      !!sessionData && (hasFilterQueryParam || JSON.parse(sessionData).title === defaultTitle || isFilteredorSearched)
    );
  }

  loadInitialData(includeRoles: boolean, includeIndustries: boolean): void {
    const requestWithRoles: Observable<TagParentInterface[] | TagInterface[]>[] = includeRoles
      ? [...this.initialDataRequestsList, this.httpService.get<TagInterface[]>(CommonApiEnum.Roles)]
      : [...this.initialDataRequestsList];

    const requestWithIndustries: Observable<TagParentInterface[] | TagInterface[]>[] = includeIndustries
      ? [...requestWithRoles, this.httpService.get<TagInterface[]>(CommonApiEnum.Industries)]
      : [...requestWithRoles];

    forkJoin(requestWithIndustries).subscribe(([categories, technologies, regions, solutions, roles, industries]) => {
      this.initialData$.next({
        categories,
        technologies,
        regions: regions as TagParentInterface[],
        solutions,
        roles,
        statuses: this.statusesList,
        industries,
        projectCapabilities: categories
      });

      this.setFilterState({
        categories,
        technologies,
        regions: regions as TagParentInterface[],
        solutions,
        roles,
        statuses: this.statusesList,
        industries,
        projectCapabilities: categories
      });
    });
  }

  clearFilters(
    filterState: FilterStateInterface,
    ignoreSearchClean?: boolean,
    isPPAMapSearch: boolean = false,
    ignoreItemUnCheck: boolean = false,
    optionalFilter: string = 'clearAll',
    isFilterEnable: boolean = false,
    skipFilter: string = null
  ): void {
    if (!ignoreSearchClean) {
      filterState.search = '';
    }

    Object.keys(filterState?.parameter)?.map(key => {
      if (typeof filterState.parameter[key] === 'string' || typeof filterState.parameter[key] === 'number') return;

      return filterState?.parameter[key]?.map(item => {
        if (
          (!ignoreItemUnCheck && key == optionalFilter) ||
          optionalFilter == 'clearAll' ||
          (skipFilter != null && skipFilter != key)
        ) {
          item.checked = false;
        }
        //isFilterEnable is set to true when a diabled filter should not be enabled on searching string parallely
        if (!isFilterEnable) {
          item.disabled = false;
        }
        item.filterSearch = '';
        if (isPPAMapSearch === false && item.hide === true) {
          item.hide = false;
        }

        if (item?.childElements) {
          item?.childElements.map(element => {
            element.checked = false;

            element.disabled = false;
            if (isPPAMapSearch === false && element.hide === true) {
              element.hide = false;
            }
          });
        }
      });
    });

    this.filterState$.next(filterState);
  }

  isAnyFilterChecked(filterState: FilterStateInterface): boolean {
    if (!filterState?.parameter) return false;

    const isChecked: Array<boolean> = Object.keys(filterState.parameter).map(key => {
      if (typeof filterState.parameter[key] === 'string' || typeof filterState.parameter[key] === 'number') {
        return;
      }

      return filterState.parameter[key].some(item => item.checked);
    });

    const childChecked: Array<boolean> = Object.keys(filterState.parameter).map(key => {
      if (typeof filterState.parameter[key] === 'string' || typeof filterState.parameter[key] === 'number') {
        return;
      }

      return filterState.parameter[key]?.some(item =>
        item?.childElements?.some(item => item?.checked && !item?.disabled)
      );
    });

    return isChecked.some(item => item) || childChecked.some(item => item);
  }

  isAllUsChecked(filterState: FilterStateInterface): boolean {
    const usaAndCanada = this.getUsaAndCanadaFilter(filterState);

    const usAll = usaAndCanada?.childElements.find(child => child.name === this.US_ALL_OPTION);

    return usAll?.checked === true;
  }

  toggleUsStateOptions(filterState: FilterStateInterface, isEnabled: boolean) {
    const usaAndCanada = this.getUsaAndCanadaFilter(filterState);

    const usStates = usaAndCanada?.childElements.filter(
      filter => filter.name !== this.US_ALL_OPTION && filter.name.startsWith('US - ')
    );

    usStates?.forEach(state => (state.disabled = !isEnabled));
  }

  // to retrieve the previous route filter state key
  getPreviousFilterStateKey(): string {
    return this.previousFilterStateKey;
  }

  // to store the previous route filter state key
  setPreviousFilterStateKey(key: string): void {
    this.previousFilterStateKey = key;
  }

  getFilterStateKey(): string {
    // use relative URL; replace all slashes with dots
    return `filter${location.pathname.replace('/', '.')}`;
  }

  private loadFilterState(key: string): FilterStateInterface | null {
    if (!this.getIsSessionStorageAvailable()) {
      return null;
    }

    const filterStateJson = sessionStorage.getItem(key);

    return filterStateJson ? JSON.parse(filterStateJson) : null;
  }

  saveFilterState(key: string): void {
    if (!this.getIsSessionStorageAvailable()) {
      return;
    }

    if (!this.filterState$.value) {
      sessionStorage.removeItem(key);
    } else {
      sessionStorage.setItem(key, JSON.stringify(this.filterState$.value));
    }
  }

  clearFilterState(key: string): void {
    if (!this.getIsSessionStorageAvailable()) {
      return;
    }

    sessionStorage.removeItem(key);
  }

  private getUsaAndCanadaFilter(filterState: FilterStateInterface): FilterChildDataInterface | null {
    const regions = filterState?.parameter['regions'];

    return regions?.find(region => region.name === 'USA & Canada');
  }

  private setFilterState(initialData: InitialDataInterface) {
    const filterState: FilterStateInterface = {
      parameter: {},
      search: ''
    };

    Object.keys(initialData)
      .filter(key => !!initialData[key])
      .map(key => {
        if (key === TaxonomyEnum.regions) {
          filterState.parameter[key] = this.mapParentData(initialData);
        } else {
          filterState.parameter[key] = initialData[key].map(item => ({
            id: item.id,
            name: item.name,
            checked: false,
            disabled: false,
            hide: false
          }));
        }
      });

    this.filterState$.next({ ...filterState });
  }

  private mapParentData(initialData: InitialDataInterface): {
    id: number;
    name: string;
    childElements: { id: number; name: string }[];
    expandedState: ExpandStateEnum;
  }[] {
    const parentData = initialData.regions.filter(data => data.parentId === 0);

    return parentData.map(data => {
      const childElements = initialData.regions.filter(element => element.parentId === data.id);

      return {
        id: data.id,
        name: data.name,
        expandedState: null,
        checked: false,
        disabled: false,
        hide: false,
        childElements: childElements.map(child => ({
          id: child.id,
          name: child.name,
          checked: false,
          disabled: false,
          hide: false
        }))
      };
    });
  }

  private getIsSessionStorageAvailable(): boolean {
    if (this.isSessionStorageAvailable !== null) {
      return this.isSessionStorageAvailable;
    }

    this.isSessionStorageAvailable = false;

    // so much effort just to see if we can store things...
    if (typeof sessionStorage !== 'undefined') {
      try {
        const key = 'session_storage_test';

        const value = 'yes';

        sessionStorage.setItem(key, value);

        if (sessionStorage.getItem(key) === value) {
          sessionStorage.removeItem(key);

          this.isSessionStorageAvailable = true;
        }
      } catch (e) {
        /* do nothing */
      }
    }

    return this.isSessionStorageAvailable;
  }

  checkCategories(filtersState: FilterStateInterface, initialData?: InitialDataInterface) {
    const categories = filtersState.parameter[TaxonomyEnum.categories] as FilterDataInterface[];
    const technologies = filtersState.parameter[TaxonomyEnum.technologies] as FilterDataInterface[];
    const selectedSolutions = filtersState.parameter[TaxonomyEnum.solutions].filter(t => t.checked);

    if (selectedSolutions.length === 0) {
      categories.forEach(c => (c.disabled = false));
      technologies.forEach(c => (c.disabled = false));
    } else {
      const availableCategoryNames = []
        .concat(
          ...initialData.solutions
            .filter(s => selectedSolutions.map(s => s.name.toLowerCase()).includes(s.name?.toLowerCase()))
            .map(s => s.categories)
        )
        .map(c => c.name.toLowerCase());

      filtersState.parameter[TaxonomyEnum.categories].forEach(c => {
        if (availableCategoryNames.includes(c.name?.toLowerCase())) {
          c.disabled = false;
        } else {
          c.disabled = true;
          c.checked = false;
        }
      });

      const selectedCategories = filtersState.parameter[TaxonomyEnum.categories].filter(t => !t.disabled);

      const availableTechnologyNames = []
        .concat(
          ...initialData.categories
            .filter(s => selectedCategories.map(s => s.name.toLowerCase()).includes(s.name?.toLowerCase()))
            .map(s => s.technologies)
        )
        .map(c => c.name.toLowerCase());

      filtersState.parameter[TaxonomyEnum.technologies].forEach(c => {
        if (availableTechnologyNames.includes(c.name?.toLowerCase())) {
          c.disabled = false;
        } else {
          c.disabled = true;
          c.checked = false;
        }
      });
    }
  }

  checkTechnologies(filtersState: FilterStateInterface, initialData: InitialDataInterface) {
    const technologies = filtersState.parameter[TaxonomyEnum.technologies] as FilterDataInterface[];
    const selectedCategories = filtersState.parameter[TaxonomyEnum.categories].filter(t => t.checked);

    if (selectedCategories.length === 0) {
      technologies.forEach(c => (c.disabled = false));
    } else {
      const availableTechnologyNames = []
        .concat(
          ...initialData.categories
            .filter(s => selectedCategories.map(s => s.name.toLowerCase()).includes(s.name?.toLowerCase()))
            .map(s => s.technologies)
        )
        .map(c => c.name.toLowerCase());

      filtersState.parameter[TaxonomyEnum.technologies].forEach(c => {
        if (availableTechnologyNames.includes(c.name?.toLowerCase())) {
          c.disabled = false;
        } else {
          c.disabled = true;
          c.checked = false;
        }
      });
    }
  }
  getLearnFilter$() {
    return this.isLearnFilter$.pipe(share());
  }

  setLearnFilter(value: boolean) {
    this.isLearnFilter$.next(value);
  }

  getRegionScaleTypes(): Observable<TagInterface[]> {
    return this.httpService.get<TagInterface[]>(CommonApiEnum.RegionScaleTypes);
  }

  public isAnyParticularFilterChecked(filterState: FilterStateInterface, key: string): boolean {
    if (
      !filterState?.parameter ||
      typeof filterState.parameter[key] === 'string' ||
      typeof filterState.parameter[key] === 'number'
    )
      return;

    const isParentChecked: boolean = filterState.parameter[key].some(item => item.checked);
    const isChildChecked: boolean = filterState.parameter[key]?.some(item =>
      item?.childElements?.some(item => item?.checked && !item?.disabled)
    );

    return isParentChecked || isChildChecked;
  }
}
