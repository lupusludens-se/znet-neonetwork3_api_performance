import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { ProjectItemComponent } from './project-item.component';
import { ContentTagModule } from 'src/app/shared/modules/content-tag/content-tag.module';
import { ContentLocationModule } from 'src/app/shared/modules/content-location/content-location.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { MenuModule } from 'src/app/shared/modules/menu/menu.module';
@NgModule({
  declarations: [ProjectItemComponent],
  imports: [CommonModule, ContentTagModule, ContentLocationModule, TranslateModule, SharedModule, MenuModule],
  exports: [ProjectItemComponent]
})
export class ProjectItemModule {}
