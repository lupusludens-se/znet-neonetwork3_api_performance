import { TestBed } from '@angular/core/testing';

import { InitiativeDetailsService } from './initiative-details.service';

describe('InitiativeDetailsService', () => {
  let service: InitiativeDetailsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InitiativeDetailsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
