import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { catchError, Observable, throwError } from 'rxjs';
import { CoreService } from 'src/app/core/services/core.service';
import { HttpService } from 'src/app/core/services/http.service';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { UserManagementApiEnum } from 'src/app/user-management/enums/user-management-api.enum';

@Injectable({
  providedIn: 'root'
})
export class UserProfileResolverService implements Resolve<UserInterface> {
  constructor(
    private readonly httpService: HttpService,
    private readonly router: Router,
    private readonly coreService: CoreService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<UserInterface> {
    const id = route.params['id'];
    return this.httpService.get<UserInterface>(`${UserManagementApiEnum.Users}/${id}`).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 404) {
          this.router.navigate(['/community']);
          this.coreService.elementNotFoundData$.next({
            iconKey: 'user-unavailable',
            mainTextTranslate: 'userProfile.notFoundText',
            buttonTextTranslate: 'userProfile.notFoundButton',
            buttonLink: '/community'
          });
        }

        return throwError(error);
      })
    );
  }
}
