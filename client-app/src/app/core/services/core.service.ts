import { Injectable, OnDestroy } from '@angular/core';
import { GuardsCheckEnd, NavigationEnd, NavigationStart, Router } from '@angular/router';

import { BehaviorSubject, filter, Observable, Subject, take } from 'rxjs';
import { share } from 'rxjs/operators';

import { PostInterface } from '../../+learn/interfaces/post.interface';
import { NotFoundDataInterface } from '../interfaces/not-found-data.interface';

import { URL_REGEXP } from '../../shared/validators/custom.validator';
import { UnreadCountersInterface } from '../interfaces/unread-counters.interface';
import { CommonApiEnum } from '../enums/common-api.enum';
import { HttpService } from './http.service';
import { AuthService } from './auth.service';
import { LocationStrategy } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class CoreService implements OnDestroy {
  globalSearchActive$: Subject<boolean> = new Subject<boolean>();
  showFeedbackPopup$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  feedbackPopupSubmitted$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  badgeCount$: BehaviorSubject<UnreadCountersInterface> = new BehaviorSubject<UnreadCountersInterface>(null);
  badgeDataFetch$: Subject<void> = new Subject<void>();
  badgeDataFetched$: Subject<void> = new Subject<void>();
  elementNotFoundData$: BehaviorSubject<NotFoundDataInterface> = new BehaviorSubject<NotFoundDataInterface>(null);

  ongoingRoute$: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  previousRoute$: BehaviorSubject<string> = new BehaviorSubject<string>(null);

  guardCheckEndedRoute$: BehaviorSubject<string> = new BehaviorSubject<string>(null);

  currentRouteGuardCheckCompleted$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  auth = AuthService;
  currentUrl: string = null;

  constructor(
    private readonly router: Router,
    private readonly locationStrategy: LocationStrategy,
    private readonly httpService: HttpService,
    private authService: AuthService
  ) {
    this.listenToPreviousRoute();
    this.listenToOngoingRoute();
    this.listenPublicandPrivateRoutes();
    this.listenRoutesForGuardCheckedEventCompletion();
  }

  ngOnDestroy(): void {
    this.badgeDataFetch$.next();
    this.badgeDataFetch$.complete();

    this.badgeDataFetched$.next();
    this.badgeDataFetched$.complete();

    this.elementNotFoundData$.next(null);
    this.elementNotFoundData$.complete();
  }

  badgeDataFetch(): Observable<void> {
    return this.badgeDataFetch$.pipe(share());
  }

  async copyTextToClipboard(value: string): Promise<void> {
    await navigator.clipboard.writeText(value);
  }

  getTaxonomyTag(post: PostInterface, taxonomyName: string): { id: number; name: string; taxonomy: string }[] {
    return post[taxonomyName]?.map(r => ({ id: r.id, name: r.name, taxonomy: taxonomyName }));
  }

  deleteEmptyProps(obj: object): object {
    const object: object = { ...obj };

    Object.keys(obj).forEach(k => {
      if (
        object[k] === '' ||
        object[k] === null ||
        object[k] === undefined ||
        (Array.isArray(object[k]) && object[k].length === 0)
      ) {
        delete object[k];
      }
    });

    return object;
  }

  goToTopics(id: number, name: string, type: number, isOpenInNewTab: boolean = false): void {
    if (isOpenInNewTab) {
      const queryParams = {
        id,
        name,
        type
      };
      const urlTree = this.router.createUrlTree(['topics'], { queryParams: queryParams });
      const getBaseHref = location.origin + this.locationStrategy.getBaseHref();
      const serializedUrl = getBaseHref + urlTree;
      window.open(serializedUrl, '_blank');
    }
    else {
      this.router
        .navigate(['/topics'], {
          queryParams: {
            id,
            name,
            type
          }
        })
        .then();
    }
  }

  modifyEditorText(htmlElement: HTMLElement): string {
    let response = htmlElement.innerHTML;

    const urls = response
      .replace(/&nbsp;/g, ' ')
      .replace(/<br>/g, ' <br> ') // add space above br to avoid it matching with URL_REGEXP
      .replace(/<div>/g, ' <div> ') // add space above div to avoid it matching with URL_REGEXP
      .replace(/<\/div>/g, ' </div> ')
      .split(' ')
      .map(str => {
        const string = str.trim();
        if (string.match(URL_REGEXP)) {
          str = string.replace(URL_REGEXP, CoreService.getUrlHtml(str));
        }

        return str;
      });

    return urls.join(' ');
  }

  getBadgeData(): void {
    this.httpService
      .get<UnreadCountersInterface>(CommonApiEnum.BadgeCounters)
      .pipe(
        filter(badgeCount => !!badgeCount),
        take(1)
      )
      .subscribe(badgeCount => this.badgeCount$.next(badgeCount));
  }

  private static getUrlHtml(url: string): string {
    return `<a class="text-dark-green text-underline fw-700 text-s" href="${url.startsWith('www') ? '//' + url : url
      }" target="_blank">${url}</a>`;
  }

  listenToOngoingRoute() {
    this.router.events.pipe(filter(event => event instanceof NavigationStart)).subscribe((event: NavigationStart) => {
      this.ongoingRoute$.next(event.url);
    });
  }

  listenToPreviousRoute() {
    this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe((event: NavigationEnd) => {
      this.previousRoute$.next(this.currentUrl);
      this.currentUrl = event.url;
    });
  }

  listenRoutesForGuardCheckedEventCompletion() {
    this.router.events.pipe(filter(event => event instanceof GuardsCheckEnd)).subscribe(() => {
      this.currentRouteGuardCheckCompleted$.next(true);
    });
  }

  listenPublicandPrivateRoutes() {
    this.router.events
      .pipe(
        filter(
          event =>
            event instanceof GuardsCheckEnd &&
            (event.url.includes('/learn/') ||
              event.url.includes('/events/') ||
              event.url.includes('/projects/') ||
              event.url.includes('/solutions/'))
        )
      )
      .subscribe((event: GuardsCheckEnd) => {
        this.guardCheckEndedRoute$.next(event.url);
      });
  }

  getOngoingRoute(): string {
    return this.ongoingRoute$.getValue();
  }

  convertToPlain(html): string {
    // Create a new div element
    var tempDivElement = document.createElement('div');

    // Set the HTML content with the given value
    tempDivElement.innerHTML = html;
    // Retrieve the text property of the element
    var elementText = tempDivElement.textContent || tempDivElement.innerText || '';
    // Regex for removing multiple spaces and repalce with single
    return elementText.replace(/\s\s+/g, ' ');
  }

  isSPAdminRoutes() {
    let previousRoute = this.previousRoute$.getValue();
    return previousRoute == null || previousRoute.includes('/manage');
  }
}
