/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ToolItemComponent } from './tool-item.component';

describe('ToolItemComponent', () => {
  let component: ToolItemComponent;
  let fixture: ComponentFixture<ToolItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ToolItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ToolItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
