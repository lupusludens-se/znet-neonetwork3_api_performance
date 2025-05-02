import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CreateInitiativeService } from '../../../services/create-initiative.service';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { PostTypeEnum } from '../../../../../core/enums/post-type.enum';
import { Subject } from 'rxjs';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import {
  BaseInitiativeContentInterface,
  InitiativeArticleInterface
} from 'src/app/initiatives/shared/models/initiative-resources.interface';

@Component({
  selector: 'neo-learn',
  templateUrl: './learn.component.html',
  styleUrls: ['./learn.component.scss']
})
export class LearnComponent implements OnInit {
  responseData: any;
  initiativeId: number;
  articleIds: number[];
  selectedTiles: any[] = [];
  readonly postType = PostTypeEnum;
  readonly loadArticles$ = new Subject<void>();
  @Output() counterChange = new EventEmitter<number>();
  @Output() isVisited = new EventEmitter<boolean>();
  @Input() initiativeBasicDetails: { id: number };
  readonly defaultPageItems = 12;
  @Input() activeTab: number;
  paging: PaginationInterface = {
    take: this.defaultPageItems,
    skip: 0,
    total: null
  };

  constructor(private readonly createInitiativeService: CreateInitiativeService) { }

  ngOnInit(): void {
    this.createInitiativeService.hasContentLoaded$.next(false);
    this.isVisited.emit(true);
    const isLearnAutoAttached = this.createInitiativeService.autoAttached?.contentType?.toString() === InitiativeModulesEnum[InitiativeModulesEnum.Learn];
    this.counterChange.emit(isLearnAutoAttached ? 1 : this.createInitiativeService.selectedContents.LearnContent.length);
    this.initiativeId = this.initiativeBasicDetails.id;
    if (this.activeTab === InitiativeModulesEnum.Learn) {
      this.loadArticles();
    }
  }

  toggleSelection(selectedTile: BaseInitiativeContentInterface): void {
    selectedTile.isSelected = !selectedTile.isSelected;

    if (selectedTile.isSelected) {
      this.createInitiativeService.pushSelectedContents(
        this.initiativeId,
        selectedTile.isSelected,
        selectedTile.id,
        InitiativeModulesEnum.Learn
      );
    } else {
      this.createInitiativeService.popSelectedContents(selectedTile.id, InitiativeModulesEnum.Learn);
    }

    this.counterChange.emit(this.createInitiativeService.selectedContents.LearnContent.length);
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.paging.take;
    this.loadArticles();
  }

  retainContentSelection(dataList: InitiativeArticleInterface[]): void {
    const selectedTiles = this.createInitiativeService.getSelectedTiles();
    dataList.forEach(tile => {
      tile.isSelected = selectedTiles.LearnContent.some(selectedTile => selectedTile.id === tile.id);
    });
  }

  private loadArticles(): void {
    this.createInitiativeService
      .getRecommendedArticles(this.initiativeId, this.paging.skip, this.paging.take, true)
      .subscribe(data => {
        this.responseData = data;
        this.retainContentSelection(this.responseData.dataList);
        this.paging = {
          ...this.paging,
          skip: this.paging.skip || 0,
          total: data.count
        };
        
        const autoAttached = this.createInitiativeService.autoAttached; 
        const autoAttachedContentType = this.createInitiativeService.autoAttached?.contentType;
        const isLearnModule = InitiativeModulesEnum[autoAttachedContentType]?.toString() === InitiativeModulesEnum.Learn.toString();
        if (this.createInitiativeService.selectedContents && !autoAttached?.isAdded && isLearnModule) {
          this.toggleSelection(this.responseData.dataList[0]);
          autoAttached.isAdded = true;
        }
        this.createInitiativeService.hasContentLoaded$.next(true);
      });
  }
}
