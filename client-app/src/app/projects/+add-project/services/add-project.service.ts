import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { ProjectInterface } from '../../../shared/interfaces/projects/project.interface';
import { ProjectStatusEnum } from 'src/app/shared/enums/projects/project-status.enum';
import { ProjectApiRoutes } from '../../shared/constants/project-api-routes.const';
import { AddProjectStepsEnum } from '../enums/add-project-steps.enum';
import { HttpService } from '../../../core/services/http.service';
import {
  ProjectBatteryStorageDetailsInterface,
  ProjectCarbonOffsetDetailsInterface,
  ProjectCommunitySolarDetailsInterface,
  ProjectEacPurchasingDetailsInterface,
  ProjectEfficiencyAuditsDetailsInterface,
  ProjectEfficiencyMeasuresDetailsInterface,
  ProjectEmergingTechnologiesDetailsInterface,
  ProjectEvChargingDetailsInterface,
  ProjectFuelCellsDetailsInterface,
  ProjectGreenTariffDetailsInterface,
  ProjectOffsitePpaInterface,
  ProjectOnsiteSolarDetailsInterface,
  ProjectRenewableElectricityDetailsInterface
} from '../../../shared/interfaces/projects/project-creation.interface';
import { CoreService } from 'src/app/core/services/core.service';

export interface AddProjectPayloadInterface {
  project: Partial<ProjectInterface>;
  projectDetails:
    | ProjectBatteryStorageDetailsInterface
    | ProjectFuelCellsDetailsInterface
    | Partial<ProjectOffsitePpaInterface>
    | Partial<ProjectCarbonOffsetDetailsInterface>
    | Partial<ProjectCommunitySolarDetailsInterface>
    | Partial<ProjectEacPurchasingDetailsInterface>
    | Partial<ProjectEfficiencyAuditsDetailsInterface>
    | Partial<ProjectEfficiencyMeasuresDetailsInterface>
    | Partial<ProjectEmergingTechnologiesDetailsInterface>
    | Partial<ProjectEvChargingDetailsInterface>
    | Partial<ProjectOnsiteSolarDetailsInterface>
    | Partial<ProjectRenewableElectricityDetailsInterface>
    | Partial<ProjectGreenTariffDetailsInterface>;
}

@Injectable()
export class AddProjectService {
  draftSuccess$: Subject<boolean> = new Subject<boolean>();
  apiRoutes = ProjectApiRoutes;
  projectStatuses = ProjectStatusEnum;
  currentFlowData: AddProjectPayloadInterface = {
    project: null,
    projectDetails: null
  };
  stepsList = AddProjectStepsEnum;
  private currentStep: BehaviorSubject<number> = new BehaviorSubject<number>(AddProjectStepsEnum.ProjectType);
  currentStep$: Observable<number> = this.currentStep.asObservable();

  constructor(private router: Router, private httpService: HttpService, private coreService: CoreService) {}

  changeStep(section: number): void {
    this.currentStep.next(section);
  }

  exitFlow(): void {
    this.router.navigate(['projects-library']);
  }

  updateProjectGeneralData(newData: Partial<ProjectInterface>): void {
    this.currentFlowData.project = this.currentFlowData?.project
      ? { ...this.currentFlowData.project, ...newData }
      : { ...newData };
  }

  updateProjectTypeData(
    projectTypeData:
      | ProjectBatteryStorageDetailsInterface
      | ProjectFuelCellsDetailsInterface
      | Partial<ProjectOffsitePpaInterface>
      | Partial<ProjectCarbonOffsetDetailsInterface>
      | Partial<ProjectCommunitySolarDetailsInterface>
      | Partial<ProjectEacPurchasingDetailsInterface>
      | Partial<ProjectEfficiencyAuditsDetailsInterface>
      | Partial<ProjectEfficiencyMeasuresDetailsInterface>
      | Partial<ProjectEmergingTechnologiesDetailsInterface>
      | Partial<ProjectEvChargingDetailsInterface>
      | Partial<ProjectOnsiteSolarDetailsInterface>
      | Partial<ProjectRenewableElectricityDetailsInterface>
      | Partial<ProjectGreenTariffDetailsInterface>
  ): void {
    this.currentFlowData.projectDetails = {
      ...this.currentFlowData.projectDetails,
      ...projectTypeData
    };
  }

  saveProject(route: string, draftStatus?: ProjectStatusEnum): void {
    if (this.currentFlowData.project.description) {
      this.currentFlowData.project.descriptionText = this.coreService.convertToPlain(
        this.currentFlowData.project.description ?? ''
      );
    }

    if (this.currentFlowData.project.opportunity) {
      this.currentFlowData.project.opportunityText = this.coreService.convertToPlain(
        this.currentFlowData.project.opportunity ?? ''
      );
    }

    if (this.currentFlowData.project.publishedBy) {
      this.currentFlowData.project.ownerId = this.currentFlowData.project.publishedBy.id;

      delete this.currentFlowData.project.publishedBy;
    }

    this.currentFlowData.project.technologies = this.currentFlowData.project.technologies?.map(t => {
      return { id: t.id };
    });

    this.currentFlowData.project.regions = this.currentFlowData.project.regions.map(r => {
      return { id: r.id };
    });

    this.currentFlowData.project.statusId = draftStatus ?? this.projectStatuses.Active;

    if (!this.currentFlowData.projectDetails) {
      this.currentFlowData.projectDetails = {};
    }

    this.currentFlowData.projectDetails.statusId = draftStatus ?? this.projectStatuses.Active;

    const httpType = this.currentFlowData.project.id ? 'put' : 'post';

    this.httpService[httpType](route, this.currentFlowData).subscribe(res => {
      if (draftStatus === this.projectStatuses.Draft) {
        this.currentFlowData.project.id = res as number;
        this.draftSuccess$.next(true);
      } else {
        this.currentFlowData = {
          project: null,
          projectDetails: null
        };
        this.router.navigate(['projects-library']);
        this.changeStep(this.stepsList.ProjectType);
      }
    });
  }
}
