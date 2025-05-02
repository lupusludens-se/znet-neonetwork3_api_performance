import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativeToolsRecommendedComponent } from './initiative-tools-recommended.component';

describe('InitiativeToolsRecommendedComponent', () => {
  let component: InitiativeToolsRecommendedComponent;
  let fixture: ComponentFixture<InitiativeToolsRecommendedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeToolsRecommendedComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeToolsRecommendedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
