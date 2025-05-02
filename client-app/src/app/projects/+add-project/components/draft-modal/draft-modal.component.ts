import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

import { ProjectStatusEnum } from '../../../../shared/enums/projects/project-status.enum';
import { ProjectApiRoutes } from '../../../shared/constants/project-api-routes.const';
import { CustomValidator } from '../../../../shared/validators/custom.validator';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { ProjectTypesSteps } from '../../enums/project-types-name.enum';
import { AddProjectService } from '../../services/add-project.service';

@Component({
  selector: 'neo-draft-modal',
  templateUrl: './draft-modal.component.html',
  styleUrls: ['draft-modal.component.scss']
})
export class DraftModalComponent implements OnInit {
  @Input() title: string;
  @Input() projectTypeSlug: string;
  @Output() closeModal: EventEmitter<boolean> = new EventEmitter();
  @Output() projectTitle: EventEmitter<string> = new EventEmitter();
  apiRoutes = ProjectApiRoutes;
  projectStatuses = ProjectStatusEnum;
  form: FormGroup = this.formBuilder.group({
    title: ['', CustomValidator.required]
  });
  stepsList = AddProjectStepsEnum;

  constructor(private formBuilder: FormBuilder, public addProjectService: AddProjectService) {}

  ngOnInit(): void {
    if (this.title) {
      this.form.patchValue({ title: this.title });
    }
  }

  saveDraft(): void {
    let routeName: string = '';

    const projId = this.addProjectService.currentFlowData.project.id
      ? `/${this.addProjectService.currentFlowData.project.id}`
      : '';

    switch (this.projectTypeSlug) {
      case ProjectTypesSteps.BatteryStorage:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.batteryStorage;
        break;
      case ProjectTypesSteps.FuelCells:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.fuelCells;
        break;
      case ProjectTypesSteps.CarbonOffset:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.carbonOffset;
        break;
      case ProjectTypesSteps.CommunitySolar:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.communitySolar;
        break;
      case ProjectTypesSteps.EacPurchasing:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.eacPurchasing;
        break;
      case ProjectTypesSteps.EfficiencyAudit:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.efficiencyAuditAndConsulting;
        break;
      case ProjectTypesSteps.EfficiencyEquipmentMeasures:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.efficiencyEquipmentMeasures;
        break;
      case ProjectTypesSteps.EmergingTechnologies:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.emergingTechnologies;
        break;
      case ProjectTypesSteps.EvCharging:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.evCharging;
        break;
      case ProjectTypesSteps.OnsiteSolar:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.onsiteSolar;
        break;
      case ProjectTypesSteps.RenewableRetail:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.renewableRetail;
        break;
      case ProjectTypesSteps.UtilityGreenTariff:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.greenTariff;
        break;
      case ProjectTypesSteps.OffsitePpa:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.offsitePpa;
        break;
      case ProjectTypesSteps.AggregatedPpa:
        routeName = this.apiRoutes.projectsList + projId + this.apiRoutes.offsitePpa;
        break;
    }

    this.addProjectService.updateProjectGeneralData({
      title: this.form.value.title
    });
    this.projectTitle.emit(this.form.value.title);

    this.addProjectService.saveProject(routeName, this.projectStatuses.Draft);
  }

  finishDraft(): void {
    this.addProjectService.changeStep(this.stepsList.ProjectType);
  }
}
