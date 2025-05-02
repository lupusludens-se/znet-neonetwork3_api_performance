import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { filter, Subject, switchMap, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { HttpService } from '../../../core/services/http.service';
import { TitleService } from '../../../core/services/title.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { CoreService } from '../../../core/services/core.service';

import { TableConfigurationInterface } from '../../../shared/interfaces/table/table-configuration.interface';
import { ToolInterface } from '../../../shared/interfaces/tool.interface';
import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import { ToolsApiEnum } from '../../../shared/enums/api/tools-api.enum';
import { DEFAULT_PER_PAGE, PaginationInterface } from '../../../shared/modules/pagination/pagination.component';
import { TableCrudEnum } from '../../../shared/modules/table/enums/table-crud.enum';
import { PermissionService } from 'src/app/core/services/permission.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { ViewportScroller } from '@angular/common';

@UntilDestroy()
@Component({
  selector: 'neo-tools-management',
  templateUrl: './tools-management.component.html',
  styleUrls: ['./tools-management.component.scss']
})
export class ToolsManagementComponent implements OnInit, OnDestroy {
  showAdd: boolean;
  showModal: boolean;
  selectedItem: Record<string, string | number | boolean>;

  defaultItemPerPage = DEFAULT_PER_PAGE;
  search: string;
  paging: PaginationInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null
  };

  configuration: TableConfigurationInterface<ToolInterface> = {
    data: null,
    optionCell: true,
    columns: [
      {
        name: 'Tool',
        propertyName: 'title',
        imageProperty: 'icon',
        defaultImageUrl: 'assets/images/default-tool-icon.png',
        isBold: true,
        isSortable: true
      },
      {
        name: 'Status',
        propertyName: 'isActive',
        isSortable: true,
        sortName: 'status',
        columnWidth: '148px'
      }
    ]
  };

  loadTools$: Subject<void> = new Subject<void>();
  deleteItem$: Subject<number> = new Subject<number>();

  currentUser: UserInterface;

  private statusChange$: Subject<ToolInterface> = new Subject<ToolInterface>();

  constructor(
    private readonly router: Router,
    private readonly httpService: HttpService,
    private readonly titleService: TitleService,
    private readonly snackbarService: SnackbarService,
    private readonly coreService: CoreService,
    private readonly authService: AuthService,
    private readonly permissionService: PermissionService,
    private viewPort: ViewportScroller
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle('toolsManagement.toolManagementLabel');

    this.listenToLoadTools();
    this.listenToDeleteTool();
    this.listenToStatusChange();
    this.listenForCurrentUser();

    this.loadTools$.next();
  }

  ngOnDestroy(): void {
    this.deleteItem$.next(null);
    this.deleteItem$.complete();

    this.statusChange$.next(null);
    this.statusChange$.complete();
  }

  optionClick(event): void {
    switch (event.option.operation) {
      case TableCrudEnum.Edit:
        this.goToEdit(event);
        break;
      case TableCrudEnum.Delete:
        this.showModal = true;
        this.selectedItem = event.dataItem;
        break;
      case TableCrudEnum.Status:
        this.statusChange$.next(event.dataItem);
        break;
    }
  }

  changePage(page: number): void {
    this.paging.skip = (page - 1) * this.defaultItemPerPage;
    this.loadTools$.next();
  }

  goToEdit({ dataItem }): void {
    this.router.navigate([`./admin/tool-management/edit/${dataItem?.id}`]);
  }

  private listenToLoadTools(): void {
    this.loadTools$
      .pipe(
        untilDestroyed(this),
        switchMap(() => {
          const paging = this.coreService.deleteEmptyProps({
            search: this.search,
            includeCount: true,
            expand: 'icon',
            orderBy: 'title.asc',
            ...this.paging
          });

          return this.httpService.get<PaginateResponseInterface<ToolInterface>>(ToolsApiEnum.Tools, paging);
        }),
        catchError(error => {
          this.snackbarService.showError('general.defaultErrorLabel');
          return throwError(error);
        })
      )
      .subscribe(toolsData => {
        this.paging = {
          ...this.paging,
          skip: this.paging?.skip ? this.paging?.skip : 0,
          total: toolsData.count
        };

        this.configuration.data = toolsData;

        this.viewPort.scrollToPosition([0, 0]);
      });
  }

  private listenToDeleteTool() {
    this.deleteItem$
      .pipe(
        untilDestroyed(this),
        filter(toolId => !!toolId),
        switchMap(toolId => this.httpService.delete(`${ToolsApiEnum.Tools}/${toolId}`)),
        catchError(error => {
          this.showModal = false;
          this.selectedItem = null;
          this.snackbarService.showError('general.defaultErrorLabel');
          return throwError(error);
        })
      )
      .subscribe(() => {
        this.showModal = false;
        this.loadTools$.next();
      });
  }

  private listenToStatusChange() {
    this.statusChange$
      .pipe(
        untilDestroyed(this),
        filter(tool => !!tool?.id),
        switchMap(tool =>
          this.httpService.patch(`${ToolsApiEnum.Tools}/${tool.id}`, {
            jsonPatchDocument: [
              {
                op: 'replace',
                path: '/IsActive',
                value: !tool.isActive
              }
            ]
          })
        ),
        catchError(error => {
          this.snackbarService.showError('general.defaultErrorLabel');
          return throwError(error);
        })
      )
      .subscribe(() => this.loadTools$.next());
  }

  private listenForCurrentUser(): void {
    this.authService
      .currentUser()
      .pipe(untilDestroyed(this))
      .subscribe(currentUser => {
        this.currentUser = currentUser;
        if (currentUser) {
          this.showAdd = this.permissionService.userHasPermission(this.currentUser, PermissionTypeEnum.ToolManagement);
          this.configuration.optionCell = this.showAdd;
        }
      });
  }
}
