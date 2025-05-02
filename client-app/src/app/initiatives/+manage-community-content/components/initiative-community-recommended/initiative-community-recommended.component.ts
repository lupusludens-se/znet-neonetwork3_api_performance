import { Component, Input, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { InitiativeCommunityInterface } from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { InitiativeCommunityContentService } from '../../services/initiative-community-content.service';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { InitiativeCommunityItemParentModuleEnum } from 'src/app/initiatives/shared/contents/community-item/community-item.component';
import { Subject, takeUntil } from 'rxjs';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { TitleService } from 'src/app/core/services/title.service';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';

@Component({
  selector: 'neo-initiative-community-recommended',
  templateUrl: './initiative-community-recommended.component.html',
  styleUrls: ['./initiative-community-recommended.component.scss']
})
export class InitiativeCommunityRecommendedComponent implements OnInit, OnDestroy {
  @Input() initiativeId!: number;
  @Output() selectedItemsCounter = new EventEmitter<number>();
  @Output() getData = new EventEmitter<{ id: number; numbers: number[] }>();
  @Output() pageChangeDetected = new EventEmitter<boolean>();
  initiativeParentModuleEnum = InitiativeCommunityItemParentModuleEnum;

  responseData: PaginateResponseInterface<InitiativeCommunityInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: -1
  };
  counter = 0;
  selectedTiles: InitiativeCommunityInterface[] = [];
  defaultPageItems = 12;
  paging: PaginationInterface = {
    take: this.defaultPageItems,
    skip: 0,
    total: null
  };
  private destroy$ = new Subject<void>();
  @Output() updateRecommendationsCounterforCommunity = new EventEmitter<number>();
  constructor(private initiativeCommunityContentService: InitiativeCommunityContentService,
    private initiativeSharedService: InitiativeSharedService,
    private titleService: TitleService
  ) { }

  ngOnInit(): void {
    this.loadCommunityUsers();
    this.titleService.setTitle('initiative.viewInitiative.viewAllCommunityLabel');

  }

  toggleSelection(selectedTile: InitiativeCommunityInterface): void {
    selectedTile.isSelected = !selectedTile.isSelected;
    this.counter = selectedTile.isSelected
      ? (this.counter = this.initiativeCommunityContentService.pushSelectedContents(this.initiativeId, selectedTile.id))
      : (this.counter = this.initiativeCommunityContentService.popSelectedContents(selectedTile.id));
    this.selectedItemsCounter.emit(this.counter);
  }

  retainContentSelection(dataList: InitiativeCommunityInterface[]): void {
    const selectedTiles = this.initiativeCommunityContentService.getSelectedTiles();
    dataList.forEach(tile => {
      tile.isSelected = selectedTiles.CommunityContent.some(selectedTile => selectedTile.id === tile.id);
    });
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.paging.take;
    this.loadCommunityUsers();
    this.pageChangeDetected.emit(true);
  }

  loadCommunityUsers(selectedDiscussionCount?: number): void {
    if (selectedDiscussionCount && this.responseData.dataList.length - selectedDiscussionCount <= 0) {
      this.adjustPagingForSelectedCount(selectedDiscussionCount);
    }

    this.initiativeCommunityContentService
      .getRecommendedCommunityUsers(this.initiativeId, this.paging.skip, this.paging.take, false)
      .subscribe(data => {
        this.responseData = data;
        this.updateRecommendationsCounterforCommunity.emit(data.newRecommendationsCount);
        this.retainContentSelection(this.responseData.dataList);
        this.paging = {
          ...this.paging,
          skip: this.paging.skip ?? 0,
          total: data.count
        };
      });
  }

  private adjustPagingForSelectedCount(selectedDiscussionCount: number): void {
    const skipPageCount = Math.floor(selectedDiscussionCount / this.paging.take);
    const totalSkipCount = skipPageCount > 0 ? (skipPageCount + 1) * this.paging.take : this.paging.take;
    this.paging.skip = Math.max(this.paging.skip - totalSkipCount, 0);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
