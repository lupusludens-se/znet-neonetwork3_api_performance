import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyTableRowComponent } from './company-table-row.component';

describe('CompanyTableRowComponent', () => {
  let component: CompanyTableRowComponent;
  let fixture: ComponentFixture<CompanyTableRowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompanyTableRowComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyTableRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
