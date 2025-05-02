import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativeMessageRecommendedComponent } from './initiative-message-recommended.component';

describe('InitiativeMessageRecommendedComponent', () => {
  let component: InitiativeMessageRecommendedComponent;
  let fixture: ComponentFixture<InitiativeMessageRecommendedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeMessageRecommendedComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeMessageRecommendedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
