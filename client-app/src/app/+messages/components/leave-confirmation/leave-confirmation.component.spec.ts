import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeaveConfirmationComponent } from './leave-confirmation.component';

describe('LeaveConfirmationComponent', () => {
  let component: LeaveConfirmationComponent;
  let fixture: ComponentFixture<LeaveConfirmationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LeaveConfirmationComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LeaveConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
