import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { catchError, filter, of, Subject, switchMap, takeUntil, throwError } from 'rxjs';

import { CoreService } from '../../../core/services/core.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { HttpService } from '../../../core/services/http.service';
import { TitleService } from '../../../core/services/title.service';

import { PostInterface } from '../../interfaces/post.interface';
import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';

import { CommonApiEnum } from '../../../core/enums/common-api.enum';
import { PostTypeEnum } from '../../../core/enums/post-type.enum';
import { FilterDataInterface } from '../../../shared/modules/filter/interfaces/filter-data.interface';
import { FilterChildDataInterface } from '../../../shared/modules/filter/interfaces/filter-child-data.interface';
import { FOR_YOU_PARAMETER } from '../../constants/parameter.const';
import { SaveContentService } from '../../../shared/services/save-content.service';
import { HttpErrorResponse } from '@angular/common/http';
import {
  CATEGORIES,
  CONTENTTAGS,
  REGIONS,
  SOLUTIONS,
  TECHNOLOGIES
} from '../../../shared/constants/taxonomy-names.const';
import { CommonService } from 'src/app/core/services/common.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { InitiativeApiEnum } from 'src/app/initiatives/enums/initiative-api.enum';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { InitiativeAttachedContent } from 'src/app/initiatives/interfaces/initiative-attached.interface';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'neo-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PostComponent implements OnInit, OnDestroy {
  postData: PostInterface;
  suggestedPostData: PostInterface[];
  auth = AuthService;
  currentUser: UserInterface;
  attachToInitiative: boolean = false;
  contentType: string = InitiativeModulesEnum[InitiativeModulesEnum.Learn];
  articleId: number;

  fetchPostData$: Subject<number> = new Subject<number>();

  private fetchSuggestedData$: Subject<void> = new Subject<void>();
  private unsubscribe$: Subject<void> = new Subject<void>();
  isCorporateUser: boolean = false;

  constructor(
    private readonly coreService: CoreService,
    private readonly httpService: HttpService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly snackbarService: SnackbarService,
    private readonly titleService: TitleService,
    private readonly saveContentService: SaveContentService,
    private readonly router: Router,
    private readonly commonService: CommonService,
    private readonly authService: AuthService,
    private activityService: ActivityService,
    private translateService: TranslateService
  ) {
    this.listenForCurrentUser();
  }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(() => {
      this.articleId = this.activatedRoute.snapshot.params.id;
      this.listenForPostDataLoading();
      this.fetchPostData$.next(null);
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    this.fetchPostData$.next(null);
    this.fetchPostData$.complete();

    this.fetchSuggestedData$.next();
    this.fetchSuggestedData$.complete();
  }

  showMessage(message: string) {
    if (message.includes('link')) {
      this.coreService.copyTextToClipboard(window.location.href);
    }

    this.snackbarService.showSuccess(message);
  }

  goBack() {
    this.commonService.goBack();
  }

  getNeoType(postTypeEnum: PostTypeEnum): string {
    if (!postTypeEnum) return;

    const typeEnumKey = Object.keys(PostTypeEnum)[Object.values(PostTypeEnum).indexOf(postTypeEnum)].toLowerCase();

    return typeEnumKey ? typeEnumKey : '';
  }

  savePost(articleId: number): void {
    if (this.postData.isSaved) {
      this.saveContentService.deleteArticle(articleId).subscribe(() => (this.postData.isSaved = false));
    } else {
      this.saveContentService.saveArticle(articleId).subscribe(() => (this.postData.isSaved = true));
    }
  }

  private listenForCurrentUser(): void {
    this.authService.currentUser().subscribe(currentUser => {
      if (currentUser) {
        this.currentUser = currentUser;
        this.isCorporateUser = (currentUser as UserInterface).roles.some(role => role.id === RolesEnum.Corporation);
      }
    });
  }

  private listenForPostDataLoading(): void {
    this.fetchPostData$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap((postId: number) =>
          this.httpService.get<PostInterface>(
            `${CommonApiEnum.Articles}/${postId ? postId : this.activatedRoute.snapshot.params.id}`,
            { expand: 'categories,regions,solutions,technologies,contenttags' }
          )
        ),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            this.router.navigate(['/learn']);
            this.coreService.elementNotFoundData$.next({
              iconKey: 'learn',
              mainTextTranslate: 'learn.notFoundText',
              buttonTextTranslate: 'learn.notFoundButton',
              buttonLink: '/learn'
            });
          }

          return throwError(error);
        })
      )
      .subscribe(response => {
        this.postData = response;
        this.postData.postTags = [
          ...this.coreService.getTaxonomyTag(this.postData, CATEGORIES),
          ...this.coreService.getTaxonomyTag(this.postData, SOLUTIONS),
          ...this.coreService.getTaxonomyTag(this.postData, TECHNOLOGIES),
          ...this.coreService.getTaxonomyTag(this.postData, CONTENTTAGS)
        ];

        this.postData.regionTags = this.coreService.getTaxonomyTag(this.postData, REGIONS);

        this.titleService.setTitle(this.postData.title);
        PostComponent.modifyContent(this.postData);



        this.authService.currentUser().subscribe(currentUser => {
          if (currentUser != null) {
            this.getSuggestedData();
            this.readPost();
          }
        });
      });
  }

  private getSuggestedData(): void {
    this.httpService
      .get<PaginateResponseInterface<PostInterface>>(CommonApiEnum.Articles, {
        filterby: this.getQueryString(['categories', 'technologies']),
        expand: 'categories,regions,solutions,technologies,contenttags',
        skip: 0,
        take: 4
      })
      .subscribe(postData => (this.suggestedPostData = postData.dataList.filter(post => post.id !== this.postData.id)));
  }

  private readPost(): void {
    this.httpService
      .post<PaginateResponseInterface<PostInterface>>(
        `${CommonApiEnum.Articles}/${this.postData.id}/${CommonApiEnum.Trendings}`,
        {}
      )
      .subscribe(() => { });
  }

  private getQueryString(params: string[]): string {
    if (!params) return FOR_YOU_PARAMETER;

    const query = params
      .map(key => {
        if (this.postData[key].some(item => item?.childElements)) {
          return PostComponent.getFilterChildDataQuery(key, this.postData[key]);
        } else if (this.postData[key].length) {
          return PostComponent.getFilterDataInterfaceQuery(key, this.postData[key]);
        }
      })
      .filter(item => !!item)
      .join('&');

    return query ? query : FOR_YOU_PARAMETER;
  }

  private static modifyContent(postData: PostInterface): void {
    if (postData.content.includes('[neo_video]')) {
      postData.content = postData.content.replace(
        '[neo_video]',
        `<video controls src="${postData.videoUrl}" poster="${postData.imageUrl}"></video>`
      );
    }

    if (postData.content.includes('[neo_pdf]')) {
      postData.content = postData.content.replace(
        '[neo_pdf]',
        `<iframe referrerpolicy="no-referrer-when-downgrade" class="w-100 pdf-container" src="${postData.pdfUrl}"></iframe>`
      );
    }
  }

  private static getFilterDataInterfaceQuery(key: string, items: FilterDataInterface[]): string {
    return `${key}=${items.map(item => item.id)}`;
  }

  private static getFilterChildDataQuery(key: string, items: FilterChildDataInterface[]): string {
    const query = `${key}=${items.map(item =>
      item.checked ? `${item.id},` : '' + `${item.childElements.map(child => child.id).join(',')}`
    )}`;

    return !query.endsWith('=') ? query : '';
  }
  trackAttachToInitiativeActivity(){
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        buttonName: this.translateService.instant('initiative.attachContent.attachContentLabel'),
        moduleName: InitiativeModulesEnum[InitiativeModulesEnum.Learn]
      })
      ?.subscribe();
  }
}
