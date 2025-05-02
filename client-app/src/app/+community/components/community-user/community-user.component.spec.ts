import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunityUserComponent } from './community-user.component';

describe('CommunityUserComponent', () => {
  let component: CommunityUserComponent;
  let fixture: ComponentFixture<CommunityUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CommunityUserComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunityUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
