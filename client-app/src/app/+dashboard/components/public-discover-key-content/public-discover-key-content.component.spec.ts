import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicDiscoverKeyContentComponent } from './public-discover-key-content.component';

describe('PublicDiscoverKeyContentComponent', () => {
  let component: PublicDiscoverKeyContentComponent;
  let fixture: ComponentFixture<PublicDiscoverKeyContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PublicDiscoverKeyContentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PublicDiscoverKeyContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
