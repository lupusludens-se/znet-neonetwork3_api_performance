import { Subject, catchError, combineLatest, filter, merge, takeUntil, throwError } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';

import { AddProjectPayloadInterface } from '../+add-project/services/add-project.service';
import { ProjectInterface } from '../../shared/interfaces/projects/project.interface';
import { ProjectStatusEnum } from 'src/app/shared/enums/projects/project-status.enum';
import { ProjectTypesSteps } from '../+add-project/enums/project-types-name.enum';
import { ProjectApiRoutes } from '../shared/constants/project-api-routes.const';
import { ProjectSchema } from '../shared/constants/project-schema-const';
import { TagInterface } from '../../core/interfaces/tag.interface';
import { CountryInterface } from 'src/app/shared/interfaces/user/country.interface';
import { HttpService } from '../../core/services/http.service';
import {
  BatteryStorageDetailsFromControls,
  CarbonOffsetDetailsFromControls,
  CommunitySolarDetailsFromControls,
  EacPurchasingDetailsFromControls,
  EfficiencyAuditDetailsFromControls,
  EfficiencyMeasuresDetailsFromControls,
  EmergingTechnologiesDetailsFromControls,
  EvChargingDetailsFromControls,
  FuelCellsDetailsFromControls,
  GreenTariffDetailsFromControls,
  OffsitePpaDetailsFromControls,
  OnsiteSolarDetailsFromControls,
  RenewableElectricityDetailsFromControls
} from './constants/details-forms.constant';
import { PermissionService } from '../../core/services/permission.service';
import { AuthService } from '../../core/services/auth.service';
import { PermissionTypeEnum } from '../../core/enums/permission-type.enum';
import { UserInterface } from '../../shared/interfaces/user/user.interface';
import { UserProfileApiEnum } from '../../shared/enums/api/user-profile-api.enum';
import { TitleService } from 'src/app/core/services/title.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CoreService } from 'src/app/core/services/core.service';
import { CustomValidator } from 'src/app/shared/validators/custom.validator';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonService } from 'src/app/core/services/common.service';
import structuredClone from '@ungap/structured-clone';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { TranslateService } from '@ngx-translate/core';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';

@UntilDestroy()
@Component({
  selector: 'neo-edit-project',
  templateUrl: './edit-project.component.html',
  styleUrls: ['./edit-project.component.scss']
})
export class EditProjectComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    title: ['', [Validators.required, Validators.maxLength(100)]],
    subTitle: ['', [Validators.required, Validators.maxLength(200)]],
    opportunity: ['', [Validators.required, Validators.maxLength(ProjectSchema.opportunityMaxLength)]],
    description: ['', [Validators.required, Validators.maxLength(ProjectSchema.descriptionMaxLength)]]
  });
  projectDetailsForm: FormGroup = this.formBuilder.group({});
  sectionsList = {
    showOverview: false,
    showProjectType: true,
    showTechnologies: false,
    showRegions: false,
    showProjectDetails: false,
    showProjectPrivateDetails: false
  };
  apiRoutes = { ...ProjectApiRoutes, ...UserProfileApiEnum };
  project: ProjectInterface;
  projectTypes = ProjectTypesSteps;
  projectStatuses = ProjectStatusEnum;
  permissionTypes = PermissionTypeEnum;
  publishedByUser: UserInterface;
  formSubmitted: boolean;
  draftFormSubmitted: boolean;
  technologiesError: ValidationErrors | null;
  upsideShareError: ValidationErrors | null;
  deletedOwnerError: ValidationErrors | null;
  continents: CountryInterface[];
  isCallFromProjectDetailsPage: boolean = false;

  descriptionLength: number = 0;
  opportunityLength: number = 0;

  regionsValidationError: boolean = false;
  projectDetailsValidationError: boolean = false;
  projectDescritionValidationError: boolean = false;

  descriptionMaxLength = ProjectSchema.descriptionMaxLength;
  opportunityMaxLength = ProjectSchema.opportunityMaxLength;
  subTitleMaxLength = ProjectSchema.subTitleMaxLength;
  titleMaxLength = ProjectSchema.titleMaxLength;
  defaultDescriptionLength: number = 0;
  descriptionTextMaxLength = ProjectSchema.descriptionTextMaxLength;
  opportunityTextMaxLength = ProjectSchema.opportunityTextMaxLength;
  currentUser: UserInterface;

  private unsubscribe$: Subject<void> = new Subject<void>();
  constructor(
    public coreService: CoreService,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private httpService: HttpService,
    private commonService: CommonService,
    private router: Router,
    public permissionService: PermissionService,
    private titleService: TitleService,
    public authService: AuthService,
    private changeDetRef: ChangeDetectorRef,
    private snackbarService: SnackbarService,
    private translateService: TranslateService
  ) {
    this.isCallFromProjectDetailsPage = this.router.getCurrentNavigation().extras.state?.isCallFromProjectDetailsPage ?? false;
  }

  ngOnInit(): void {
    this.commonService
      .initialData()
      .pipe(
        untilDestroyed(this),
        filter(response => !!response)
      )
      .subscribe(initialData => {
        this.continents = structuredClone(initialData.regions.filter(r => r.parentId === 0));
      });
    const projectId: string = this.activatedRoute.snapshot.params.id;

    combineLatest([
      this.httpService.get<ProjectInterface>(this.apiRoutes.projectsList + `/${projectId}`, {
        expand: 'owner,projectDetails,regions,technologies,category.technologies'
      }),
      this.authService.currentUser()
    ])
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            this.router.navigate(['/projects-library']);
            this.coreService.elementNotFoundData$.next({
              iconKey: 'projects',
              mainTextTranslate: 'projects.notFoundText',
              buttonTextTranslate: 'projects.notFoundButton',
              buttonLink: '/projects-library'
            });
          }

          return throwError(error);
        })
      )
      .subscribe(([proj, currentUser]) => {
        if (currentUser != null) { 
          if (
            !this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.AdminAll) &&
            !(
              this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.ProjectsManageOwn) &&
              currentUser.id === proj.ownerId
            )
          ) {
            if (
              !(
                this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.ManageCompanyProjects) &&
                currentUser.companyId === proj.companyId
              )
            ) {
              this.router.navigate(['403']);
            }
          }

          this.currentUser = currentUser;
          this.project = proj;

          this.form.patchValue({
            title: proj.title,
            subTitle: proj.subTitle,
            opportunity: proj.opportunity,
            description: proj.description
          });

          this.opportunityLength =
            this.coreService.convertToPlain(this.form.controls['opportunity'].value ?? '')?.length ?? 0;
          this.descriptionLength = this.coreService.convertToPlain(
            this.form.controls['description'].value ?? ''
          ).length;

          if (proj.owner) {
            this.publishedByUser = proj.owner;
          }

         

          let detailsForm;

          switch (this.project.category.slug) {
            case ProjectTypesSteps.BatteryStorage:
              detailsForm = BatteryStorageDetailsFromControls;
              break;
            case ProjectTypesSteps.FuelCells:
              detailsForm = FuelCellsDetailsFromControls;
              break;
            case ProjectTypesSteps.CarbonOffset:
              detailsForm = CarbonOffsetDetailsFromControls;
              break;
            case ProjectTypesSteps.CommunitySolar:
              detailsForm = CommunitySolarDetailsFromControls;
              break;
            case ProjectTypesSteps.EacPurchasing:
              detailsForm = EacPurchasingDetailsFromControls;
              break;
            case ProjectTypesSteps.EfficiencyAudit:
              detailsForm = EfficiencyAuditDetailsFromControls;
              break;
            case ProjectTypesSteps.EfficiencyEquipmentMeasures:
              detailsForm = EfficiencyMeasuresDetailsFromControls;
              break;
            case ProjectTypesSteps.EmergingTechnologies:
              detailsForm = EmergingTechnologiesDetailsFromControls;
              break;
            case ProjectTypesSteps.EvCharging:
              detailsForm = EvChargingDetailsFromControls;
              break;
            case ProjectTypesSteps.OnsiteSolar:
              detailsForm = OnsiteSolarDetailsFromControls;
              break;
            case ProjectTypesSteps.RenewableRetail:
              detailsForm = RenewableElectricityDetailsFromControls;
              break;
            case ProjectTypesSteps.UtilityGreenTariff:
              detailsForm = GreenTariffDetailsFromControls;
              break;
            case ProjectTypesSteps.OffsitePpa:
              detailsForm = OffsitePpaDetailsFromControls;
              break;
            case ProjectTypesSteps.AggregatedPpa:
              detailsForm = OffsitePpaDetailsFromControls;
              break;
          }

          this.projectDetailsForm = this.formBuilder.group({
            ...detailsForm
          });
        }
      });

    this.titleService.setTitle('projects.editProject.editProjectLabel');
  }

  updateRegions(regions: TagInterface[]) {
    this.project.regions = regions;
  }

  publishProject(draftStatus?: ProjectStatusEnum): void {
    if (
      this.projectDetailsForm.controls['pricingStructureId']?.value === 2 &&
      +this.projectDetailsForm.controls['upsidePercentageToDeveloper']?.value +
      +this.projectDetailsForm.controls['upsidePercentageToOfftaker']?.value !==
      100
    ) {
      this.upsideShareError = { total: 100 };

      merge(
        this.projectDetailsForm.controls['upsidePercentageToDeveloper'].valueChanges,
        this.projectDetailsForm.controls['upsidePercentageToOfftaker'].valueChanges
      )
        .pipe(untilDestroyed(this))
        .subscribe(() => {
          this.upsideShareError = null;
        });
    }

    switch (this.project.category.slug) {
      case ProjectTypesSteps.BatteryStorage:
        this.updateBatteryStorageValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.FuelCells:
        this.updateFuelCellsValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.CarbonOffset:
        this.updateCarbonOffsetValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.CommunitySolar:
        this.updateCommunitySolarValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.EacPurchasing:
        this.updateEacPurchasingValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.EfficiencyAudit:
        this.updateEfficiencyAuditValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.EfficiencyEquipmentMeasures:
        this.updateEfficiencyMeasuresValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.EmergingTechnologies:
        this.updateEmergingTechnologiesValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.EvCharging:
        this.updateEvChargingValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.OnsiteSolar:
        this.updateOnsiteSolarValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.RenewableRetail:
        this.updateRenewableElectricityValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.UtilityGreenTariff:
        this.updateGreenTariffValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.OffsitePpa:
        this.updateOffsitePpaValidators(draftStatus === this.projectStatuses.Draft);
        break;
      case ProjectTypesSteps.AggregatedPpa:
        this.updateOffsitePpaValidators(draftStatus === this.projectStatuses.Draft);
        break;
    }

    if (draftStatus) this.draftFormSubmitted = true;
    if (draftStatus && this.projectDetailsForm.invalid) {
      this.projectDetailsValidationError = true;
      return;
    }

    if (
      this.project.owner?.statusId === UserStatusEnum.Deleted &&
      this.publishedByUser?.id === this.project.owner?.id
    ) {
      this.formSubmitted = true;
      this.deletedOwnerError = { deletedUser: true };
    }

    if (!draftStatus) {
      if (this.project.category.technologies.length && this.project.technologies.length) {
        this.technologiesError = null;
      } else if (this.project.category.technologies.length && !this.project.technologies.length) {
        this.technologiesError = { required: true };
      }

      this.changeDetRef.detectChanges();
      this.formSubmitted = true;
    } else {
      this.technologiesError = null;
    }

    this.regionsValidationError = !this.project.regions?.length && !draftStatus;
    this.projectDetailsValidationError = this.projectDetailsForm.invalid && !draftStatus;
    this.projectDescritionValidationError =
      (this.form.invalid ||
        !this.form.controls['description'].value ||
        this.form.controls['description'].value === '<br>' ||
        !this.form.controls['opportunity'].value ||
        this.form.controls['opportunity'].value === '<br>') &&
      !draftStatus;

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
      (!draftStatus &&
        (this.projectDetailsValidationError ||
          this.projectDescritionValidationError ||
          this.regionsValidationError ||
          this.technologiesError)) ||
      this.upsideShareError || // !! temp solution
      this.deletedOwnerError
    )
      return;
    const payload: AddProjectPayloadInterface = {
      projectDetails: {
        ...this.projectDetailsForm.getRawValue(),
        statusId:
          draftStatus ??
          (this.project.statusId === ProjectStatusEnum.Draft ? ProjectStatusEnum.Active : this.project.statusId)
      },
      project: {
        ...this.form.value,
        statusId:
          draftStatus ??
          (this.project.statusId === ProjectStatusEnum.Draft ? ProjectStatusEnum.Active : this.project.statusId),
        regions: this.project.regions,
        technologies: this.project.technologies,
        categoryId: this.project.categoryId,
        ownerId: this.publishedByUser?.id,
        companyId: this.getCompanyId(),
        descriptionText: this.coreService.convertToPlain(this.form.controls['description'].value ?? ''),
        opportunityText: this.coreService.convertToPlain(this.form.controls['opportunity'].value ?? '')
      }
    };

    let routeName: string;

    switch (this.project.category.slug) {
      case ProjectTypesSteps.BatteryStorage:
        routeName = this.apiRoutes.batteryStorage;
        break;
      case ProjectTypesSteps.FuelCells:
        routeName = this.apiRoutes.fuelCells;
        break;
      case ProjectTypesSteps.CarbonOffset:
        routeName = this.apiRoutes.carbonOffset;
        break;
      case ProjectTypesSteps.CommunitySolar:
        routeName = this.apiRoutes.communitySolar;
        break;
      case ProjectTypesSteps.EacPurchasing:
        routeName = this.apiRoutes.eacPurchasing;
        break;
      case ProjectTypesSteps.EfficiencyAudit:
        routeName = this.apiRoutes.efficiencyAuditAndConsulting;
        break;
      case ProjectTypesSteps.EfficiencyEquipmentMeasures:
        routeName = this.apiRoutes.efficiencyEquipmentMeasures;
        break;
      case ProjectTypesSteps.EmergingTechnologies:
        routeName = this.apiRoutes.emergingTechnologies;
        break;
      case ProjectTypesSteps.EvCharging:
        routeName = this.apiRoutes.evCharging;
        break;
      case ProjectTypesSteps.OnsiteSolar:
        routeName = this.apiRoutes.onsiteSolar;
        break;
      case ProjectTypesSteps.RenewableRetail:
        routeName = this.apiRoutes.renewableRetail;
        break;
      case ProjectTypesSteps.UtilityGreenTariff:
        routeName = this.apiRoutes.greenTariff;
        break;
      case ProjectTypesSteps.OffsitePpa:
        routeName = this.apiRoutes.offsitePpa;
        break;
      case ProjectTypesSteps.AggregatedPpa:
        routeName = this.apiRoutes.offsitePpa;
        break;
    }




    this.httpService.put(this.apiRoutes.projectsList + `/${this.project.id}` + routeName, payload).
      pipe(
        takeUntil(this.unsubscribe$),
        catchError(error => {

          let errorMsg = "";
          const errors = Object.values(error?.error?.errors);
          errors?.forEach(error => {
            errorMsg += error + '\n';
          });

          this.snackbarService.showError(errorMsg);
          return throwError(error);
        })
      )
      .subscribe(() => {
        this.snackbarService.showSuccess(this.project.id == 0 ? 'projects.addProject.successLabel' : 'projects.editProject.successLabel');
        let redirectionUrl = 'projects-library';
        if (this.project.id > 0) {
          redirectionUrl = this.isCallFromProjectDetailsPage ? `projects/${this.project.id}` : 'projects-library';
        }
        this.router.navigate([redirectionUrl]);
      });
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  private getCompanyId(): number {
    if (this.currentUser?.roles.findIndex(x => x.id == RolesEnum.SPAdmin) > -1 && !this.publishedByUser?.companyId) {
      return this.currentUser.companyId;
    }
    return this.publishedByUser?.companyId;
  }

  changeStatus($event: number) {
    this.project.statusId = $event;
  }

  toggleSection(sectionName: string) {
    for (let s in this.sectionsList) {
      if (s === sectionName) this.sectionsList[s] = !this.sectionsList[s];
    }

    this.changeDetRef.detectChanges();
    this.form.controls['subTitle'].updateValueAndValidity();
  }

  goBack() {
    this.router.navigate(['projects-library']);
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
    this.deletedOwnerError = null;
  }

  clearSearch() {
    this.publishedByUser = null;
    this.deletedOwnerError = null;
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

  private updateBatteryStorageValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['minimumAnnualPeakKW'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['contractStructures'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].removeValidators(Validators.required);
    } else {
      this.projectDetailsForm.controls['minimumAnnualPeakKW'].addValidators(Validators.required);
      this.projectDetailsForm.controls['contractStructures'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].addValidators(Validators.required);
    }

    this.projectDetailsForm.controls['minimumAnnualPeakKW'].updateValueAndValidity();
    this.projectDetailsForm.controls['contractStructures'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesProvided'].updateValueAndValidity();
  }

  private updateCarbonOffsetValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['minimumPurchaseVolume'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['stripLengths'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].removeValidators(Validators.required);
    } else {
      this.projectDetailsForm.controls['minimumPurchaseVolume'].addValidators(Validators.required);
      this.projectDetailsForm.controls['stripLengths'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].addValidators(Validators.required);
    }

    this.projectDetailsForm.controls['minimumPurchaseVolume'].updateValueAndValidity();
    this.projectDetailsForm.controls['stripLengths'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesProvided'].updateValueAndValidity();
  }

  private updateCommunitySolarValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['minimumAnnualMWh'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['totalAnnualMWh'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['utilityTerritory'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['projectAvailable'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['isInvestmentGradeCreditOfOfftakerRequired'].removeValidators(
        Validators.required
      );
      this.projectDetailsForm.controls['contractStructures'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].removeValidators(Validators.required);

      if (
        this.projectDetailsForm.controls['projectAvailable'].value &&
        this.projectDetailsForm.controls['projectAvailable'].value === false
      ) {
        this.projectDetailsForm.patchValue({ projectAvailabilityApproximateDate: null });
        this.projectDetailsForm.controls['projectAvailabilityApproximateDate']?.clearValidators();
        this.projectDetailsForm.controls['projectAvailabilityApproximateDate']?.updateValueAndValidity();
      }
    } else {
      this.projectDetailsForm.controls['minimumAnnualMWh'].addValidators(Validators.required);
      this.projectDetailsForm.controls['totalAnnualMWh'].addValidators(Validators.required);
      this.projectDetailsForm.controls['utilityTerritory'].addValidators(Validators.required);
      this.projectDetailsForm.controls['isInvestmentGradeCreditOfOfftakerRequired'].addValidators(Validators.required);
      this.projectDetailsForm.controls['contractStructures'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].addValidators(Validators.required);

      if (!this.projectDetailsForm.controls['projectAvailable'].value) {
        this.projectDetailsForm.controls['projectAvailable'].removeValidators(Validators.required);
        this.projectDetailsForm.controls['projectAvailabilityApproximateDate']?.addValidators(Validators.required);
      }

      if (this.projectDetailsForm.controls['projectAvailable'].value) {
        this.projectDetailsForm.controls['projectAvailable'].removeValidators(Validators.required);
        this.projectDetailsForm.controls['projectAvailabilityApproximateDate']?.removeValidators(Validators.required);
      }
    }

    this.projectDetailsForm.controls['minimumAnnualMWh'].updateValueAndValidity();
    this.projectDetailsForm.controls['totalAnnualMWh'].updateValueAndValidity();
    this.projectDetailsForm.controls['utilityTerritory'].updateValueAndValidity();
    this.projectDetailsForm.controls['isInvestmentGradeCreditOfOfftakerRequired'].updateValueAndValidity();
    this.projectDetailsForm.controls['contractStructures'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesProvided'].updateValueAndValidity();
    this.projectDetailsForm.controls['projectAvailable']?.updateValueAndValidity();
    this.projectDetailsForm.controls['projectAvailabilityApproximateDate']?.updateValueAndValidity();
  }

  private updateEacPurchasingValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['minimumPurchaseVolume'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['stripLengths'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].removeValidators(Validators.required);
    } else {
      this.projectDetailsForm.controls['minimumPurchaseVolume'].addValidators(Validators.required);
      this.projectDetailsForm.controls['stripLengths'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].addValidators(Validators.required);
    }

    this.projectDetailsForm.controls['minimumPurchaseVolume'].updateValueAndValidity();
    this.projectDetailsForm.controls['stripLengths'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesProvided'].updateValueAndValidity();
  }

  private updateEfficiencyAuditValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['contractStructures'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['isInvestmentGradeCreditOfOfftakerRequired'].removeValidators(
        Validators.required
      );
      this.projectDetailsForm.controls['valuesProvided'].removeValidators(Validators.required);
    } else {
      this.projectDetailsForm.controls['contractStructures'].addValidators(Validators.required);
      this.projectDetailsForm.controls['isInvestmentGradeCreditOfOfftakerRequired'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].addValidators(Validators.required);
    }

    this.projectDetailsForm.controls['contractStructures'].updateValueAndValidity();
    this.projectDetailsForm.controls['isInvestmentGradeCreditOfOfftakerRequired'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesProvided'].updateValueAndValidity();
  }

  private updateEfficiencyMeasuresValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['contractStructures'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['isInvestmentGradeCreditOfOfftakerRequired'].removeValidators(
        Validators.required
      );
      this.projectDetailsForm.controls['valuesProvided'].removeValidators(Validators.required);
    } else {
      this.projectDetailsForm.controls['contractStructures'].addValidators(Validators.required);
      this.projectDetailsForm.controls['isInvestmentGradeCreditOfOfftakerRequired'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].addValidators(Validators.required);
    }

    this.projectDetailsForm.controls['contractStructures'].updateValueAndValidity();
    this.projectDetailsForm.controls['isInvestmentGradeCreditOfOfftakerRequired'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesProvided'].updateValueAndValidity();
  }

  private updateEmergingTechnologiesValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['contractStructures'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['minimumAnnualValue'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].removeValidators(Validators.required);
    } else {
      this.projectDetailsForm.controls['contractStructures'].addValidators(Validators.required);
      this.projectDetailsForm.controls['minimumAnnualValue'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].addValidators(Validators.required);
    }

    this.projectDetailsForm.controls['contractStructures'].updateValueAndValidity();
    this.projectDetailsForm.controls['minimumAnnualValue'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesProvided'].updateValueAndValidity();
  }

  private updateEvChargingValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['contractStructures'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['minimumChargingStationsRequired'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].removeValidators(Validators.required);
    } else {
      this.projectDetailsForm.controls['contractStructures'].addValidators(Validators.required);
      this.projectDetailsForm.controls['minimumChargingStationsRequired'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].addValidators(Validators.required);
    }

    this.projectDetailsForm.controls['contractStructures'].updateValueAndValidity();
    this.projectDetailsForm.controls['minimumChargingStationsRequired'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesProvided'].updateValueAndValidity();
  }

  private updateFuelCellsValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['minimumAnnualSiteKWh'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['contractStructures'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].removeValidators(Validators.required);
    } else {
      this.projectDetailsForm.controls['minimumAnnualSiteKWh'].addValidators(Validators.required);
      this.projectDetailsForm.controls['contractStructures'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].addValidators(Validators.required);
    }

    this.projectDetailsForm.controls['minimumAnnualSiteKWh'].updateValueAndValidity();
    this.projectDetailsForm.controls['contractStructures'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesProvided'].updateValueAndValidity();
  }

  private updateGreenTariffValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['utilityName'].removeValidators(CustomValidator.required);
      this.projectDetailsForm.controls['programWebsite'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['minimumPurchaseVolume'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['termLengthId'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].removeValidators(Validators.required);
    } else {
      this.projectDetailsForm.controls['utilityName'].addValidators(CustomValidator.required);
      this.projectDetailsForm.controls['programWebsite'].addValidators([Validators.required, CustomValidator.url]);
      this.projectDetailsForm.controls['minimumPurchaseVolume'].addValidators(Validators.required);
      this.projectDetailsForm.controls['termLengthId'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].addValidators(Validators.required);
    }

    this.projectDetailsForm.controls['utilityName'].updateValueAndValidity();
    this.projectDetailsForm.controls['programWebsite'].updateValueAndValidity();
    this.projectDetailsForm.controls['minimumPurchaseVolume'].updateValueAndValidity();
    this.projectDetailsForm.controls['termLengthId'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesProvided'].updateValueAndValidity();
  }

  private updateOnsiteSolarValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['minimumAnnualSiteKWh'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['contractStructures'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].removeValidators(Validators.required);
    } else {
      this.projectDetailsForm.controls['minimumAnnualSiteKWh'].addValidators(Validators.required);
      this.projectDetailsForm.controls['contractStructures'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].addValidators(Validators.required);
    }

    this.projectDetailsForm.controls['minimumAnnualSiteKWh'].updateValueAndValidity();
    this.projectDetailsForm.controls['contractStructures'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesProvided'].updateValueAndValidity();
  }

  private updateRenewableElectricityValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['minimumAnnualSiteKWh'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].removeValidators(Validators.required);
    } else {
      this.projectDetailsForm.controls['minimumAnnualSiteKWh'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesProvided'].addValidators(Validators.required);
    }

    this.projectDetailsForm.controls['minimumAnnualSiteKWh'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesProvided'].updateValueAndValidity();
  }

  private updateOffsitePpaValidators(draft: boolean) {
    if (draft) {
      this.projectDetailsForm.controls['productTypeId'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['latitude'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['longitude'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['productType'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['commercialOperationDate'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['valuesToOfftakers'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['ppaTermYearsLength'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['totalProjectNameplateMWACCapacity'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['totalProjectExpectedAnnualMWhProductionP50'].removeValidators(
        Validators.required
      );
      this.projectDetailsForm.controls['minimumOfftakeMWhVolumeRequired'].removeValidators(Validators.required);
      //* PRIVATE
      this.projectDetailsForm.controls['forAllPriceEntriesCurrencyId'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['pricingStructureId'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['eacId'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['settlementPriceIntervalId'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['settlementCalculationIntervalId'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['forAllPriceEntriesCurrency'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['contractPricePerMWh'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['pricingStructure'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['eac'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['eacCustom']?.removeValidators(Validators.required);
      this.projectDetailsForm.controls['settlementPriceInterval'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['settlementPriceIntervalCustom']?.removeValidators(Validators.required);
      this.projectDetailsForm.controls['settlementCalculationInterval'].removeValidators(Validators.required);
      this.projectDetailsForm.controls['projectMWCurrentlyAvailable'].removeValidators(Validators.required);
    } else {
      this.projectDetailsForm.controls['productTypeId'].addValidators(Validators.required);
      this.projectDetailsForm.controls['latitude'].addValidators(Validators.required);
      this.projectDetailsForm.controls['longitude'].addValidators(Validators.required);
      this.projectDetailsForm.controls['productType'].addValidators(Validators.required);
      this.projectDetailsForm.controls['commercialOperationDate'].addValidators(Validators.required);
      this.projectDetailsForm.controls['valuesToOfftakers'].addValidators(Validators.required);
      this.projectDetailsForm.controls['ppaTermYearsLength'].addValidators(Validators.required);
      this.projectDetailsForm.controls['totalProjectNameplateMWACCapacity'].addValidators(Validators.required);
      this.projectDetailsForm.controls['totalProjectExpectedAnnualMWhProductionP50'].addValidators(Validators.required);
      this.projectDetailsForm.controls['minimumOfftakeMWhVolumeRequired'].addValidators(Validators.required);
      //* PRIVATE
      this.projectDetailsForm.controls['forAllPriceEntriesCurrencyId'].addValidators(Validators.required);
      this.projectDetailsForm.controls['pricingStructureId'].addValidators(Validators.required);
      this.projectDetailsForm.controls['eacId'].addValidators(Validators.required);
      this.projectDetailsForm.controls['settlementPriceIntervalId'].addValidators(Validators.required);
      this.projectDetailsForm.controls['settlementCalculationIntervalId'].addValidators(Validators.required);
      this.projectDetailsForm.controls['forAllPriceEntriesCurrency'].addValidators(Validators.required);
      this.projectDetailsForm.controls['contractPricePerMWh'].addValidators([
        Validators.required,
        CustomValidator.floatNumber
      ]);
      this.projectDetailsForm.controls['pricingStructure'].addValidators(Validators.required);
      this.projectDetailsForm.controls['eac'].addValidators(Validators.required);
      this.projectDetailsForm.controls['settlementPriceInterval'].addValidators(Validators.required);
      this.projectDetailsForm.controls['settlementCalculationInterval'].addValidators(Validators.required);
      this.projectDetailsForm.controls['projectMWCurrentlyAvailable'].addValidators(Validators.required);
    }

    this.projectDetailsForm.controls['productTypeId'].updateValueAndValidity();
    this.projectDetailsForm.controls['latitude'].updateValueAndValidity();
    this.projectDetailsForm.controls['longitude'].updateValueAndValidity();
    this.projectDetailsForm.controls['productType'].updateValueAndValidity();
    this.projectDetailsForm.controls['commercialOperationDate'].updateValueAndValidity();
    this.projectDetailsForm.controls['valuesToOfftakers'].updateValueAndValidity();
    this.projectDetailsForm.controls['ppaTermYearsLength'].updateValueAndValidity();
    this.projectDetailsForm.controls['totalProjectNameplateMWACCapacity'].updateValueAndValidity();
    this.projectDetailsForm.controls['totalProjectExpectedAnnualMWhProductionP50'].updateValueAndValidity();
    this.projectDetailsForm.controls['minimumOfftakeMWhVolumeRequired'].updateValueAndValidity();
    this.projectDetailsForm.controls['forAllPriceEntriesCurrencyId'].updateValueAndValidity();
    this.projectDetailsForm.controls['pricingStructureId'].updateValueAndValidity();
    this.projectDetailsForm.controls['settlementPriceIntervalId'].updateValueAndValidity();
    this.projectDetailsForm.controls['settlementCalculationIntervalId'].updateValueAndValidity();
    this.projectDetailsForm.controls['forAllPriceEntriesCurrency'].updateValueAndValidity();
    this.projectDetailsForm.controls['contractPricePerMWh'].updateValueAndValidity();
    this.projectDetailsForm.controls['pricingStructure'].updateValueAndValidity();
    this.projectDetailsForm.controls['eac'].updateValueAndValidity();
    this.projectDetailsForm.controls['eacCustom']?.updateValueAndValidity();
    this.projectDetailsForm.controls['settlementPriceInterval'].updateValueAndValidity();
    this.projectDetailsForm.controls['settlementPriceIntervalCustom']?.updateValueAndValidity();
    this.projectDetailsForm.controls['settlementCalculationInterval'].updateValueAndValidity();
    this.projectDetailsForm.controls['projectMWCurrentlyAvailable'].updateValueAndValidity();
  }

  private checkIfToShow(categoryslug: string) {
    return categoryslug !== ProjectTypesSteps.EfficiencyAudit && categoryslug !== ProjectTypesSteps.CarbonOffset;
  }
}
