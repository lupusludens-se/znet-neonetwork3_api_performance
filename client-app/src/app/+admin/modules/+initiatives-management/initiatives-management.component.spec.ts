import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiativesManagementComponent } from './initiatives-management.component';

describe('InitiativesManagementComponent', () => {
  let component: InitiativesManagementComponent;
  let fixture: ComponentFixture<InitiativesManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiativesManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiativesManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
