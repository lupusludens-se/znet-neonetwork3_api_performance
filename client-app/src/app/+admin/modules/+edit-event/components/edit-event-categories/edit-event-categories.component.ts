import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Component, OnInit } from '@angular/core';
import { filter, take } from 'rxjs';

import { CreateEventService } from '../../../+create-event/services/create-event.service';
import { EventCategoryInterface } from '../../interfaces/event-category.interface';
import { CommonService } from '../../../../../core/services/common.service';
import { TagInterface } from '../../../../../core/interfaces/tag.interface';
import structuredClone from '@ungap/structured-clone';
import { EventsService } from '../../../../services/events.service';

@UntilDestroy()
@Component({
  selector: 'neo-edit-event-categories',
  templateUrl: 'edit-event-categories.component.html',
  styleUrls: ['../../edit-event.component.scss', 'edit-event-categories.component.scss']
})
export class EditEventCategoriesComponent implements OnInit {
  categoriesList: EventCategoryInterface[];

  constructor(
    private commonService: CommonService,
    private createEventService: CreateEventService,
    private eventsService: EventsService
  ) {}

  ngOnInit(): void {
    this.eventsService.currentFormValue$
      .pipe(
        filter(v => !!v),
        take(1)
      )
      .subscribe(formVal => {
        this.commonService
          .initialData()
          .pipe(
            filter(v => !!v),
            untilDestroyed(this)
          )
          .subscribe(data => {
            this.categoriesList = structuredClone(data)?.categories || [];

            if (formVal?.invitedCategories) {
              formVal.invitedCategories.forEach(categ => {
                this.categoriesList.forEach(cl => {
                  if (categ.id === cl.id) cl.preSelected = true;
                });
              });
            }
          });
      });
  }

  addCategory(category: TagInterface[]): void {
    const selectedCategories: EventCategoryInterface[] = [
      ...category.filter(t => t.selected),
      ...this.categoriesList.filter(c => c.preSelected)
    ];

    this.eventsService.updateFormValue({
      invitedCategories: selectedCategories
    });
  }
}
