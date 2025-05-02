import { Component, EventEmitter, Input, Output } from '@angular/core';

import { TableConfigurationInterface } from '../../../../interfaces/table/table-configuration.interface';
import { MENU_OPTIONS, MenuOptionInterface } from '../../../menu/interfaces/menu-option.interface';
import { StatusEnum } from '../../enums/status.enum';

@Component({
  selector: 'neo-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class TableComponent {
  @Input() configurations: TableConfigurationInterface<unknown>;
  @Input() menuOptions: MenuOptionInterface[] = MENU_OPTIONS;
  @Input() currentOrdering: Record<string, string> = {
    propertyName: null,
    direction: null
  };

  @Output() cellClick: EventEmitter<{
    property: string;
    dataItem: unknown;
  }> = new EventEmitter<{
    property: string;
    dataItem: unknown;
  }>();
  @Output() rowClick: EventEmitter<unknown> = new EventEmitter<unknown>();
  @Output() optionClick: EventEmitter<{
    option: MenuOptionInterface;
    dataItem: Record<string, string | number | boolean>;
  }> = new EventEmitter<{
    option: MenuOptionInterface;
    dataItem: Record<string, string | number | boolean>;
  }>();

  @Output() orderChange: EventEmitter<Record<string, string>> = new EventEmitter<Record<string, string>>();

  statusEnum = StatusEnum;

  getOptions(dataItem: Record<string, string | boolean>) {
    this.menuOptions = this.menuOptions.map(option => {
      if (dataItem?.isActive) {
        if (option.name === 'actions.deactivateLabel') option.hidden = false;
        if (option.name === 'actions.activateLabel') option.hidden = true;
        if (option.name === 'actions.previewLabel') option.hidden = true;
      } else {
        if (option.name === 'actions.deactivateLabel') option.hidden = true;
        if (option.name === 'actions.activateLabel') option.hidden = false;
        if (option.name === 'actions.previewLabel') option.hidden = true;
      }

      return option;
    });

    return this.menuOptions;
  }

  setSortingPosition(propertyName: string): void {
    if (this.currentOrdering.propertyName !== propertyName) {
      this.currentOrdering.direction = 'asc';
    } else {
      this.currentOrdering.direction = this.currentOrdering.direction === 'asc' ? 'desc' : 'asc';
    }

    this.currentOrdering.propertyName = propertyName;

    this.orderChange.emit(this.currentOrdering);
  }
}
