import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditDiscussionComponent } from './edit-discussion.component';

describe('EditDiscussionComponent', () => {
  let component: EditDiscussionComponent;
  let fixture: ComponentFixture<EditDiscussionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EditDiscussionComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditDiscussionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
