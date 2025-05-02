import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { combineLatest, filter, Subject, takeUntil } from 'rxjs';
import { CompanyAnnouncementInterface } from 'src/app/core/interfaces/company-announcement-interface';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { CompanyProfileService } from '../services/company-profile.service';
import { TranslateService } from '@ngx-translate/core';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { RegionScaleTypeEnum } from 'src/app/initiatives/shared/enums/geographic-scale-type.enum';
import { ViewportScroller } from '@angular/common';
import { CustomValidator } from 'src/app/shared/validators/custom.validator';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { CommonService } from 'src/app/core/services/common.service';
import { InitialDataInterface } from 'src/app/core/interfaces/initial-data.interface';
import { CountryInterface } from 'src/app/shared/interfaces/user/country.interface';
import structuredClone from '@ungap/structured-clone';

@Component({
  selector: 'neo-edit-company-announcement',
  templateUrl: './edit-company-announcement.component.html',
  styleUrls: ['./edit-company-announcement.component.scss']
})
export class EditCompanyAnnouncementComponent implements OnInit, OnDestroy {
  @Output() closeModal = new EventEmitter<void>();
  @Output() announcementUpdated = new EventEmitter<boolean>();
  @ViewChild('modalContent') modalContent: ElementRef; // Reference to the modal content

  private unsubscribe$ = new Subject<void>();
  companyAnnouncementForm: FormGroup;
  formSubmitted: boolean;
  currentUser: UserInterface;
  initialData: InitialDataInterface;
  continents: CountryInterface[];
  @Input() announcementId: number;
  @Input() companyId: number;
  announcementRegions: TagInterface[] = [];
  announcementScalesList: TagInterface[];
  announcementScaleCustomTitle: string;
  maxTitleLength = 100;

  constructor(
    private companyProfileService: CompanyProfileService,
    private snackbarService: SnackbarService,
    private viewPort: ViewportScroller,
    private translateService: TranslateService,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private commonService: CommonService
  ) {
    // Initia the form in constructor
    this.initializeForm();
  }

  ngOnInit(): void {
    combineLatest([
      this.authService.currentUser$,
      this.commonService.getRegionScaleTypes(),
      this.commonService.initialData(),
      this.companyProfileService.getCompanyAnnouncementById(this.announcementId)
    ])
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(
          ([currentUser, scaleData, initialData, announcementDetails]) =>
            !!currentUser && !!scaleData && !!initialData && !!announcementDetails
        )
      )
      .subscribe({
        next: ([currentUser, scaleData, initialData, announcementDetails]) => {
          if (currentUser != null) {
            this.currentUser = currentUser;
            this.announcementScalesList = scaleData;
            this.initialData = initialData;
            this.continents = structuredClone(initialData.regions.filter(r => r.parentId === 0));
            this.fetchCompanyAnnouncementDetails(announcementDetails);
          }
        },
        error: error => {
          this.snackbarService.showError('general.defaultErrorLabel');
        }
      });
  }

  initializeForm(): void {
    this.companyAnnouncementForm = this.formBuilder.group({
      title: [
        '',
        [CustomValidator.required, Validators.maxLength(this.maxTitleLength), Validators.pattern(/^[a-zA-Z0-9\s]*$/)]
      ],
      link: ['', [Validators.required, CustomValidator.url]],
      regions: [[], Validators.required],
      scale: ['', Validators.required]
    });
  }

  fetchCompanyAnnouncementDetails(data: CompanyAnnouncementInterface): void {
    this.companyAnnouncementForm.patchValue({
      title: data.title,
      link: data.link,
      scale: this.announcementScalesList.find(x => x.id == data.scaleId),
      regions: data.regions
    });
    this.announcementRegions = data.regions;
  }

  onScaleChange(event: TagInterface): void {
    this.announcementScaleCustomTitle =
      event.id === RegionScaleTypeEnum.State
        ? 'companyProfile.formContent.chooseStateLeabel'
        : event.id === RegionScaleTypeEnum.National
        ? 'companyProfile.formContent.selectRegionCountryLabel'
        : 'companyProfile.formContent.selectRegionLabel';
    setTimeout(() => this.scrollToElement('regionsListControl'));
  }

  scrollToElement(elementId: string): void {
    const element = this.modalContent.nativeElement.querySelector(`#${elementId}`);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  updateRegions(regions: TagInterface[]): void {
    this.companyAnnouncementForm.get('regions').setValue(regions);
    this.announcementRegions = regions;
  }

  save(): void {
    this.formSubmitted = true;

    if (!this.companyAnnouncementForm.valid) {
      Object.keys(this.companyAnnouncementForm.controls).forEach(key => {
        const control = this.companyAnnouncementForm.get(key);
        if (control) {
          control.markAsTouched();
        }
      });
      return;
    }

    let announcementData: CompanyAnnouncementInterface = {
      title: this.companyAnnouncementForm.get('title')?.value,
      link: this.companyAnnouncementForm.get('link')?.value,
      scaleId: this.companyAnnouncementForm.get('scale').value.id,
      regionIds: this.companyAnnouncementForm.get('regions').value.map((region: TagInterface) => region.id),
      companyId: this.companyId,
      id: this.announcementId
    };

    this.companyProfileService
      .createOrUpdateAnnouncement(announcementData)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe({
        next: response => {
          this.snackbarService.showSuccess(this.translateService.instant('companyProfile.announcementUpdated'));
          this.announcementUpdated.emit(true);
          this.close();
        },
        error: error => {
          this.snackbarService.showError(this.translateService.instant('companyProfile.errorWhileSavingAnnouncement'));
        }
      });
  }

  close(): void {
    this.closeModal.emit();
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
