import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LearnItemComponent } from './learn-item.component';

describe('LearnItemComponent', () => {
  let component: LearnItemComponent;
  let fixture: ComponentFixture<LearnItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LearnItemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LearnItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
