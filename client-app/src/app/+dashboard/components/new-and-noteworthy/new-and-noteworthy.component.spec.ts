/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { NewAndNoteworthyComponent } from './new-and-noteworthy.component';

describe('NewAndNoteworthyComponent', () => {
  let component: NewAndNoteworthyComponent;
  let fixture: ComponentFixture<NewAndNoteworthyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewAndNoteworthyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewAndNoteworthyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
