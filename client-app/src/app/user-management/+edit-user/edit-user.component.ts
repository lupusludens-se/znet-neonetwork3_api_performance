import { Component, ElementRef, OnDestroy, OnInit, QueryList, ViewChildren } from '@angular/core';
import { UserInterface } from '../../shared/interfaces/user/user.interface';
import { ActivatedRoute, Router } from '@angular/router';
import { UserDataService } from '../services/user.data.service';
import { UserStatusEnum } from '../enums/user-status.enum';
import { UserManagementApiEnum } from '../enums/user-management-api.enum';
import { CompanyApiEnum } from '../enums/company-api.enum';
import { BlobTypeEnum, GeneralApiEnum } from '../../shared/enums/api/general-api.enum';
import { UserRoleInterface } from '../../shared/interfaces/user/user-role.interface';
import { UserPermissionInterface } from '../../shared/interfaces/user/user-permission.interface';
import { CompanyInterface } from '../../shared/interfaces/user/company.interface';
import { combineLatest } from 'rxjs';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { HttpService } from '../../core/services/http.service';
import { filter, map } from 'rxjs/operators';
import { CountryInterface } from '../../shared/interfaces/user/country.interface';
import { TagInterface } from '../../core/interfaces/tag.interface';
import { CountriesService } from 'src/app/shared/services/countries.service';
import { HeardViaService } from '../services/heard-via.service';
import { AuthService } from '../../core/services/auth.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { SnackbarService } from '../../core/services/snackbar.service';
import { ImageUploadService } from '../../shared/services/image-upload.service';
import { CustomValidator } from '../../shared/validators/custom.validator';
import { RolesEnum } from '../../shared/enums/roles.enum';
import { CompanyRolesEnum } from '../../shared/enums/company-roles.enum';
import { CommonService } from 'src/app/core/services/common.service';
import { CoreService } from 'src/app/core/services/core.service';
import { UserComponentsEnum } from '../enums/user-component.enum';
import { TranslateService } from '@ngx-translate/core';
import { DROPDOWN_OPTIONS } from 'src/app/shared/constants/email-alert-options.const';
import { EmailAlertFrequencyEnum, EmailAlertInterface } from 'src/app/+admin/modules/+email-alerts/models/email-alert';
import { TitleService } from 'src/app/core/services/title.service';
import { MAX_IMAGE_SIZE } from 'src/app/shared/constants/image-size.const';
import { DEFAULT_COMPANY, ZEIGO_NETWORK_TEAM } from 'src/app/shared/constants/default-company-data.const';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';

@UntilDestroy()
@Component({
  selector: 'neo-edit-user',
  templateUrl: 'edit-user.component.html',
  styleUrls: ['edit-user.component.scss']
})
export class EditUserComponent implements OnInit, OnDestroy {
  user: UserInterface;
  userStatuses = UserStatusEnum;
  apiRoutes = { ...UserManagementApiEnum, ...CompanyApiEnum };
  generalApiRoutes = GeneralApiEnum;
  rolesData: UserRoleInterface[];
  permissionsData: UserPermissionInterface[];
  editUserPayload: UserInterface;
  companiesList: CompanyInterface[];
  blobType = BlobTypeEnum;
  countriesList: CountryInterface[];
  heardViaList: TagInterface[];
  selectedCountry: CountryInterface | null;
  formSubmitted: boolean;
  maxImageSize = MAX_IMAGE_SIZE;
  countryError: ValidationErrors;
  imageFormData: File;
  rolesList = RolesEnum;
  companyRolesList = CompanyRolesEnum;
  routesEnum: string[] = [`${UserComponentsEnum.UserManagementComponent}`];
  selectedCompany: Partial<CompanyInterface>;

  isCheckedPrivate?: boolean;
  roleId: number;
  EnablePrivateUser: boolean;
  showModal: boolean;
  selectedRole: UserRoleInterface;
  selectedUserId: string;
  enableEmailAlertSettings: boolean = false;

  private frequencies: { id: number; frequency: EmailAlertFrequencyEnum }[] = [];

  @ViewChildren('checkboxes') checkboxes: QueryList<ElementRef>;
  spAdminConfirmationTitle: string;
  form: FormGroup = this.formBuilder.group({
    firstName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(64), CustomValidator.userName]],
    lastName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(64), CustomValidator.userName]],
    company: [''],
    email: ['', [Validators.required, CustomValidator.email, Validators.maxLength(70)]],
    role: ['', Validators.required],
    heardViaId: ['', Validators.required],
    adminComments: ['']
  });
  defaultCompany: Partial<CompanyInterface> = DEFAULT_COMPANY;
  znTeam: Partial<CompanyInterface> = ZEIGO_NETWORK_TEAM;

  constructor(
    private userDataService: UserDataService,
    private activatedRoute: ActivatedRoute,
    public commonService: CommonService,
    private formBuilder: FormBuilder,
    private httpService: HttpService,
    private router: Router,
    private countriesService: CountriesService,
    private heardViaService: HeardViaService,
    private authService: AuthService,
    private snackbarService: SnackbarService,
    private imageUploadService: ImageUploadService,
    private readonly translateService: TranslateService,
    private coreService: CoreService,
    private titleService: TitleService
  ) {}

  ngOnInit(): void {
    this.getHeardViaList();
    this.activatedRoute.params.subscribe(() => {
      this.selectedUserId = this.activatedRoute.snapshot.params.id;
      this.userDataService.getUserData(this.selectedUserId, 'emailAlerts').subscribe((res: UserInterface) => {
        this.userDataService.setUserData(res);
        this.titleService.setTitle('userManagement.editUserLabel', `${res.firstName} ${res.lastName}`);
        res.emailAlerts.forEach(setting =>
          this.form.addControl(
            this.getControlName(setting?.title),
            new FormControl({ id: setting.frequency, name: this.getOptionName(setting.frequency) })
          )
        );
        this.enableEmailAlertSettings = true;
      });
    });

    const roles$ = this.httpService
      .get<UserRoleInterface[]>(this.apiRoutes.UserRoles, {
        expand: 'permissions'
      })
      .pipe(
        map(roles => {
          return roles.filter(r => r.name !== 'All');
        })
      );

    const permissionsObs$ = this.httpService.get<UserPermissionInterface[]>(this.apiRoutes.Permissions);
    const currentUser$ = this.authService.currentUser();
    combineLatest([roles$, permissionsObs$, currentUser$]).subscribe(res => {
      this.userDataService.userData$
        .pipe(
          filter(v => !!v),
          untilDestroyed(this)
        )
        .subscribe((u: UserInterface) => {
          const roles = u.roles.filter(x => x.id != RolesEnum.All);
          const currentuser = res[2] as UserInterface;
          if (
            !currentuser?.permissions.map(x => x.id).includes(PermissionTypeEnum.TierManagement) &&
            u.roles.some(c => c.id == RolesEnum.SystemOwner)
          ) {
            this.router.navigate(['403']);
            return;
          }
          this.selectedCompany = u.company;
          this.form.patchValue({
            firstName: u.firstName,
            lastName: u.lastName,
            company: u.company?.name,
            email: u.email,
            role: roles[0].id,
            heardViaId: { id: u.heardViaId, name: u.heardViaName },
            adminComments: u.adminComments
          });
          this.roleId = u.roles[0].id;
          this.EnablePrivateUser =
            this.roleId == RolesEnum.Admin || this.roleId == RolesEnum.SystemOwner || this.roleId == RolesEnum.Internal
              ? true
              : false;
          this.selectedCountry = u.country;
          this.rolesData = res[0].map(r => Object.assign({}, r)) as UserRoleInterface[];
          this.permissionsData = res[1] as UserPermissionInterface[];
          this.user = u;
          this.editUserPayload = u;
          this.isCheckedPrivate = u.isPrivateUser;
          this.permissionsData.forEach((pd: UserPermissionInterface) => {
            pd['checked'] = false;
            pd['disabled'] = false;

            this.user.permissions?.forEach(up => {
              if (up.id === pd.id) pd['checked'] = true;
            });

            this.user.roles.forEach((ur: UserRoleInterface) => {
              ur.permissions.forEach((r: UserPermissionInterface) => {
                if (r.id === pd.id) {
                  pd['checked'] = true;
                  pd['disabled'] = true;
                }
              });
            });
          });

          /* disable roles according to a company*/
          if (u.company.typeId === this.companyRolesList.Corporation) {
            this.rolesData.map(r => {
              if (r.id === this.rolesList.SolutionProvider) r.disabled = true;
              if (r.id === this.rolesList.SPAdmin) r.disabled = true;
            });
          }

          if (u.company.typeId === this.companyRolesList.SolutionProvider) {
            this.rolesData.map(r => {
              if (r.id === this.rolesList.Corporation) r.disabled = true;
            });
          }

          if (u.statusId == UserStatusEnum.Inactive) {
            this.rolesData.map(r => {
              if (r.id === this.rolesList.SPAdmin) r.disabled = true;
            });
          }
          const currentUser = res[2] as UserInterface;
          const isAdmin = currentUser?.roles?.some(r => r.id === this.rolesList.Admin);
          const isSystemOwner = currentUser?.roles?.some(r => r.id === this.rolesList.SystemOwner);
          const isZnTeam = u.company.name === this.znTeam.name;

          if ((isAdmin && !isSystemOwner) || (isSystemOwner && !isZnTeam)) {
            this.rolesData.forEach(r => {
              if (r.id === this.rolesList.SystemOwner) r.disabled = true;
            });

            if (isAdmin && !isSystemOwner && this.roleId === this.rolesList.SystemOwner) {
              this.rolesData.forEach(r => {
                r.disabled = true;
              });
            }
          }
        });
    });
  }

  onFileSelect(event: any): void {
    const supported: boolean =
      event.target.files[0].type.includes('png') ||
      event.target.files[0].type.includes('jpeg') ||
      event.target.files[0].type.includes('jpg');

    if (!supported) {
      this.snackbarService.showError('general.wrongFileTypeLabel');
      return;
    }

    const isLarge = event.target.files[0].size > this.maxImageSize;
    if (isLarge) {
      this.snackbarService.showError('general.wrongMediumFileSizeLabel');
      return;
    }
    if (event.target.files.length > 0) {
      this.imageUploadService.renderPreview(event).then(r => {
        this.removeImg();
        if (!this.user.image) {
          this.user.image = { uri: '', name: '', blobType: null };
        }
        this.imageFormData = event.target.files[0];
        this.user.image.uri = r;
        this.user.image.name = null;
      });
    }
  }

  removeImg(): void {
    this.user.image = null;
    this.user.imageName = null;
    this.imageFormData = null;
  }

  editUser(): void {
    this.formSubmitted = true;
    this.countryError = this.selectedCountry ? null : { required: true };

    if (this.form.invalid || this.countryError) return;

    const editObJ: UserInterface = {
      userName: this.form.controls['email'].value,
      companyId: this.editUserPayload.companyId,
      ...this.editUserPayload,
      ...this.form.getRawValue(),
      statusId: this.editUserPayload.statusId,
      countryId: this.selectedCountry.id,
      heardViaId: this.form.getRawValue().heardViaId.id,
      roles: this.rolesData.filter(rd => rd.id === this.form.getRawValue().role),
      permissions: this.permissionsData.filter(pd => pd.checked === true),
      isPrivateUser: this.isCheckedPrivate,
      emailAlerts: { emailAlertsData: this.frequencies }
    };
    if (this.imageFormData) {
      const formData: FormData = new FormData();
      formData.append('file', this.imageFormData);

      this.imageUploadService
        .uploadImage(BlobTypeEnum.Users, formData)
        .pipe(untilDestroyed(this))
        .subscribe(image => {
          editObJ.imageName = image.name;
          this.updateUserRequest(editObJ);
        });
    } else {
      this.updateUserRequest(editObJ);
    }
  }

  changeUserRole(role: UserRoleInterface): void {
    this.selectedRole = role;
    const spRole = this.rolesData.find(r => r.id === this.rolesList.SolutionProvider);
    if (role.id == RolesEnum.SPAdmin) {
      if (this.editUserPayload.statusId == UserStatusEnum.Inactive) {
        this.form.patchValue({ role: spRole.id });
        this.configurePermission(spRole);
        return;
      }
      if (this.selectedCompany?.id > 0) {
        this.userDataService.getSPAdminByCompany(this.selectedCompany?.id, 0).subscribe((result: UserInterface) => {
          if (result?.id > 0 && result?.id != Number(this.selectedUserId)) {
            this.showModal = true;
            const spAdminConfirmationTitle = this.translateService.instant(
              'userManagement.form.SPAdminConfirmationTitle'
            );
            this.spAdminConfirmationTitle = spAdminConfirmationTitle?.replace(
              '_username_',
              `${result.lastName}, ${result.firstName}`
            );
          } else {
            this.showModal = false;
            this.configurePermission(this.selectedRole);
          }
        });
      }
    } else {
      this.configurePermission(this.selectedRole);
    }
  }

  private configurePermission(role: UserRoleInterface) {
    this.isCheckedPrivate = false;
    this.permissionsData.forEach(p => {
      p.checked = false;
      p.disabled = false;

      role.permissions?.forEach(per => {
        if (p.id === per.id) {
          p.checked = true;
          p.disabled = true;
        }
      });
    });
    this.EnablePrivateUser =
      role.id == RolesEnum.Admin || role.id == RolesEnum.SystemOwner || role.id == RolesEnum.Internal ? true : false;
  }

  // * change permission from user
  changeUserPermissions(permission: UserPermissionInterface): void {
    this.permissionsData.forEach(p => {
      if (p.id === permission.id) p.checked = !p.checked;
    });
  }

  changeStatus(status: number): void {
    this.editUserPayload.statusId = status;
    if (
      this.form.controls['role'].value == RolesEnum.SolutionProvider ||
      this.form.controls['role'].value == RolesEnum.SPAdmin
    ) {
      const spAdminRole = this.rolesData.find(r => r.id === this.rolesList.SPAdmin);
      const spRole = this.rolesData.find(r => r.id === this.rolesList.SolutionProvider);
      if (status == UserStatusEnum.Inactive) {
        spAdminRole.disabled = true;
        if (this.form.controls['role'].value == RolesEnum.SPAdmin) {
          this.configurePermission(spRole);
          this.form.patchValue({ role: spRole.id });
          spAdminRole.disabled = true;
        }
      } else if (status == UserStatusEnum.Active && this.form.controls['role'].value == RolesEnum.SolutionProvider) {
        spAdminRole.disabled = false;
      }
    }
  }

  onCancel(): void {
    this.router.navigate(['admin/user-management']);
  }

  getCompanies(searchStr: string): void {
    this.httpService
      .get<CompanyInterface[]>(this.apiRoutes.Companies, { search: searchStr })
      .subscribe((com: CompanyInterface[]) => {
        this.companiesList = com;
      });
  }

  getCountries(searchStr?: string): void {
    this.countriesService.getCountriesList(searchStr).subscribe((c: CountryInterface[]) => {
      this.countriesList = [...c];
    });
  }

  getHeardViaList(): void {
    this.heardViaService.getHeardViaList().subscribe((h: TagInterface[]) => {
      this.heardViaList = h;
    });
  }

  chooseCountry(country: CountryInterface): void {
    this.selectedCountry = country;
    this.countriesList = [];
    this.countryError = null;
  }

  updateUserRequest(userPayload: UserInterface): void {
    this.httpService.put(this.apiRoutes.Users + `/${this.user.id}`, userPayload).subscribe(
      () => {
        this.user = { ...userPayload };
        this.userDataService.clearRequestParams();
        this.commonService.clearFilters(this.commonService.filterState$.getValue());
        this.onCancel();
        this.authService.getCurrentUser$.next(true);
      },
      error => {
        const errorsKeys = Object.keys(error.error.errors);
        this.snackbarService.showError(error.error.errors[errorsKeys[0]][0]);
      }
    );
  }
  checkboxClicked(isCheckedPrivate: boolean) {
    this.isCheckedPrivate = !isCheckedPrivate;
  }

  ngOnDestroy(): void {
    const routesFound = this.routesEnum.some(val => this.coreService.getOngoingRoute().includes(val));
    if (!routesFound) {
      if (this.commonService.filterState$.getValue() != null) {
        this.commonService.clearFilters(this.commonService.filterState$.getValue());
      }
      this.userDataService.clearRequestParams();
    }
  }

  cancel(): void {
    const spRole = this.rolesData.find(r => r.id === this.rolesList.SolutionProvider);
    this.configurePermission(spRole);
    this.form.patchValue({ role: spRole.id });
    this.showModal = false;
  }

  proceed(): void {
    this.configurePermission(this.selectedRole);
    this.showModal = false;
  }

  dropdownOptions(title: string): TagInterface[] {
    const options = DROPDOWN_OPTIONS.map(option => ({
      ...option,
      name: this.translateService.instant(option.name)
    }));

    if (title.toLowerCase().includes('summary')) {
      options.splice(1, 1);
    }

    return options;
  }

  getControlName(title: string): string {
    return title.toLowerCase().replace(/\s/g, '');
  }

  updateSetting(alertSetting: EmailAlertInterface, frequency: TagInterface): void {
    const frequencyIndex = this.frequencies.findIndex(fr => fr.id === alertSetting.id);

    if (frequencyIndex >= 0) {
      this.frequencies[frequencyIndex].frequency = frequency.id;
    } else {
      this.frequencies.push({ id: alertSetting.id, frequency: frequency.id });
    }
  }

  getOptionName(frequency: EmailAlertFrequencyEnum): string {
    switch (frequency) {
      case EmailAlertFrequencyEnum.Off:
        return this.translateService.instant('settings.offLabel');
      case EmailAlertFrequencyEnum.Immediately:
        return this.translateService.instant('settings.immediatelyLabel');
      case EmailAlertFrequencyEnum.Daily:
        return this.translateService.instant('settings.dailyLabel');
      case EmailAlertFrequencyEnum.Weekly:
        return this.translateService.instant('settings.weeklyLabel');
      case EmailAlertFrequencyEnum.Monthly:
        return this.translateService.instant('settings.monthlyLabel');
    }
  }
}
