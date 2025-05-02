import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestPortfolioReviewFormComponent } from './request-portfolio-review-form.component';

describe('RequestPortfolioReviewFormComponent', () => {
  let component: RequestPortfolioReviewFormComponent;
  let fixture: ComponentFixture<RequestPortfolioReviewFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RequestPortfolioReviewFormComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestPortfolioReviewFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
