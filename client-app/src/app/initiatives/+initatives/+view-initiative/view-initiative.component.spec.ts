import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewInitiativeComponent } from './view-initiative.component';

describe('ViewInitiativeComponent', () => {
  let component: ViewInitiativeComponent;
  let fixture: ComponentFixture<ViewInitiativeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ViewInitiativeComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewInitiativeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
