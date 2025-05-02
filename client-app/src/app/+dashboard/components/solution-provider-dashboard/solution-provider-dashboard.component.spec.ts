import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SolutionProviderDashboardComponent } from './solution-provider-dashboard.component';

describe('SolutionProviderDashboardComponent', () => {
  let component: SolutionProviderDashboardComponent;
  let fixture: ComponentFixture<SolutionProviderDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SolutionProviderDashboardComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SolutionProviderDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
