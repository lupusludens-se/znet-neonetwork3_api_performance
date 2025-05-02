import { Component, OnDestroy, OnInit } from '@angular/core';
import { animate, style, transition, trigger } from '@angular/animations';

import { Subject, takeUntil } from 'rxjs';

import { SnackbarService } from '../../services/snackbar.service';
import { SnackbarTypeEnum } from '../../enums/snackbar-type.enum';

@Component({
  selector: 'neo-snackbar',
  templateUrl: './snackbar.component.html',
  styleUrls: ['./snackbar.component.scss'],
  animations: [
    trigger('state', [
      transition(':enter', [
        style({
          bottom: '-100px',
          transform: 'translate(-50%, 0%) scale(0.3)'
        }),
        animate(
          '150ms cubic-bezier(0, 0, 0.2, 1)',
          style({
            transform: 'translate(-50%, 0%) scale(1)',
            opacity: 1,
            bottom: '10px'
          })
        )
      ]),
      transition(':leave', [
        animate(
          '150ms cubic-bezier(0.4, 0.0, 1, 1)',
          style({
            transform: 'translate(-50%, 0%) scale(0.3)',
            opacity: 0,
            bottom: '-100px'
          })
        )
      ])
    ])
  ]
})
export class SnackbarComponent implements OnInit, OnDestroy {
  snackbarType = SnackbarTypeEnum;
  show: boolean = false;
  message: string = 'This is snackbar';
  type: SnackbarTypeEnum = SnackbarTypeEnum.Success;

  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(private snackbarService: SnackbarService) {}

  ngOnInit() {
    this.snackbarService.snackbarState.pipe(takeUntil(this.unsubscribe$)).subscribe(state => {
      if (state.type) {
        this.type = state.type;
      } else {
        this.type = SnackbarTypeEnum.Success;
      }

      this.message = state.message;
      this.show = state.show;

      setTimeout(() => {
        this.show = false;
      }, 7000);
    });
  }

  ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
