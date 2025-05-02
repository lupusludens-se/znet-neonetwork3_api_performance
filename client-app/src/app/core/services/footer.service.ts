import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FooterService {
  private footerDisabled$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  setFooterDisabled(footerDisabled: boolean): void {
    this.footerDisabled$.next(footerDisabled);
  }

  isFooterDisabled(): BehaviorSubject<boolean> {
    return this.footerDisabled$;
  }
}
