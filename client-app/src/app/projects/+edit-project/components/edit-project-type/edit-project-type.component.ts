import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { filter } from 'rxjs/operators';

import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { CommonService } from '../../../../core/services/common.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'neo-edit-project-type',
  templateUrl: './edit-project-type.component.html',
  styleUrls: ['../../edit-project.component.scss']
})
export class EditProjectTypeComponent implements OnChanges {
  @Output() changeTypeId: EventEmitter<number> = new EventEmitter<number>();
  @Input() selectedTypeId: number;
  selectedType: TagInterface;
  typesList: TagInterface[];

  constructor(private commonService: CommonService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.selectedTypeId.currentValue && !changes.selectedTypeId.previousValue) {
      this.commonService
        .initialData()
        .pipe(
          filter(v => !!v),
          untilDestroyed(this)
        )
        .subscribe(data => {
          this.selectedType = data.categories.filter(d => d.id === this.selectedTypeId)[0];
          this.typesList = data.categories;

          if (this.selectedType) {
            this.typesList.forEach(t => {
              if (t.id === this.selectedType.id) t.selected = true;
            });
          }
        });
    }
  }
}
