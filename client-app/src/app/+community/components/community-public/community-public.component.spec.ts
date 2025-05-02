import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunityPublicComponent } from './community-public.component';

describe('CommunityPublicComponent', () => {
  let component: CommunityPublicComponent;
  let fixture: ComponentFixture<CommunityPublicComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CommunityPublicComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunityPublicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
