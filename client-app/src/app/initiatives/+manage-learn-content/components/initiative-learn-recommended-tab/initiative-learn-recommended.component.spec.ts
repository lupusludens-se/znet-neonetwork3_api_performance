import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativeLearnRecommendedComponent } from './initiative-learn-recommended.component';

describe('InitiativeLearnRecommendedComponent', () => {
  let component: InitiativeLearnRecommendedComponent;
  let fixture: ComponentFixture<InitiativeLearnRecommendedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeLearnRecommendedComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeLearnRecommendedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
