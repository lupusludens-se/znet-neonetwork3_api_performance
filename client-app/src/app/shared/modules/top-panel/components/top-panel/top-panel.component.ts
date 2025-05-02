import { Component, Input } from '@angular/core';

@Component({
  selector: 'neo-top-panel',
  templateUrl: './top-panel.component.html',
  styleUrls: ['./top-panel.component.scss']
})
export class TopPanelComponent {
  @Input() imageCssClasses: string = 'right-0 bottom-0 w-60';
  @Input() topPanelClasses: string;
  @Input() imageUrl: string = 'assets/images/user-profile-background-image.svg';
}
