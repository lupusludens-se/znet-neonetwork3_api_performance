import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyAnnouncementsSectionComponent } from './company-announcements-section.component';

describe('CompanyAnnouncementsSectionComponent', () => {
  let component: CompanyAnnouncementsSectionComponent;
  let fixture: ComponentFixture<CompanyAnnouncementsSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyAnnouncementsSectionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyAnnouncementsSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
