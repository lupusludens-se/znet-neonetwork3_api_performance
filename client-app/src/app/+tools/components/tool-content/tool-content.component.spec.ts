import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ToolContentComponent } from './tool-content.component';

describe('ToolContentComponent', () => {
  let component: ToolContentComponent;
  let fixture: ComponentFixture<ToolContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ToolContentComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ToolContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
