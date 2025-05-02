import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { MessageTabService } from 'src/app/shared/services/message-tab.service';

@Injectable()
export class MessagesService {
  inboxSortBy$: BehaviorSubject<string> = new BehaviorSubject<string>('recent');
  selectedConvTypeIdsValue$: BehaviorSubject<number[]> = new BehaviorSubject<number[]>([]);
  networkSortBy$: BehaviorSubject<string> = new BehaviorSubject<string>('recent');
  paging$: BehaviorSubject<PaginationInterface> = new BehaviorSubject<PaginationInterface>({
    take: 20,
    skip: 0,
    total: null
  });
  constructor(private messageTabService: MessageTabService) {}

  setSortValue(sortKey: string, type: 'network' | 'inbox') {
    if (type == 'network') {
      this.networkSortBy$.next(sortKey);
    } else {
      this.inboxSortBy$.next(sortKey);
    }
  }

  setSelectedConvTypeIdsValue(selectedIds: number[]) {
    this.selectedConvTypeIdsValue$.next(selectedIds);
  }

  getInboxSortBy() {
    return this.inboxSortBy$.asObservable();
  }

  getSelectedConvTypeIdsValue() {
    return this.selectedConvTypeIdsValue$.asObservable();
  }

  getNetworkSortBy() {
    return this.networkSortBy$.asObservable();
  }

  getPaging() {
    return this.paging$.asObservable();
  }

  setPaging(pageData: PaginationInterface) {
    this.paging$.next(pageData);
  }

  clearPreferencesData() {
    this.inboxSortBy$.next('recent');
    this.networkSortBy$.next('recent');
    this.messageTabService.tabState$.next('inbox');
    this.paging$.next({ take: 20, skip: 0, total: null });
    this.selectedConvTypeIdsValue$.next([]);
  }
}
