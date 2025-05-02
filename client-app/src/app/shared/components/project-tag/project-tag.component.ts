import { Component, Input } from '@angular/core';
import { ResourceInterface } from '../../../core/interfaces/resource.interface';

@Component({
  selector: 'neo-project-tag',
  templateUrl: './project-tag.component.html',
  styleUrls: ['./project-tag.component.scss']
})
export class ProjectTagComponent {
  @Input() tag: ResourceInterface;

  constructor() {}
}
