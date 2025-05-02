// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

// components
import { ToolComponent } from './components/tool/tool.component';
import { RouterModule } from '@angular/router';
import { PublicModule } from '../../../public/public.module';

@NgModule({
  declarations: [ToolComponent],
  imports: [SharedModule, RouterModule, PublicModule],
  exports: [ToolComponent]
})
export class ToolModule {}
