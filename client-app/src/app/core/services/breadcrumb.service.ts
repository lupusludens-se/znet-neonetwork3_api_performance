import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Data, NavigationEnd, Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { filter, share } from 'rxjs/operators';

import { BreadcrumbInterface } from '../interfaces/breadcrumb.interface';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class BreadcrumbService {
  // Subject emitting the breadcrumb hierarchy
  private _breadcrumbs$ = new BehaviorSubject<BreadcrumbInterface[]>([]);
  auth = AuthService;
  isPublicUser: boolean;

  constructor(private router: Router) {
    this.router.events
      .pipe(
        // Filter the NavigationEnd events as the breadcrumb is updated only when the route reaches its end
        filter(event => event instanceof NavigationEnd)
      )
      .subscribe(() => {
        // Construct the breadcrumb hierarchy
        const root = this.router.routerState.snapshot.root;
        const breadcrumbs: BreadcrumbInterface[] = [];
        this.addBreadcrumb(root, [], breadcrumbs);

        // Emit the new hierarchy
        this._breadcrumbs$.next(breadcrumbs);
      });
  }

  private static getLabel(data: Data) {
    // The breadcrumb can be defined as a static string or as a function to construct the breadcrumb element out of the route data
    return typeof data.breadcrumb === 'function' ? data.breadcrumb(data) : data.breadcrumb;
  }

  public pushBreadcrumb(breadcrumb: BreadcrumbInterface): void {
    if (!this._breadcrumbs$.value.some(b => b.url === breadcrumb.url || b.url === 'search')) {
      const newBreadcrumbs = [...this._breadcrumbs$.value, breadcrumb];
      this._breadcrumbs$.next(newBreadcrumbs);
    }
  }

  public updateBreadcrumbs(breadcrumbs: BreadcrumbInterface[]): void {
    this._breadcrumbs$.next(breadcrumbs);
  }

  public getBreadcrumbs(): Observable<BreadcrumbInterface[]> {
    return this._breadcrumbs$.pipe(share());
  }

  private addBreadcrumb(route: ActivatedRouteSnapshot, parentUrl: string[], breadcrumbs: BreadcrumbInterface[]) {
    this.isPublicUser = !(this.auth.isLoggedIn() || this.auth.needSilentLogIn());

    if (route) {
      // Construct the route URL
      const routeUrl = parentUrl.concat(route.url.map(url => url.path));

      // Add an element for the current route part
      if (route.data.breadcrumb && !route.data.breadcrumbSkip) {
        const breadcrumb = {
          label:
            this.isPublicUser && route.data.breadcrumb === 'Solutions'
              ? 'Projects'
              : BreadcrumbService.getLabel(route.data),
          url: routeUrl.join('/')
        };
        breadcrumbs.push(breadcrumb);
      }

      // Add another element for the next route part
      this.addBreadcrumb(route.firstChild, routeUrl, breadcrumbs);
    }
  }
}
