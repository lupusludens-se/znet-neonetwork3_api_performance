import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SPDashboardProjectDetails } from 'src/app/shared/interfaces/projects/project.interface';

@Component({
  selector: 'neo-sp-project-carousal',
  templateUrl: './sp-project-carousal.component.html',
  styleUrls: ['./sp-project-carousal.component.scss']
})
export class SpProjectCarousalComponent {
  @Input() projects: SPDashboardProjectDetails[] = [];
  @Input() loading: boolean = false;
  @Input() projectsCountPerSlide: number = 3;
  @Input() totalSlides: number = 3;
  @Output() forwardClick: EventEmitter<void> = new EventEmitter<void>();
  @Input() recentlyViewed: boolean = false;
  @Input() isNoProjects: boolean = false;
  @Input() isShowAddProject: boolean = true;
  public position: number = 0;
  @Input() circlesArr: number[] = [0, -3, -6];
  @Input() projectsCount: number;
  forward(): void {
    this.position = this.position - this.projectsCountPerSlide;
    this.forwardClick.emit();
    if (this.circlesArr.findIndex(y => y == this.position) == -1) {
      this.position = this.circlesArr[0];
    }
  }
  backward(): void {
    this.position = this.position + this.projectsCountPerSlide;
    if (this.circlesArr.findIndex(y => y == this.position) == -1) {
      this.position = this.circlesArr[this.circlesArr.length - 1];
    }
  }
  showArrow(): Boolean {
    return this.projectsCount > 3 ? true : false;
  }
}
