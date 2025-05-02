import { Component, Input } from '@angular/core';
import { TagInterface } from '../../../../core/interfaces/tag.interface';
import { TaxonomyTypeEnum } from '../../../../shared/enums/taxonomy-type.enum';

@Component({
  selector: 'neo-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.scss']
})
export class CompanyComponent {
  @Input() image: string;
  @Input() title: string;
  @Input() tags: TagInterface[];

  type = TaxonomyTypeEnum;
}
