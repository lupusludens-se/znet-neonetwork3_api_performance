import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StartADiscussionComponent } from './start-a-discussion.component';

describe('StartADiscussionComponent', () => {
  let component: StartADiscussionComponent;
  let fixture: ComponentFixture<StartADiscussionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StartADiscussionComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StartADiscussionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
