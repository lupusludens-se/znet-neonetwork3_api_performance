import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativeGeographicScaleComponent } from './initiative-geographic-scale.component';

describe('InitiativeGeographicScaleComponent', () => {
  let component: InitiativeGeographicScaleComponent;
  let fixture: ComponentFixture<InitiativeGeographicScaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeGeographicScaleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeGeographicScaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
