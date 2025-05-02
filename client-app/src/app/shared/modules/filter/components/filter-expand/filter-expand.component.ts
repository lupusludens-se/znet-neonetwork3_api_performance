import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';

import { ExpandStateEnum } from '../../enums/expand-state.enum';

@Component({
  selector: 'neo-filter-expand',
  templateUrl: './filter-expand.component.html',
  styleUrls: ['./filter-expand.component.scss']
})
export class FilterExpandComponent implements OnInit {
  @Input() name: string;
  @Input() icon: string;
  @Input() parentControlName: string = 'control';
  @Input() type: 'checkbox' | 'radio';
  @Input() horizontalLayout: boolean;
  @Input() showIcon: boolean = true;
  @Input() options: TagInterface[];
  @Input() isActive: boolean;
  @Input() radioForm: FormGroup = new FormGroup({});

  @Output() optionsChange: EventEmitter<unknown> = new EventEmitter<unknown>();

  expandedState: ExpandStateEnum = null;

  ngOnInit() {
    this.radioForm.addControl(this.parentControlName, new FormControl(null));
  }
  changeExpandingState(): void {
    this.expandedState =
      this.expandedState === ExpandStateEnum.expanded ? ExpandStateEnum.collapsed : ExpandStateEnum.expanded;
  }

  collapseFilter(): void {
    this.expandedState = ExpandStateEnum.collapsed;
  }
}
