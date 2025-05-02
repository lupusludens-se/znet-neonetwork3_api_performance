import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaxonomyHierarchicalFilterComponent } from './taxonomy-hierarchical-filter.component';

describe('TaxonomyFilterGroupComponent', () => {
  let component: TaxonomyHierarchicalFilterComponent;
  let fixture: ComponentFixture<TaxonomyHierarchicalFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TaxonomyHierarchicalFilterComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TaxonomyHierarchicalFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
