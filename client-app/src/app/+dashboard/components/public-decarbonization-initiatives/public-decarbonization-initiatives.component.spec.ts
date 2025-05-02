import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicDecarbonizationInitiativesComponent } from './public-decarbonization-initiatives.component';

describe('PublicDecarbonizationInitiativesComponent', () => {
  let component: PublicDecarbonizationInitiativesComponent;
  let fixture: ComponentFixture<PublicDecarbonizationInitiativesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PublicDecarbonizationInitiativesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PublicDecarbonizationInitiativesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
