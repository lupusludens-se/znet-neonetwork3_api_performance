import { Component, Input } from '@angular/core';

@Component({
  selector: 'neo-saved-content-loader',
  templateUrl: './saved-content-loader.component.html',
  styleUrls: ['./saved-content-loader.component.scss']
})
export class SavedContentLoaderComponent {
  @Input() showWrapper: boolean;
}
