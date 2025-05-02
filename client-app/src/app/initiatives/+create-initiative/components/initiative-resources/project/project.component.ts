import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CreateInitiativeService } from '../../../services/create-initiative.service';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { InitiativeProjectInterface } from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';

@Component({
  selector: 'neo-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit {
  projectsData: PaginateResponseInterface<InitiativeProjectInterface>;
  initiativeId: number;
  @Output() counterChange = new EventEmitter<number>();
  @Input() initiativeBasicDetails: { id: number };
  @Input() activeTab: number;
  @Output() isVisited = new EventEmitter<boolean>();
  defaultPageItems = 12;
  paging: PaginationInterface = {
    take: this.defaultPageItems,
    skip: 0,
    total: null
  };

  constructor(private createInitiativeService: CreateInitiativeService) { }

  ngOnInit(): void {
    this.createInitiativeService.hasContentLoaded$.next(false);
    this.isVisited.emit(true);
    this.initiativeId = this.initiativeBasicDetails.id;
    this.emitCounterChange();
    if (this.activeTab === InitiativeModulesEnum.Projects) {
      this.loadProjects();
    }
  }

  private emitCounterChange(): void {
    const autoAttached = this.createInitiativeService.autoAttached;
    this.counterChange.emit(autoAttached?.contentType?.toString() === InitiativeModulesEnum[InitiativeModulesEnum.Projects] ? 1 : this.createInitiativeService.selectedContents.ProjectContent.length);
  }

  toggleSelection(selectedTile: InitiativeProjectInterface): void {
    selectedTile.isSelected = !selectedTile.isSelected;
    if (selectedTile.isSelected) {
      this.createInitiativeService.pushSelectedContents(
        this.initiativeId,
        selectedTile.isSelected,
        selectedTile.id,
        InitiativeModulesEnum.Projects
      );
    } else {
      this.createInitiativeService.popSelectedContents(selectedTile.id, InitiativeModulesEnum.Projects);
    }
    this.counterChange.emit(this.createInitiativeService.selectedContents.ProjectContent.length);
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.paging.take;
    this.loadProjects();
  }

  private retainContentSelection(dataList: InitiativeProjectInterface[]): void {
    const selectedTiles = this.createInitiativeService.getSelectedTiles();
    dataList.forEach(tile => {
      tile.isSelected = selectedTiles.ProjectContent.some(selectedTile => selectedTile.id === tile.id);
    });
  }

  private loadProjects(): void {
    this.createInitiativeService
      .getRecommendedProjects(this.initiativeId, this.paging.skip, this.paging.take, true)
      .subscribe(data => {
        this.projectsData = data;
        this.retainContentSelection(this.projectsData.dataList);
        this.paging = {
          ...this.paging,
          skip: this.paging.skip || 0,
          total: data.count
        };
        this.autoAttachFirstProject();
        this.createInitiativeService.hasContentLoaded$.next(true);
      });
  }

  private autoAttachFirstProject(): void {
    const autoAttached = this.createInitiativeService.autoAttached;
    //const isProjectsModule = autoAttached?.contentType.toString() === "Projects";   
    const autoAttachedContentType = this.createInitiativeService.autoAttached?.contentType;
    const isProjectsModule = InitiativeModulesEnum[autoAttachedContentType]?.toString() === InitiativeModulesEnum.Projects.toString();
    if (this.createInitiativeService.selectedContents && !autoAttached?.isAdded && isProjectsModule) {
      this.toggleSelection(this.projectsData.dataList[0]);
      autoAttached.isAdded = true;
    }
  }
}
