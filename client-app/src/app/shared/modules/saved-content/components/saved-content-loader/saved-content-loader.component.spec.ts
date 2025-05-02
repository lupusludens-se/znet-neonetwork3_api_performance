import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SavedContentLoaderComponent } from './saved-content-loader.component';

describe('SavedContentLoaderComponent', () => {
  let component: SavedContentLoaderComponent;
  let fixture: ComponentFixture<SavedContentLoaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SavedContentLoaderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SavedContentLoaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
