import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativeContentComponent } from './initiative-content.component';

describe('InitiativeContentComponent', () => {
  let component: InitiativeContentComponent;
  let fixture: ComponentFixture<InitiativeContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeContentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
