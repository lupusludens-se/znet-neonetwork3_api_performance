import { LocationStrategy } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { NewTrendingProjectResponse } from 'src/app/shared/interfaces/projects/project.interface';

@Component({
  selector: 'neo-new-trending-project-tile',
  templateUrl: './corporate-new-trending-project-tile.component.html',
  styleUrls: ['./corporate-new-trending-project-tile.component.scss']
})
export class NewTrendingProjectTileComponent {
  @Input() recentlyViewed: boolean = false;
  @Input() post: NewTrendingProjectResponse;

  constructor(private locationStrategy: LocationStrategy) {}

  goToLink(url: string) {
    const getBaseHref = location.origin + this.locationStrategy.getBaseHref();
    const learnUrl = getBaseHref + url;
    window.open(learnUrl, '_blank');
  }
  taxonomyType: TaxonomyTypeEnum = TaxonomyTypeEnum.Category;
}
