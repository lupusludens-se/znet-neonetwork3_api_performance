import { ControlContainer, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { filter } from 'rxjs/operators';
import { take } from 'rxjs';

import { ProjectInterface } from '../../../../shared/interfaces/projects/project.interface';
import { ProjectStatusEnum } from '../../../../shared/enums/projects/project-status.enum';
import { ProjectApiRoutes } from '../../../shared/constants/project-api-routes.const';
import { ProjectSchema } from '../../../shared/constants/project-schema-const';
import { UserInterface } from '../../../../shared/interfaces/user/user.interface';
import { CustomValidator } from '../../../../shared/validators/custom.validator';
import { PermissionTypeEnum } from '../../../../core/enums/permission-type.enum';
import { PermissionService } from '../../../../core/services/permission.service';
import { RegionsService } from '../../../../shared/services/regions.service';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { ProjectTypesSteps } from '../../enums/project-types-name.enum';
import { AddProjectService } from '../../services/add-project.service';
import { AuthService } from '../../../../core/services/auth.service';
import { CoreService } from 'src/app/core/services/core.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'neo-project-overview',
  templateUrl: './project-overview.component.html',
  styleUrls: ['../../add-project.component.scss', './project-overview.component.scss']
})
export class ProjectOverviewComponent implements OnInit {
  stepsList = AddProjectStepsEnum;
  projectTypes = ProjectTypesSteps;
  apiRoutes = ProjectApiRoutes;
  form: FormGroup = this.formBuilder.group({
    title: ['', [CustomValidator.required, Validators.maxLength(100)]],
    subTitle: ['', [CustomValidator.required, Validators.maxLength(200)]],
    opportunity: ['', [CustomValidator.required, Validators.maxLength(ProjectSchema.opportunityMaxLength)]],
    description: ['', [CustomValidator.required, Validators.maxLength(ProjectSchema.descriptionMaxLength)]]
  });
  showDraftModal: boolean;
  publishedByUser: UserInterface;
  permissionTypes = PermissionTypeEnum;
  permissionToAddPublisher: boolean;
  formSubmitted: boolean;

  descriptionLength: number = 0;
  opportunityLength: number = 0;

  descriptionMaxLength = ProjectSchema.descriptionMaxLength;
  descriptionTextMaxLength = ProjectSchema.descriptionTextMaxLength;
  opportunityTextMaxLength = ProjectSchema.opportunityTextMaxLength;
  opportunityMaxLength = ProjectSchema.opportunityMaxLength;
  subTitleMaxLength = ProjectSchema.subTitleMaxLength;
  titleMaxLength = ProjectSchema.titleMaxLength;
  defaultDescriptionLength: number = 0;

  constructor(
    public addProjectService: AddProjectService,
    public coreService: CoreService,
    public controlContainer: ControlContainer,
    public regionsService: RegionsService,
    private formBuilder: FormBuilder,
    public permissionService: PermissionService,
    public authService: AuthService,
    private snackbarService: SnackbarService,
    private translateService: TranslateService
  ) {}

  ngOnInit() {
    this.authService
      .currentUser()
      .pipe(
        filter(v => !!v),
        take(1)
      )
      .subscribe(user => {
        this.permissionToAddPublisher = this.permissionService.userHasPermission(
          user,
          this.permissionTypes.ProjectsManageAll
        );

        if (this.addProjectService.currentFlowData?.project?.description) {
          this.form.patchValue({
            description: this.addProjectService.currentFlowData?.project.description
          });
        } else {
          this.form.patchValue({
            description: user.company.about
          });
        }

        this.descriptionLength = this.coreService.convertToPlain(this.form.controls['description'].value ?? '').length;
      });

    if (this.addProjectService.currentFlowData?.project) {
      const formVal: Partial<ProjectInterface> = this.addProjectService.currentFlowData?.project;

      this.form.patchValue({
        title: formVal.title,
        subTitle: formVal.subTitle,
        opportunity: formVal.opportunity
      });

      this.opportunityLength =
        this.coreService.convertToPlain(this.form.controls['opportunity'].value ?? '')?.length ?? 0;

      this.publishedByUser = this.addProjectService.currentFlowData?.project.publishedBy;
    }
  }

  update(v): void {
    this.form.controls['opportunity'].patchValue(v);
    this.form.controls['opportunity'].updateValueAndValidity();
  }

  goBack(): void {
    this.addProjectService.updateProjectGeneralData(this.form.value);
    const projectType = this.controlContainer.control.get('categoryId').value;

    switch (projectType.slug) {
      case ProjectTypesSteps.AggregatedPpa:
        this.addProjectService.changeStep(this.stepsList.OffsitePpaPrivate);
        break;
      case ProjectTypesSteps.OffsitePpa:
        this.addProjectService.changeStep(this.stepsList.OffsitePpaPrivate);
        break;
      case ProjectTypesSteps.BatteryStorage:
        this.addProjectService.changeStep(this.stepsList.BatteryStorage);
        break;
      case ProjectTypesSteps.CarbonOffset:
        this.addProjectService.changeStep(this.stepsList.CarbonOffset);
        break;
      case ProjectTypesSteps.CommunitySolar:
        this.addProjectService.changeStep(this.stepsList.CommunitySolar);
        break;
      case ProjectTypesSteps.EacPurchasing:
        this.addProjectService.changeStep(this.stepsList.EacPurchasing);
        break;
      case ProjectTypesSteps.EfficiencyAudit:
        this.addProjectService.changeStep(this.stepsList.EfficiencyAudit);
        break;
      case ProjectTypesSteps.EfficiencyEquipmentMeasures:
        this.addProjectService.changeStep(this.stepsList.EfficiencyEquipmentMeasures);
        break;
      case ProjectTypesSteps.EmergingTechnologies:
        this.addProjectService.changeStep(this.stepsList.EmergingTechnologies);
        break;
      case ProjectTypesSteps.EvCharging:
        this.addProjectService.changeStep(this.stepsList.EvCharging);
        break;
      case ProjectTypesSteps.FuelCells:
        this.addProjectService.changeStep(this.stepsList.FuelCells);
        break;
      case ProjectTypesSteps.OnsiteSolar:
        this.addProjectService.changeStep(this.stepsList.OnsiteSolar);
        break;
      case ProjectTypesSteps.RenewableRetail:
        this.addProjectService.changeStep(this.stepsList.RenewableRetail);
        break;
      case ProjectTypesSteps.UtilityGreenTariff:
        this.addProjectService.changeStep(this.stepsList.UtilityGreenTariff);
        break;
    }
  }

  proceed(projectStatus?: ProjectStatusEnum) {
    this.formSubmitted = true;
    if (
      this.descriptionLength > this.descriptionTextMaxLength ||
      this.opportunityLength > this.opportunityTextMaxLength
    ) {
      return;
    }
    if (this.form.controls['description'].value.length > this.descriptionMaxLength) {
      this.snackbarService.showError(
        this.translateService.instant('projects.addProject.providerFormattingMaxLengthError')
      );
      return;
    }
    if (this.form.controls['opportunity'].value.length > this.opportunityMaxLength) {
      this.snackbarService.showError(
        this.translateService.instant('projects.addProject.opportunityFormattingMaxLengthError')
      );
      return;
    }
    if (
      !this.form.valid ||
      !this.form.controls['description'].value ||
      this.form.controls['description'].value === '<br>' || // !! temp solution
      !this.form.controls['opportunity'].value ||
      this.form.controls['opportunity'].value === '<br>'
    ) {
      return;
    }

    this.addProjectService.updateProjectGeneralData(this.form.value);
    const projectTypeName: string = this.controlContainer.control.get('categoryId').value.slug;
    const projId = this.addProjectService.currentFlowData.project.id
      ? `/${this.addProjectService.currentFlowData.project.id}`
      : '';
    let routeName: string;

    switch (projectTypeName) {
      case ProjectTypesSteps.BatteryStorage:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.batteryStorage;
        break;
      case ProjectTypesSteps.FuelCells:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.fuelCells;
        break;
      case ProjectTypesSteps.CarbonOffset:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.carbonOffset;
        break;
      case ProjectTypesSteps.CommunitySolar:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.communitySolar;
        break;
      case ProjectTypesSteps.EacPurchasing:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.eacPurchasing;
        break;
      case ProjectTypesSteps.EfficiencyAudit:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.efficiencyAuditAndConsulting;
        break;
      case ProjectTypesSteps.EfficiencyEquipmentMeasures:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.efficiencyEquipmentMeasures;
        break;
      case ProjectTypesSteps.EmergingTechnologies:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.emergingTechnologies;
        break;
      case ProjectTypesSteps.EvCharging:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.evCharging;
        break;
      case ProjectTypesSteps.OnsiteSolar:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.onsiteSolar;
        break;
      case ProjectTypesSteps.RenewableRetail:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.renewableRetail;
        break;
      case ProjectTypesSteps.UtilityGreenTariff:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.greenTariff;
        break;
      case ProjectTypesSteps.OffsitePpa:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.offsitePpa;
        break;
      case ProjectTypesSteps.AggregatedPpa:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.offsitePpa;
        break;
    }

    this.addProjectService.saveProject(routeName, projectStatus);
  }

  chooseUser(user: UserInterface) {
    this.publishedByUser = user;
    if (this.form.controls['description'].value != user.company?.about) {
      this.form.patchValue({
        description: user.company?.about
      });
      this.descriptionLength =
        this.coreService.convertToPlain(user.company?.about).length ?? this.defaultDescriptionLength;
    }
    this.addProjectService.updateProjectGeneralData({ publishedBy: user });
  }

  clearSearch() {
    this.publishedByUser = null;
    this.addProjectService.updateProjectGeneralData({ publishedBy: null });
  }

  generateUserDisplayName(publishedByUser: UserInterface): string {
    return publishedByUser ? `${publishedByUser.firstName} ${publishedByUser.lastName}` : '';
  }

  onDescriptionLengthChanged(value: number) {
    this.descriptionLength = value;
  }

  onOpportunityLengthChanged(value: number) {
    this.opportunityLength = value;
  }
}
