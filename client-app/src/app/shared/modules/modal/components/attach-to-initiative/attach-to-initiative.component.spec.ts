import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AttachToInitiativeComponent } from './attach-to-initiative.component';

describe('AttachToInitiativeComponent', () => {
  let component: AttachToInitiativeComponent;
  let fixture: ComponentFixture<AttachToInitiativeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AttachToInitiativeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AttachToInitiativeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
