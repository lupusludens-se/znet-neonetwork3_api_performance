import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostsCarouselComponent } from './posts-carousel.component';

describe('PostsCarouselComponent', () => {
  let component: PostsCarouselComponent;
  let fixture: ComponentFixture<PostsCarouselComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PostsCarouselComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PostsCarouselComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
