import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { catchError, map } from 'rxjs/operators';

import { PaginateResponseInterface } from '../../shared/interfaces/common/pagination-response.interface';
import { UserPermissionInterface } from '../../shared/interfaces/user/user-permission.interface';
import { UserManagementApiEnum } from '../../user-management/enums/user-management-api.enum';
import { UserRoleInterface } from '../../shared/interfaces/user/user-role.interface';
import { HeardViaService } from '../../user-management/services/heard-via.service';
import { CompanyInterface } from '../../shared/interfaces/user/company.interface';
import { CountryInterface } from '../../shared/interfaces/user/country.interface';
import { CompanyApiEnum } from '../../user-management/enums/company-api.enum';
import { PendingUserInterface } from '../interfaces/pending-user.interface';
import { CountriesService } from '../../shared/services/countries.service';
import { CompanyStatusEnum } from '../../shared/enums/company-status.enum';
import { CompanyRolesEnum } from '../../shared/enums/company-roles.enum';
import { SnackbarService } from '../../core/services/snackbar.service';
import { TagInterface } from '../../core/interfaces/tag.interface';
import { TitleService } from '../../core/services/title.service';
import { HttpService } from '../../core/services/http.service';
import { AdmitUserEnum } from '../emuns/admit-user.enum';
import { RolesEnum } from '../../shared/enums/roles.enum';
import { CustomValidator } from '../../shared/validators/custom.validator';
import { DEFAULT_COMPANY, DEFAULT_HEARD_VIA } from '../../shared/constants/default-company-data.const';
import { TranslateService } from '@ngx-translate/core';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { CoreService } from 'src/app/core/services/core.service';
import { AdminCommentsSchema } from '../constants/parameter.const';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { UserDataService } from 'src/app/user-management/services/user.data.service';

@UntilDestroy()
@Component({
	selector: 'neo-review-user',
	templateUrl: 'review-user.component.html',
	styleUrls: ['./review-user.component.scss']
})
export class ReviewUserComponent implements OnInit {
	apiList = { ...AdmitUserEnum, ...UserManagementApiEnum, ...CompanyApiEnum };
	form: FormGroup = this.formBuilder.group({
		firstName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(64), CustomValidator.userName]],
		lastName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(64), CustomValidator.userName]],
		email: ['', [Validators.required, CustomValidator.email, Validators.maxLength(70)]],
		roleId: ['', Validators.required],
		heardViaId: ['', Validators.required],
		adminComments: [''],
		joiningInterestDetails: ['']
	});
	rolesData: TagInterface[];
	permissionsData: UserPermissionInterface[];
	companiesList: CompanyInterface[];
	countriesList: CountryInterface[];
	heardViaList: TagInterface[];
	selectedCountry: CountryInterface;
	selectedCompany: Partial<CompanyInterface> | Record<string, string>;
	user: PendingUserInterface;
	companyStatusError: boolean;
	companyDeletedStatusError: boolean;
	companyNotExistError: boolean;
	countryError: ValidationErrors;
	companyStatuses = CompanyStatusEnum;
	internalRoleId: number = 4;
	internalRoleSelected: boolean;
	defaultCompany: Partial<CompanyInterface> = DEFAULT_COMPANY;
	defaultHeardVia: TagInterface = DEFAULT_HEARD_VIA;
	approveDisabled: boolean = false;
	roleSelectionChanged: boolean = false;
	formSubmitted: boolean;
	enteredCompanyName: string;

	adminCommentsLength: number = 0;
	showModal: boolean = false;
	adminCommentsMaxLength = AdminCommentsSchema.aboutMaxLength;
	adminCommentsTextMaxLength = AdminCommentsSchema.aboutTextMaxLength;
	spAdminConfirmationTitle: string;

	constructor(
		private activatedRoute: ActivatedRoute,
		private formBuilder: FormBuilder,
		private httpService: HttpService,
		private router: Router,
		private countriesService: CountriesService,
		private heardViaService: HeardViaService,
		private titleService: TitleService,
		private snackbarService: SnackbarService,
		private translateService: TranslateService,
		private readonly coreService: CoreService,
		private userDataService: UserDataService
	) { }

	ngOnInit(): void {
		this.getRolesData();

		this.heardViaService
			.getHeardViaList()
			.pipe(untilDestroyed(this))
			.subscribe(hv => {
				this.heardViaList = hv;
			});

		this.form.controls['roleId'].valueChanges.pipe(untilDestroyed(this)).subscribe(v => {
			if (v === this.internalRoleId) {
				this.internalRoleSelected = true;
				this.companyStatusError = false;
				this.companyDeletedStatusError = false;
				this.selectedCompany = { ...this.defaultCompany };
				this.companyNotExistError = !this.selectedCompany.id;
				this.form.patchValue({ heardViaId: this.defaultHeardVia });
				this.setUserRoleDisabledByCompany(this.selectedCompany);
			} else this.internalRoleSelected = false;
		});
	}

	onCancel(): void {
		this.router.navigate(['admin/admit-users']);
	}

	getCountries(searchStr?: string): void {
		this.countriesService.getCountriesList(searchStr).subscribe((c: CountryInterface[]) => {
			this.countriesList = c;
			this.countryError = c.length ? null : { required: "'admitUsers.countryError' | translate" };
		});
	}

	getRolesData(): void {
		this.httpService
			.get<UserRoleInterface[]>(this.apiList.UserRoles)
			.pipe(
				map(roles => {
					return roles.filter(r => r.name !== 'All' && r.name !== 'Admin'&& r.name !== 'System Owner');
				})
			)
			.subscribe(r => { 
				this.rolesData = r.map(role => {
					role.disabled = false;
					return role;
				});
				this.adminCommentsLength = this.coreService.convertToPlain(
					this.form.controls['adminComments'].value ?? ''
				).length;
				const userId: string = this.activatedRoute.snapshot.params.id;
				this.httpService
					.get<PendingUserInterface>(this.apiList.UserPendings + `/${userId}`, {
						expand: 'role,company,country'
					})
					.pipe(
						catchError((error: HttpErrorResponse) => {
							if (error.status === 404) {
								this.router.navigate(['/admin/user-management']);
								this.coreService.elementNotFoundData$.next({
									iconKey: 'user-management',
									mainTextTranslate: 'admitUsers.notFoundText',
									buttonTextTranslate: 'admitUsers.notFoundButton',
									buttonLink: '/admin/user-management'
								});
							}

							return throwError(error);
						})
					)
					.subscribe(user => {
						this.user = user;
						this.companyStatusError = user.company?.statusId === this.companyStatuses.Inactive;
						this.companyDeletedStatusError = user.company?.statusId === this.companyStatuses.Deleted;
						this.companyNotExistError = !user.company?.id;
						this.titleService.setTitle('admitUsers.reviewUserLabel', `${user.firstName} ${user.lastName}`);
						this.form.patchValue({
							firstName: user.firstName,
							lastName: user.lastName,
							email: user.email,
							roleId: user.role.id,
							heardViaId: { id: user.heardViaId, name: user.heardViaName },
							adminComments: user.adminComments,
							joiningInterestDetails: user.joiningInterestDetails
						});

						this.adminCommentsLength = this.coreService.convertToPlain(
							this.form.controls['adminComments'].value ?? ''
						).length;
						this.form.valueChanges.subscribe(() => {
							this.approveDisabled = true;
						});

						this.selectedCompany = user.company ?? { name: user.companyName };
						this.selectedCountry = user.country;
						this.enteredCompanyName = this.selectedCompany.name;
						if (user.company) {
							this.setUserRoleDisabledByCompany(user.company, true);
						}

						this.approveDisabled = !this.selectedCompany.id || this.form.invalid || this.roleSelectionChanged;
					});
			});
	}

	chooseCountry(country: CountryInterface): void {
		this.selectedCountry = country;
		this.approveDisabled = true;

		if (!country) this.countryError = { required: "'admitUsers.countryError' | translate" };
		else this.countryError = null;
	}

	editUser(): void {
		this.formSubmitted = true;
		if (this.form.invalid || this.countryError) {
			return;
		}
		if (this.adminCommentsLength > this.adminCommentsTextMaxLength) {
			return;
		}
		if (this.form.controls['adminComments'].value?.length > this.adminCommentsMaxLength) {
			this.snackbarService.showError(
				this.translateService.instant('companyManagement.form.aboutFormattingMaxLengthError')
			);
			return;
		}
		if (this.selectedCompany?.id == 0 || this.selectedCompany?.id == null) {
			let company = this.companiesList?.find(
				company => company.name.toLowerCase() == this.enteredCompanyName?.toLowerCase()
			);
			this.selectedCompany = company ?? this.selectedCompany;
		}
		this.httpService
			.put(this.apiList.UserPendings + `/${this.user.id}`, {
				...this.form.getRawValue(),
				heardViaId: this.form.getRawValue().heardViaId.id,
				countryId: this.selectedCountry.id,
				companyId: this.selectedCompany.id,
				companyName: this.enteredCompanyName != undefined ? this.enteredCompanyName : this.selectedCompany.name
			})
			.subscribe(
				() => this.onCancel(),
				err => {
					if (err instanceof HttpErrorResponse) {
						if (err.status === 422) {
							const validationErrors = err.error?.errors;
							Object.keys(validationErrors).forEach(prop => {
								const formControl = this.form.get(prop);
								if (formControl) {
									formControl.setErrors({
										serverError: validationErrors[prop]
									});
								}
							});
						}
					}
				}
			);
	}

	getCompanies(searchStr: string) {
		this.enteredCompanyName = searchStr;

		this.httpService
			.get<PaginateResponseInterface<CompanyInterface>>(this.apiList.Companies, {
				Search: searchStr,
				FilterBy: `statusids=${this.companyStatuses.Active}`
			})
			.subscribe(c => {
				if (!c.dataList.length) {
					this.companyNotExistError = !c.dataList.length;
					this.selectedCompany.id = null;
				} else this.companiesList = c.dataList;
			});
	}

	chooseCompany(company: CompanyInterface) {
		this.selectedCompany = company;
		this.enteredCompanyName = this.selectedCompany.name;
		this.companyStatusError = false;
		this.companyDeletedStatusError = false;
		this.companyNotExistError = false;
		this.approveDisabled = true;
		this.setUserRoleDisabledByCompany(company);
	}

	setUserRoleDisabledByCompany(company: Partial<CompanyInterface>, isPageLoad: boolean = false) {
		{
			if (company.typeId === CompanyRolesEnum.SolutionProvider) {
				if ((this.form.controls['roleId'].value !== RolesEnum.SolutionProvider && this.form.controls['roleId'].value !== RolesEnum.SPAdmin) || !isPageLoad
				) {
					this.form.patchValue({ roleId: RolesEnum.SolutionProvider });
					this.roleSelectionChanged = true;
				}
				this.rolesData.forEach(role => {
					role.disabled = role.id === RolesEnum.Corporation;
				});
			}
			if (company.typeId === CompanyRolesEnum.Corporation) {
				if (this.form.controls['roleId'].value !== RolesEnum.Corporation) {
					if (!this.internalRoleSelected) {
						this.roleSelectionChanged = true;
						this.form.patchValue({ roleId: RolesEnum.Corporation });
					}
				}
				this.rolesData.forEach(role => {
					role.disabled = role.id === RolesEnum.SolutionProvider || role.id === RolesEnum.SPAdmin;
				});
			}
		}
	}


	denyUser(): void {
		const denyMessage: string = `${this.user.lastName}, ${this.user.firstName}
          ${this.translateService.instant(
			this.user.isDenied ? 'admitUsers.unDenySuccessMessageLabel' : 'admitUsers.denySuccessMessageLabel'
		)}
          `;

		this.httpService
			.patch(
				this.apiList.UserPendings + `/${this.user.id}/` + this.apiList.Denial + `?isDenied=${!this.user.isDenied}`,
				{ id: this.user.id }
			)
			.subscribe(() => {
				if (!this.user.isDenied) {
					this.snackbarService.showError(denyMessage);
				} else this.snackbarService.showSuccess(denyMessage);

				this.onCancel();
			});
	}

	approveUser(): void {
		if (Number(this.selectedCompany?.id) > 0) {
			if (this.form.controls['roleId'].value == RolesEnum.SPAdmin) {
				this.userDataService.getSPAdminByCompany(Number(this.selectedCompany?.id), 0).subscribe((result: UserInterface) => {
					if (result?.id > 0) {
						this.showModal = true;
						const spAdminConfirmationTitle = this.translateService.instant('userManagement.form.SPAdminConfirmationTitle');
						this.spAdminConfirmationTitle = spAdminConfirmationTitle?.replace("_username_", `${result.lastName}, ${result.firstName}`);
					}
					else {
						this.showModal = false;
					}
				});
			}
			else {
				this.proceed();
			}
		}
	}

	onAdminCommentsLengthChanged(value: number) {
		this.adminCommentsLength = value;
	}

	cancel(): void {
		this.showModal = false;
	}


	proceed(): void {
		this.httpService
			.post(this.apiList.UserPendings + `/${this.user.id}/` + this.apiList.Approval, { id: this.user.id })
			.subscribe(() => this.onCancel());
	}
}
