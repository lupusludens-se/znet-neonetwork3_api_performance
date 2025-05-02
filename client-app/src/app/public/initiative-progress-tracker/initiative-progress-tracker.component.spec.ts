import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativeProgressTrackerComponent } from './initiative-progress-tracker.component';

describe('InitiativeProgressTrackerComponent', () => {
  let component: InitiativeProgressTrackerComponent;
  let fixture: ComponentFixture<InitiativeProgressTrackerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeProgressTrackerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeProgressTrackerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
