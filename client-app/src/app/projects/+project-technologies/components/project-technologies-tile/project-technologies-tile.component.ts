import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

import { FilterDataInterface } from 'src/app/shared/modules/filter/interfaces/filter-data.interface';
import { TechnologiesResourcesInterface } from '../../interfaces/technologies-resources.interface';
import { CommonService } from 'src/app/core/services/common.service';

@Component({
  selector: 'neo-project-technologies-tile',
  templateUrl: './project-technologies-tile.component.html',
  styleUrls: ['./project-technologies-tile.component.scss']
})
export class ProjectTechnologiesTileComponent {
  @Input() technology: TechnologiesResourcesInterface;

  constructor(private router: Router, private readonly commonService: CommonService) {}

  browse(): void {
    const categoriesFilter = this.commonService.filterState$.value.parameter['technologies'] as FilterDataInterface[];

    categoriesFilter.forEach(c => {
      c.checked = c.id == this.technology.id;
    });

    this.router.navigate(['projects']);
  }
}
