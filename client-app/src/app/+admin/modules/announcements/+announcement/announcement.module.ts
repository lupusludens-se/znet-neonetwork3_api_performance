// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { NoResultsModule } from '../../../../shared/modules/no-results/no-results.module';
import { PaginationModule } from '../../../../shared/modules/pagination/pagination.module';
import { TableModule } from '../../../../shared/modules/table/table.module';
import { ModalModule } from '../../../../shared/modules/modal/modal.module';
import { ConfirmAnnouncementModule } from '../modules/confirm-announcement/confirm-announcement.module';
import { MenuModule } from '../../../../shared/modules/menu/menu.module';

// components
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { AnnouncementComponent } from './announcement.component';
import { AnnouncementsService } from '../services/announcements.service';
import { AnnouncementTableRowComponent } from './announcement-table-row/announcement-table-row.component';
import { EmptyPageModule } from 'src/app/shared/modules/empty-page/empty-page.module';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: AnnouncementComponent,
    canActivate: [MsalGuard]
  },
  {
    path: 'add',
    data: { breadcrumb: 'Add Announcement' },
    loadChildren: () => import('../+edit-announcement/edit-announcement.module').then(m => m.EditAnnouncementModule),
    canActivate: [MsalGuard]
  },
  {
    path: 'edit/:id',
    data: { breadcrumb: 'Edit Announcement' },
    pathMatch: 'full',
    loadChildren: () => import('../+edit-announcement/edit-announcement.module').then(m => m.EditAnnouncementModule),
    canActivate: [MsalGuard]
  }
];

@NgModule({
  declarations: [AnnouncementComponent, AnnouncementTableRowComponent],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    NoResultsModule,
    PaginationModule,
    TableModule,
    ModalModule,
    ConfirmAnnouncementModule,
    MenuModule,
    EmptyPageModule
  ],
  providers: [AnnouncementsService]
})
export class AnnouncementModule {}
