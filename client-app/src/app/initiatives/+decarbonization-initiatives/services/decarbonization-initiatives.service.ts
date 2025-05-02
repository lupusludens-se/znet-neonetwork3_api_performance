import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { PaginationIncludeCountInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { InitiativeViewSource } from '../enums/initiative-view-source';

@Injectable()
export class DecarbonizationInitiativeService {
  paging$: BehaviorSubject<PaginationIncludeCountInterface> = new BehaviorSubject<PaginationIncludeCountInterface>({
    take: 6,
    skip: 0,
    total: null,
    includeCount: true
  });

  sourceId$: BehaviorSubject<number> = new BehaviorSubject<number>(InitiativeViewSource['YourInitiatives']);

  getPaging() {
    return this.paging$.asObservable();
  }

  getSource(){
    return this.sourceId$.asObservable();
  }

  setSource(sourceId: number){
    this.sourceId$.next(sourceId);
  }

  setPaging(pageData: PaginationIncludeCountInterface) {
    this.paging$.next(pageData);
  }

  clearPaging(){
    this.setPaging({
      take: 6,
      skip: 0,
      total: null,
      includeCount: true
    });
  }
}
