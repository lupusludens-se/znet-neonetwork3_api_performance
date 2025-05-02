import { Component, Input, OnInit } from '@angular/core';

import { filter } from 'rxjs/operators';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { CommonService } from '../../../../../core/services/common.service';
import { FilterStateInterface } from '../../interfaces/filter-state.interface';
import { TaxonomyEnum } from '../../../../../core/enums/taxonomy.enum';
import { FilterDataInterface } from '../../interfaces/filter-data.interface';
import { FilterChildDataInterface } from '../../interfaces/filter-child-data.interface';
import { InitialDataInterface } from 'src/app/core/interfaces/initial-data.interface';

@UntilDestroy()
@Component({
  selector: 'neo-filter-state',
  templateUrl: './filter-state.component.html'
})
export class FilterStateComponent implements OnInit {
  taxonomies: string[] = Object.values(TaxonomyEnum);
  filterState: FilterStateInterface;
  expandAllSelected: boolean;
  @Input() maxAppliedFiltersViewCount: number = 7;
  selectedTags: { id: number; name: string; taxonomy: string; parentId?: number }[] = [];
  initialData: InitialDataInterface;

  constructor(private readonly commonService: CommonService) {}

  ngOnInit(): void {
    this.commonService
      .filterState()
      .pipe(
        untilDestroyed(this),
        filter(filterState => !!filterState)
      )
      .subscribe(filterState => this.mapFilterState(filterState));
    this.listenForInitialData();
  }

  removeFilter(tag: { id: number; name: string; taxonomy: string; parentId?: number }): void {
    const selectedParent = this.filterState.parameter[tag.taxonomy].find(
      item => item.id === (tag.parentId ? tag.parentId : tag.id)
    );

    if (tag.name === 'US - All') {
      this.uncheckFilter(selectedParent, true);
    }
    if (!tag?.parentId) {
      this.uncheckFilter(selectedParent);
    } else {
      const selectedChild = selectedParent.childElements.find(item => item.id === tag.id);
      selectedChild.checked = false;
    }

    if (tag.taxonomy === TaxonomyEnum.solutions) {
      this.commonService.checkCategories(this.filterState, this.initialData);
    }
    if (tag.taxonomy === TaxonomyEnum.categories) {
      this.commonService.checkTechnologies(this.filterState, this.initialData);
    }

    this.commonService.filterState$.next(this.filterState);
  }

  uncheckFilter(
    selectedParent: FilterDataInterface | FilterChildDataInterface,
    isUSAllSelected: boolean = false
  ): void {
    selectedParent.checked = false;
    selectedParent.disabled = false;
    const childElements = (<FilterChildDataInterface>selectedParent).childElements;
    if (childElements) {
      if (isUSAllSelected) {
        for (let child of childElements) {
          if (child.name.startsWith('US')) {
            this.uncheckFilter(child);
          }
        }
      } else {
        for (let child of childElements) {
          this.uncheckFilter(child);
        }
      }
    }
  }

  toggle(): void {
    this.expandAllSelected = !this.expandAllSelected;
  }

  private mapFilterState(filterState: FilterStateInterface): void {
    this.filterState = filterState;
    this.selectedTags = [];

    this.taxonomies?.forEach(taxonomy => {
      this.filterState.parameter[taxonomy]
        ?.filter(tag => tag.checked)
        .map(tag => this.selectedTags.push({ id: tag.id, name: tag.name, taxonomy: taxonomy }));

      this.filterState.parameter[taxonomy]
        ?.filter(tag => tag?.childElements?.length)
        .forEach(tag => {
          const checkedTags = tag?.childElements?.filter(t => t.checked && !t.disabled);

          if (checkedTags?.length) {
            checkedTags.forEach(t =>
              this.selectedTags.push({ id: t.id, name: t.name, taxonomy: taxonomy, parentId: tag.id })
            );
          }
        });
    });
  }

  private listenForInitialData(): void {
    this.commonService
      .initialData()
      .pipe(untilDestroyed(this))
      .subscribe(initialData => (this.initialData = { ...initialData }));
  }
}
