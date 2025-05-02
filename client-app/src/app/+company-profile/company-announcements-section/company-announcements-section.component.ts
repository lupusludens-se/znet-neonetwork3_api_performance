import { DatePipe } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { CompanyAnnouncementInterface } from 'src/app/core/interfaces/company-announcement-interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { CompanyProfileService } from '../services/company-profile.service';
import { TranslateService } from '@ngx-translate/core';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';

@Component({
  selector: 'neo-company-announcements-section',
  templateUrl: './company-announcements-section.component.html',
  styleUrls: ['./company-announcements-section.component.scss'],
  providers: [DatePipe]
})
export class CompanyAnnouncementsSectionComponent implements OnInit, OnDestroy {
  showAnnouncementModal: boolean = false;
  showEditAnnouncementModal: boolean = false;
  currentUser: UserInterface;
  roles = RolesEnum;
  companyId: number;
  announcementId: number;
  private unsubscribe$ = new Subject<void>();
  showDeleteModal = false;
  announcementList: CompanyAnnouncementInterface[] = [];
  options: MenuOptionInterface[] = [
    {
      icon: 'pencil',
      name: 'actions.editLabel',
      operation: TableCrudEnum.Edit
    },
    {
      icon: 'trash-can-red',
      name: 'actions.deleteLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];

  constructor(
    private authService: AuthService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly snackbarService: SnackbarService,
    private companyProfileService: CompanyProfileService,
    private readonly translateService: TranslateService
  ) {}

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(() => {
      this.companyId = this.activatedRoute.snapshot.params.id;
      this.authService.currentUser().subscribe(user => {
        if (user) {
          this.currentUser = user;
          this.getCompanyAnnouncements();
        }
      });
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  getCompanyAnnouncements(): void {
    this.companyProfileService
      .getCompanyAnnouncements(this.companyId)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe({
        next: response => {
          this.announcementList = response.dataList;
        },
        error: error => {
          this.snackbarService.showError(
            this.translateService.instant('companyProfile.errorAnnouncementsRetrievingLabel')
          );
        }
      });
  }

  checkIfUserCanAccess(): boolean {
    return (
      this.isInRole(this.currentUser, this.roles.Admin) ||
      (this.currentUser.companyId.toString() === this.companyId.toString() &&
        (this.isInRole(this.currentUser, this.roles.SolutionProvider) ||
          this.isInRole(this.currentUser, this.roles.SPAdmin)))
    );
  }

  isInRole(currentUser: UserInterface, roleId: RolesEnum): boolean {
    return currentUser?.roles.some(role => role.id === roleId && role.isSpecial) ?? false;
  }

  openAnnouncementLink(announcement: CompanyAnnouncementInterface) {
    window.open(announcement.link, '_blank');
  }

  showAnnouncementmodal() {
    this.showAnnouncementModal = true;
  }

  // Add the method to close the modal
  closeModal() {
    this.showAnnouncementModal = false;
  }

  showEditAnnouncementmodal() {
    this.showEditAnnouncementModal = true;
  }

  // Add the method to close the modal
  closeEditAnnouncementModal() {
    this.showEditAnnouncementModal = false;
  }

  announcementAdded(event: boolean) {
    if (event) {
      this.getCompanyAnnouncements();
    }
  }

  getRegionNames(regions: TagInterface[]): string {
    return regions
      .map(region => region.name)
      .slice(0, 2)
      .join(', ');
  }

  getRegionStringTooltip(regions: TagInterface[]): string {
    return regions
      .map(region => region.name)
      .slice(2)
      .join(', ');
  }

  optionClick(event: any, announcementId: number): void {
    this.announcementId = announcementId;
    if (event.operation === TableCrudEnum.Edit) {
      this.showEditAnnouncementModal = true;
    }
    if (event.operation === TableCrudEnum.Delete) {
      this.showDeleteModal = true;
    }
  }

  confirmDelete(): void {
    this.companyProfileService
      .deleteAnnouncement(this.announcementId)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe({
        next: response => {
          if (response) {
            this.snackbarService.showSuccess(this.translateService.instant('companyProfile.announcementDeleted'));
            this.getCompanyAnnouncements();
          }
          this.showDeleteModal = false;
        },
        error: error => {
          this.snackbarService.showError(
            this.translateService.instant('companyProfile.errorWhileDeletingAnnouncement')
          );
        }
      });
  }
}
