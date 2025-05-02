import { Component, OnInit } from '@angular/core';
import { AnnouncementInterface } from '../interfaces/announcement.interface';
import { AnnouncementsService } from '../services/announcements.service';
import { DEFAULT_PER_PAGE, PaginationInterface } from '../../../../shared/modules/pagination/pagination.component';
import { TitleService } from '../../../../core/services/title.service';

@Component({
  selector: 'neo-announcement',
  templateUrl: 'announcement.component.html',
  styleUrls: ['announcement.component.scss']
})
export class AnnouncementComponent implements OnInit {
  defaultItemPerPage = DEFAULT_PER_PAGE;
  announcementsList: AnnouncementInterface[];
  nameAsc: boolean = true;
  statusAsc: boolean;
  audienceAsc: boolean;
  paging: PaginationInterface;
  tdTitleClick: string;

  sortingCriteria: Record<string, string> = {
    name: 'name',
    isactive: 'isactive',
    audience: 'audience'
  };

  constructor(private announcementService: AnnouncementsService, private titleService: TitleService) {}

  ngOnInit(): void {
    this.titleService.setTitle('announcement.announcementsLabel');
    this.getAnnouncements();
  }

  getAnnouncements(Skip: number = 0, OrderBy: string = ''): void {
    this.announcementService.getAnnouncementsList(Skip, OrderBy).subscribe(list => {
      this.announcementsList = list.dataList;

      this.paging = {
        ...this.paging,
        skip: Skip,
        take: this.defaultItemPerPage,
        total: list.count
      };
    });
  }

  sortCriteriaSelection(sortDirection: string, sortKey: string, secondSortCol: string, thirdSortCol: string): void {
    this.tdTitleClick = sortKey;
    if (this[sortDirection]) {
      this.getAnnouncements(0, `${this.sortingCriteria[sortKey]}.desc`);
    } else {
      this.getAnnouncements(0, `${this.sortingCriteria[sortKey]}.asc`);
    }

    this[secondSortCol] = false;
    this[thirdSortCol] = false;
    this[sortDirection] = !this[sortDirection];
  }

  updatePaging(page: number) {
    const skip: number = Math.round((page - 1) * this.defaultItemPerPage);
    this.getAnnouncements(skip);
  }
}
