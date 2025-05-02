import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { throwError } from 'rxjs';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { AnnouncementsService } from '../../services/announcements.service';
import { AnnouncementInterface } from '../../interfaces/announcement.interface';
import { MENU_OPTIONS, MenuOptionInterface } from '../../../../../shared/modules/menu/interfaces/menu-option.interface';
import { RolesEnum } from '../../../../../shared/enums/roles.enum';
import { TableCrudEnum } from '../../../../../shared/modules/table/enums/table-crud.enum';
import structuredClone from '@ungap/structured-clone';

@Component({
  selector: 'neo-announcement-table-row',
  templateUrl: 'announcement-table-row.component.html',
  styleUrls: ['../announcement.component.scss', 'announcement-table-row.component.scss']
})
export class AnnouncementTableRowComponent implements OnInit {
  @Output() updateAnnouncements: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() announcement: AnnouncementInterface;
  rolesList = RolesEnum;
  showOptions: boolean;
  showDeleteModal: boolean;
  showConfirmAnnouncement: boolean;
  menuOptions: MenuOptionInterface[] = structuredClone(MENU_OPTIONS);

  constructor(private router: Router, private announcementsService: AnnouncementsService) {}

  ngOnInit() {
    if (this.announcement.isActive) {
      this.menuOptions.map(opt => {
        if (opt.name === 'actions.deactivateLabel') opt.hidden = false;
        if (opt.name === 'actions.activateLabel') opt.hidden = true;
        if (opt.name === 'actions.previewLabel') opt.hidden = true;
      });
    } else {
      this.menuOptions.map(opt => {
        if (opt.name === 'actions.deactivateLabel') opt.hidden = true;
        if (opt.name === 'actions.activateLabel') opt.hidden = false;
        if (opt.name === 'actions.previewLabel') opt.hidden = true;
      });
    }
  }

  deleteAnnouncement(id: number): void {
    this.announcementsService.deleteAnnouncement(id).subscribe(() => {
      this.showDeleteModal = false;
      this.updateAnnouncements.emit(true);
    });
  }

  optionClick(option: MenuOptionInterface): void {
    switch (option.operation) {
      case TableCrudEnum.Edit:
        this.goToAnnouncement(this.announcement.id);
        break;
      case TableCrudEnum.Delete:
        this.showDeleteModal = true;
        break;
      case TableCrudEnum.Status:
        this.updateAnnouncementStatus(this.announcement.id, this.announcement.isActive ? 'false' : 'true');
        break;
    }
  }

  goToAnnouncement(id: number): void {
    this.router.navigate([`admin/announcements/edit/${id}`]).then();
  }

  updateAnnouncementStatus(id: number, status, param?: boolean) {
    this.announcementsService
      .updateAnnouncement(id, status, param)
      .pipe(
        catchError(err => {
          if (err.status === 409) {
            this.showConfirmAnnouncement = true;
          }
          return throwError(null);
        })
      )
      .subscribe(() => {
        this.updateAnnouncements.emit(true);
      });
  }
}
