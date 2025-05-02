import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NewAndNoteworthyPostInterface } from 'src/app/+learn/interfaces/post.interface';

@Component({
  selector: 'neo-new-and-noteworthy',
  templateUrl: './new-and-noteworthy.component.html',
  styleUrls: ['./new-and-noteworthy.component.scss']
})
export class NewAndNoteworthyComponent {
  @Input() newAndNoteworthyData: NewAndNoteworthyPostInterface;
  @Output() postClick: EventEmitter<void> = new EventEmitter<void>();
  constructor() {}
}
