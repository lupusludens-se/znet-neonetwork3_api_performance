import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativeLearnSavedComponent } from './initiative-learn-saved.component';

describe('InitiativeLearnSavedComponent', () => {
  let component: InitiativeLearnSavedComponent;
  let fixture: ComponentFixture<InitiativeLearnSavedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeLearnSavedComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeLearnSavedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
