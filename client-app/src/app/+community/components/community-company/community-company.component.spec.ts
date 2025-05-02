import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunityCompanyComponent } from './community-company.component';

describe('CommunityCompanyComponent', () => {
  let component: CommunityCompanyComponent;
  let fixture: ComponentFixture<CommunityCompanyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CommunityCompanyComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunityCompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
