/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { DecarbonizationInitiativesComponent } from './decarbonization-initiatives.component';

describe('+decarbonizationInitiativesComponent', () => {
  let component: DecarbonizationInitiativesComponent;
  let fixture: ComponentFixture<DecarbonizationInitiativesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DecarbonizationInitiativesComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DecarbonizationInitiativesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
