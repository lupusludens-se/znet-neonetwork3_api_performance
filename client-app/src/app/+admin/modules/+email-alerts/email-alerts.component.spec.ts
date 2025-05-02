import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailAlertsComponent } from './email-alerts.component';

describe('EmailAlertsComponent', () => {
  let component: EmailAlertsComponent;
  let fixture: ComponentFixture<EmailAlertsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EmailAlertsComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailAlertsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
