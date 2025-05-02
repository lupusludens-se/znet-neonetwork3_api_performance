import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativeDetailsComponent } from './initiative-details.component';

describe('InitiativeDetailsComponent', () => {
  let component: InitiativeDetailsComponent;
  let fixture: ComponentFixture<InitiativeDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
