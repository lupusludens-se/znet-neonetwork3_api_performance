import { ChangeDetectionStrategy, ChangeDetectorRef, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Observable, switchMap, tap, catchError, filter, combineLatest, Subject, throwError } from 'rxjs';

import { HttpService } from '../../../core/services/http.service';
import { ToolInterface } from '../../../shared/interfaces/tool.interface';
import { ToolsApiEnum } from '../../../shared/enums/api/tools-api.enum';
import { TitleService } from '../../../core/services/title.service';
import { SOLAR_QUOTE_NAME } from 'src/app/shared/constants/solar-quote-name.const';
import { AuthService } from 'src/app/core/services/auth.service';
import { PermissionService } from 'src/app/core/services/permission.service';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { FooterService } from 'src/app/core/services/footer.service';
import { CommonService } from 'src/app/core/services/common.service';
import { InitiativeApiEnum } from 'src/app/initiatives/enums/initiative-api.enum';
import { InitiativeAttachedContent } from 'src/app/initiatives/interfaces/initiative-attached.interface';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { takeUntil } from 'rxjs/operators';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { TranslateService } from '@ngx-translate/core';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
@Component({
  selector: 'neo-tool-content',
  templateUrl: './tool-content.component.html',
  styleUrls: ['./tool-content.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ToolContentComponent {
  isSolarQuote: boolean = true;
  toolHeight: number = 0;
  toolData$: Observable<ToolInterface> = this.activatedRoute.params.pipe(
    switchMap(params =>
      this.httpService.get<ToolInterface>(`${ToolsApiEnum.Tools}/${params.id}?expand=roles,companies `)
    ),
    tap(tool => {
      this.isSolarQuote = tool.title.includes(SOLAR_QUOTE_NAME);
      this.titleService.setTitle(tool.title);
      this.toolHeight = tool.toolHeight;
      this.footerService.setFooterDisabled(this.isSolarQuote);

      this.listenForCurrentUser();
    })
  );
  attachToInitiative: boolean = false;
  contentType: string = InitiativeModulesEnum[InitiativeModulesEnum.Tools];
  toolId: number;
  isCorporateUser: boolean = false;
  private unsubscribe$ = new Subject<void>();

  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly httpService: HttpService,
    private readonly router: Router,
    public sanitizer: DomSanitizer,
    private readonly titleService: TitleService,
    private readonly authService: AuthService,
    private readonly permissionService: PermissionService,
    private readonly footerService: FooterService,
    private cdr: ChangeDetectorRef,
    private activityService: ActivityService,
    private translateService: TranslateService
  ) {}

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(() => {
      this.toolId = this.activatedRoute.snapshot.params.id;
    });
  }

  getUrl(toolUrl: string): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(toolUrl);
  }

  goBack() {
    this.router.navigate(['tools']);
  }

  private listenForCurrentUser(): void {
    this.authService.currentUser().subscribe(currentUser => {
      if (
        currentUser &&
        this.isSolarQuote &&
        !this.permissionService.userHasPermission(currentUser, PermissionTypeEnum.SendQuote)
      ) {
        this.router.navigate(['403']);
      } else {
        this.isCorporateUser = currentUser.roles.some(role => role.id === RolesEnum.Corporation);
        this.cdr.detectChanges();
      }
    });
  }

  openAttachToInitiativeModal() {
    this.attachToInitiative = true;
    this.cdr.detectChanges();
  }
  trackToolsDetailsActivity(){
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        buttonName: this.translateService.instant('initiative.attachContent.attachToolLabel'),
        moduleName: InitiativeModulesEnum[InitiativeModulesEnum.Tools]
      })
      ?.subscribe();
  }
}
