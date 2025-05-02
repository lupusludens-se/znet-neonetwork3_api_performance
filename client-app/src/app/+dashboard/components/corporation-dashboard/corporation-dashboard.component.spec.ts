import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CorporationDashboardComponent } from './corporation-dashboard.component';

describe('CorporationDashboardComponent', () => {
  let component: CorporationDashboardComponent;
  let fixture: ComponentFixture<CorporationDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CorporationDashboardComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CorporationDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
