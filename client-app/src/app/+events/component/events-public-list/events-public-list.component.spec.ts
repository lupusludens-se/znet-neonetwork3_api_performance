import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventsPublicListComponent } from './events-public-list.component';

describe('EventsPublicListComponent', () => {
  let component: EventsPublicListComponent;
  let fixture: ComponentFixture<EventsPublicListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventsPublicListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EventsPublicListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
