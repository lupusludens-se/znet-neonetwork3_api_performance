import { Injectable } from '@angular/core';
import { TimezoneInterface } from '../../shared/interfaces/common/timezone.interface';
import { HttpService } from '../../core/services/http.service';
import { CommonApiEnum } from '../../core/enums/common-api.enum';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TimezoneService {
  apiRoutes = CommonApiEnum;

  constructor(private httpService: HttpService) {
   }
   getTimeZones(): Observable<TimezoneInterface[]>{
   return this.httpService.get<TimezoneInterface[]>(this.apiRoutes.Timezones); 
   }
}





