import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'neo-term-of-use',
  templateUrl: './term-of-use.component.html',
  styleUrls: ['./term-of-use.component.scss']
})
export class TermOfUseComponent {
  @Output() closed = new EventEmitter<Record<string, string>>();
}
