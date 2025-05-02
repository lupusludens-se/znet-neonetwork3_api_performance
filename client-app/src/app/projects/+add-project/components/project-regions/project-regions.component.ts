import { UntilDestroy } from '@ngneat/until-destroy';
import { ControlContainer } from '@angular/forms';
import { Component, Input } from '@angular/core';

import { RegionsService } from '../../../../shared/services/regions.service';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { AddProjectStepsEnum } from '../../enums/add-project-steps.enum';
import { ProjectTypesSteps } from '../../enums/project-types-name.enum';
import { AddProjectService } from '../../services/add-project.service';
import { CountryInterface } from 'src/app/shared/interfaces/user/country.interface';

@UntilDestroy()
@Component({
  selector: 'neo-project-regions',
  styleUrls: ['../../add-project.component.scss'],
  templateUrl: './project-regions.component.html'
})
export class ProjectRegionsComponent {
  @Input() continents: CountryInterface[];

  stepsList = AddProjectStepsEnum;
  projectTypes = ProjectTypesSteps;
  selectedAll: boolean;

  constructor(
    public addProjectService: AddProjectService,
    public controlContainer: ControlContainer,
    public regionsService: RegionsService
  ) {}

  changeStep(step: number): void {
    this.addProjectService.changeStep(step);
  }

  goBack(): void {
    if (this.controlContainer.control.get('categoryId').value.technologies?.length) {
      this.addProjectService.changeStep(this.stepsList.Technologies);
    } else this.addProjectService.changeStep(this.stepsList.ProjectType);
  }

  updateForm(selectedRegions: TagInterface[]): void {
    this.controlContainer.control.get('regions').patchValue(selectedRegions);
    this.addProjectService.updateProjectGeneralData({
      regions: this.controlContainer.control.get('regions').value
    });
  }

  proceed(): void {
    const projectType = this.controlContainer.control.get('categoryId').value;

    switch (projectType.slug) {
      case ProjectTypesSteps.AggregatedPpa:
        this.changeStep(this.stepsList.OffsitePpa);
        break;
      case ProjectTypesSteps.BatteryStorage:
        this.changeStep(this.stepsList.BatteryStorage);
        break;
      case ProjectTypesSteps.CarbonOffset:
        this.changeStep(this.stepsList.CarbonOffset);
        break;
      case ProjectTypesSteps.CommunitySolar:
        this.changeStep(this.stepsList.CommunitySolar);
        break;
      case ProjectTypesSteps.EacPurchasing:
        this.changeStep(this.stepsList.EacPurchasing);
        break;
      case ProjectTypesSteps.EfficiencyAudit:
        this.changeStep(this.stepsList.EfficiencyAudit);
        break;
      case ProjectTypesSteps.EfficiencyEquipmentMeasures:
        this.changeStep(this.stepsList.EfficiencyEquipmentMeasures);
        break;
      case ProjectTypesSteps.EmergingTechnologies:
        this.changeStep(this.stepsList.EmergingTechnologies);
        break;
      case ProjectTypesSteps.EvCharging:
        this.changeStep(this.stepsList.EvCharging);
        break;
      case ProjectTypesSteps.FuelCells:
        this.changeStep(this.stepsList.FuelCells);
        break;
      case ProjectTypesSteps.OffsitePpa:
        this.changeStep(this.stepsList.OffsitePpa);
        break;
      case ProjectTypesSteps.OnsiteSolar:
        this.changeStep(this.stepsList.OnsiteSolar);
        break;
      case ProjectTypesSteps.RenewableRetail:
        this.changeStep(this.stepsList.RenewableRetail);
        break;
      case ProjectTypesSteps.UtilityGreenTariff:
        this.changeStep(this.stepsList.UtilityGreenTariff);
        break;
    }
  }
}
