import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { SvgIconsModule } from '@ngneat/svg-icon';

import { ManageFilesRoutingModule } from './manage-files-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { MenuModule } from 'src/app/shared/modules/menu/menu.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';

import { FileService } from 'src/app/shared/services/file.service';

import { FileTableRowComponent } from './components/file-table-row/file-table-row.component';
import { FileListViewComponent } from './components/file-list-view/file-list-view.component';
import { InitiativeSharedService } from '../shared/services/initiative-shared.service';

@NgModule({
  declarations: [FileTableRowComponent, FileListViewComponent],
  imports: [
    CommonModule,
    SharedModule,
    ManageFilesRoutingModule,
    TranslateModule,
    SvgIconsModule,
    MenuModule,
    ModalModule,
    PaginationModule
  ],
  providers: [FileService, InitiativeSharedService]
})
export class ManageFilesModule {}
