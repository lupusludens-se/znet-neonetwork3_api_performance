import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedbackTableRowComponent } from './feedback-table-row.component';

describe('FeedbackTableRowComponent', () => {
  let component: FeedbackTableRowComponent;
  let fixture: ComponentFixture<FeedbackTableRowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FeedbackTableRowComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FeedbackTableRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
