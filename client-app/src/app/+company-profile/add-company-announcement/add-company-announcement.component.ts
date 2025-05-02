import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';

import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { filter, forkJoin, Subject, takeUntil } from 'rxjs';
import { CountryInterface } from 'src/app/shared/interfaces/user/country.interface';
import { CommonService } from 'src/app/core/services/common.service';
import { InitialDataInterface } from 'src/app/core/interfaces/initial-data.interface';
import structuredClone from '@ungap/structured-clone';
import { ViewportScroller } from '@angular/common';
import { RegionScaleTypeEnum } from 'src/app/initiatives/shared/enums/geographic-scale-type.enum';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { CompanyProfileService } from '../services/company-profile.service';
import { CompanyAnnouncementInterface } from 'src/app/core/interfaces/company-announcement-interface';
import { TranslateService } from '@ngx-translate/core';
import { CustomValidator } from 'src/app/shared/validators/custom.validator';

@Component({
  selector: 'neo-add-company-announcement',
  templateUrl: './add-company-announcement.component.html',
  styleUrls: ['./add-company-announcement.component.scss']
})
export class AddCompanyAnnouncementComponent implements OnInit, OnDestroy {
  @Output() closeModal = new EventEmitter<void>();
  @Output() announcementAdded = new EventEmitter<boolean>();
  @Input() companyId: number;
  @ViewChild('modalContent') modalContent: ElementRef; // Reference to the modal content

  maxTitleLength = 100;
  formSubmitted = false;
  showModal = false;
  form: FormGroup;
  announcementScaleCustomTitle: string;
  private unsubscribe$ = new Subject<void>();
  announcementScalesList: TagInterface[];
  announcementRegions: TagInterface[] = [];
  currentUser: UserInterface;
  initialData: InitialDataInterface;
  continents: CountryInterface[];
  constructor(
    private formBuilder: FormBuilder,
    private viewPort: ViewportScroller,
    private snackbarService: SnackbarService,
    private commonService: CommonService,
    private authService: AuthService,
    private companyProfileService: CompanyProfileService,
    private translateService: TranslateService
  ) {
    // Initialize the form in constructor
    this.initializeForm();
  }

  initializeForm(): void {
    this.form = this.formBuilder.group({
      title: [
        '',
        [CustomValidator.required, Validators.maxLength(this.maxTitleLength), Validators.pattern(/^[a-zA-Z0-9\s]*$/)]
      ],
      link: ['', [Validators.required, CustomValidator.url]],
      regions: [[], Validators.required],
      scale: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      if (user != null) {
        this.currentUser = user;
        this.loadData();
        this.commonService
          .initialData()
          .pipe(
            takeUntil(this.unsubscribe$),
            filter(response => !!response)
          )
          .subscribe(initialData => {
            this.initialData = initialData;
            this.continents = structuredClone(initialData.regions.filter(r => r.parentId === 0));
          });
      }
    });
  }

  save(): void {
    this.formSubmitted = true;

    if (!this.form.valid) {
      Object.keys(this.form.controls).forEach(key => {
        const control = this.form.get(key);
        if (control) {
          control.markAsTouched();
        }
      });
      return;
    }

    let announcementData: CompanyAnnouncementInterface = {
      title: this.form.get('title')?.value,
      link: this.form.get('link')?.value,
      scaleId: this.form.get('scale').value.id,
      regionIds: this.form.get('regions').value.map((region: TagInterface) => region.id),
      companyId: this.companyId
    };

    this.companyProfileService
      .createOrUpdateAnnouncement(announcementData)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe({
        next: response => {
          this.snackbarService.showSuccess(this.translateService.instant('companyProfile.announcementSaved'));
          this.announcementAdded.emit(true);
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

  loadData(): void {
    forkJoin({
      scaleData: this.commonService.getRegionScaleTypes()
    }).subscribe({
      next: (data: any) => {
        this.announcementScalesList = data.scaleData;
      }
    });
  }

  updateRegions(regions: TagInterface[]): void {
    this.form.get('regions').setValue(regions);
    this.announcementRegions = regions;
  }
}
