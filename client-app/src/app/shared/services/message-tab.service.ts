import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MessageTabService {
  tabState$: BehaviorSubject<'inbox' | 'network'> = new BehaviorSubject<'inbox' | 'network'>('inbox');

  constructor() {}

  getTabState() {
    return this.tabState$.asObservable();
  }

  setTabState(tabstate: 'inbox' | 'network') {
    this.tabState$.next(tabstate);
  }
}
