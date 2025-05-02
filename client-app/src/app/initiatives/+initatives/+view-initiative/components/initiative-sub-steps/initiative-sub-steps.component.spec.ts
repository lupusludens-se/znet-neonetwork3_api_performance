import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativeSubStepsComponent } from './initiative-sub-steps.component';

describe('InitiativeSubStepsComponent', () => {
  let component: InitiativeSubStepsComponent;
  let fixture: ComponentFixture<InitiativeSubStepsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeSubStepsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeSubStepsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
