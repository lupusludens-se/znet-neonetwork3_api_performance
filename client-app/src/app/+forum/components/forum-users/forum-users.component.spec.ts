import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumUsersComponent } from './forum-users.component';

describe('ForumUsersComponent', () => {
  let component: ForumUsersComponent;
  let fixture: ComponentFixture<ForumUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForumUsersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ForumUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
