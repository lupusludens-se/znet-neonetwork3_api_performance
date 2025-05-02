import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunityTestimonialComponent } from './community-testimonial.component';

describe('CommunityTestimonialComponent', () => {
  let component: CommunityTestimonialComponent;
  let fixture: ComponentFixture<CommunityTestimonialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CommunityTestimonialComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunityTestimonialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
