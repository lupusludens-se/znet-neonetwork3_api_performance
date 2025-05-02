import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { combineLatest, Subject, takeUntil } from 'rxjs';
import { ControlContainer, FormArray, FormBuilder, FormGroupDirective } from '@angular/forms';
import { Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { CommonService } from '../../../core/services/common.service';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { EventsService } from '../../services/events.service';

@Component({
  selector: 'neo-tags-step',
  templateUrl: 'tags-step.component.html',
  styleUrls: ['../+create-event/create-event.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class TagsStepComponent implements OnInit, OnDestroy {
  @Input() formSubmitted: boolean;
  initialCategoriesList: TagInterface[];
  categoriesListResult: TagInterface[];
  selectedCategories: TagInterface[] = [];
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private commonService: CommonService,
    public controlContainer: ControlContainer,
    private eventsService: EventsService,
    private changeDetRef: ChangeDetectorRef
  ) {}

  get tags(): FormArray {
    return this.controlContainer.control.get('categories') as FormArray;
  }

  ngOnInit(): void {
    combineLatest([this.commonService.initialData(), this.eventsService.currentFormValue$])
      .pipe(
        filter(v => !!v[0]),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(([initialData, currentFormValue]) => {
        this.initialCategoriesList = initialData.categories;

        if (currentFormValue?.categories) {
          currentFormValue.categories.forEach(tag => {
            const tagToAdd: TagInterface = this.initialCategoriesList.filter(t => {
              return t.id === tag.id;
            })[0];
            this.selectTag(tagToAdd);
          });

          this.changeDetRef.markForCheck();
        }
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  searchTag($event: string) {
    this.categoriesListResult = [...this.initialCategoriesList].filter(c =>
      c.name.toLowerCase().includes($event.toLowerCase())
    );
  }

  selectTag(category: TagInterface): void {
    if (!this.selectedCategories.some(c => c.id === category.id)) {
      this.selectedCategories.push(category);

      const linkForm = this.formBuilder.group({ ...category });
      this.tags.push(linkForm);
    }
  }

  removeCategory(index: number, category: TagInterface) {
    this.selectedCategories = [...this.selectedCategories.filter(c => c.id !== category.id)];

    const currentLinks = this.controlContainer.control.get('categories') as FormArray;
    currentLinks.removeAt(index);
  }
}
