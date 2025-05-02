import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyManipulationComponent } from './company-manipulation.component';

describe('CompanyManipulationComponent', () => {
  let component: CompanyManipulationComponent;
  let fixture: ComponentFixture<CompanyManipulationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompanyManipulationComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyManipulationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
