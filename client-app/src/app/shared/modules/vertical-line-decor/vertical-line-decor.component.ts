import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'neo-vertical-line-decor',
  template: `<div class="vert-line"></div>`,
  styleUrls: ['vertical-line-decor.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class VerticalLineDecorComponent {}
