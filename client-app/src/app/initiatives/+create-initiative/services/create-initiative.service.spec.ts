import { TestBed } from '@angular/core/testing';

import { CreateInitiativeService } from './create-initiative.service';

describe('CreateInitiativeService', () => {
  let service: CreateInitiativeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreateInitiativeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
