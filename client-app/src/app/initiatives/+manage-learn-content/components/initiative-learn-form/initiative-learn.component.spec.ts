import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativeLearnComponent } from './initiative-learn.component';

describe('InitiativeLearnComponent', () => {
  let component: InitiativeLearnComponent;
  let fixture: ComponentFixture<InitiativeLearnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeLearnComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeLearnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
