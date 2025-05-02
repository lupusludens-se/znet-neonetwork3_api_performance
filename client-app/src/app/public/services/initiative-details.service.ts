import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/core/services/http.service';
import { PostInterface } from 'src/app/+learn/interfaces/post.interface';
import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class InitiativeDetailsService {

  constructor(private readonly httpService:HttpService) { }
   getDIGArticle():Observable<PostInterface> {
      return this.httpService
        .get<PostInterface>(`${CommonApiEnum.Articles}/${CommonApiEnum.GetPublicInitiativeArticle}`)
    }

}
