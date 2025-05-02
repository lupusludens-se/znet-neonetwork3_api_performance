import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndicativeQuoteRequestFormComponent } from './indicative-quote-request-form.component';

describe('IndicativeQuoteRequestFormComponent', () => {
  let component: IndicativeQuoteRequestFormComponent;
  let fixture: ComponentFixture<IndicativeQuoteRequestFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [IndicativeQuoteRequestFormComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IndicativeQuoteRequestFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
