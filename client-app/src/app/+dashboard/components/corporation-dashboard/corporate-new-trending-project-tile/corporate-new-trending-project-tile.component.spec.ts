/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewTrendingProjectTileComponent } from './corporate-new-trending-project-tile.component';

describe('CorporateNewTrendingProjectTileComponent', () => {
  let component: NewTrendingProjectTileComponent;
  let fixture: ComponentFixture<NewTrendingProjectTileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NewTrendingProjectTileComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewTrendingProjectTileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
