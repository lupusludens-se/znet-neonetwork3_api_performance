import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CreateInitiativeService } from '../../../services/create-initiative.service';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import {
	BaseInitiativeContentInterface,
	InitiativeToolInterface
} from 'src/app/initiatives/shared/models/initiative-resources.interface';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';

@Component({
	selector: 'neo-tools',
	templateUrl: './tools.component.html',
	styleUrls: ['./tools.component.scss']
})
export class ToolsComponent implements OnInit {
	@Input() initiativeBasicDetails!: { id: number };
	@Input() activeTab!: number;
	@Output() counterChange = new EventEmitter<number>();
	@Output() isVisited = new EventEmitter<boolean>();
	responseData: PaginateResponseInterface<InitiativeToolInterface> = {
		dataList: [],
		skip: 0,
		take: 0,
		count: -1
	};
	readonly defaultPageItems = 6;
	paging: PaginationInterface = {
		take: this.defaultPageItems,
		skip: 0,
		total: null
	};
	initiativeId!: number;

	constructor(private readonly createInitiativeService: CreateInitiativeService) { }

	ngOnInit() {
		this.isVisited.emit(true);
		this.createInitiativeService.hasContentLoaded$.next(false);
		const { autoAttached, selectedContents } = this.createInitiativeService;
		const contentType = autoAttached?.contentType?.toString();
		const toolsEnum = InitiativeModulesEnum[InitiativeModulesEnum.Tools];

		this.counterChange.emit(contentType === toolsEnum ? 1 : selectedContents.ToolContent.length);
		this.initiativeId = this.initiativeBasicDetails.id;
		if (this.activeTab === InitiativeModulesEnum.Tools) {
			this.loadTools();
		}
	}

	toggleSelection(selectedTile: BaseInitiativeContentInterface) {
		selectedTile.isSelected = !selectedTile.isSelected;
		if (selectedTile.isSelected === true) {
			this.createInitiativeService.pushSelectedContents(
				this.initiativeId,
				selectedTile.isSelected,
				selectedTile.id,
				InitiativeModulesEnum.Tools
			);
		} else if (selectedTile.isSelected === false) {
			this.createInitiativeService.popSelectedContents(selectedTile.id, InitiativeModulesEnum.Tools);
		}

		this.counterChange.emit(this.createInitiativeService.selectedContents.ToolContent.length);
	}

	changePage = (page: number): void => {
		this.paging.skip = (page - 1) * this.paging.take;
		this.loadTools();
	}

	retainContentSelection = (dataList: InitiativeToolInterface[]) => {
		const selectedTiles = this.createInitiativeService.getSelectedTiles().ToolContent;
		dataList.forEach(tile => {
			tile.isSelected = selectedTiles.some(selectedTile => selectedTile.id === tile.id);
		});
	}

	private loadTools = () => {
		const { id } = this.initiativeBasicDetails;
		const { skip, take } = this.paging;

		this.createInitiativeService
			.getRecommendedTools(id, skip, take, true)
			.subscribe(data => {
				this.responseData = data;
				this.retainContentSelection(data.dataList);
				this.paging = {
					...this.paging,
					skip: this.paging.skip || 0,
					total: data.count
				};

				
        const autoAttached = this.createInitiativeService.autoAttached; 
        const autoAttachedContentType = this.createInitiativeService.autoAttached?.contentType;
        const isToolsModule = InitiativeModulesEnum[autoAttachedContentType]?.toString() === InitiativeModulesEnum.Tools.toString();
				if (this.createInitiativeService.selectedContents !== undefined && !autoAttached?.isAdded && isToolsModule) {
					this.toggleSelection(this.responseData.dataList[0]);
					this.createInitiativeService.autoAttached.isAdded = true;
				}
				this.createInitiativeService.hasContentLoaded$.next(true);
			});

	}
}
