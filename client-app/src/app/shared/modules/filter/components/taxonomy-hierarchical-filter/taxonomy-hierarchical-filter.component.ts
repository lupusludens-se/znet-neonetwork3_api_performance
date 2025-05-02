import { Component, Input, OnInit } from '@angular/core';

import { Subject, takeUntil } from 'rxjs';

import { CommonService } from '../../../../../core/services/common.service';
import { FilterStateInterface } from '../../interfaces/filter-state.interface';
import { ExpandStateEnum } from '../../enums/expand-state.enum';
import { FilterChildDataInterface } from '../../interfaces/filter-child-data.interface';

@Component({
  selector: 'neo-taxonomy-hierarchical-filter',
  templateUrl: './taxonomy-hierarchical-filter.component.html',
  styleUrls: ['./taxonomy-hierarchical-filter.component.scss', '../../../../styles/filters-common.scss']
})
export class TaxonomyHierarchicalFilterComponent implements OnInit {
  @Input() taxonomy: string;
  @Input() name: string;
  @Input() icon: string;
  @Input() horizontalLayout: boolean;
  @Input() showIcon: boolean = true;
  @Input() filterLayout: string = '';
  expandedState: ExpandStateEnum = null;
  filterState: FilterStateInterface;

  private unsubscribe$: Subject<void> = new Subject<void>();
  private hierarchicalData: FilterChildDataInterface[] = [];

  constructor(private readonly commonService: CommonService) {}

  get getSelectedNumber(): number {
    if (!this.filterState?.parameter) {
      return 0;
    }

    const selectedIds = [];

    this.filterState?.parameter[this.taxonomy]?.map(item => {
      if (item.checked) {
        selectedIds.push(item.id);
      }

      if (item.childElements) {
        item.childElements.map(child => {
          if (child.checked) {
            selectedIds.push(item.id);
          }
        });
      }
    });

    return selectedIds.length;
  }

  ngOnInit(): void {
    this.listenForFilterState();
  }

  changeExpandingState(): void {
    this.expandedState =
      this.expandedState === ExpandStateEnum.expanded ? ExpandStateEnum.collapsed : ExpandStateEnum.expanded;
  }

  closeExpansion() {
    if (this.filterLayout === 'map') {
      this.expandedState = ExpandStateEnum.collapsed;
    }
  }

  collapseFilter(): void {
    this.expandedState = ExpandStateEnum.collapsed;
  }

  changeChildExpandingState(index: number): void {
    this.filterState.parameter[this.taxonomy][index].expandedState =
      this.filterState.parameter[this.taxonomy][index].expandedState === ExpandStateEnum.expanded
        ? ExpandStateEnum.collapsed
        : ExpandStateEnum.expanded;
  }

  click(parentIndex: number, childIndex?: number): void {
    if (childIndex >= 0) {
      this.selectChild(parentIndex, childIndex);
    } else {
      this.selectParentAndChildElements(parentIndex);
    }

    this.filterState.skip = 0;
    this.commonService.filterState$.next(this.filterState);
  }

  input(value: string, index: number): void {
    this.filterState.parameter[this.taxonomy][index].filterSearch = value;
    const filtered = this.hierarchicalData[index].childElements.filter(element =>
      element.name.trim().toLowerCase().includes(value?.toLowerCase())
    );

    if (filtered.length) {
      this.hierarchicalData[index].childElements.forEach(function (item, index) {
        let filteredItemsAvailable = filtered.filter(x => x.id == item.id);
        item.hide = filteredItemsAvailable.length > 0 ? false : true;
      });
    } else {
      this.hierarchicalData[index].childElements.forEach(function (item, index) {
        item.hide = false;
      });
    }

    this.filterState.parameter[this.taxonomy][index].childElements = this.hierarchicalData[index].childElements;
  }

  private listenForFilterState() {
    this.commonService
      .filterState()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(filterState => {
        let searchStr = this.filterState ? this.filterState?.search : '';
        this.filterState = filterState;
        if (this.filterState) this.filterState.search = searchStr;

        if (!this.hierarchicalData.length) {
          filterState?.parameter[this.taxonomy]?.map(item => {
            this.hierarchicalData.push(Object.assign({}, item));
          });
        }
      });
  }

  private selectChild(parentIndex: number, childIndex?: number): void {
    // TODO: Use id instead of string + refactor and cleanup
    if (this.filterState.parameter[this.taxonomy][parentIndex].childElements[childIndex].name === 'US - All') {
      this.filterState.parameter[this.taxonomy][parentIndex].childElements.forEach(child => {
        if (child.name.toLowerCase().startsWith('us')) {
          if (child.name === 'US - All') {
            child.checked = !child.checked;
            child.disabled = false;
          } else {
            if (this.filterState.parameter[this.taxonomy][parentIndex].childElements[childIndex].checked) {
              child.checked = true;
              child.disabled = true;
            } else {
              child.checked = false;
              child.disabled = false;
            }
          }
        }
      });
    } else {
      this.filterState.parameter[this.taxonomy][parentIndex].childElements[childIndex].checked =
        !this.filterState.parameter[this.taxonomy][parentIndex].childElements[childIndex].checked;
    }
  }

  private selectParentAndChildElements(parentIndex: number): void {
    this.filterState.parameter[this.taxonomy][parentIndex].checked =
      !this.filterState.parameter[this.taxonomy][parentIndex].checked;

    this.filterState.parameter[this.taxonomy][parentIndex]?.childElements.forEach(item => {
      if (this.filterState.parameter[this.taxonomy][parentIndex].checked) {
        item.checked = true;
        item.disabled = true;
      } else {
        item.checked = false;
        item.disabled = false;
      }
    });

    this.hierarchicalData[parentIndex].childElements.forEach(item => {
      if (this.filterState.parameter[this.taxonomy][parentIndex].checked) {
        item.checked = true;
        item.disabled = true;
      } else {
        item.checked = false;
        item.disabled = false;
      }
    });
  }
}
