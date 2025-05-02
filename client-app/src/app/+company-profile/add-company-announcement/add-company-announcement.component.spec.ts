import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCompanyAnnouncementComponent } from './add-company-announcement.component';


describe('AddCompanyAnnouncementComponent', () => {
  let component: AddCompanyAnnouncementComponent;
  let fixture: ComponentFixture<AddCompanyAnnouncementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddCompanyAnnouncementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddCompanyAnnouncementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
