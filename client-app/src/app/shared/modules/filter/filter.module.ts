// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { BlueCheckboxModule } from '../blue-checkbox/blue-checkbox.module';
import { RadioControlModule } from '../radio-control/radio-control.module';
import { ReactiveFormsModule } from '@angular/forms';

// components
import { FilterButtonComponent } from './components/filter-button/filter-button.component';
import { TaxonomyFilterComponent } from './components/taxonomy-filter/taxonomy-filter.component';
import { TaxonomyHierarchicalFilterComponent } from './components/taxonomy-hierarchical-filter/taxonomy-hierarchical-filter.component';
import { FilterStateComponent } from './components/filter-state/filter-state.component';
import { FilterExpandComponent } from './components/filter-expand/filter-expand.component';

@NgModule({
  declarations: [
    FilterButtonComponent,
    TaxonomyFilterComponent,
    TaxonomyHierarchicalFilterComponent,
    FilterStateComponent,
    FilterExpandComponent
  ],
  exports: [
    FilterButtonComponent,
    TaxonomyFilterComponent,
    TaxonomyHierarchicalFilterComponent,
    FilterStateComponent,
    FilterExpandComponent
  ],
  imports: [SharedModule, BlueCheckboxModule, RadioControlModule, ReactiveFormsModule]
})
export class FilterModule {}
