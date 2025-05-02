import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CompanyFileListViewComponent } from './company-file-list-view.component';


describe('CompanyFileListViewComponent', () => {
  let component: CompanyFileListViewComponent;
  let fixture: ComponentFixture<CompanyFileListViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyFileListViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyFileListViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
