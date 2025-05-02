import { ChangeDetectorRef, Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { filter } from 'rxjs/operators';
import { take } from 'rxjs';

import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { CommonService } from '../../../../core/services/common.service';
import { ValidationErrors } from '@angular/forms';

@Component({
  selector: 'neo-edit-project-technologies',
  templateUrl: './edit-project-technologies.component.html',
  styleUrls: ['../../edit-project.component.scss']
})
export class EditProjectTechnologiesComponent implements OnChanges {
  @Output() changeSelectedTechnologies: EventEmitter<TagInterface[]> = new EventEmitter<TagInterface[]>();
  @Input() preSelectedTechnologies: TagInterface[];
  @Input() formSubmitted: boolean;
  @Input() selectedTypeId: number;
  @Input() technologiesError: ValidationErrors | null;
  technologiesList: TagInterface[];
  defaultCategoriesList: TagInterface[];

  constructor(private commonService: CommonService, private changeDetRef: ChangeDetectorRef) {}

  ngOnChanges(changes: SimpleChanges) {
    if (!changes['selectedTypeId']?.previousValue && changes['selectedTypeId']?.currentValue) {
      this.commonService
        .initialData()
        .pipe(
          filter(v => !!v),
          take(1)
        )
        .subscribe(data => {
          this.defaultCategoriesList = data.categories;

          this.technologiesList = data.categories.filter(c => {
            return c.id === this.selectedTypeId;
          })[0]?.technologies;

          this.technologiesList = this.technologiesList.map(t => Object.assign({}, t));

          this.technologiesList?.forEach(tl => {
            this.preSelectedTechnologies.forEach(preSelT => {
              if (tl.id === preSelT.id) tl.selected = true;
            });
          });
        });
    }
    if (
      changes['selectedTypeId']?.previousValue &&
      changes['selectedTypeId']?.currentValue !== changes['selectedTypeId']?.previousValue
    ) {
      this.updateProjectTechnologiesList();
    }
  }

  selectTechnology(technology: TagInterface): void {
    this.technologiesList.forEach(c => {
      if (c.id === technology.id) {
        c.selected = !c.selected;
        this.changeDetRef.markForCheck();
      }
    });

    // if (!this.technologiesList.filter(t => t.selected)) {
    //   this.technologiesError = null;
    // } else this.technologiesError = { required: true };
    this.changeSelectedTechnologies.emit(this.technologiesList.filter(t => t.selected));
  }

  updateProjectTechnologiesList(): void {
    this.technologiesList = this.defaultCategoriesList?.filter(dt => {
      return dt.id === this.selectedTypeId;
    })[0]['technologies'];
  }
}
