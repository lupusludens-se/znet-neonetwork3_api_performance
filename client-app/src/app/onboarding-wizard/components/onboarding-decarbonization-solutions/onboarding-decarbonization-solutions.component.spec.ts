import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OnboardingDecarbonizationSolutionsComponent } from './onboarding-decarbonization-solutions.component';

describe('OnboardingDecarbonizationSolutionsComponent', () => {
  let component: OnboardingDecarbonizationSolutionsComponent;
  let fixture: ComponentFixture<OnboardingDecarbonizationSolutionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OnboardingDecarbonizationSolutionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OnboardingDecarbonizationSolutionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
