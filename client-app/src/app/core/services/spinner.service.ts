import { HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Subject } from 'rxjs';

const excludeRequests: string[] = [];

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {
  public onLoadingChanged$: Subject<boolean> = new Subject();

  private requests: Array<HttpRequest<any>> = [];

  public onStarted(request: HttpRequest<any>): void {
    if (excludeRequests.some(urlSegment => request.url.includes(urlSegment))) {
      return;
    }
    this.requests.push(request);
    this.notify();
  }

  public onFinished(request: HttpRequest<any>): void {
    const index = this.requests.indexOf(request);
    if (index !== -1) {
      this.requests.splice(index, 1);
    }
    this.notify();
  }

  private notify(): void {
    this.onLoadingChanged$.next(this.requests.length !== 0);
  }
}
