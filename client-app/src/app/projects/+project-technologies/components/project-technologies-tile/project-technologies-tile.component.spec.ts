import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTechnologiesTileComponent } from './project-technologies-tile.component';

describe('ProjectTechnologiesTileComponent', () => {
  let component: ProjectTechnologiesTileComponent;
  let fixture: ComponentFixture<ProjectTechnologiesTileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProjectTechnologiesTileComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTechnologiesTileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
