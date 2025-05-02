import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientsFeedbackComponent } from './clients-feedback.component';

describe('ClientsFeedbackComponent', () => {
  let component: ClientsFeedbackComponent;
  let fixture: ComponentFixture<ClientsFeedbackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ClientsFeedbackComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientsFeedbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
