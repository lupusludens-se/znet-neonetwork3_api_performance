import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ControlContainer } from '@angular/forms';
import { filter } from 'rxjs/operators';
import { combineLatest } from 'rxjs';

import { CompanyInterface } from '../../../../shared/interfaces/user/company.interface';
import { CompanyApiEnum } from '../../../../shared/enums/api/company-api-enum';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { CommonService } from '../../../../core/services/common.service';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { AddProjectService } from '../../services/add-project.service';
import { AuthService } from '../../../../core/services/auth.service';
import { HttpService } from '../../../../core/services/http.service';

@UntilDestroy()
@Component({
  selector: 'neo-project-type',
  templateUrl: './project-type.component.html',
  styleUrls: ['../../add-project.component.scss']
})
export class ProjectTypeComponent implements OnInit {
  stepsList = AddProjectStepsEnum;
  typesList: TagInterface[];
  selectedType: TagInterface;
  allCategoriesList: TagInterface[];
  @Output() chosenProjectType: EventEmitter<TagInterface> = new EventEmitter<TagInterface>();

  constructor(
    public addProjectService: AddProjectService,
    public controlContainer: ControlContainer,
    private commonService: CommonService,
    private authService: AuthService,
    private httpService: HttpService
  ) {}

  ngOnInit(): void {
    combineLatest([this.commonService.initialData(), this.authService.currentUser()])
      .pipe(
        filter(v => !!v),
        untilDestroyed(this)
      )
      .subscribe(res => {
        this.allCategoriesList = res[0]?.categories;

        if (res[1]) {
          this.httpService
            .get<CompanyInterface>(CompanyApiEnum.Companies + `/${res[1].companyId}`, {
              expand: 'categories',
              skipActivities: true
            })
            .pipe(untilDestroyed(this))
            .subscribe(c => {
              this.selectedType = this.controlContainer.control.get('categoryId').value;

              this.typesList = c.categories;

              if (this.selectedType) {
                this.typesList.forEach(t => {
                  if (t.id === this.selectedType.id) t.selected = true;
                });
              }
              this.addProjectService.updateProjectGeneralData({
                description: this.authService.currentUser$.getValue()?.company?.about
              });
            });
        }
      });
  }

  changeStep(): void {
    if (this.controlContainer.control.get('categoryId').value.technologies.length) {
      this.addProjectService.changeStep(this.stepsList.Technologies);
    } else this.addProjectService.changeStep(this.stepsList.ProjectGeography);
  }

  selectInterest(type: TagInterface): void {
    if (this.selectedType && type.id === this.selectedType.id) {
      // * unselect
      this.selectedType = null;
      this.controlContainer.control.get('categoryId').patchValue(null);
    } else {
      this.selectedType = type;

      this.controlContainer.control.get('categoryId').patchValue({
        id: this.selectedType.id,
        slug: this.selectedType.slug,
        technologies: this.allCategoriesList.filter(c => c.id === this.selectedType.id)[0].technologies
      });

      this.controlContainer.control.get('technologies').patchValue(null);

      this.addProjectService.updateProjectGeneralData({
        categoryId: this.controlContainer.control.get('categoryId').value.id,
        technologies: null
      });
    }
    this.chosenProjectType.emit(this.selectedType);
  }
}
