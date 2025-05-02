import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmitAddressFormComponent } from './submit-address-form.component';

describe('SubmitAddressFormComponent', () => {
  let component: SubmitAddressFormComponent;
  let fixture: ComponentFixture<SubmitAddressFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SubmitAddressFormComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubmitAddressFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
