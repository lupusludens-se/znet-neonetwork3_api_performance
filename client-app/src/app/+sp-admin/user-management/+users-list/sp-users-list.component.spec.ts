import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SPUsersListComponent } from './sp-users-list.component';

describe('UsersListComponent', () => {
  let component: SPUsersListComponent;
  let fixture: ComponentFixture<SPUsersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SPUsersListComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SPUsersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
