import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyFilesComponent } from './company-files-section.component';

describe('CompanyFilesComponent', () => {
  let component: CompanyFilesComponent;
  let fixture: ComponentFixture<CompanyFilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyFilesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyFilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
