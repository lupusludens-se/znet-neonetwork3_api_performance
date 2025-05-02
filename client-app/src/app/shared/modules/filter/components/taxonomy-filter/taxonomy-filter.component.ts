import { Component, Input, OnDestroy, OnInit } from '@angular/core';

import { Subject, takeUntil } from 'rxjs';

import { CommonService } from '../../../../../core/services/common.service';

import { FilterStateInterface } from '../../interfaces/filter-state.interface';
import { TaxonomyEnum } from '../../../../../core/enums/taxonomy.enum';
import { ExpandStateEnum } from '../../enums/expand-state.enum';
import { InitialDataInterface } from 'src/app/core/interfaces/initial-data.interface';

@Component({
  selector: 'neo-taxonomy-filter',
  templateUrl: './taxonomy-filter.component.html',
  styleUrls: ['./taxonomy-filter.component.scss']
})
export class TaxonomyFilterComponent implements OnInit, OnDestroy {
  @Input() taxonomy: TaxonomyEnum;
  @Input() name: string;
  @Input() icon: string;
  @Input() horizontalLayout: boolean;
  @Input() showIcon: boolean = true;
  @Input() widthClass?: string = '';
  @Input() filterLayout: string = '';

  @Input() expandedState: ExpandStateEnum = null;
  filtersState: FilterStateInterface;
  initialData: InitialDataInterface;
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(private readonly commonService: CommonService) {}

  get getSelectedNumber(): number {
    if (!this.filtersState?.parameter) return 0;

    const number = this.filtersState?.parameter[this.taxonomy]?.filter(item => item.checked);

    return number ? number.length : 0;
  }

  ngOnInit(): void {
    this.listenForFilterStateChange();
    this.listenForInitialData();
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  changeExpandingState(): void {
    this.expandedState =
      this.expandedState === ExpandStateEnum.expanded ? ExpandStateEnum.collapsed : ExpandStateEnum.expanded;
  }

  collapseFilter(): void {
    this.expandedState = ExpandStateEnum.collapsed;
  }

  closeExpansion(): void {
    if (this.filterLayout === 'map' || this.filterLayout === 'msg') {
      this.expandedState = ExpandStateEnum.collapsed;
    }
  }

  click(index: number): void {
    this.filtersState.parameter[this.taxonomy][index].checked =
      !this.filtersState.parameter[this.taxonomy][index].checked;

    this.filtersState.skip = 0;

    if (this.taxonomy === TaxonomyEnum.solutions) {
      this.commonService.checkCategories(this.filtersState, this.initialData);
    }
    if (this.taxonomy === TaxonomyEnum.categories) {
      this.commonService.checkTechnologies(this.filtersState, this.initialData);
    }
    this.commonService.filterState$.next(this.filtersState);
  }

  private listenForFilterStateChange(): void {
    this.commonService
      .filterState()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(filterState => (this.filtersState = { ...filterState }));
  }

  private listenForInitialData(): void {
    this.commonService
      .initialData()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(initialData => (this.initialData = { ...initialData }));
  }
}
