import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaticDropdownComponent } from './static-dropdown.component';

describe('StaticDropdownComponent', () => {
  let component: StaticDropdownComponent;
  let fixture: ComponentFixture<StaticDropdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaticDropdownComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaticDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
