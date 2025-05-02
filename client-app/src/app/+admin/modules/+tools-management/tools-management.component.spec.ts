import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ToolsManagementComponent } from './tools-management.component';

describe('ToolsManagementComponent', () => {
  let component: ToolsManagementComponent;
  let fixture: ComponentFixture<ToolsManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ToolsManagementComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ToolsManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
