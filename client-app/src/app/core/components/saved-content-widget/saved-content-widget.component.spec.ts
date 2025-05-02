import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SavedContentWidgetComponent } from './saved-content-widget.component';

describe('SavedContentWidgetComponent', () => {
  let component: SavedContentWidgetComponent;
  let fixture: ComponentFixture<SavedContentWidgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SavedContentWidgetComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SavedContentWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
