import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { DropdownModule } from 'src/app/shared/modules/controls/dropdown/dropdown.module';
import { TranslateModule } from '@ngx-translate/core';
import { MenuModule } from 'src/app/shared/modules/menu/menu.module';
import { ContentTagModule } from 'src/app/shared/modules/content-tag/content-tag.module';
import { ToolItemComponent } from './tool-item.component';

@NgModule({
  declarations: [ToolItemComponent],
  imports: [
    CommonModule,
    SharedModule,
    DropdownModule,
    TranslateModule,
    MenuModule,
    ContentTagModule
  ],
  exports: [ToolItemComponent]
})
export class ToolItemModule {}