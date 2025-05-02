import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpService } from '../../core/services/http.service';
import { CommonApiEnum } from '../../core/enums/common-api.enum';
import { CountryInterface } from '../interfaces/user/country.interface';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';

@Injectable({
  providedIn: 'root'
})
export class CountriesService {
  countriesList: BehaviorSubject<CountryInterface[]> = new BehaviorSubject<CountryInterface[]>(null);
  statesList: BehaviorSubject<TagInterface[]> = new BehaviorSubject<TagInterface[]>(null);
  apiRoutes = CommonApiEnum;

  constructor(private httpService: HttpService) {}

  getCountriesList(Search: string = ''): Observable<CountryInterface[]> {
    return this.httpService.get(this.apiRoutes.Countries, {
      Search
    });
  }

  getInitialCountriesList(): void {
    if (this.countriesList.value) return;

    this.httpService.get<CountryInterface[]>(this.apiRoutes.Countries).subscribe(cl => {
      this.countriesList.next(cl);

      const states: TagInterface[] = cl.filter(c => c.code.toLowerCase() === 'us')[0].states;
      this.statesList.next(states);
    });
  }
}
