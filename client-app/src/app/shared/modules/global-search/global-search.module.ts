// modules
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared.module';
import { VerticalLineDecorModule } from '../vertical-line-decor/vertical-line-decor.module';
import { NoResultsModule } from '../no-results/no-results.module';

// components
import { GlobalSearchComponent } from './global-search.component';

@NgModule({
  declarations: [GlobalSearchComponent],
  exports: [GlobalSearchComponent],
  imports: [VerticalLineDecorModule, SharedModule, FormsModule, RouterModule, NoResultsModule]
})
export class GlobalSearchModule {}
