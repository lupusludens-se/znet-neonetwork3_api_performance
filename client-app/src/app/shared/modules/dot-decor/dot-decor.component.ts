import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'neo-dot-decor',
  template: `<div class="dot ml-6 mr-6"></div>`,
  styleUrls: ['dot-decor.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DotDecorComponent {}
