import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventTemplateModalComponent } from './event-template-modal.component';

describe('EventTemplateModalComponent', () => {
  let component: EventTemplateModalComponent;
  let fixture: ComponentFixture<EventTemplateModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EventTemplateModalComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EventTemplateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
