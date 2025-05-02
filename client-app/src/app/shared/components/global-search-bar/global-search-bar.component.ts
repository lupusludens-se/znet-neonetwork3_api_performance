import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { PaginationInterface } from '../../modules/pagination/pagination.component';
import { HttpService } from 'src/app/core/services/http.service';
import { CoreService } from 'src/app/core/services/core.service';
import { TopicInterface } from '../../interfaces/topic.interface';
import { TopicApiEnum } from '../../enums/topic-api.enum';
import { ContentInterface } from '../../interfaces/content.interface';
import { TopicTypeEnum } from '../../enums/topic-type.enum';
import { Router } from '@angular/router';
import { SnackbarService } from 'src/app/core/services/snackbar.service';

@Component({
  selector: 'neo-global-search-bar',
  templateUrl: './global-search-bar.component.html',
  styleUrls: ['./global-search-bar.component.scss']
})
export class GlobalSearchBarComponent {
  constructor(
    private readonly httpService: HttpService,
    private readonly coreService: CoreService,
    private readonly router: Router,
    private elRef: ElementRef,
    protected snackbarService: SnackbarService
  ) {}

  @ViewChild('searchInput') searchInput;
  value: string;
  paging: PaginationInterface = {
    total: null,
    take: 5,
    skip: 0
  };

  results: ContentInterface<TopicTypeEnum>[];
  showResult: boolean = false;
  clickOutside: boolean;

  @HostListener('document:click', ['$event'])
  documentClick(event: Event) {
    this.clickOutside = !this.elRef.nativeElement.contains(event.target);

    if (this.clickOutside && this.results?.length > 0) {
      this.results = [];
    }
  }

  search(): void {
    if (this.value?.length > 2) {
      this.showResult = false;
      const paging = this.coreService.deleteEmptyProps({
        ...this.paging,
        includeCount: true,
        entityType: null
      });
      this.httpService
        .get<TopicInterface>(
          TopicApiEnum.Topics,
          this.coreService.deleteEmptyProps({
            ...paging,
            search: this.value
          })
        )
        .subscribe(response => {
          this.results = response.dataList;
          this.showResult = true;
        });
    }
  }

  clearSearch(): void {
    this.value = '';
    this.results = [];
    this.showResult = false;
  }

  getLink(content: ContentInterface<TopicTypeEnum>): string {
    switch (content?.type) {
      case TopicTypeEnum.Learn:
        return `/learn/${content?.id}`;
      case TopicTypeEnum.Forum:
        return `/forum/topic/${content?.id}`;
      case TopicTypeEnum.Project:
        return `/projects/${content?.id}`;
      case TopicTypeEnum.Event:
        return `/events/${content?.id}`;
      case TopicTypeEnum.Company:
        return `/company-profile/${content?.id}`;
    }
  }

  getAll() {
    this.showResult = false;
    if (this.value?.length > 2) {
      this.router.navigate(['/search'], { queryParams: { data: this.value } }).then();
    } else {
      this.snackbarService.showError('general.searchMinCharactersLabel');
    }
  }

  getKey(content: ContentInterface<TopicTypeEnum>): string {
    switch (content.type) {
      case TopicTypeEnum.Project:
        return 'projects';
      case TopicTypeEnum.Learn:
        return 'learn';
      case TopicTypeEnum.Event:
        return 'events';
      case TopicTypeEnum.Forum:
        return 'forum';
      case TopicTypeEnum.Company:
        return 'building';
      default:
        return 'topic';
    }
  }
}
