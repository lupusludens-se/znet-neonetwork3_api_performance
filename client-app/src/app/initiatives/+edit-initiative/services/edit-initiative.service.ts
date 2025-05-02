import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from 'src/app/core/services/http.service';
import { InitiativeApiEnum } from '../../enums/initiative-api.enum';
import { BaseInitiativeInterface } from '../../+initatives/+view-initiative/interfaces/base-initiative.interface';

@Injectable({
  providedIn: 'root'
})
export class EditInitiativeService {
  constructor(private httpService: HttpService) {}

  updateInitiative(formData: any, initiativeId: number): Observable<{ id: number }> {
    return this.httpService.post<{ id: number }>(
      `${InitiativeApiEnum.CreateUpdateInitiative}/${initiativeId}`,
      formData
    );
  }

  getInitiativeDetailsByInitiativeId(id: number): Observable<BaseInitiativeInterface> {
    return this.httpService.get<BaseInitiativeInterface>(
      `${InitiativeApiEnum.GetInitiativeAndProgressDetailsById}/${id}/true`
    );
  }
}
