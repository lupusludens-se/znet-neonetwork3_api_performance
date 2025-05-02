import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThankYouInterestModalPopupComponent } from './thank-you-interest-modal-popup.component';

describe('ThankYouInterestModalPopupComponent', () => {
  let component: ThankYouInterestModalPopupComponent;
  let fixture: ComponentFixture<ThankYouInterestModalPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ThankYouInterestModalPopupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ThankYouInterestModalPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
