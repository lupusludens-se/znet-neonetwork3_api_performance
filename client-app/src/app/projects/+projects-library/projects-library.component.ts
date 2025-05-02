import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { filter } from 'rxjs/operators';
import { ProjectCatalogService, ProjectsViewTypeEnum } from '../+projects/services/project-catalog.service';

import { UserStatusEnum } from '../../user-management/enums/user-status.enum';
import { DEFAULT_PER_PAGE, PaginationInterface } from '../../shared/modules/pagination/pagination.component';
import { ProjectInterface } from '../../shared/interfaces/projects/project.interface';
import { TitleService } from '../../core/services/title.service';
import { AuthService } from '../../core/services/auth.service';
import { RolesEnum } from '../../shared/enums/roles.enum';
import { CoreService } from 'src/app/core/services/core.service';
import { ProjectComponentEnum } from '../shared/enums/project-component.enum';
import { ViewportScroller } from '@angular/common';

@UntilDestroy()
@Component({
  selector: 'neo-projects-library',
  templateUrl: './projects-library.component.html',
  styleUrls: ['./projects-library.component.scss']
})
export class ProjectsLibraryComponent implements OnInit, OnDestroy {
  projectsList: ProjectInterface[];
  userStatuses = UserStatusEnum;
  searchString: string;
  exportModal: boolean;
  requestParams: string;
  projectsViewType = ProjectsViewTypeEnum;
  currentUserId: Number;
  tdTitleClick: string;
  sortingParamNames: Record<string, string> = {
    title: 'title',
    category: 'category',
    company: 'company',
    regions: 'regions',
    changedon: 'changedon',
    status: 'status',
    publishedBy: 'publishedBy'
  };
  columnsList: Record<string, boolean> = {
    // * for styling carets
    titleAsc: true,
    categoryAsc: false,
    changedonAsc: false,
    companyAsc: false,
    statusAsc: false,
    regionsAsc: false,
    publishedByAsc: false
  };
  paging: PaginationInterface;
  defaultItemPerPage: number = DEFAULT_PER_PAGE;
  showCompany: boolean;
  userRoles = RolesEnum;
  routesEnum: string[] = [
    `${ProjectComponentEnum.EditProjectComponent}`,
    `${ProjectComponentEnum.ProjectDetailsComponent}`
  ];

  constructor(
    public projectsService: ProjectCatalogService,
    private titleService: TitleService,
    public authService: AuthService,
    private readonly coreService: CoreService,    
    private viewPort: ViewportScroller
  ) { }

  ngOnInit(): void {
    this.coreService.elementNotFoundData$
      .pipe(
        untilDestroyed(this),
        filter(data => !data)
      )
      .subscribe(() => {
        this.authService
          .currentUser()
          .pipe(
            untilDestroyed(this),
            filter(v => !!v)
          )
          .subscribe(user => {
            this.titleService.setTitle('projects.projectLibraryLabel');
            this.searchString = this.searchString ?? this.projectsService.getSearchType();
            this.getProjectsList(this.searchString, null, null);
    
            this.showCompany = user.roles.some(r => r.id === this.userRoles.Admin);
            this.currentUserId = user.id;
          });
      });
  }

  getProjectsList(searchString?: string, orderBy?: string, skip?: number) {
    skip = skip ?? this.projectsService.getSkip();
    this.projectsService.getProjectsList(this.projectsViewType.Library, searchString, orderBy, skip).subscribe(pl => {
      this.projectsList = pl.dataList;

      this.paging = {
        ...this.paging,
        skip: skip,
        take: 1,
        total: pl.count
      };
 
      this.viewPort.scrollToPosition([0, 0]);
    });
  }

  searchProjects(searchStr: string): void {
    this.searchString = searchStr;
    this.getProjectsList(searchStr, null, 0);
  }

  sortCriteriaSelection(sortedColumn: string, sortParam: string): void {
    this.tdTitleClick = sortParam;
    if (this.columnsList[sortedColumn]) {
      this.sortProjectsList(`${this.sortingParamNames[sortParam]}.desc`);

      for (let col in this.columnsList) {
        this.columnsList[col] = false;
      }

      this.columnsList[sortedColumn] = false;
    } else {
      this.sortProjectsList(`${this.sortingParamNames[sortParam]}.asc`);

      for (let col in this.columnsList) {
        this.columnsList[col] = false;
      }

      this.columnsList[sortedColumn] = true;
    }
  }

  sortProjectsList(OrderBy: string): void {
    this.getProjectsList(this.searchString, OrderBy);
  }

  updatePaging(page: number): void {
    const skip: number = Math.round((page - 1) * this.defaultItemPerPage);
    this.getProjectsList(null, null, skip);
  }

  ngOnDestroy(): void {
    const routesFound = this.routesEnum.some(val => this.coreService.getOngoingRoute().includes(val));
    if (!routesFound) {
      this.projectsService.clearRequestParams();
    }
  }
}
