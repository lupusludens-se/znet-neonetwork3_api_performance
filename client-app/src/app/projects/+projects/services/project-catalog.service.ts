import { Injectable } from '@angular/core';
import { Observable, from } from 'rxjs';

import { DEFAULT_PER_PAGE } from '../../../shared/modules/pagination/pagination.component';
import { ProjectApiRoutes } from '../../shared/constants/project-api-routes.const';
import { HttpService } from '../../../core/services/http.service';
import { ProjectInterface } from '../../../shared/interfaces/projects/project.interface';
import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import * as dayjs from 'dayjs';
import { environment } from '../../../../environments/environment';
import { ProjectComponentEnum } from '../../shared/enums/project-component.enum';

export enum ProjectsViewTypeEnum {
  Library = 1,
  Catalog
}

export interface FileResponseInterface {
  fileData: string;
  fileName: string;
}

@Injectable()
export class ProjectCatalogService {
  defaultItemPerPage: number = DEFAULT_PER_PAGE;
  skip: number;
  searchString: string;
  orderBy: string;
  apiRoutes = ProjectComponentEnum;
  fileObj: FileResponseInterface;
  private readonly routes = ProjectApiRoutes;

  constructor(private httpClient: HttpService) { }

  getSkip(): number {
    return this.skip;
  }

  getSearchType(): string {
    return this.searchString;
  }

  getProjectsList(
    projectsViewType: ProjectsViewTypeEnum,
    searchString?: string,
    orderBy?: string,
    skip?: number
  ): Observable<PaginateResponseInterface<ProjectInterface>> {
    this.skip = skip ?? this.skip;
    this.searchString = searchString ?? this.searchString;
    this.orderBy = orderBy ?? this.orderBy;

    const paramsObj = {
      projectsViewType,
      Search: this.searchString,
      Skip: this.skip,
      OrderBy: this.orderBy,
      Take: this.defaultItemPerPage
    };

    for (let prop in paramsObj) {
      if (!paramsObj[prop]) {
        delete paramsObj[prop];
      }
    }

    return this.httpClient.get(
      `${this.routes.projectsList}?includeCount=true&expand=company,category,owner,regions,saved`,
      paramsObj
    );
  }

  duplicateProject(projId: number, _projectName: string): Observable<unknown> {
    
    return this.httpClient.post(`${this.routes.projectsList}/${projId}`, {}, { projectName: _projectName });
  }

  changeProjectStatus(projId: number, status: number): Observable<unknown> {
    return this.httpClient.patch(`${this.routes.projectsList}/${projId}`, {
      jsonPatchDocument: [
        {
          op: 'replace',
          value: status,
          path: '/StatusId'
        }
      ]
    });
  }

  changeProjectPinned(projId: number, pinned: boolean): Observable<unknown> {
    return this.httpClient.patch(`${this.routes.projectsList}/${projId}`, {
      jsonPatchDocument: [
        {
          op: 'replace',
          value: pinned ? 1 : 0,
          path: '/IsPinned'
        }
      ]
    });
  }

  clearRequestParams(): void {
    this.searchString = null;
    this.skip = null;
    this.orderBy = null;
  }


  exportProjects(): Observable<FileResponseInterface> {
    const key = Object.keys(localStorage).find(item => item.includes('accesstoken'));
    const token: string = JSON.parse(localStorage[key]).secret;
    const exportSearch = this.searchString ? `Search=${this.searchString}&` : '';
    const exportOrder = this.orderBy ? `OrderBy=${this.orderBy}` : '';
    let todaysdate = new Date();

    const request = new Request(
      `${environment.apiUrl}/${this.apiRoutes.ExportProjects}?v=${todaysdate.getTime()}&${exportSearch}${exportOrder}&includeCount=true&expand=technologies,company,category,owner,regions,saved`,
      {
        method: 'GET',
        headers: new Headers({
          authorization: `Bearer ${token}`
        })
      }
    );

    return from(
      fetch(request)
        .then(response => response.text())
        .then(data => {
          const fileDate: string = dayjs().format('D_MM_YYYY');
          this.fileObj = {
            fileData: data,
            fileName: `Projects Export file ${fileDate}.csv`
          };

          return this.fileObj;
        })
    );
  }
}
