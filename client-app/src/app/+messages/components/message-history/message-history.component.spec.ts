import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageHistoryComponent } from './message-history.component';

describe('HistoryComponent', () => {
  let component: MessageHistoryComponent;
  let fixture: ComponentFixture<MessageHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MessageHistoryComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MessageHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
