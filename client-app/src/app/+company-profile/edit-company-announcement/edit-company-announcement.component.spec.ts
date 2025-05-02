import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCompanyAnnouncementComponent } from './edit-company-announcement.component';

describe('EditCompanyAnnouncementComponent', () => {
  let component: EditCompanyAnnouncementComponent;
  let fixture: ComponentFixture<EditCompanyAnnouncementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditCompanyAnnouncementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCompanyAnnouncementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
