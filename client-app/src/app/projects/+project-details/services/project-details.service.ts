import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ProjectApiRoutes } from '../../shared/constants/project-api-routes.const';
import { ProjectInterface, ProjectResourceInterface } from 'src/app/shared/interfaces/projects/project.interface';
import { InitiativeModulesEnum } from 'src/app/initiatives/enums/initiative-modules.enum';
import { InitiativeAttachedContent } from 'src/app/initiatives/interfaces/initiative-attached.interface';
import { InitiativeApiEnum } from 'src/app/initiatives/enums/initiative-api.enum';
import { HttpService } from 'src/app/core/services/http.service';

@Injectable()
export class ProjectDetailsService {
  private readonly routes = ProjectApiRoutes;

  constructor(private httpService: HttpService) {}

  public getProjectDetails(id: number, viewType: string): Observable<ProjectInterface> {
    return this.httpService.get<ProjectInterface>(
      `${this.routes.projectsList}/${id}?expand=category,company,company.image,owner,technologies,regions,projectdetails,saved${viewType}`
    );
  }

  public getProjectResourceDetails(id: number, viewType: string): Observable<ProjectResourceInterface> {
    return this.httpService.get<ProjectResourceInterface>(
      `${this.routes.projectResourcesById}/${id}?expand=category.resources,technologies.resources,saved${viewType}`
    );
  }

  public getInitiativeProjectDetails(
    projectId: number,
    contentType: InitiativeModulesEnum
  ): Observable<InitiativeAttachedContent[]> {
    return this.httpService.get<InitiativeAttachedContent[]>(
      `${InitiativeApiEnum.GetInitiativeAttachedDetails}/${projectId}/contentType/${contentType}`
    );
  }
}
