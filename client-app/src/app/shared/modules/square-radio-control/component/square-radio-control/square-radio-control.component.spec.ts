import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SquareRadioControlComponent } from './square-radio-control.component';

describe('SquareRadioControlComponent', () => {
  let component: SquareRadioControlComponent;
  let fixture: ComponentFixture<SquareRadioControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SquareRadioControlComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SquareRadioControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
