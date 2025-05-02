import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterStateComponent } from './filter-state.component';

describe('FilterStateComponent', () => {
  let component: FilterStateComponent;
  let fixture: ComponentFixture<FilterStateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FilterStateComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FilterStateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
