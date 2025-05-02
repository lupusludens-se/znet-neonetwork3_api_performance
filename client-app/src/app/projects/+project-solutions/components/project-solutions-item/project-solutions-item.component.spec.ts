import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectSolutionsItemComponent } from './project-solutions-item.component';

describe('ProjectSolutionsItemComponent', () => {
  let component: ProjectSolutionsItemComponent;
  let fixture: ComponentFixture<ProjectSolutionsItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProjectSolutionsItemComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectSolutionsItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
