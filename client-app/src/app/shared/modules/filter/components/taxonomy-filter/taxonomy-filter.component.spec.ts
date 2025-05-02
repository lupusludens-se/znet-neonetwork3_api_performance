import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaxonomyFilterComponent } from './taxonomy-filter.component';

describe('TaxonomyFilterComponent', () => {
  let component: TaxonomyFilterComponent;
  let fixture: ComponentFixture<TaxonomyFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TaxonomyFilterComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TaxonomyFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
