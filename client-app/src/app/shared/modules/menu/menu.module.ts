// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

// components
import { MenuComponent } from './menu.component';

@NgModule({
  declarations: [MenuComponent],
  imports: [SharedModule],
  exports: [MenuComponent]
})
export class MenuModule {}
