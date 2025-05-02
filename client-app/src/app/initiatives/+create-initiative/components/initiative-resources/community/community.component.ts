import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { CreateInitiativeService } from '../../../services/create-initiative.service';
import { CoreService } from 'src/app/core/services/core.service';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import {
  BaseInitiativeContentInterface,
  InitiativeCommunityInterface
} from 'src/app/initiatives/shared/models/initiative-resources.interface';

@Component({
  selector: 'neo-community',
  templateUrl: './community.component.html',
  styleUrls: ['./community.component.scss']
})
export class CommunityComponent implements OnInit {
  responseData: PaginateResponseInterface<InitiativeCommunityInterface>;
  initiativeId: number;
  selectedTiles: any[] = [];
  @Input() activeTab: number;
  @Output() counterChange: EventEmitter<number> = new EventEmitter<number>();
  @Input() initiativeBasicDetails: any;
  readonly defaultPageItems: number = 12;
  @Output() isVisited = new EventEmitter<boolean>();
  paging: PaginationInterface = {
    take: this.defaultPageItems,
    skip: 0,
    total: null
  };

  constructor(
    private readonly createInitiativeService: CreateInitiativeService
  ) {}

  ngOnInit(): void {

    this.createInitiativeService.hasContentLoaded$.next(false);
    this.isVisited.emit(true);
    const isCommunityAutoAttached = this.createInitiativeService.autoAttached?.contentType?.toString() === InitiativeModulesEnum[InitiativeModulesEnum.Community];

    this.counterChange.emit(isCommunityAutoAttached ? 1 : this.createInitiativeService.selectedContents.CommunityContent.length);
    this.initiativeId = this.initiativeBasicDetails.id;
    if (this.activeTab === InitiativeModulesEnum.Community) {
       this.loadCommunityUsers();
    }
    
  }

  toggleSelection(selectedTile: BaseInitiativeContentInterface): void {
    selectedTile.isSelected = !selectedTile.isSelected;

    if (selectedTile.isSelected) {
      this.createInitiativeService.pushSelectedContents(
        this.initiativeId,
        selectedTile.isSelected,
        selectedTile.id,
        InitiativeModulesEnum.Community
      );
    } else {
      this.createInitiativeService.popSelectedContents(selectedTile.id, InitiativeModulesEnum.Community);
    }

    this.counterChange.emit(this.createInitiativeService.selectedContents.CommunityContent.length);
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.paging.take;
    this.loadCommunityUsers();
  }

  retainContentSelection(dataList: InitiativeCommunityInterface[]): void {
    const selectedTiles = this.createInitiativeService.getSelectedTiles();
    dataList.forEach(tile => {
      tile.isSelected = selectedTiles.CommunityContent.some(selectedTile => selectedTile.id === tile.id);
    });
  }

  private loadCommunityUsers(): void {
    this.createInitiativeService
      .getRecommendedCommunityUsers(this.initiativeId, this.paging.skip, this.paging.take, true)
      .subscribe(data => {
        this.responseData = data;
        this.retainContentSelection(this.responseData.dataList);
        this.paging = {
          ...this.paging,
          skip: this.paging.skip || 0,
          total: data.count
        };

        const autoAttached = this.createInitiativeService.autoAttached;
        //const isCommunityModule = autoAttached?.contentType.toString() === "Community";   
        const autoAttachedContentType = this.createInitiativeService.autoAttached?.contentType;
        const isCommunityModule = InitiativeModulesEnum[autoAttachedContentType]?.toString() === InitiativeModulesEnum.Community.toString();
        if (this.createInitiativeService.selectedContents && !autoAttached?.isAdded && isCommunityModule) {
          this.toggleSelection(this.responseData.dataList[0]);
          autoAttached.isAdded = true;
        }
        this.createInitiativeService.hasContentLoaded$.next(true);
      });
  }
}
