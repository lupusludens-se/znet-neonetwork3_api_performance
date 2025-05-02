import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativeToolsSavedComponent } from './initiative-tools-saved-tab.component';

describe('InitiativeToolsSavedComponent', () => {
  let component: InitiativeToolsSavedComponent;
  let fixture: ComponentFixture<InitiativeToolsSavedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [InitiativeToolsSavedComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativeToolsSavedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
