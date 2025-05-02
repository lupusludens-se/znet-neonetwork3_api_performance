import { Component, OnInit, Output, Input, EventEmitter, OnDestroy } from "@angular/core";
import { Subject, Subscription } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { PostTypeEnum } from "src/app/core/enums/post-type.enum";
import { CoreService } from "src/app/core/services/core.service";
import { InitiativeArticleInterface } from "src/app/initiatives/shared/models/initiative-resources.interface";
import { PaginateResponseInterface } from "src/app/shared/interfaces/common/pagination-response.interface";
import { PaginationInterface } from "src/app/shared/modules/pagination/pagination.component";
import { InitiativeLearnContentService } from "../../services/initiative-learn-content.service";
import { InitiativeModulesEnum } from "src/app/initiatives/enums/initiative-modules.enum";
import { InitiativeSharedService } from "src/app/initiatives/shared/services/initiative-shared.service";
import { TitleService } from "src/app/core/services/title.service";

@Component({
  selector: 'neo-initiative-learn-recommended',
  templateUrl: './initiative-learn-recommended.component.html',
  styleUrls: ['./initiative-learn-recommended.component.scss']
})
export class InitiativeLearnRecommendedComponent implements OnInit, OnDestroy {
  responseData: PaginateResponseInterface<InitiativeArticleInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: -1
  };

  counter: number = 0;
  selectedTiles: any[] = [];
  postType = PostTypeEnum;
  loadArticles$: Subject<void> = new Subject<void>();
  private destroy$ = new Subject<void>();

  @Output() counterChange = new EventEmitter<number>();
  @Input() initiativeId: number;
  @Output() getData = new EventEmitter<{ id: number; numbers: number[] }>();
  @Output() pageChangeDetected = new EventEmitter<boolean>();
  @Output() updateRecommendationsCounterforArticles = new EventEmitter<number>();
  defaultPageItems = 12;
  paging: PaginationInterface = {
    take: this.defaultPageItems,
    skip: 0,
    total: null
  };


  constructor(
    private initiativeLearnContentService: InitiativeLearnContentService,
    private titleService: TitleService,
    private initiativeSharedService: InitiativeSharedService,
  ) { }

  ngOnInit(): void {
    this.loadArticles();
    this.titleService.setTitle('initiative.viewInitiative.viewAllLearnLabel');
  }

  toggleSelection(selectedTile: InitiativeArticleInterface): void {
    selectedTile.isSelected = !selectedTile.isSelected;
    this.counter = selectedTile.isSelected
      ? this.initiativeLearnContentService.pushSelectedContents(this.initiativeId, selectedTile.id)
      : this.initiativeLearnContentService.popSelectedContents(selectedTile.id);
    this.counterChange.emit(this.counter);
  }

  retainContentSelection(dataList: InitiativeArticleInterface[]): void {
    const selectedTiles = this.initiativeLearnContentService.getSelectedTiles();
    dataList.forEach(tile => {
      tile.isSelected = selectedTiles.LearnContent.some(selectedTile => selectedTile.id === tile.id);
    });
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.paging.take;
    this.loadArticles();
    this.pageChangeDetected.emit(true);
  }

  loadArticles(selectedArticleCount?: number): void {
    if (
      selectedArticleCount &&
      selectedArticleCount > 0 &&
      this.responseData.dataList.length - selectedArticleCount <= 0
    ) {
      const skipPageCount = Math.floor(selectedArticleCount / this.paging.take);
      const totalSkipCount = skipPageCount > 0 ? (skipPageCount + 1) * this.paging.take : this.paging.take;
      this.paging.skip -= totalSkipCount;
    }

    this.initiativeLearnContentService
      .getRecommendedArticles(this.initiativeId, this.paging.skip, this.paging.take, false)
      .pipe(takeUntil(this.destroy$))
      .subscribe(data => {
        this.responseData = data;
        this.updateRecommendationsCounterforArticles.emit(data.newRecommendationsCount);
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
