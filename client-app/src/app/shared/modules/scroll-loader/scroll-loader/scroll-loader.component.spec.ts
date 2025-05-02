import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScrollLoaderComponent } from './scroll-loader.component';

describe('ScrollLoaderComponent', () => {
  let component: ScrollLoaderComponent;
  let fixture: ComponentFixture<ScrollLoaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScrollLoaderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScrollLoaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
