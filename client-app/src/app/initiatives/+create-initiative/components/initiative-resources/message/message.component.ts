import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { CreateInitiativeService } from '../../../services/create-initiative.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import {
  BaseInitiativeContentInterface,
  InitiativeMessageInterface
} from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';

@Component({
  selector: 'neo-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit {
  messagesData: PaginateResponseInterface<InitiativeMessageInterface>;
  @Input() initiativeBasicDetails: { id: number };
  @Input() activeTab: number;
  hasContentLoaded : boolean = false;
  loadMessages$: Subject<void> = new Subject<void>();
  @Output() counterChange: EventEmitter<number> = new EventEmitter<number>();
  @Output() isVisited = new EventEmitter<boolean>();
  defaultPageItems: number = 12;
  currentUser: UserInterface;
  paging: PaginationInterface = {
    take: this.defaultPageItems,
    skip: 0,
    total: null
  };

  constructor(
    private createInitiativeService: CreateInitiativeService,
    private readonly authService: AuthService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.createInitiativeService.hasContentLoaded$.next(false);
    this.isVisited.emit(true);
    this.emitAutoAttachedCounterChange();
    if (this.activeTab === InitiativeModulesEnum.Messages) {
      this.loadMessages();
    }

    this.authService.currentUser().subscribe((user: UserInterface) => {
      this.currentUser = user;
      this.cdr.detectChanges();
    });
  }

  private emitAutoAttachedCounterChange(): void {
    const autoAttachedContentType = this.createInitiativeService.autoAttached?.contentType?.toString();
    const isMessagesModule = autoAttachedContentType === InitiativeModulesEnum[InitiativeModulesEnum.Messages];
    const messageCount = isMessagesModule ? 1 : this.createInitiativeService.selectedContents.MessageContent.length;
    this.counterChange.emit(messageCount);
  } 

  toggleSelection(selectedTile: BaseInitiativeContentInterface): void {
    selectedTile.isSelected = !selectedTile.isSelected;
    if (selectedTile.isSelected) {
      this.createInitiativeService.pushSelectedContents(
        this.initiativeBasicDetails.id,
        selectedTile.isSelected,
        selectedTile.id,
        InitiativeModulesEnum.Messages
      );
    } else {
      this.createInitiativeService.popSelectedContents(selectedTile.id, InitiativeModulesEnum.Messages);
    }
    this.counterChange.emit(this.createInitiativeService.selectedContents.MessageContent.length);
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.paging.take;
    this.loadMessages();
  }

  private retainContentSelection(dataList: InitiativeMessageInterface[]): void {
    const selectedTiles = this.createInitiativeService.getSelectedTiles();
    dataList.forEach(tile => {
      tile.isSelected = selectedTiles.MessageContent.some(selectedTile => selectedTile.id === tile.id);
    });
  }

  private loadMessages(): void {
    this.createInitiativeService
      .getRecommendedMessages(this.initiativeBasicDetails.id, this.paging.skip, this.paging.take, true)
      .subscribe(data => {
        this.hasContentLoaded = true;
        if (data?.dataList?.length > 0) {
          this.messagesData = data;
          this.retainContentSelection(this.messagesData.dataList);
          this.paging = {
            ...this.paging,
            skip: this.paging.skip || 0,
            total: data.count
          };

          const autoAttachedContentType = this.createInitiativeService.autoAttached?.contentType;
          const isMessagesModule = InitiativeModulesEnum[autoAttachedContentType]?.toString() === InitiativeModulesEnum.Messages.toString();

          if (this.createInitiativeService.selectedContents && !this.createInitiativeService.autoAttached?.isAdded && isMessagesModule) {
            this.toggleSelection(this.messagesData.dataList[0]);
            this.createInitiativeService.autoAttached.isAdded = true;
          }
        }
        this.createInitiativeService.hasContentLoaded$.next(true);
        this.cdr.detectChanges();
      });
  }
}
