import { FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { UserInterface } from '../../shared/interfaces/user/user.interface';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Component, ElementRef, OnInit, QueryList, ViewChildren } from '@angular/core';
import { filter, map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { combineLatest } from 'rxjs';

import { PaginateResponseInterface } from '../../shared/interfaces/common/pagination-response.interface';
import { UserPermissionInterface } from '../../shared/interfaces/user/user-permission.interface';
import { UserRoleInterface } from '../../shared/interfaces/user/user-role.interface';
import { CompanyInterface } from '../../shared/interfaces/user/company.interface';
import { CountryInterface } from '../../shared/interfaces/user/country.interface';
import { CountriesService } from '../../shared/services/countries.service';
import { CompanyStatusEnum } from '../../shared/enums/company-status.enum';
import { CustomValidator } from '../../shared/validators/custom.validator';
import { UserManagementApiEnum } from '../enums/user-management-api.enum';
import { GeneralApiEnum } from '../../shared/enums/api/general-api.enum';
import { SnackbarService } from '../../core/services/snackbar.service';
import { TagInterface } from '../../core/interfaces/tag.interface';
import { TitleService } from 'src/app/core/services/title.service';
import { CommonApiEnum } from '../../core/enums/common-api.enum';
import { HeardViaService } from '../services/heard-via.service';
import { UserDataService } from '../services/user.data.service';
import { HttpService } from '../../core/services/http.service';
import { UserStatusEnum } from '../enums/user-status.enum';
import { CompanyApiEnum } from '../enums/company-api.enum';
import { CompanyRolesEnum } from '../../shared/enums/company-roles.enum';
import { RolesEnum } from '../../shared/enums/roles.enum';
import {
  DEFAULT_COMPANY,
  DEFAULT_HEARD_VIA,
  DEFAULT_TIMEZONE,
  ZEIGO_NETWORK_TEAM
} from '../../shared/constants/default-company-data.const';
import { TimezoneService } from '../../shared/services/timezone.service';
import { TimezoneInterface } from '../../shared/interfaces/common/timezone.interface';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/services/auth.service';

@UntilDestroy()
@Component({
  selector: 'neo-add-user',
  templateUrl: 'add-user.component.html',
  styleUrls: ['add-user.component.scss']
})
export class AddUserComponent implements OnInit {
  userStatuses = UserStatusEnum;
  apiRoutes = { ...UserManagementApiEnum, ...CompanyApiEnum, ...CommonApiEnum };
  generalApiRoutes = GeneralApiEnum;
  rolesData: UserRoleInterface[];
  permissionsData: UserPermissionInterface[];
  companiesList: CompanyInterface[];
  selectedCompany: Partial<CompanyInterface>;
  countriesList: CountryInterface[];
  heardViaList: TagInterface[];
  selectedCountry: CountryInterface;
  companyStatuses = CompanyStatusEnum;
  formSubmitted: boolean;
  companyError: ValidationErrors;
  countryError: ValidationErrors;
  rolesList = RolesEnum;
  companyRolesList = CompanyRolesEnum;
  timeZonesList: TagInterface[];
  form: FormGroup = this.formBuilder.group({
    firstName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(64), CustomValidator.userName]],
    lastName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(64), CustomValidator.userName]],
    email: ['', [Validators.required, CustomValidator.email, Validators.maxLength(70)]],
    role: ['', Validators.required],
    heardViaId: ['', Validators.required],
    timeZonesId: ['', Validators.required]
  });
  defaultCompany: Partial<CompanyInterface> = DEFAULT_COMPANY;
  znTeam: Partial<CompanyInterface> = ZEIGO_NETWORK_TEAM;
  defaultHeardVia: TagInterface = DEFAULT_HEARD_VIA;
  isEnablePrivateUser: Boolean = false;
  isChecked: boolean = false;
  PrivateUserText = 'Private User';
  isCheckedPrivate?: boolean;
  showModal: boolean;
  selectedRole: UserRoleInterface;


  @ViewChildren('checkboxes') checkboxes: QueryList<ElementRef>;
  spAdminConfirmationTitle: string;
  currentUser: UserInterface;

  constructor(
    private formBuilder: FormBuilder,
    private userDataService: UserDataService,
    private countriesService: CountriesService,
    private httpService: HttpService,
    private router: Router,
    private heardViaService: HeardViaService,
    private authService: AuthService,
    private snackbarService: SnackbarService,
    private titleService: TitleService,
    private timezoneService: TimezoneService,
    private readonly translateService: TranslateService,
  ) { }

  ngOnInit(): void {
    this.getHeardViaList();
    this.getTimeZoneList();

    const currentUser$ = this.authService.currentUser();
    const rolesObs$ = this.httpService
      .get<UserRoleInterface[]>(this.apiRoutes.UserRoles, {
        expand: 'permissions'
      })
      .pipe(
        map(roles => {
          return roles.filter(r => r.name !== 'All');
        })
      );
    const permissionsObs$ = this.httpService.get<UserPermissionInterface[]>(this.apiRoutes.Permissions);

    combineLatest([rolesObs$, permissionsObs$, currentUser$])
      .pipe(
        filter(v => !!v),
        untilDestroyed(this)
      )
      .subscribe(res => {
        this.rolesData = res[0] as UserRoleInterface[];
        const currentUser = res[2] as UserInterface;
        if (currentUser) {
          this.currentUser = currentUser;
          const isAdmin = this.currentUser?.roles?.some(r => r.id === this.rolesList.Admin);
          const isSystemOwner = this.currentUser?.roles?.some(r => r.id === this.rolesList.SystemOwner);
          if (isAdmin && !isSystemOwner) {
            this.rolesData.forEach(r => {
              if (r.id === this.rolesList.SystemOwner) r.disabled = true;
            });
          }
        }

        this.permissionsData = res[1] as UserPermissionInterface[];
      });

    this.titleService.setTitle('userManagement.addUserLabel');

    this.form.controls['role'].valueChanges.pipe(untilDestroyed(this)).subscribe(v => {
      if (v === this.rolesList.Internal) {
        // when select Internal - Company field should be predefined by “Schneider Electric” and Heard Via by “I am an Employee“. Solution Provider role radio-button should be disabled.
        this.selectedCompany = { ...this.defaultCompany };
        this.form.patchValue({ heardViaId: this.defaultHeardVia });
        this.rolesData.forEach(r => {
          if (r.id === this.rolesList.SolutionProvider) r.disabled = true;
          if (r.id === this.rolesList.SPAdmin) r.disabled = true;
          if (r.id === this.rolesList.Corporation) r.disabled = false;
        });
      } else if (
        this.selectedCompany?.typeId === this.companyRolesList.Corporation &&
        (v === this.rolesList.Admin || v === this.rolesList.Corporation)
      ) {
        this.rolesData.forEach(r => {
          if (r.id === this.rolesList.SolutionProvider) r.disabled = true;
          if (r.id === this.rolesList.SPAdmin) r.disabled = true;
        });
        return;
      } else {
        this.rolesData.forEach(r => {
          if (r.id === this.rolesList.SolutionProvider) r.disabled = false;
          if (r.id === this.rolesList.SPAdmin) r.disabled = false;

          const isSystemOwnerRole = r.id === this.rolesList.SystemOwner;
          const isAdmin = this.currentUser?.roles?.some(role => role.id === this.rolesList.Admin);
          const isNotSystemOwner = !this.currentUser?.roles?.some(role => role.id === this.rolesList.SystemOwner);

          if (isSystemOwnerRole && isAdmin && isNotSystemOwner) {
            r.disabled = true;
          }
        });
      }
    });
  }

  saveUser(): void {
    this.formSubmitted = true;
    this.companyError = this.selectedCompany ? null : { required: true };
    this.countryError = this.selectedCountry ? null : { required: true };

    if (this.form.invalid || this.companyError || this.countryError) return;

    const payload: UserInterface = {
      userName: this.form.controls['email'].value,
      imageKey: null,
      ...this.form.getRawValue(),
      roles: this.rolesData.filter(rd => rd.id === this.form.getRawValue().role),
      permissions: this.permissionsData.filter(pd => pd.checked === true),
      statusId: this.userStatuses.Onboard,
      companyId: this.selectedCompany.id,
      countryId: this.selectedCountry.id,
      heardViaId: this.form.getRawValue().heardViaId.id,
      isPrivateUser: this.isChecked,
      TimeZoneId: this.form.getRawValue().timeZonesId.id
    };

    this.httpService.post(this.apiRoutes.Users, payload).subscribe(
      () => {
        this.snackbarService.showSuccess('userManagement.userCreatedMessageLabel');
        this.onCancel();
      },
      error => {
        const errorsKeys = Object.keys(error.error.errors);
        this.snackbarService.showError(error.error.errors[errorsKeys[0]][0]);
      }
    );
  }

  changeUserRole(role: UserRoleInterface): void {
    this.selectedRole = null;
    if (role.id == RolesEnum.SPAdmin) {
      this.selectedRole = role;
      if (this.selectedCompany?.id > 0) {

        this.userDataService.getSPAdminByCompany(this.selectedCompany?.id, 0).subscribe((result: UserInterface) => {
          if (result?.id > 0) {
            this.showModal = true;
            const spAdminConfirmationTitle = this.translateService.instant('userManagement.form.SPAdminConfirmationTitle');
            this.spAdminConfirmationTitle = spAdminConfirmationTitle?.replace("_username_", `${result.lastName}, ${result.firstName}`);
          }
          else {
            this.showModal = false;
            this.configurePermission(this.selectedRole);
          }
        });
      }
      else {
        this.configurePermission(this.selectedRole);
      }
    }
    else {
      this.configurePermission(role);
    }
  }

  private configurePermission(role: UserRoleInterface) {
    this.permissionsData.forEach(p => {
      p.checked = false;
      p.disabled = false;

      role.permissions.forEach(per => {
        if (p.id === per.id) {
          p.checked = true;
          p.disabled = true;
        }
      });
    });
    if (role.id == RolesEnum.Admin || role.id == RolesEnum.SystemOwner || role.id == RolesEnum.Internal) {
      this.isEnablePrivateUser = true;
    } else {
      this.checkboxes.forEach(element => {
        element.nativeElement.checked = false;
      });
      this.isChecked = false;
      this.isEnablePrivateUser = false;
    }
  }

  // * change permission from user
  changeUserPermissions(permission: UserPermissionInterface): void {
    this.permissionsData.forEach(p => {
      if (p.id === permission.id) p.checked = !p.checked;
    });
  }

  onCancel(): void {
    this.router.navigate(['admin/user-management']);
  }

  chooseCompany(company: CompanyInterface): void {
    this.selectedCompany = company;
    this.companiesList = [];
    this.companyError = null;

    if (!this.selectedCompany) {
      this.rolesData.map(r => {
        r.disabled = false;
      });
      this.permissionsData.forEach(per => {
        per.checked = false;
        per.disabled = false;
      });
      this.form.controls['role'].patchValue(null);
      return;
    }

    if (this.selectedCompany?.typeId === this.companyRolesList.Corporation) {
      // when select Corporation Company Solution Provider role radio-button should be disabled and unselected
      this.changeUserRole(this.rolesData.find(r => r.id === this.rolesList.Corporation));
      this.rolesData.map(r => {
        if (r.id === this.rolesList.SolutionProvider) r.disabled = true;
        if (r.id === this.rolesList.SPAdmin) r.disabled = true;
        if (r.id === this.rolesList.Corporation) {
          this.form.patchValue({ role: r.id });
          r.disabled = false;
        }
      });
    }

    if (this.selectedCompany?.typeId === this.companyRolesList.SolutionProvider) {
      // when select Solution Provider Company Corporation role radio-button should be disabled and unselected
      this.changeUserRole(this.rolesData.find(r => r.id === this.rolesList.SolutionProvider));
      this.rolesData.map(r => {
        if (r.id === this.rolesList.Corporation) r.disabled = true;
        if (r.id === this.rolesList.SolutionProvider) {
          this.form.patchValue({ role: r.id });
          r.disabled = false;
        }
      });
    }
    const isSystemOwnerAndDefaultCompany = this.znTeam &&
      this.selectedCompany.name === this.znTeam.name &&
      this.currentUser?.roles?.some(r => r.id === this.rolesList.SystemOwner);

    this.rolesData.forEach(r => {
      if (r.id === this.rolesList.SystemOwner) {
        r.disabled = !isSystemOwnerAndDefaultCompany;
      }
    });
  }

  getCompanies(searchStr: string): void {
    this.httpService
      .get<PaginateResponseInterface<CompanyInterface>>(this.apiRoutes.Companies, {
        search: searchStr,
        FilterBy: `statusids=${this.companyStatuses.Active}`
      })
      .pipe(
        map(data => {
          // !! temp solution until be will be ready
          return data.dataList.filter(r => r.name !== 'All');
        }),
        map(comp => {
          // !! temp solution until be will be ready
          return comp.filter(r => {
            return r.name.toLowerCase().includes(searchStr.toLowerCase());
          });
        })
      )
      .subscribe((com: CompanyInterface[]) => {
        this.companiesList = [...com];
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
  checkboxClicked(isChecked: boolean): void {
    {
      this.isChecked = isChecked;
    }
  }
  getTimeZoneList(): void {
    this.timezoneService.getTimeZones().subscribe((t: TimezoneInterface[]) => {
      this.timeZonesList = t.map(x => ({
        id: x.id,
        name: x.displayName
      }));
      this.form.patchValue({ timeZonesId: this.timeZonesList.find(item => item.name == DEFAULT_TIMEZONE.name) });
    });
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
}
