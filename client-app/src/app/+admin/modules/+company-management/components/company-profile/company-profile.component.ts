import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CompanyInterface } from 'src/app/shared/interfaces/user/company.interface';
import { CustomValidator } from 'src/app/shared/validators/custom.validator';
import { CompaniesSchema } from '../../constants/parameter.const';
import { CoreService } from 'src/app/core/services/core.service';
import { TranslateService } from '@ngx-translate/core';
import { HttpService } from 'src/app/core/services/http.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { TitleService } from 'src/app/core/services/title.service';
import { CountriesService } from 'src/app/shared/services/countries.service';
import { Router } from '@angular/router';
import { Subject, catchError, throwError } from 'rxjs';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { UserPermissionInterface } from 'src/app/shared/interfaces/user/user-permission.interface';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { CountryInterface } from 'src/app/shared/interfaces/user/country.interface';
import { ImageInterface } from 'src/app/shared/interfaces/image.interface';
import { BlobTypeEnum, GeneralApiEnum } from 'src/app/shared/enums/api/general-api.enum';
import { MAX_IMAGE_SIZE } from 'src/app/shared/constants/image-size.const';
import { SearchResultInterface } from 'src/app/shared/interfaces/search-result.interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { FormControlStatusEnum } from 'src/app/shared/enums/form-control-status.enum';
@UntilDestroy()
@Component({
  selector: 'neo-company-profile',
  templateUrl: './company-profile.component.html',
  styleUrls: ['./company-profile.component.scss']
})
export class CompanyProfileComponent implements OnInit {
  formData: CompanyInterface;
  @ViewChild('fileInput') fileInput: ElementRef;
  companyNameMinLength: number = 2;
  companyNameMaxLength: number = 250;
  maxImageSize = MAX_IMAGE_SIZE;
  imageLarge: boolean;
  industriesList: TagInterface[];
  currentUser: UserInterface;
  permissionsData: UserPermissionInterface[];
  selectedCountry: CountryInterface;
  titleApiError: string;
  countryError: boolean;
  countriesList: SearchResultInterface[];
  companyID: number;
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
    linkedInUrl: new FormControl(null, [CustomValidator.linkedInUrl]),
    statusId: new FormControl(),
    companyId: new FormControl(),
    mdmKey: new FormControl(),
    categories: new FormControl([]),
    about: new FormControl(''),
    urlLinks: this.formBuilder.array([
      this.formBuilder.group({
        urlName: new FormControl(''),
        urlLink: new FormControl('', [CustomValidator.url])
      }, { validators: CustomValidator.urlValidator() })
    ]),
    typeId: new FormControl(null, [Validators.required]),
    imageLogo: new FormControl(null)
  });
  formSubmitted: boolean;
  aboutLength: number = 0;
  aboutMaxLength = CompaniesSchema.aboutMaxLength;
  aboutTextMaxLength = CompaniesSchema.aboutTextMaxLength;
  loadData$: Subject<void> = new Subject<void>();
  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly httpService: HttpService,
    private readonly snackbarService: SnackbarService,
    private readonly router: Router,
    private readonly titleService: TitleService,
    private countriesService: CountriesService,
    public coreService: CoreService,
    public translateService: TranslateService,
    private readonly authService: AuthService
  ) { }

  ngOnInit(): void {
    this.listenForCurrentUser();
    this.titleService.setTitle('companyManagement.pageTitleLabel');
  }
  private listenForCurrentUser(): any {
    this.authService
      .currentUser()
      .pipe(untilDestroyed(this))
      .subscribe(currentUser => {
        if (currentUser !== null) {
          this.companyID = currentUser.companyId;
          this.loadData();
        }
      });
  }
  loadData(): void {
    this.listenForActivatedRoute();
  }
  private listenForActivatedRoute(): void {
    this.httpService
      .get<CompanyInterface>(`${CommonApiEnum.Companies}/${this.companyID}`, {
        expand: 'country,offsiteppas,categories,urllinks,image'
      })
      .subscribe(data => {
        this.formData = data;
        this.patchData();
      });
  }
  private patchData(): void {
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
      about: this.formData?.about,
      statusId: this.formData?.statusId,
      typeId: this.formData.typeId,
      mdmKey: this.formData?.mdmKey,
      categories: this.formData.categories,
      companyId: this.formData.id
    });
    this.selectedCountry = this.formData.country;
    let links = this.companyFormGroup?.get('urlLinks') as FormArray;

    if (this.formData?.urlLinks.length) links.controls = [];

    this.formData?.urlLinks.forEach(l => {
      links.push(
        this.formBuilder.group({
          urlName: new FormControl(l.urlName),
          urlLink: new FormControl(l.urlLink, [CustomValidator.url])
        }, { validators: CustomValidator.urlValidator() })
      );
      this.companyFormGroup.get("urlLinks")?.updateValueAndValidity();
    });


    this.aboutLength = this.coreService.convertToPlain(this.companyFormGroup.controls['about'].value ?? '').length;
  }
  chooseCountry(country: CountryInterface): void {
    this.selectedCountry = country;
    this.companyFormGroup.get('countryId').patchValue(country.id);
  }
  saveData(): void {
    this.formSubmitted = true;
    this.isAboutCompanyEmpty();
    this.countryError = false;
    if (
      this.aboutLength > this.aboutTextMaxLength ||
      !this.companyFormGroup.controls['about'].value ||
      this.companyFormGroup.controls['about'].value === '<br>' ||
      this.isAboutCompanyEmpty()
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
    }

    this.saveChanges();
  }
  private saveChanges(): void {
    const formData = this.companyFormGroup.value;
    formData['image'] = this.formData.image;
    formData.industryId = this.companyFormGroup.getRawValue().industryId.id;
    formData.countryId = this.selectedCountry.id;
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
  cancelClick(): void {
    this.router.navigate(['./manage/']);
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

      this.httpService
        .post<ImageInterface>(
          GeneralApiEnum.Media,
          formData,
          {
            BlobType: BlobTypeEnum.Companies,
            IsLogoOnlyAllowed : true
          },
          false
        )
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
  clearSearch(): void {
    this.countriesList = [];
    this.selectedCountry = null;
    this.countryError = false;
    this.companyFormGroup.get('countryId').patchValue(null);
  }
  getCountries(searchStr?: string): void {
    this.countriesService.getCountriesList(searchStr).subscribe((c: CountryInterface[]) => {
      this.countriesList = c.map(country => ({
        id: country.id,
        name: country.name
      }));
    });
  }
  get urlLinks(): FormArray {
    return this.companyFormGroup.get('urlLinks') as FormArray;
  }
  addLink(name: string = '', url: string = ''): void {
    const linkForm = this.formBuilder.group({
      urlLink: [url, [CustomValidator.url]],
      urlName: [name]
    }, { validators: CustomValidator.urlValidator() });
    this.urlLinks.push(linkForm);
  }


  ulrLinkHasError(controlName: string, index: number): boolean {
    const control = this.urlLinks.controls[index].get(controlName);

    return control?.invalid && control?.touched && control?.dirty;
  }
  removeLink(index: number) {
    this.urlLinks.removeAt(index);
  }
  isAboutCompanyEmpty(): boolean {
    const stringsToCheck = ['', ' '];
    const aboutText = this.companyFormGroup.controls['about'].value as string;
    const cleanedText = aboutText.replace(/\s/g, '').replace(/<div><br><\/div>/g, '');
    return stringsToCheck.includes(cleanedText.trim()) ? true : false;
  }
}
