import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, filter, Subject, switchMap, takeUntil, throwError } from 'rxjs';
import { CoreService } from 'src/app/core/services/core.service';
import { HttpService } from 'src/app/core/services/http.service';
import { TitleService } from 'src/app/core/services/title.service';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { TranslateService } from '@ngx-translate/core';
import { InitiativeProgress } from '../../interfaces/initiative-progress.interface';
import { InitiativeApiEnum } from '../../enums/initiative-api.enum';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { CommonService } from 'src/app/core/services/common.service';

@Component({
  selector: 'neo-view-initiative',
  templateUrl: './view-initiative.component.html',
  styleUrls: ['./view-initiative.component.scss']
})
export class ViewInitiativeComponent implements OnInit, OnDestroy {
  roles = RolesEnum;
  readonly contactUsModal: boolean = false;
  readonly unsubscribe$: Subject<void> = new Subject<void>();
  showDeleteModal: boolean = false;
  initiativeData: InitiativeProgress;
  regionString: string = '';
  regionStringTooltip: string = '';
  collaborators: string ='';
  readonly options: MenuOptionInterface[] = [
    {
      icon: 'trash-can-red',
      name: 'actions.deleteLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];

  constructor(
    private readonly titleService: TitleService,
    private readonly coreService: CoreService,
    private readonly httpService: HttpService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly snackbarService: SnackbarService,
    public readonly authService: AuthService,
    private readonly translateService: TranslateService,
    private readonly router: Router,
    private readonly commonService: CommonService
  ) { }

  ngOnInit(): void {
    this.activatedRoute.params
      .pipe(
        takeUntil(this.unsubscribe$),
        filter(params => params?.id),
        switchMap(params =>
          this.httpService.get<InitiativeProgress>(
            `${InitiativeApiEnum.GetInitiativeAndProgressDetailsById}/${params.id}`
          )
        ),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            this.coreService.elementNotFoundData$.next({
              iconKey: 'initiative',
              mainTextTranslate: 'initiative.viewInitiative.notFoundText',
              buttonTextTranslate: 'initiative.viewInitiative.notFoundButton',
              buttonLink: '/dashboard'
            });
          }
          return throwError(error);
        })
      )
      .subscribe(response => {
        if (response) {
          this.initiativeData = response;
          this.titleService.setTitle(response.title);
          const regions = response.regions.map(item => item.name);
          this.regionString = regions.slice(0, 5).join(', ');
          this.regionStringTooltip = regions.slice(5).join(', ');
          const collaborators = response.collaborators.map(item => item.firstName + ' ' + item.lastName);
          this.collaborators = response.collaborators.map(item => item.firstName + ' ' + item.lastName).slice(0, 5).join(', ');
        }
      });

  }

  optionClick(): void {
    this.showDeleteModal = true;
  }

  goBack() {
    this.commonService.goBack();
  }

  async confirmDelete() {
    this.showDeleteModal = false;
    try {
      const result = await this.httpService
        .delete(InitiativeApiEnum.Delete + `/${this.initiativeData.initiativeId}`)
        .pipe(takeUntil(this.unsubscribe$))
        .toPromise();

      if (result === true) {
        this.snackbarService.showSuccess(this.translateService.instant('initiative.deleteInitiative.successMessage'));
        this.router.navigate(['/decarbonization-initiatives']);
      } else {
        this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
      }
    } catch (error) {
      this.snackbarService.showError(this.translateService.instant('general.defaultErrorLabel'));
    }
  }

  closeDeletePopup() {
    this.showDeleteModal = false;
  }

  isInRole(currentUser: UserInterface, roleId: RolesEnum): boolean {
    if (currentUser != null) {
      return currentUser.roles.some(role => role.id === roleId && role.isSpecial);
    }
  }
  hasAccessToInitiative(currentUser: UserInterface): boolean {
    return !!currentUser && (
      currentUser.roles.some(role => role.id === RolesEnum.Admin && role.isSpecial) ||
      this.initiativeData.collaborators.some(collaborator => collaborator.id === currentUser.id)
    );
  }

  toggleFeedbackPopup(event): void {
    if (event == 'nav') {
      const feedbackFormStatus = this.coreService.feedbackPopupSubmitted$.getValue();
      if (feedbackFormStatus) this.coreService.showFeedbackPopup$.next(false);
    } else {
      const showStatus = this.coreService.showFeedbackPopup$.getValue();
      this.coreService.showFeedbackPopup$.next(!showStatus);
    }
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
