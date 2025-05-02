import { LocationStrategy } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { SPDashboardProjectDetails } from 'src/app/shared/interfaces/projects/project.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'neo-sp-project-tile',
  templateUrl: './sp-project-tile.component.html',
  styleUrls: ['./sp-project-tile.component.scss']
})
export class SpProjectTileComponent {
  @Input() recentlyViewed: boolean = false;
  @Input() project: SPDashboardProjectDetails;
  @Input() isShowAddProject: boolean = false;
  @Input() isNoProjects: boolean = false;
  constructor(private locationStrategy: LocationStrategy, private router: Router) {}

  goToLink(url: string) {
    const getBaseHref = location.origin + this.locationStrategy.getBaseHref();
    const learnUrl = getBaseHref + url;
    window.open(learnUrl, '_blank');
  }
  taxonomyType: TaxonomyTypeEnum = TaxonomyTypeEnum.Category;

  addProjectRedirect() {
    this.router.navigate(['projects-library/add-project']);
  }
}
