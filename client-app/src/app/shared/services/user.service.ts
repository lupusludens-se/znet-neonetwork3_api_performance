import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpService } from 'src/app/core/services/http.service';
import { UserProfileApiEnum } from 'src/app/shared/enums/api/user-profile-api.enum';

@Injectable()
export class UserProfileService {
  private apiRoutes = UserProfileApiEnum;

  constructor(private httpService: HttpService) {}

  followUser(id: number): Observable<number> {
    return this.httpService.post(`${this.apiRoutes.Followers}/${id}`);
  }
}
