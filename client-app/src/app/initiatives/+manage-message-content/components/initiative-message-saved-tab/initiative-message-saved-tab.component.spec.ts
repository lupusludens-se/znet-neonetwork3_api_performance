import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativeMessageSavedComponent } from './initiative-message-saved-tab.component';

describe('InitiativeMessageSavedComponent', () => {
  let component: InitiativeMessageSavedComponent;
  let fixture: ComponentFixture<InitiativeMessageSavedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativeMessageSavedComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeMessageSavedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
