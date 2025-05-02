import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicCorporationDashboardComponent } from './public-corporation-dashboard.component';

describe('PublicCorporationDashboardComponent', () => {
  let component: PublicCorporationDashboardComponent;
  let fixture: ComponentFixture<PublicCorporationDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PublicCorporationDashboardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PublicCorporationDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
