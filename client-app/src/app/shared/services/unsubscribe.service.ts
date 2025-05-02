import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpService } from "src/app/core/services/http.service";
import { UnsubscribeApiEnum } from "../enums/api/general-api.enum";


@Injectable()
export class UnsubscribeService {
  private apiRoutes = UnsubscribeApiEnum;

  constructor(private httpService: HttpService) { }

  getUserEmailFromToken(token: string): Observable<any> {
    const payload  = {
      token : token,
      frequency : -1
    }
    return this.httpService.post(`${this.apiRoutes.UnsubscribeGetDetailsApi}`,payload);
  }

  updateUserEmailPreferences(token: string, frequencyId : number): Observable<any> {
    const payload  = {
      frequency : frequencyId,
      token : token
    }
    return this.httpService.post(`${this.apiRoutes.UnsubscribeEmailsApi}`,payload);
  }
}
