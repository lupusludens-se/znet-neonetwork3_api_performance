import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ControlContainer } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { filter } from 'rxjs/operators';

import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { CommonService } from '../../../../core/services/common.service';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { AddProjectService } from '../../services/add-project.service';

@Component({
  selector: 'neo-project-technologies',
  templateUrl: './project-technologies.component.html',
  styleUrls: ['../../add-project.component.scss', './project-technologies.component.scss']
})
export class ProjectTechnologiesComponent implements OnInit, OnDestroy {
  stepsList = AddProjectStepsEnum;
  selectedAll: boolean;
  technologiesList: TagInterface[];
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    public addProjectService: AddProjectService,
    public controlContainer: ControlContainer,
    private commonService: CommonService,
    private changeDetRef: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.commonService
      .initialData()
      .pipe(
        filter(v => !!v),
        takeUntil(this.unsubscribe$)
      )
      .subscribe(data => {
        const selectedProjectType: number = this.controlContainer.control.get('categoryId').value.id;
        this.technologiesList = data.categories.filter(c => {
          return c.id === selectedProjectType;
        })[0].technologies;
      });
  }

  ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  changeStep(step: number): void {
    this.addProjectService.changeStep(step);
  }

  selectTechnology(technology: TagInterface): void {
    this.technologiesList.forEach(c => {
      if (c.id === technology.id) {
        c.selected = !c.selected;
        this.changeDetRef.markForCheck();
      }
    });
    this.addDataToForm();

    this.selectedAll = !this.technologiesList.some(c => c.selected === false);
  }

  selectAll(): void {
    this.technologiesList.forEach(c => {
      c.selected = !this.selectedAll;
    });
    this.addDataToForm();
    this.changeDetRef.markForCheck();

    this.selectedAll = !this.selectedAll;
  }

  addDataToForm(): void {
    const selectedCategories: TagInterface[] = this.technologiesList.filter(c => c.selected === true);
    this.controlContainer.control.get('technologies').patchValue(selectedCategories);

    this.addProjectService.updateProjectGeneralData({
      technologies: this.controlContainer.control.get('technologies').value
    });
  }
}
