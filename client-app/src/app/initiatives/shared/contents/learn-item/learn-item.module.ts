import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { DropdownModule } from 'src/app/shared/modules/controls/dropdown/dropdown.module';
import { TranslateModule } from '@ngx-translate/core';
import { LearnItemComponent } from './learn-item.component';
import { MenuModule } from 'src/app/shared/modules/menu/menu.module';
import { ContentTagModule } from 'src/app/shared/modules/content-tag/content-tag.module';

@NgModule({
  declarations: [LearnItemComponent],
  imports: [CommonModule, SharedModule, DropdownModule, TranslateModule, MenuModule, ContentTagModule],
  exports: [LearnItemComponent]
})
export class LearnItemModule {}
