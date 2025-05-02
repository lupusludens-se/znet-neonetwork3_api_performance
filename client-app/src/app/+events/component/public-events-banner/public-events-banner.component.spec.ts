import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicEventsBannerComponent } from './public-events-banner.component';

describe('PublicEventsBannerComponent', () => {
  let component: PublicEventsBannerComponent;
  let fixture: ComponentFixture<PublicEventsBannerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PublicEventsBannerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PublicEventsBannerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
