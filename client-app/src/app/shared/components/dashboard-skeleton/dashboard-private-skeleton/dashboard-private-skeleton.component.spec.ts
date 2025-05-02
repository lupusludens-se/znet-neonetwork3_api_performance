import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardPrivateSkeletonComponent } from './dashboard-private-skeleton.component';

describe('DashboardPrivateSkeletonComponent', () => {
  let component: DashboardPrivateSkeletonComponent;
  let fixture: ComponentFixture<DashboardPrivateSkeletonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DashboardPrivateSkeletonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardPrivateSkeletonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
