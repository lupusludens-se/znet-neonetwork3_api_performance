import { TestBed } from '@angular/core/testing';

import { EditInitiativeService } from './edit-initiative.service';

describe('EditInitiativeService', () => {
  let service: EditInitiativeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EditInitiativeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
