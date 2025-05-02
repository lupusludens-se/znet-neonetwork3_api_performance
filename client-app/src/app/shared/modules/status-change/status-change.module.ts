// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

// components
import { StatusChangeComponent } from './components/status-change/status-change.component';

@NgModule({
  declarations: [StatusChangeComponent],
  exports: [StatusChangeComponent],
  imports: [SharedModule]
})
export class StatusChangeModule {}
