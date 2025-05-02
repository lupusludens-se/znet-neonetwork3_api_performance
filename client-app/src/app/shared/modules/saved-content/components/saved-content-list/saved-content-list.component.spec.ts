import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SavedContentListComponent } from './saved-content-list.component';

describe('SavedContentListComponent', () => {
  let component: SavedContentListComponent;
  let fixture: ComponentFixture<SavedContentListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SavedContentListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SavedContentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
