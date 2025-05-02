import { Component, Input } from '@angular/core';

@Component({
  selector: 'neo-empty-page',
  templateUrl: './empty-page.component.html',
  styleUrls: ['./empty-page.component.scss']
})
export class EmptyPageComponent {
  @Input() iconKey: string;
  @Input() heightClass: string = 'h-100';
  @Input() line2TextClass: string;
  @Input() headlinePart1: string;
  @Input() headlinePart2: string;
  @Input() buttonText: string;
  @Input() link: string;
  @Input() circleBackground: boolean = true;
}
