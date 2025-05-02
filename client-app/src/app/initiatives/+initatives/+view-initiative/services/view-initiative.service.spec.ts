import { TestBed } from '@angular/core/testing';

import { ViewInitiativeService } from './view-initiative.service';

describe('ViewInitiativeService', () => {
  let service: ViewInitiativeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ViewInitiativeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
