import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpProjectTileComponent } from './sp-project-tile.component';

describe('SpProjectTileComponent', () => {
  let component: SpProjectTileComponent;
  let fixture: ComponentFixture<SpProjectTileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SpProjectTileComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SpProjectTileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
