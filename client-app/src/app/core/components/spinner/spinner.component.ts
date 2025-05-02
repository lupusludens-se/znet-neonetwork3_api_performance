import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import {
  Event as RouterEvent,
  NavigationCancel,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  Router
} from '@angular/router';
import { combineLatest } from 'rxjs';

import { map, take } from 'rxjs/operators';

import { SpinnerService } from '../../services/spinner.service';

import { ThankYouPopupServiceService } from 'src/app/public/services/thank-you-popup-service.service';
import { CoreService } from '../../services/core.service';

@Component({
  selector: 'neo-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss']
})
export class SpinnerComponent implements OnInit {
  @Input() isLoading: boolean = false;

  constructor(
    private spinnerService: SpinnerService,
    private coreService: CoreService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private thankYouPopupServiceService: ThankYouPopupServiceService
  ) {}

  private static navigationInterceptor(event: RouterEvent): boolean {
    if (event instanceof NavigationStart) {
      return true;
    }
    if (event instanceof NavigationEnd) {
      return false;
    }

    if (event instanceof NavigationCancel) {
      return false;
    }
    if (event instanceof NavigationError) {
      return false;
    }

    return false;
  }

  public ngOnInit(): void {
    //Merge is not waiting for first observable to complete; sequential is not happening; the latest result is taken
    //because of which the loader symbol is not working on learn and community pages when search text is entered for the
    //very first time. - NNG 163

    /*merge(
      this.router.events.pipe(map(event => SpinnerComponent.navigationInterceptor(event))),
      this.spinnerService.onLoadingChanged$
    ).subscribe(isLoading => {
      if (this.isLoading != isLoading) {
        this.isLoading = isLoading;
        this.cdr.detectChanges();
      }
    });*/

    combineLatest([
      this.router.events.pipe(map(event => SpinnerComponent.navigationInterceptor(event))),
      this.spinnerService.onLoadingChanged$
    ]).subscribe(([obs1Result, obs2Result]) => {
      if (this.isLoading != obs2Result) {
        this.isLoading = obs2Result;
        this.cdr.detectChanges();
      }
    });
    this.coreService.currentRouteGuardCheckCompleted$.pipe(take(2)).subscribe(val => {
      if (val === true) {
        this.isLoading = false;
      }
    });
  }
}
