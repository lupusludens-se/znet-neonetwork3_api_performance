import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserFeedbackDetailsComponent } from './user-feedback-details.component';

describe('UserFeedbackDetailsComponent', () => {
  let component: UserFeedbackDetailsComponent;
  let fixture: ComponentFixture<UserFeedbackDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserFeedbackDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserFeedbackDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
