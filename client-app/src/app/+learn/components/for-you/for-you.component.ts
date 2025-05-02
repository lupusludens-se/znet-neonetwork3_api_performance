import { Component, Input, OnChanges, EventEmitter, Output, SimpleChanges } from '@angular/core';

import { combineLatest, Observable, take } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { HttpService } from '../../../core/services/http.service';

import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import { PostInterface } from '../../interfaces/post.interface';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { TagInterface } from '../../../core/interfaces/tag.interface';

import { CommonApiEnum } from '../../../core/enums/common-api.enum';

import { FOR_YOU_PARAMETER, INITIAL_POSTS_TO_LOAD, POSTS_PER_PAGE } from '../../constants/parameter.const';
import { CommonService } from 'src/app/core/services/common.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { SpinnerService } from 'src/app/core/services/spinner.service';

@UntilDestroy()
@Component({
  selector: 'neo-for-you',
  styleUrls: ['./for-you.component.scss'],
  templateUrl: './for-you.component.html'
})
export class ForYouComponent implements OnChanges {
  @Input() currentUser: UserInterface;
  @Output() removeParentCssEmitter : EventEmitter<boolean> = new EventEmitter<boolean>();

  categories: TagInterface[];
  auth = AuthService;
  isUserLoggedIn: boolean = false;
  pageData: PaginateResponseInterface<PostInterface>[] = [];
  startIndex: number = 0;
  endIndex: number = 4;
  pagination: {
    skip: number;
    take: number;
    includeCount?: boolean;
  }[] = [
      {
        skip: 0,
        take: INITIAL_POSTS_TO_LOAD
      },
      {
        skip: 0,
        take: INITIAL_POSTS_TO_LOAD,
        includeCount: true
      }
    ];

  requests: { title: string; observable: Observable<PaginateResponseInterface<PostInterface>> }[] = [
    {
      title: 'general.forYouLabel',
      observable: this.httpService.get<PaginateResponseInterface<PostInterface>>(CommonApiEnum.Articles, {
        filterby: FOR_YOU_PARAMETER,
        expand: 'categories,regions,solutions,technologies,contenttags',
        includeCount: true,
        ...this.pagination[0]
      })
    },
    {
      title: 'general.trendingLabel',
      observable: this.httpService.get<PaginateResponseInterface<PostInterface>>(
        `${CommonApiEnum.Articles}/${CommonApiEnum.Trendings}`,
        {
          ...this.pagination[1]
        }
      )
    }
  ];
  selectedIndex: number = null;
  isEmpty: boolean;

  constructor(private readonly httpService: HttpService, private readonly commonService: CommonService,
    private spinnerService: SpinnerService
  ) { }

  ngOnInit(): void {
    this.isUserLoggedIn = this.auth.isLoggedIn() || this.auth.needSilentLogIn();
    this.spinnerService.onLoadingChanged$.subscribe((item) => {
      if (item == false) {
        this.isEmpty = false;
        this.removeParentCssEmitter.emit(true);
      }
    })
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.isUserLoggedIn = this.auth.isLoggedIn() || this.auth.needSilentLogIn();
    if (this.isUserLoggedIn && changes?.currentUser?.currentValue?.id !== changes?.currentUser?.previousValue?.id) {
      this.categories = this.currentUser?.userProfile?.categories;

      this.categories?.forEach((category, index) => {
        this.pagination.push({ skip: 0, take: INITIAL_POSTS_TO_LOAD });
        this.requests.push({
          title: category.name,
          observable: this.getCategoryRequest(index, category.id)
        });
      });

      if (this.requests.length < this.endIndex) {
        this.endIndex = this.requests.length;
      }
      this.loadMoreCategories();
    } else if (!this.isUserLoggedIn) {
      this.httpService.get<TagInterface[]>(CommonApiEnum.Categories).subscribe(val => {
        this.categories = val;
        val?.forEach((category: TagInterface, index: number) => {
          this.pagination.push({ skip: 0, take: INITIAL_POSTS_TO_LOAD });

          this.requests.push({
            title: category.name,
            observable: this.getCategoryRequest(index, category.id)
          });
        });

        if (this.requests.length < this.endIndex) {
          this.endIndex = this.requests.length;
        }
        this.loadMoreCategories();
      });
    }
  }

  loadMoreCategories(): void {
    if (this.endIndex <= this.requests.length) {
      combineLatest(this.requests.slice(this.startIndex, this.endIndex).map(request => request.observable))
        .pipe(untilDestroyed(this))
        .subscribe(forYouData => {
          this.pageData?.push(...forYouData);
          this.startIndex = this.endIndex;
          this.endIndex = this.startIndex + 4 > this.requests.length ? this.requests.length : this.startIndex + 4;
        });
    }
  }

  setIndex(index: number): void {
    this.selectedIndex = index === this.selectedIndex ? null : index;
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
  }

  forwardClick(requestIndex: number): void {
    if (this.pageData[requestIndex].dataList.length >= this.pageData[requestIndex].count) {
      return;
    }

    this.pagination[requestIndex].take = POSTS_PER_PAGE;
    this.pagination[requestIndex].skip =
      this.pagination[requestIndex].skip === 0
        ? INITIAL_POSTS_TO_LOAD
        : this.pagination[requestIndex].skip + POSTS_PER_PAGE;

    if (this.requests[requestIndex].title.includes('general')) {
      this.getDefaultRequest(requestIndex)
        .pipe(take(1))
        .subscribe(data => this.updateRequestedData(requestIndex, data));
    } else {
      const categoryIndex = this.categories.findIndex(cat => cat.name === this.requests[requestIndex].title);
      this.getCategoryRequest(requestIndex, this.categories[categoryIndex].id)
        .pipe(take(1))
        .subscribe(data => this.updateRequestedData(requestIndex, data));
    }
  }

  private updateRequestedData(requestIndex: number, data: PaginateResponseInterface<PostInterface>): void {
    this.pageData[requestIndex].skip = data.skip;
    this.pageData[requestIndex].take = data.take;
    this.pageData[requestIndex].count = data.count;
    this.pageData[requestIndex].dataList = [...this.pageData[requestIndex].dataList, ...data.dataList];
  }

  private getCategoryRequest(index: number, categoryId: number): Observable<PaginateResponseInterface<PostInterface>> {
    return this.httpService.get<PaginateResponseInterface<PostInterface>>(CommonApiEnum.Articles, {
      filterby: `categoryids=${categoryId}`,
      skipActivities: true,
      expand: 'categories,regions,solutions,technologies,contenttags',
      includeCount: true,
      ...this.pagination[index]
    });
  }

  private getDefaultRequest(requestIndex: number): Observable<PaginateResponseInterface<PostInterface>> {
    switch (this.requests[requestIndex].title) {
      case 'general.forYouLabel':
        return this.httpService.get<PaginateResponseInterface<PostInterface>>(CommonApiEnum.Articles, {
          filterby: FOR_YOU_PARAMETER,
          expand: 'categories,regions,solutions,technologies,contenttags',
          includeCount: true,
          ...this.pagination[0]
        });
      case 'general.trendingLabel':
        return this.httpService.get<PaginateResponseInterface<PostInterface>>(
          `${CommonApiEnum.Articles}/${CommonApiEnum.Trendings}`,
          {
            ...this.pagination[1]
          }
        );
    }
  }
}
