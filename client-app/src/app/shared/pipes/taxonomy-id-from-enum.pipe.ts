import { Pipe, PipeTransform } from '@angular/core';
import { TaxonomyEnum } from '../../core/enums/taxonomy.enum';
import { TaxonomyTypeEnum } from '../enums/taxonomy-type.enum';

@Pipe({
  name: 'toId'
})
export class TaxonomyIdFromEnumPipe implements PipeTransform {
  transform(value: string) {
    switch (value) {
      case TaxonomyEnum.solutions:
        return TaxonomyTypeEnum.Solution;
      case TaxonomyEnum.categories:
        return TaxonomyTypeEnum.Category;
      case TaxonomyEnum.technologies:
        return TaxonomyTypeEnum.Technology;
      case TaxonomyEnum.regions:
        return TaxonomyTypeEnum.Region;
      case TaxonomyEnum.contentTags:
        return TaxonomyTypeEnum.ContentTag;
    }
  }
}
