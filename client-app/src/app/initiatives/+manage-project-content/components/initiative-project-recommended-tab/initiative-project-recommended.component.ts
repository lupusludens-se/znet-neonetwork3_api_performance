import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs';
import { PostTypeEnum } from 'src/app/core/enums/post-type.enum';
import { InitiativeProjectInterface } from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { InitiativeProjectService } from '../../services/initiative-project.service';
import { takeUntil } from 'rxjs/operators';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { TitleService } from 'src/app/core/services/title.service';
@Component({
  selector: 'neo-initiative-project-recommended',
  templateUrl: './initiative-project-recommended.component.html',
  styleUrls: ['./initiative-project-recommended.component.scss']
})
export class InitiativeProjectRecommendedComponent implements OnInit, OnDestroy {
  responseData: PaginateResponseInterface<InitiativeProjectInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: -1
  };

  counter: number = 0;
  projectIds: number[];
  selectedTiles: any[] = [];
  postType = PostTypeEnum;
  loadProjects$: Subject<void> = new Subject<void>();
  @Output() selectedItemsCounter = new EventEmitter<number>();
  @Input() initiativeId: number;
  defaultPageItems: number = 12;
  paging: PaginationInterface = {
    take: this.defaultPageItems,
    skip: 0,
    total: null
  };
  @Output() pageChangeDetected: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() updateRecommendationsCounterforProjects = new EventEmitter<number>();
  private destroy$ = new Subject<void>();
  constructor(private initiativeProjectService: InitiativeProjectService,
    private titleService: TitleService,
    private initiativeSharedService: InitiativeSharedService,
  ) { }

  ngOnInit(): void {
    //currentPageNumber$ holds the current page number. When the page changes, the changePage method is called to update the value of currentPageNumber$.
    //Whenever this changes loadProjects() is called.
    this.loadProjects();
    this.titleService.setTitle('initiative.viewInitiative.viewAllProjectsLabel');
  }

  toggleSelection(selectedTile) {
    selectedTile.isSelected = !selectedTile.isSelected;
    this.counter = selectedTile.isSelected
      ? this.initiativeProjectService.pushSelectedContents(this.initiativeId, selectedTile.id)
      : this.initiativeProjectService.popSelectedContents(selectedTile.id);
    this.selectedItemsCounter.emit(this.counter);
  }

  // Method to retain selections of contents while navigating between pages of a content.
  retainContentSelection(dataList: InitiativeProjectInterface[]) {
    var selectedTiles = this.initiativeProjectService.getSelectedTiles();
    dataList.forEach(tile => {
      tile.isSelected = selectedTiles.ProjectContent.some(selectedTile => selectedTile.id === tile.id);
    });
  }
  //Method updates the value of currentPageNumber$
  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.paging.take;
    this.loadProjects();
    this.pageChangeDetected.emit(true);
  }
  //Method to get contents for each page
  loadProjects(selectedProjectCount?: number) {
    if (
      selectedProjectCount != undefined &&
      selectedProjectCount > 0 &&
      this.responseData.dataList.length - selectedProjectCount <= 0
    ) {
      const skipPageCount = Math.floor(selectedProjectCount / this.paging.take);
      const totalSkipCount = skipPageCount > 0 ? (skipPageCount + 1) * this.paging.take : this.paging.take;
      this.paging.skip = Math.max(this.paging.skip - totalSkipCount, 0);
    }
    this.initiativeProjectService
      .getRecommendedProjects(this.initiativeId, this.paging.skip, this.paging.take, false)
      .subscribe(data => {
        this.responseData = data;
        this.updateRecommendationsCounterforProjects.emit(data.newRecommendationsCount);
        this.retainContentSelection(this.responseData.dataList);
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip ? this.paging?.skip : 0,
          total: data.count
        };
      });
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
