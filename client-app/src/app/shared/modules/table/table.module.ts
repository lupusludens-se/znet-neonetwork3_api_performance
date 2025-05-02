//modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { MenuModule } from '../menu/menu.module';

//components
import { TableComponent } from './components/table/table.component';

@NgModule({
  declarations: [TableComponent],
  imports: [SharedModule, MenuModule],
  exports: [TableComponent]
})
export class TableModule {}
