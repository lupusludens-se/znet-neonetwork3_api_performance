import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SavedContentComponent } from './saved-content.component';

describe('SavedContentComponent', () => {
  let component: SavedContentComponent;
  let fixture: ComponentFixture<SavedContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SavedContentComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SavedContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
