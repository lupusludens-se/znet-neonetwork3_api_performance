import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from '../../core/services/http.service';
import { CommonApiEnum } from '../../core/enums/common-api.enum';
import { TagInterface } from '../../core/interfaces/tag.interface';

@Injectable()
export class HeardViaService {
  apiRoutes = CommonApiEnum;

  constructor(private httpService: HttpService) {}

  getHeardViaList(): Observable<TagInterface[]> {
    return this.httpService.get('heardvia');
  }
}
