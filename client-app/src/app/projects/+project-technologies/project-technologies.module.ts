import { NgModule } from '@angular/core';
import { ProjectTechnologiesComponent } from './project-technologies.component';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';
import { ProjectTechnologiesTileComponent } from './components/project-technologies-tile/project-technologies-tile.component';

const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: ProjectTechnologiesComponent
  }
];

@NgModule({
  declarations: [ProjectTechnologiesComponent, ProjectTechnologiesTileComponent],
  imports: [SharedModule, RouterModule.forChild(routes), ModalModule]
})
export class ProjectTechnologiesModule {}
