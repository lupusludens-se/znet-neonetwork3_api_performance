import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';

import { combineLatest, Observable, Subject, switchMap, take, takeUntil, throwError } from 'rxjs';
import { catchError, filter, map } from 'rxjs/operators';

import { HttpService } from '../../../../../core/services/http.service';
import { TitleService } from '../../../../../core/services/title.service';
import { SnackbarService } from '../../../../../core/services/snackbar.service';
import { ToolInterface } from '../../../../../shared/interfaces/tool.interface';

import { CompanyInterface } from '../../../../../shared/interfaces/user/company.interface';
import { UserRoleInterface } from '../../../../../shared/interfaces/user/user-role.interface';
import { PaginateResponseInterface } from '../../../../../shared/interfaces/common/pagination-response.interface';

import { ToolsApiEnum } from '../../../../../shared/enums/api/tools-api.enum';
import { CommonApiEnum } from '../../../../../core/enums/common-api.enum';
import { FormControlStatusEnum } from '../../../../../shared/enums/form-control-status.enum';
import { BlobTypeEnum } from '../../../../../shared/enums/api/general-api.enum';
import { CustomValidator } from '../../../../../shared/validators/custom.validator';
import { MAX_IMAGE_SIZE } from '../../../../../shared/constants/image-size.const';
import { CompanyStatusEnum } from '../../../../../shared/enums/company-status.enum';
import { INT32_MAX } from 'src/app/shared/constants/math.const';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { ImageUploadService } from 'src/app/shared/services/image-upload.service';

@Component({
  selector: 'neo-tools-manipulation',
  templateUrl: './tools-manipulation.component.html',
  styleUrls: ['./tools-manipulation.component.scss']
})
export class ToolsManipulationComponent implements OnInit, OnDestroy {
  isAdd: boolean;
  imageLarge: boolean;
  disableCompanies: boolean;
  enableRoles: boolean;

  formData: ToolInterface;
  formControlStatus = FormControlStatusEnum;

  companyIds: number[];

  titleApiError: string;
  titleMaxLength: number = 80;
  descriptionMaxLength: number = 400;
  showClearButton: boolean;
  maxImageSize = MAX_IMAGE_SIZE;

  // TODO: create ToolsService and wire up all requests at that place
  roles$: Observable<UserRoleInterface[]> = this.httpService.get<UserRoleInterface[]>(CommonApiEnum.Roles).pipe(
    map(roles => {
      const allRoleIndex = roles.findIndex(role => role.name === 'All');
      const allRole = roles[allRoleIndex];

      roles.splice(allRoleIndex, 1);
      roles.splice(0, 0, allRole);
      roles.splice(roles.findIndex(role => role.name === RolesEnum.SPAdmin.toString()));

      return roles;
    })
  );

  allCompanies: CompanyInterface = {
    id: -1,
    name: 'All'
  } as CompanyInterface;

  companies: CompanyInterface[] = [];

  toolFormGroup: FormGroup = new FormGroup({
    title: new FormControl(null, [Validators.required, Validators.maxLength(this.titleMaxLength)]),
    toolHeight: new FormControl(null, [Validators.required, Validators.max(INT32_MAX)]),
    description: new FormControl(null, [
      Validators.required,
      CustomValidator.preventWhitespacesOnly,
      Validators.maxLength(this.descriptionMaxLength)
    ]),
    toolUrl: new FormControl(null, [Validators.required, CustomValidator.url]),
    roleIds: new FormArray([]),
    companyIds: new FormArray([]),
    iconName: new FormControl(null, Validators.required),
    isActive: new FormControl(true)
  });

  private unsubscribe$: Subject<void> = new Subject<void>();
  private companiesDuplicateList: CompanyInterface[] = [];

  roleCompanyIncorrect: boolean;

  @ViewChild('fileInput') fileInput: ElementRef;

  constructor(
    private readonly httpService: HttpService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly snackbarService: SnackbarService,
    private readonly router: Router,
    private readonly titleService: TitleService,
    private readonly imageUploadService: ImageUploadService
  ) {}

  get selectedCompanies(): CompanyInterface[] {
    const selectedIds = this.toolFormGroup.get('companyIds').value;

    if (this.companiesDuplicateList?.length && !!selectedIds) {
      return this.companiesDuplicateList?.filter(company => selectedIds?.some(id => id === company.id));
    }

    return [];
  }

  ngOnInit(): void {
    this.isAdd = this.activatedRoute.snapshot.params?.id === undefined;
    this.titleService.setTitle('toolsManagement.addToolLabel');
    this.listenForActivatedRoute();

    if (this.isAdd) {
      this.getCompanies();
    }
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  saveData(): void {
    this.toolFormGroup;

    if (
      this.toolFormGroup.status === FormControlStatusEnum.Invalid ||
      (!this.getFormArray('companyIds')?.length && !this.getFormArray('roleIds')?.length)
    ) {
      Object.keys(this.toolFormGroup.value).map(key => {
        this.toolFormGroup.controls[key].markAsDirty();
        this.toolFormGroup.controls[key].markAsTouched();
      });

      this.roleCompanyIncorrect = !this.getFormArray('companyIds')?.length && !this.getFormArray('roleIds')?.length;

      return;
    }

    if (this.hasToolHeightRangeError()) {
      return;
    }

    this.roleCompanyIncorrect = false;

    this.isAdd ? this.addTool() : this.saveChanges();
  }

  cancelClick(): void {
    this.router.navigate(['./admin/tool-management']);
  }

  setRole(role: UserRoleInterface): void {
    const roleFormArray = this.getFormArray('roleIds');

    if (role.name === 'All') {
      this.enableRoles = !this.enableRoles;

      if (this.enableRoles) {
        this.toolFormGroup.controls.roleIds = new FormArray([]);
        (<FormArray>this.toolFormGroup.controls.roleIds).push(new FormControl(role.id));
      }
    }

    if (roleFormArray?.length) {
      const roleIndex = roleFormArray?.controls.findIndex(item => item.value === role.id);

      if (roleIndex >= 0) {
        roleFormArray.removeAt(roleIndex);
      } else {
        roleFormArray.push(new FormControl(role.id));
      }
    } else {
      roleFormArray.push(new FormControl(role.id));
    }

    this.roleCompanyIncorrect = !this.getFormArray('companyIds')?.length && !this.getFormArray('roleIds')?.length;
  }

  setCompany(company: CompanyInterface) {
    let companyFormArray = this.getFormArray('companyIds');

    if (company.id === -1) {
      this.disableCompanies = !this.disableCompanies;

      if (this.disableCompanies) {
        this.toolFormGroup.controls.companyIds = new FormArray([]);
        (<FormArray>this.toolFormGroup.controls.companyIds).push(new FormControl(company.id));
      }
    }

    if (companyFormArray?.length) {
      const companyIndex = companyFormArray?.controls.findIndex(item => item.value === company.id);

      if (companyIndex >= 0) {
        companyFormArray.removeAt(companyIndex);
      } else {
        companyFormArray.push(new FormControl(company.id));
      }
    } else {
      companyFormArray.push(new FormControl(company.id));
    }

    this.toolFormGroup.updateValueAndValidity();
    this.roleCompanyIncorrect = !this.getFormArray('companyIds')?.length && !this.getFormArray('roleIds')?.length;
  }

  getFormArray(property: string): FormArray {
    return this.toolFormGroup.get(property) as FormArray;
  }

  findCompanies(search: string): void {
    search ? this.filterCompanies(search) : this.getCompanies();
  }

  clearSelectedCompanies(): void {
    this.getFormArray('companyIds').clear();
    this.formData.companies = [];
    this.disableCompanies = false;
  }

  onFileSelect(event): void {
    const notSupported: boolean = event.target.files[0].type.includes('image');
    if (!notSupported) {
      this.snackbarService.showError('general.wrongFileLabel');
      return;
    }

    if (event.target.files.length > 0) {
      const file: File = event.target.files[0];
      const isLarge = file.size > this.maxImageSize;

      if (isLarge) {
        this.imageLarge = isLarge;
        this.snackbarService.showError('general.wrongMediumFileSizeLabel');
        return;
      }

      const formData: FormData = new FormData();
      formData.append('file', file);

      this.imageUploadService
        .uploadImage(BlobTypeEnum.Tools, formData)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(image => {
          this.toolFormGroup.get('iconName').patchValue(image.name);

          if (this.formData) {
            this.formData.icon = image;
          } else {
            this.formData = {
              companies: [],
              iconName: '',
              id: 0,
              isActive: false,
              isPinned: false,
              roles: [],
              title: '',
              toolUrl: '',
              icon: image,
              toolHeight: 0
            };
          }
        });
    }
  }

  removeIcon(): void {
    this.toolFormGroup.get('iconName').patchValue(null);
    this.formData.icon = null;
    this.fileInput.nativeElement.value = null;
  }

  hasError(controlName: string): boolean {
    const control = this.toolFormGroup?.get(controlName);

    return control?.invalid && control?.touched && control?.dirty;
  }

  hasToolHeightRangeError(): boolean {
    const control = this.toolFormGroup?.get('toolHeight');
    return (control.value < 400 || control.value > 1500) && !(control.value == '' || control.value == undefined);
  }

  private getCompanies(): void {
    this.httpService
      .get(CommonApiEnum.Companies, {
        filterBy: `statusids=${CompanyStatusEnum.Active}`,
        orderBy: 'name.asc'
      })
      .pipe(take(1))
      .subscribe((companies: PaginateResponseInterface<CompanyInterface>) => {
        this.setCompaniesList(companies);
      });
  }

  private setCompaniesList(companies: PaginateResponseInterface<CompanyInterface>): void {
    this.companiesDuplicateList = [];
    this.companiesDuplicateList.push(Object.assign({}, this.allCompanies));
    companies?.dataList?.map(company => this.companiesDuplicateList.push(Object.assign({}, company)));

    this.companies = [this.allCompanies, ...companies.dataList];
  }

  private filterCompanies(search: string): void {
    this.companies = this.companiesDuplicateList.filter(company =>
      company.name.toLowerCase().includes(search.toLowerCase())
    );
  }

  private listenForActivatedRoute(): void {
    this.activatedRoute.params
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(params => params?.id),
        switchMap(params =>
          combineLatest([
            this.httpService.get<ToolInterface>(`${ToolsApiEnum.Tools}/${params.id}`, {
              expand: 'companies,roles,icon'
            }),
            this.httpService.get(CommonApiEnum.Companies, {
              filterBy: `statusids=${CompanyStatusEnum.Active}`,
              orderBy: 'name.asc'
            })
          ])
        )
      )
      .subscribe(([response, companies]: [ToolInterface, PaginateResponseInterface<CompanyInterface>]) => {
        this.formData = response;
        this.setCompaniesList(companies);
        this.companyIds = this.formData.companies.map(fc => fc.id);
        this.titleService.setTitle('toolsManagement.editToolLabel', this.formData.title);
        this.patchData();
      });
  }

  private patchData(): void {
    this.toolFormGroup.patchValue({
      title: this.formData?.title,
      description: this.formData?.description,
      toolUrl: this.formData?.toolUrl,
      iconName: this.formData?.iconName,
      roleIds: this.formData.roles.map(role => role.id),
      isActive: this.formData.isActive,
      toolHeight: this.formData.toolHeight
    });

    if (this.formData?.roles?.length) {
      const roles = this.getFormArray('roleIds');

      this.formData?.roles.map(role => {
        roles.push(new FormControl(role.id));
      });
    }

    if (this.formData?.companies?.length) {
      const companyIds = this.getFormArray('companyIds');

      if (
        this.formData.companies.some(c => c.id === -1) ||
        this.companies.filter(c => c.id !== -1).every(c => this.companyIds.includes(c.id))
      ) {
        this.formData.companies = [this.allCompanies];
      }

      this.formData?.companies.map(company => {
        companyIds.push(new FormControl(company.id));
      });

      if (this.formData.companies.some(company => company.id === -1)) {
        this.disableCompanies = true;
      }
    }

    if (this.formData.roles.some(role => role.name === 'All')) {
      this.enableRoles = true;
    }
  }

  private addTool(): void {
    const formData = this.toolFormGroup.value;
    const companyIds = this.toolFormGroup.get('companyIds').value;

    if (companyIds?.includes(this.allCompanies.id)) {
      formData['companyIds'] = this.companiesDuplicateList.filter(c => c.id !== this.allCompanies.id).map(c => c.id);
    }

    formData['icon'] = this.formData.icon;

    this.httpService
      .post<ToolInterface>(ToolsApiEnum.Tools, formData)
      .pipe(
        takeUntil(this.unsubscribe$),
        catchError(error => {
          this.titleApiError = error?.error?.errors?.title?.[0];

          if (this.titleApiError) {
            this.toolFormGroup.get('title').markAsDirty();
            this.toolFormGroup.get('title').markAsTouched();
            this.titleApiError = 'toolsManagement.uniqueTitleLabel';
          }

          this.snackbarService.showError('general.defaultErrorLabel');
          return throwError(error);
        })
      )
      .subscribe(() => {
        this.snackbarService.showSuccess('toolsManagement.addToolSuccessLabel');
        this.cancelClick();
      });
  }

  private saveChanges(): void {
    const formData = this.toolFormGroup.value;
    const companyIds = this.toolFormGroup.controls.companyIds?.value;

    if (companyIds?.includes(this.allCompanies.id)) {
      formData['companyIds'] = this.companiesDuplicateList.filter(c => c.id !== this.allCompanies.id).map(c => c.id);
    }

    formData['icon'] = this.formData.icon;

    this.httpService
      .put<unknown>(`${ToolsApiEnum.Tools}/${this.formData.id}`, formData)
      .pipe(
        takeUntil(this.unsubscribe$),
        catchError(error => {
          this.titleApiError = error?.error?.errors?.title?.[0];

          if (this.titleApiError) {
            this.toolFormGroup.get('title').markAsDirty();
            this.toolFormGroup.get('title').markAsTouched();
            this.titleApiError = 'toolsManagement.uniqueTitleLabel';
          }

          this.snackbarService.showError('general.defaultErrorLabel');
          return throwError(error);
        })
      )
      .subscribe(() => {
        this.snackbarService.showSuccess('toolsManagement.editToolSuccessLabel');
        this.cancelClick();
      });
  }
}
