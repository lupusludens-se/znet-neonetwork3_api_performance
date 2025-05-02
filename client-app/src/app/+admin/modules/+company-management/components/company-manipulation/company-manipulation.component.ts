import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

import { combineLatest, Subject, throwError } from 'rxjs';
import { catchError, filter, switchMap } from 'rxjs/operators';

import { HttpService } from 'src/app/core/services/http.service';
import { FormControlStatusEnum } from 'src/app/shared/enums/form-control-status.enum';
import { MAX_IMAGE_SIZE } from 'src/app/shared/constants/image-size.const';
import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { CustomValidator } from 'src/app/shared/validators/custom.validator';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { TitleService } from 'src/app/core/services/title.service';
import { BlobTypeEnum } from 'src/app/shared/enums/api/general-api.enum';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { UserPermissionInterface } from 'src/app/shared/interfaces/user/user-permission.interface';
import { CompanyStatusEnum } from 'src/app/shared/enums/company-status.enum';
import { CompaniesSchema, COMPANY_ROLES_DATA } from '../../constants/parameter.const';
import { CompanyInterface } from 'src/app/shared/interfaces/user/company.interface';
import { CountryInterface } from 'src/app/shared/interfaces/user/country.interface';
import { CountriesService } from 'src/app/shared/services/countries.service';
import { SearchResultInterface } from 'src/app/shared/interfaces/search-result.interface';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CoreService } from 'src/app/core/services/core.service';
import { TranslateService } from '@ngx-translate/core';
import { ImageUploadService } from 'src/app/shared/services/image-upload.service';

@UntilDestroy()
@Component({
  selector: 'neo-company-manipulation',
  templateUrl: './company-manipulation.component.html',
  styleUrls: ['./company-manipulation.component.scss']
})
export class CompanyManipulationComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  isAdd: boolean = !this.activatedRoute.snapshot.params.id;
  imageLarge: boolean;
  industriesList: TagInterface[];
  categoryList: TagInterface[];
  offsitePPAList: TagInterface[];
  rolesData: TagInterface[] = COMPANY_ROLES_DATA;
  permissionsData: UserPermissionInterface[];

  disableOffsitePPAs: boolean;
  disableSolutionCapabilities: boolean;
  offsitePPACategoryIndex: number;
  corporationTypeIndex: number;

  loadData$: Subject<void> = new Subject<void>();

  formData: CompanyInterface;
  activeCompanyStatus: number = CompanyStatusEnum.Active;
  inactiveCompanyStatus: number = CompanyStatusEnum.Inactive;

  titleApiError: string;
  companyNameMinLength: number = 2;
  companyNameMaxLength: number = 250;
  maxImageSize = MAX_IMAGE_SIZE;
  countriesList: SearchResultInterface[];
  selectedCountry: CountryInterface;
  countryError: boolean;
  isSECompany: boolean = false;
  seCompanyId: number = 1;
  mdmValues: string = '';
  selectedMDM: TagInterface;
  public mdmKeys = [
    { id: 1, name: 'ORG-' },
    { id: 2, name: 'MDM-' },
    { id: 0, name: 'Select One' }
  ];
  public tempKeys = this.mdmKeys;
  mdmKeyErrorMessage = 'MDM Key must be in format ORG-X(X) or MDM-X(X)';
  showMDMError = false;
  companyFormGroup: FormGroup = new FormGroup({
    name: new FormControl(null, [
      Validators.required,
      CustomValidator.companyNameExcludeSymbols,
      Validators.minLength(this.companyNameMinLength),
      Validators.maxLength(this.companyNameMaxLength)
    ]),
    companyUrl: new FormControl(null, [CustomValidator.url]),
    countryId: new FormControl(null, [Validators.required]),
    industryId: new FormControl([], [Validators.required]),
    mdmKey: new FormControl(null, [CustomValidator.mdmKey]),
    linkedInUrl: new FormControl(null, [CustomValidator.linkedInUrl]),
    about: new FormControl(''),
    urlLinks: this.formBuilder.array([
      this.formBuilder.group(
        {
          urlName: new FormControl(''),
          urlLink: new FormControl('', [CustomValidator.url])
        },
        { validators: CustomValidator.urlValidator() }
      )
    ]),
    typeId: new FormControl(null, [Validators.required]),
    categories: new FormArray([]),
    offsitePPAIds: new FormArray([]),
    imageLogo: new FormControl(null),
    statusId: new FormControl(CompanyStatusEnum.Active),
    mdmKeyId: new FormControl([]),
    mdmValue: new FormControl()
  });
  formSubmitted: boolean;

  aboutLength: number = 0;

  aboutMaxLength = CompaniesSchema.aboutMaxLength;
  aboutTextMaxLength = CompaniesSchema.aboutTextMaxLength;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly httpService: HttpService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly snackbarService: SnackbarService,
    private readonly router: Router,
    private readonly titleService: TitleService,
    private countriesService: CountriesService,
    public coreService: CoreService,
    public translateService: TranslateService,
    public imageUploadService: ImageUploadService
  ) {}

  get urlLinks(): FormArray {
    return this.companyFormGroup.get('urlLinks') as FormArray;
  }

  ngOnInit(): void {
    this.rolesData = this.rolesData.filter(r => r.id != 3);
    this.loadData();
    this.loadData$.next();
    this.titleService.setTitle('companyManagement.addCompanyLabel');

    this.companyFormGroup.controls['name'].patchValue(this.activatedRoute.snapshot.params['name']);
  }

  saveData(): void {
    const formData = this.companyFormGroup.value;
    formData.mdmKey = this.companyFormGroup.getRawValue().mdmKeyId.name + this.companyFormGroup.getRawValue().mdmValue;
    if (
      (this.companyFormGroup.getRawValue().mdmKeyId.id == 0 ||
        this.companyFormGroup.getRawValue().mdmKeyId.id == undefined) &&
      this.companyFormGroup.getRawValue().mdmValue == ''
    ) {
      formData.mdmKey = null;
    }
    if (Number.isNaN(formData.mdmKey) || formData.mdmKey == 'Select One') {
      formData.mdmKey = null;
    }
    this.companyFormGroup.controls['mdmKey'].patchValue(formData.mdmKey);
    this.showMDMError =
      this.companyFormGroup.controls['mdmKey'].status === FormControlStatusEnum.Invalid ? true : false;
    this.formSubmitted = true;
    this.countryError = false;
    if (
      this.aboutLength > this.aboutTextMaxLength ||
      !this.companyFormGroup.controls['about'].value ||
      this.companyFormGroup.controls['about'].value === '<br>'
    ) {
      return;
    }
    if (this.companyFormGroup.controls['about'].value.length > this.aboutMaxLength) {
      this.snackbarService.showError(
        this.translateService.instant('companyManagement.form.aboutFormattingMaxLengthError')
      );
      return;
    }
    if (this.companyFormGroup.status === FormControlStatusEnum.Invalid) {
      Object.keys(this.companyFormGroup.value).map(key => {
        this.companyFormGroup.controls[key].markAsDirty();
        this.companyFormGroup.controls[key].markAsTouched();
      });
      if (!this.selectedCountry) {
        this.countryError = true;
      }
    }

    if (this.companyFormGroup.invalid || this.countryError) return;

    if (
      this.companyFormGroup.value?.urlLinks?.length === 1 &&
      (!this.companyFormGroup.value?.urlLinks[0].urlLink || !this.companyFormGroup.value?.urlLinks[0].urlName)
    ) {
      delete this.companyFormGroup.value?.urlLinks;
    } else {
      this.companyFormGroup.value.urlLinks = this.companyFormGroup.value.urlLinks.filter(
        link => link.urlLink && link.urlName
      );
    }

    this.isAdd ? this.addCompany() : this.saveChanges();
  }

  addLink(name: string = '', url: string = ''): void {
    const linkForm = this.formBuilder.group(
      {
        urlLink: [url, [CustomValidator.url]],
        urlName: [name]
      },
      { validators: CustomValidator.urlValidator() }
    );
    this.urlLinks.push(linkForm);
  }

  removeLink(index: number) {
    this.urlLinks.removeAt(index);
  }

  cancelClick(): void {
    this.router.navigate(['./admin/company-management']);
  }

  getFormArray(property: string): FormArray {
    return this.companyFormGroup.get(property) as FormArray;
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
        .uploadImage(BlobTypeEnum.Companies, formData)
        .pipe(untilDestroyed(this))
        .subscribe(image => {
          this.companyFormGroup.get('imageLogo').patchValue(image.name);

          if (this.formData) {
            this.formData.image = image;
          } else {
            this.formData = {
              image: image
            } as CompanyInterface;
          }
        });
    }
  }

  removeIcon(): void {
    this.companyFormGroup.get('imageLogo').patchValue(null);
    this.formData.image = null;
    this.fileInput.nativeElement.value = null;
  }

  hasError(controlName: string): boolean {
    const control = this.companyFormGroup?.get(controlName);

    return control?.invalid && control?.touched && control?.dirty;
  }

  ulrLinkHasError(controlName: string, index: number): boolean {
    const control = this.urlLinks.controls[index].get(controlName);

    return control?.invalid && control?.touched && control?.dirty;
  }

  roleChange(role: TagInterface) {
    this.disableSolutionCapabilities = role.id === this.corporationTypeIndex;
    this.disableOffsitePPAs = this.disableOffsitePPAs || this.disableSolutionCapabilities;

    if (this.disableSolutionCapabilities) {
      this.categoryList
        .filter(p => p.selected)
        .forEach(p => {
          this.setCheckBoxElement(p, 'categories');
          p.selected = false;
        });

      this.companyFormGroup.get('categories').setValidators(null);
    } else {
      this.companyFormGroup.get('categories').setValidators([Validators.required]);
    }
    this.companyFormGroup.get('categories').updateValueAndValidity();

    if (this.disableOffsitePPAs) {
      this.offsitePPAList
        .filter(p => p.selected)
        .forEach(p => {
          this.setCheckBoxElement(p, 'offsitePPAIds');
          p.selected = false;
        });
    }
  }

  setCategory(category: TagInterface) {
    this.setCheckBoxElement(category, 'categories');

    this.categoryList.forEach(p => {
      if (p.id === category.id) p.selected = !p.selected;
    });

    if (category.id === this.offsitePPACategoryIndex) {
      this.disableOffsitePPAs = !this.getFormArray('categories').value.includes(this.offsitePPACategoryIndex);
    }

    if (this.disableOffsitePPAs) {
      this.offsitePPAList
        .filter(p => p.selected)
        .forEach(p => {
          this.setCheckBoxElement(p, 'offsitePPAIds');
          p.selected = false;
        });
    }
  }

  setOffsitePPA(offsitePPA: TagInterface) {
    this.setCheckBoxElement(offsitePPA, 'offsitePPAIds');

    this.offsitePPAList.forEach(p => {
      if (p.id === offsitePPA.id) p.selected = !p.selected;
    });
  }

  setCheckBoxElement(element: TagInterface, listName: string) {
    let formArray = this.getFormArray(listName);

    if (formArray?.length) {
      const index = formArray?.controls.findIndex(item => item.value === element.id);

      if (index >= 0) {
        formArray.removeAt(index);
      } else {
        formArray.push(new FormControl(element.id));
      }
    } else {
      formArray.push(new FormControl(element.id));
    }
  }

  loadData(): void {
    this.loadData$
      .pipe(
        untilDestroyed(this),
        switchMap(() => {
          return combineLatest([
            this.httpService.get<TagInterface[]>(CommonApiEnum.Industries),
            this.httpService.get<TagInterface[]>(CommonApiEnum.Categories),
            this.httpService.get<TagInterface[]>(CommonApiEnum.OffsitePPAs)
          ]);
        })
      )
      .subscribe(([industries, categories, ofsitePPas]) => {
        this.industriesList = industries;
        this.categoryList = categories;
        this.offsitePPAList = ofsitePPas;
        let offsiteTag = this.categoryList.filter(c => c.name.toLowerCase().includes('offsite'))[0];
        let corporationTag = this.rolesData.filter(r => r.name.toLowerCase().includes('corporation'))[0];

        this.offsitePPACategoryIndex = offsiteTag?.id;
        this.corporationTypeIndex = corporationTag?.id;

        let formArray = this.getFormArray('categories');

        if (formArray?.length) {
          const index = formArray?.controls.findIndex(item => item.value === this.offsitePPACategoryIndex);

          this.disableOffsitePPAs = index < 0;
        } else {
          this.disableOffsitePPAs = true;
        }

        this.listenForActivatedRoute();
        this.aboutLength = this.coreService.convertToPlain(this.companyFormGroup.controls['about'].value ?? '').length;
        this.mdmKeys = this.tempKeys.filter(value => value.id != 0);
      });
  }

  getCountries(searchStr?: string): void {
    this.countriesService.getCountriesList(searchStr).subscribe((c: CountryInterface[]) => {
      this.countriesList = c.map(country => ({
        id: country.id,
        name: country.name
      }));
    });
  }

  chooseCountry(country: CountryInterface): void {
    this.selectedCountry = country;
    this.countryError = false;
    this.companyFormGroup.get('countryId').patchValue(country.id);
  }

  clearSearch(): void {
    this.countriesList = [];
    this.selectedCountry = null;
    this.countryError = false;
    this.companyFormGroup.get('countryId').patchValue(null);
  }

  private listenForActivatedRoute(): void {
    this.activatedRoute.params
      .pipe(
        untilDestroyed(this),
        filter(params => params?.id),
        switchMap(params =>
          this.httpService.get<CompanyInterface>(`${CommonApiEnum.Companies}/${params.id}`, {
            expand: 'country,offsiteppas,categories,urllinks,image'
          })
        )
      )
      .subscribe(response => {
        this.formData = response;
        this.isSECompany = response.id === this.seCompanyId;
        this.patchData();
        this.titleService.setTitle('companyManagement.editCompanyLabel', this.formData.title);
      });
  }

  private patchData(): void {
    const mdmValue = this.mdmKeys.find(value => value.name === this.formData?.mdmKey.split('-')[0] + '-');
    this.mdmKeys = this.tempKeys.filter(value => value.name != this.formData?.mdmKey.split('-')[0] + '-');
    this.companyFormGroup.patchValue({
      name: this.formData?.name,
      companyUrl: this.formData?.companyUrl,
      industryId: {
        id: this.formData?.industryId,
        name: this.formData?.industryName
      },
      countryId: this.formData?.countryId,
      imageLogo: this.formData?.imageLogo,
      linkedInUrl: this.formData?.linkedInUrl,
      mdmKey: this.formData?.mdmKey,
      about: this.formData?.about,
      statusId: this.formData?.statusId,
      typeId: this.formData.typeId,
      mdmKeyId: { id: mdmValue?.id, name: mdmValue?.name },
      mdmValue: this.formData?.mdmKey.split('-')[1]
    });
    this.selectedCountry = this.formData.country;
    let links = this.companyFormGroup?.get('urlLinks') as FormArray;

    if (this.formData?.urlLinks.length) links.controls = [];

    this.formData?.urlLinks.forEach(l => {
      links.push(
        this.formBuilder.group(
          {
            urlName: new FormControl(l.urlName),
            urlLink: new FormControl(l.urlLink, [CustomValidator.url])
          },
          { validators: CustomValidator.urlValidator() }
        )
      );
      this.companyFormGroup.get('urlLinks')?.updateValueAndValidity();
    });

    const capabilities = this.companyFormGroup?.get('categories') as FormArray;
    this.formData?.categories.forEach(c => {
      capabilities.push(new FormControl(c.id));
      this.categoryList.filter(cat => cat.id === c.id)[0].selected = true;
    });

    this.disableOffsitePPAs = this.formData?.categories.filter(c => c.id === this.offsitePPACategoryIndex).length === 0;

    this.disableSolutionCapabilities = this.formData?.typeId === this.corporationTypeIndex;

    const offsites = this.companyFormGroup?.get('offsitePPAIds') as FormArray;
    this.formData?.offsitePPAs.forEach(o => {
      offsites.push(new FormControl(o.id));
      this.offsitePPAList.filter(off => off.id === o.id)[0].selected = true;
    });
    this.aboutLength = this.coreService.convertToPlain(this.companyFormGroup.controls['about'].value ?? '').length;
  }

  private addCompany(): void {
    const formData = this.companyFormGroup.value;
    formData['image'] = this.formData?.image;
    formData.industryId = this.companyFormGroup.getRawValue().industryId.id;
    formData.countryId = this.selectedCountry.id;
    formData.categories = this.categoryList.filter(c => c.selected && !this.disableSolutionCapabilities);
    formData.offsitePPAs = this.offsitePPAList.filter(o => o.selected && !this.disableOffsitePPAs);
    if (this.companyFormGroup.get('about').value) {
      formData.aboutText = this.coreService.convertToPlain(this.companyFormGroup.get('about').value ?? '');
    }
    this.httpService
      .post<CompanyInterface>(CommonApiEnum.Companies, formData)
      .pipe(
        catchError(error => {
          this.titleApiError = error?.error?.errors;
          if (this.titleApiError) {
            this.companyFormGroup.get('name').markAsDirty();
            this.companyFormGroup.get('name').markAsTouched();
            this.titleApiError = 'companyManagement.form.uniqueCompanyNameLabel';
          }

          const errorName: string = Object.keys(error.error.errors)[0]; // !! model on the BE should be the same in every case
          this.snackbarService.showError(error.error.errors[errorName][0]);
          return throwError(error);
        })
      )
      .subscribe(() => {
        this.snackbarService.showSuccess('companyManagement.form.addCompanySuccessLabel');

        this.cancelClick();
      });
  }

  private saveChanges(): void {
    const formData = this.companyFormGroup.value;
    formData['image'] = this.formData.image;
    formData.industryId = this.companyFormGroup.getRawValue().industryId.id;
    formData.countryId = this.selectedCountry.id;
    formData.categories = this.categoryList.filter(c => c.selected && !this.disableSolutionCapabilities);
    formData.offsitePPAs = this.offsitePPAList.filter(o => o.selected && !this.disableOffsitePPAs);
    if (this.companyFormGroup.get('about').value) {
      formData.aboutText = this.coreService.convertToPlain(this.companyFormGroup.get('about').value ?? '');
    }
    this.httpService
      .put<unknown>(`${CommonApiEnum.Companies}/${this.formData.id}`, formData)
      .pipe(
        catchError(error => {
          this.titleApiError = error?.error?.errors;

          if (this.titleApiError) {
            this.companyFormGroup.get('name').markAsDirty();
            this.companyFormGroup.get('name').markAsTouched();
            this.titleApiError = 'companyManagement.form.uniqueCompanyNameLabel';
          }

          const errorName: string = Object.keys(error.error.errors)[0]; // !! model on the BE should be the same in every case
          this.snackbarService.showError(error.error.errors[errorName][0]);
          return throwError(error);
        })
      )
      .subscribe(() => {
        this.snackbarService.showSuccess('companyManagement.form.editCompanySuccessLabel');

        this.cancelClick();
      });
  }

  onAboutLengthChanged(value: number) {
    this.aboutLength = value;
  }
  onMDMKeyChange(selectedMDM: TagInterface) {
    this.selectedMDM = selectedMDM;
    if (this.selectedMDM.id == 0) {
      this.mdmValues = '';
    }
    this.mdmKeys =
      this.selectedMDM.id != undefined
        ? this.tempKeys.filter(value => value.id != this.selectedMDM.id)
        : this.tempKeys.filter(value => value.id != 0);
    this.showMDMError =
      (this.selectedMDM.id > 0 && this.companyFormGroup.getRawValue().mdmValue != '') ||
      (this.selectedMDM == null && this.companyFormGroup.getRawValue().mdmValue == '') ||
      this.selectedMDM.id == 0
        ? false
        : true;
  }
  onMDMValueChange(event: KeyboardEvent) {
    this.showMDMError =
      (this.companyFormGroup.getRawValue().mdmKeyId.id > 0 && this.companyFormGroup.getRawValue().mdmValue != '') ||
      (this.companyFormGroup.getRawValue().mdmKeyId == null && this.companyFormGroup.getRawValue().mdmValue == '') ||
      (this.companyFormGroup.getRawValue().mdmKeyId.id == 0 && this.companyFormGroup.getRawValue().mdmValue == '')
        ? false
        : true;
  }
}
