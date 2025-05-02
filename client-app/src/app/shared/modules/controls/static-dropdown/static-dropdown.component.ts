import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";
import { Subject, takeUntil } from "rxjs";
import { CommonService } from "src/app/core/services/common.service";
import { ExpandStateEnum } from "../../filter/enums/expand-state.enum";
import { FilterChildDataInterface } from "../../filter/interfaces/filter-child-data.interface";
import { FilterStateInterface } from "../../filter/interfaces/filter-state.interface";
import { FilterDataInterface } from "../../filter/interfaces/filter-data.interface";

@Component({
  selector: 'neo-static-dropdown',
  templateUrl: './static-dropdown.component.html',
  styleUrls: ['./static-dropdown.component.scss', '../../../styles/filters-common.scss']
})
export class StaticDropdownComponent implements OnInit {
  @Input() taxonomy: FilterDataInterface[];
  @Input() name: string;
  @Input() icon: string;
  @Input() horizontalLayout: boolean;
  @Input() showIcon: boolean = true;
	@Input() filterLayout: string = '';
	@Output() optionSelected: EventEmitter<number[]> = new EventEmitter<number[]>();
  expandedState: ExpandStateEnum = ExpandStateEnum.collapsed;
  filterState: FilterStateInterface;
  private unsubscribe$: Subject<void> = new Subject<void>();
  constructor(private readonly commonService: CommonService) { }

  get getSelectedNumber(): number {
    const selectedIds = [];
    this.taxonomy?.map(item => {
      if (item.checked) {
        selectedIds.push(item.id);
      }
    });
    return selectedIds?.length;
  }

  ngOnInit(): void {
  }

  changeExpandingState(): void {
    this.expandedState =
      this.expandedState === ExpandStateEnum.expanded ? ExpandStateEnum.collapsed : ExpandStateEnum.expanded;
  }

  closeExpansion() {
    this.expandedState = ExpandStateEnum.collapsed;
  }

  click(i: number) {
		this.taxonomy[i].checked = !this.taxonomy[i].checked;
		this.optionSelected.emit(this.taxonomy?.filter(x=> x.checked)?.map(x => x.id));
  }
}
