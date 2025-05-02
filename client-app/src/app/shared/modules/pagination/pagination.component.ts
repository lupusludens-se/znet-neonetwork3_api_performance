import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  HostListener,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';

export interface PaginationInterface {
  skip: number;
  take: number;
  orderBy?: number | string;
  asc?: boolean;
  total: number;
}

export interface PaginationIncludeCountInterface extends PaginationInterface {
  includeCount: boolean;
}

export const DEFAULT_PER_PAGE = 25;

const SMALL_WIDTH: number = 1200;
const MEDIUM_WIDTH: number = 1440;
const FULL_HD_WIDTH: number = 1920;

@Component({
  selector: 'neo-pagination',
  templateUrl: 'pagination.component.html',
  styleUrls: ['./pagination.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PaginationComponent implements OnChanges, OnInit {
  @Output() changePage: EventEmitter<number> = new EventEmitter<number>();
  @Input() paging: PaginationInterface;
  @Input() defaultItemPerPage: number = DEFAULT_PER_PAGE;

  currentPage: number = 1;
  pagesLeftRightShow: number = 4;
  pages: number[];
  lastPage: number;

  @HostListener('window:resize', ['$event'])
  onResize(event): void {
    if (event.target.innerWidth >= SMALL_WIDTH) {
      this.pagesLeftRightShow = 1;
    }

    if (event.target.innerWidth >= MEDIUM_WIDTH) {
      this.pagesLeftRightShow = 2;
    }

    if (event.target.innerWidth >= FULL_HD_WIDTH) {
      this.pagesLeftRightShow = 4;
    }

    this.setPages();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.paging.previousValue) {
      if (changes.paging.currentValue.skip === 0) {
        this.currentPage = 1;
      }

      this.lastPage = Math.ceil(this.paging.total / this.defaultItemPerPage);

      if (this.currentPage > this.lastPage) {
        // * handle change of items by deleting item
        this.currentPage = this.lastPage;
      }

      this.setPages();
      return;
    }

    if (this.paging) {
      this.currentPage = 1;
      this.lastPage = Math.ceil(this.paging.total / this.defaultItemPerPage);
      this.setPages();
    }
  }

  ngOnInit() {
    if (this.paging.skip > 0) {
      const pageNumber = this.paging.skip / this.defaultItemPerPage + 1;
      this.setPage(pageNumber);
      this.setPages();
    }
  }

  setPage(page: number) {
    if (page < 1 && page >= this.paging.total) return;
    this.currentPage = page;
    this.changePage.emit(this.currentPage);
  }

  setPages() {
    this.pages = [];

    for (let i = this.currentPage - this.pagesLeftRightShow; i <= this.currentPage + this.pagesLeftRightShow; i++) {
      if (i > 1 && i < this.lastPage) this.pages.push(i);
    }
  }
}
