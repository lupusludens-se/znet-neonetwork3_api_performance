import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectSolutionsTileComponent } from './project-solutions-tile.component';

describe('ProjectSolutionsTileComponent', () => {
  let component: ProjectSolutionsTileComponent;
  let fixture: ComponentFixture<ProjectSolutionsTileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProjectSolutionsTileComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectSolutionsTileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
