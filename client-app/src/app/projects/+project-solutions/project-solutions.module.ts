import { NgModule } from '@angular/core';
import { ProjectSolutionsComponent } from './project-solutions.component';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';
import { ProjectSolutionsTileComponent } from './components/project-solutions-tile/project-solutions-tile.component';
import { ProjectSolutionsItemComponent } from './components/project-solutions-item/project-solutions-item.component';
import { ProjectSolutionsItemElementComponent } from './components/project-solutions-item/components/project-solutions-item-element/project-solutions-item-element.component';
import { ProjectSolutionTagComponent } from './components/project-solutions-item/components/project-solution-tag/project-solution-tag.component';
import { PublicModule } from 'src/app/public/public.module';
import { TopPanelModule } from 'src/app/shared/modules/top-panel/top-panel.module';
const routes: Routes = [
  {
    path: '',
    data: { breadcrumbSkip: true },
    component: ProjectSolutionsComponent
  },
  {
    path: ':id',
    pathMatch: 'full',
    data: { breadcrumbSkip: true },
    component: ProjectSolutionsItemComponent
  }
];

@NgModule({
  declarations: [
    ProjectSolutionsComponent,
    ProjectSolutionsTileComponent,
    ProjectSolutionsItemComponent,
    ProjectSolutionsItemElementComponent,
    ProjectSolutionTagComponent
  ],
  imports: [SharedModule, RouterModule.forChild(routes), ModalModule, PublicModule, TopPanelModule]
})
export class ProjectSolutionsModule {}
