import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterExpandComponent } from './filter-expand.component';

describe('TaxonomyFilterComponent', () => {
  let component: FilterExpandComponent;
  let fixture: ComponentFixture<FilterExpandComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FilterExpandComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FilterExpandComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
