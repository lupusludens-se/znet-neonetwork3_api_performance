import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTechnologiesComponent } from './project-technologies.component';

describe('ProjectTechnologiesComponent', () => {
  let component: ProjectTechnologiesComponent;
  let fixture: ComponentFixture<ProjectTechnologiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProjectTechnologiesComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTechnologiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
