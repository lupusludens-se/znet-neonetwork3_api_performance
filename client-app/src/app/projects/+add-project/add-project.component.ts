import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { filter, Subject, takeUntil } from 'rxjs';

import { InitialDataInterface } from '../../core/interfaces/initial-data.interface';
import { CountryInterface } from 'src/app/shared/interfaces/user/country.interface';
import { AddProjectStepsEnum } from './enums/add-project-steps.enum';
import { AddProjectService } from './services/add-project.service';
import { CommonService } from '../../core/services/common.service';
import { TitleService } from 'src/app/core/services/title.service';
import structuredClone from '@ungap/structured-clone';
import { ViewportScroller } from '@angular/common';

@Component({
  selector: 'neo-add-project',
  templateUrl: './add-project.component.html',
  styleUrls: ['./add-project.component.scss']
})
export class AddProjectComponent implements OnInit, OnDestroy {
  stepsList = AddProjectStepsEnum;
  continents: CountryInterface[];
  projectName: string;
  form: FormGroup = this.formBuilder.group({
    categoryId: [null, Validators.required],
    technologies: [null, Validators.required],
    regions: [null, Validators.required]
  });

  private initialData: InitialDataInterface;
  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    public addProjectService: AddProjectService,
    private formBuilder: FormBuilder,
    private commonService: CommonService,
    private titleService: TitleService,
    private viewPort: ViewportScroller
  ) {}

  ngOnInit(): void {
    this.commonService
      .initialData()
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(response => !!response)
      )
      .subscribe(initialData => {
        this.initialData = initialData;
        this.continents = structuredClone(initialData.regions.filter(r => r.parentId === 0));
      });
    this.addProjectService.currentStep$.pipe(takeUntil(this.unsubscribe$)).subscribe(() => {
      this.viewPort.scrollToPosition([0, 0]);
    });

    this.titleService.setTitle('projects.addProject.addProjectTitleLabel');
  }

  setProjectType(projectName: string): void {
    this.projectName = projectName;
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    this.addProjectService.changeStep(AddProjectStepsEnum.ProjectType);
    this.addProjectService.currentFlowData = {
      project: null,
      projectDetails: null
    };

    this.form.reset();

    this.initialData.categories.map(item => item.technologies?.forEach(i => (i.selected = false)));

    this.commonService.initialData$.next(this.initialData);
  }
}
