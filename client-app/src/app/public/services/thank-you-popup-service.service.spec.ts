import { TestBed } from '@angular/core/testing';

import { ThankYouPopupServiceService } from './thank-you-popup-service.service';

describe('ThankYouPopupServiceService', () => {
  let service: ThankYouPopupServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ThankYouPopupServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
