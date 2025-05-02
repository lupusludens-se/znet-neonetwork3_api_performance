import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectSolutionsItemElementComponent } from './project-solutions-item-element.component';

describe('ProjectSolutionsItemElementComponent', () => {
  let component: ProjectSolutionsItemElementComponent;
  let fixture: ComponentFixture<ProjectSolutionsItemElementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProjectSolutionsItemElementComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectSolutionsItemElementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
