import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetCompleteComponent } from './reset-complete.component';

describe('ResetCompleteComponent', () => {
  let component: ResetCompleteComponent;
  let fixture: ComponentFixture<ResetCompleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ResetCompleteComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ResetCompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
