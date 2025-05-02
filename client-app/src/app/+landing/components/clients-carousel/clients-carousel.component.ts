import { Component, OnDestroy, OnInit } from '@angular/core';

import { interval, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'neo-clients-carousel',
  templateUrl: './clients-carousel.component.html',
  styleUrls: ['./clients-carousel.component.scss']
})
export class ClientsCarouselComponent implements OnInit, OnDestroy {
  scrollActive: boolean = false;
  clients: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
  private unsubscribe$: Subject<void> = new Subject<void>();

  ngOnInit(): void {
    interval(2000)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(() => {
        this.scrollActive = true;

        setTimeout(() => {
          this.scrollActive = false;
          this.clients.splice(this.clients.length, 1, this.clients[0]);
          this.clients.splice(0, 1);
        }, 400);
      });
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
