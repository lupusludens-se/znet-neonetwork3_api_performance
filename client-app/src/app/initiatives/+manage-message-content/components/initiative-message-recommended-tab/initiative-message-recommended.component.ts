import { Component, OnInit, Output, Input, EventEmitter } from "@angular/core";
import { Subject } from "rxjs";
import { InitiativeMessageInterface } from "src/app/initiatives/shared/models/initiative-resources.interface";
import { PaginateResponseInterface } from "src/app/shared/interfaces/common/pagination-response.interface";
import { PaginationInterface } from "src/app/shared/modules/pagination/pagination.component";
import { InitiativeMessageContentService } from "../../services/initiative-message-content.service";
import { untilDestroyed } from "@ngneat/until-destroy";
import { AuthService } from "src/app/core/services/auth.service";
import { UserInterface } from "src/app/shared/interfaces/user/user.interface";
import { TitleService } from "src/app/core/services/title.service";

@Component({
  selector: 'neo-initiative-message-recommended',
  templateUrl: './initiative-message-recommended.component.html',
  styleUrls: ['./initiative-message-recommended.component.scss']
})
export class InitiativeMessageRecommendedComponent implements OnInit {
  @Input() initiativeId!: number;
  @Output() selectedItemsCounter = new EventEmitter<number>();
  @Output() getData = new EventEmitter<{ id: number; numbers: number[] }>();
  @Output() pageChangeDetected = new EventEmitter<boolean>();

  responseData: PaginateResponseInterface<InitiativeMessageInterface> = {
    dataList: [],
    skip: 0,
    take: 0,
    count: -1
  };
  counter = 0;
  selectedTiles: InitiativeMessageInterface[] = [];
  loadDiscussions$ = new Subject<void>();
  defaultPageItems = 12;
  paging: PaginationInterface = {
    take: this.defaultPageItems,
    skip: 0,
    total: null
  };
  currentUser: UserInterface;

  constructor(private initiativeMessageContentService: InitiativeMessageContentService,
    private authService: AuthService,
    private titleService: TitleService) { }

  ngOnInit(): void {
    this.authService.currentUser().subscribe((user: UserInterface) => {
      if (user) {
        this.currentUser = user;
        this.loadDiscussions();
        this.titleService.setTitle('initiative.viewInitiative.viewAllMessagesLabel');
      }
    });
  }
  toggleSelection(selectedTile: InitiativeMessageInterface): void {
    selectedTile.isSelected = !selectedTile.isSelected;
    this.counter = selectedTile.isSelected
      ? this.initiativeMessageContentService.pushSelectedContents(this.initiativeId, selectedTile.id)
      : this.initiativeMessageContentService.popSelectedContents(selectedTile.id);
    this.selectedItemsCounter.emit(this.counter);
  }

  retainContentSelection(dataList: InitiativeMessageInterface[]): void {
    const selectedTiles = this.initiativeMessageContentService.getSelectedTiles();
    dataList.forEach(tile => {
      tile.isSelected = selectedTiles.MessageContent.some(selectedTile => selectedTile.id === tile.id);
    });
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.paging.take;
    this.loadDiscussions();
    this.pageChangeDetected.emit(true);
  }

  loadDiscussions(selectedDiscussionCount?: number): void {
    if (selectedDiscussionCount && this.responseData.dataList.length - selectedDiscussionCount <= 0) {
      this.adjustPagingForSelectedCount(selectedDiscussionCount);
    }

    this.initiativeMessageContentService
      .getRecommendedMessages(this.initiativeId, this.paging.skip, this.paging.take, false)
      .subscribe(data => {
        this.responseData = data;
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

}
