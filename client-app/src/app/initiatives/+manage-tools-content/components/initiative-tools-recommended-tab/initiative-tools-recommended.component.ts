import { Component, EventEmitter, Input, Output, OnDestroy } from "@angular/core";
import { InitiativeToolInterface } from "src/app/initiatives/shared/models/initiative-resources.interface";
import { PaginateResponseInterface } from "src/app/shared/interfaces/common/pagination-response.interface";
import { PaginationInterface } from "src/app/shared/modules/pagination/pagination.component";
import { InitiativeToolContentService } from "../../services/initiative-tool-content.service";
import { InitiativeModulesEnum } from "src/app/initiatives/enums/initiative-modules.enum";
import { takeUntil } from "rxjs/operators";
import { Subject } from "rxjs";
import { InitiativeSharedService } from "src/app/initiatives/shared/services/initiative-shared.service";
import { TitleService } from "src/app/core/services/title.service";
@Component({
  selector: 'neo-initiative-tools-recommended',
  templateUrl: './initiative-tools-recommended.component.html',
  styleUrls: ['./initiative-tools-recommended.component.scss']
})
export class InitiativeToolsRecommendedComponent implements OnDestroy {
  counter = 0;
  selectedTiles: any[] = [];
  readonly defaultPageItems = 12;
  paging: PaginationInterface = {
    take: this.defaultPageItems,
    skip: 0,
    total: null
  };
  responseData: PaginateResponseInterface<InitiativeToolInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: -1
  };

  @Input() initiativeId: number;
  @Output() counterChange = new EventEmitter<number>();
  @Output() pageChangeDetected = new EventEmitter<boolean>();
  @Output() updateRecommendationsCounterforTools = new EventEmitter<number>();
  private destroy$ = new Subject<void>();
  constructor(private readonly initiativeToolContentService: InitiativeToolContentService,
    private titleService: TitleService,
    private initiativeSharedService: InitiativeSharedService,
  ) { }

  ngOnInit() {
    this.loadTools();
    this.titleService.setTitle('initiative.viewInitiative.viewAllToolsLabel');
  }

  retainContentSelection(dataList: InitiativeToolInterface[]) {
    const selectedTiles = this.initiativeToolContentService.getSelectedTiles();
    dataList.forEach(tile => {
      tile.isSelected = selectedTiles.ToolContent.some(selectedTile => selectedTile.id === tile.id);
    });
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.paging.take;
    this.loadTools();
    this.pageChangeDetected.emit(true);
  }

  toggleSelection(selectedTile: InitiativeToolInterface) {
    this.counter = this.initiativeToolContentService.getSelectedTiles()?.ToolContent?.length ?? 0;
    selectedTile.isSelected = !selectedTile.isSelected;
    this.counter = selectedTile.isSelected
      ? this.initiativeToolContentService.pushSelectedContents(this.initiativeId, selectedTile.id)
      : this.initiativeToolContentService.popSelectedContents(selectedTile.id);
    this.counterChange.emit(this.counter);
  }

  loadTools(selectedToolCount?: number) {
    if (selectedToolCount && this.responseData.dataList.length - selectedToolCount <= 0) {
      const skipPageCount = Math.floor(selectedToolCount / this.paging.take);
      const totalSkipCount = skipPageCount > 0 ? (skipPageCount + 1) * this.paging.take : this.paging.take;
      this.paging.skip = Math.max(this.paging.skip - totalSkipCount, 0);
    }
    this.initiativeToolContentService
      .getRecommendedTools(this.initiativeId, this.paging.skip, this.paging.take, false)
      .subscribe(data => {
        this.responseData = data;
        this.updateRecommendationsCounterforTools.emit(data.newRecommendationsCount);
        this.retainContentSelection(this.responseData.dataList);
        this.paging = {
          ...this.paging,
          skip: this.paging.skip ?? 0,
          total: data.count
        };
      });
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
