import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ToolsManipulationComponent } from './tools-manipulation.component';

describe('ToolsEditComponent', () => {
  let component: ToolsManipulationComponent;
  let fixture: ComponentFixture<ToolsManipulationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ToolsManipulationComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ToolsManipulationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
