import { Component, Input } from '@angular/core';
import { NotFoundDataInterface } from '../../interfaces/not-found-data.interface';

@Component({
  selector: 'neo-element-not-found',
  templateUrl: './element-not-found.component.html',
  styleUrls: ['./element-not-found.component.scss']
})
export class ElementNotFoundComponent {
  @Input() data: NotFoundDataInterface;
}
