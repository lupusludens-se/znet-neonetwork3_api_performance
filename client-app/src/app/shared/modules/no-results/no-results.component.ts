import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'neo-no-results',
  templateUrl: 'no-results.component.html',
  styleUrls: ['./no-results.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NoResultsComponent {
  @Input() searchedStr: string;
  @Input() headlineStyleClasses: string = 'no-results-label mb-10';
  @Input() pointsStyleClasses: string = 'text-s mb-4 pl-8';
}
