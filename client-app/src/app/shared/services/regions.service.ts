import { BehaviorSubject, Observable } from 'rxjs';
import { Injectable } from '@angular/core';

import { CountryInterface } from '../interfaces/user/country.interface';
import { CommonApiEnum } from '../../core/enums/common-api.enum';
import { HttpService } from '../../core/services/http.service';

@Injectable()
export class RegionsService {
  private continents: BehaviorSubject<CountryInterface[] | null> = new BehaviorSubject<CountryInterface[] | null>(null);
  continents$: Observable<CountryInterface[] | null> = this.continents.asObservable();
  apiRoutes = CommonApiEnum;

  constructor(private httpService: HttpService) {}

  getContinentsList(): void {
    this.httpService
      .get<CountryInterface[]>(this.apiRoutes.Regions, {
        FilterBy: 'parentids=0'
      })
      .subscribe(c => {
        this.continents.next(c);
      });
  }
}
