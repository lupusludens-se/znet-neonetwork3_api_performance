import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpProjectCarousalComponent } from './sp-project-carousal.component';

describe('SpProjectCarousalComponent', () => {
  let component: SpProjectCarousalComponent;
  let fixture: ComponentFixture<SpProjectCarousalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SpProjectCarousalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SpProjectCarousalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
