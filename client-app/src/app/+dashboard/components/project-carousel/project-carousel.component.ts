import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NewTrendingProjectResponse } from 'src/app/shared/interfaces/projects/project.interface';

@Component({
  selector: 'neo-project-carousel',
  templateUrl: './project-carousel.component.html',
  styleUrls: ['./project-carousel.component.scss']
})
export class ProjectCarouselComponent {
  @Input() postData: NewTrendingProjectResponse[];
  @Input() title: string = '';
  @Input() subTitle: string = '';
  @Input() loading: boolean = false;
  @Input() postsCountPerSlide: number = 3;
  @Input() totalSlides: number = 3;
  @Output() forwardClick: EventEmitter<void> = new EventEmitter<void>();
  @Input() recentlyViewed: boolean = false;

  public position: number = 0;
  @Input() circlesArr: number[] = [0, -3, -6];

  forward(): void {
    this.position = this.position - this.postsCountPerSlide;
    if (this.circlesArr.findIndex(y => y == this.position) == -1) {
      this.position = this.circlesArr[0];
    }
    this.forwardClick.emit();
  }

  backward(): void {
    this.position = this.position + this.postsCountPerSlide;
    if (this.circlesArr.findIndex(y => y == this.position) == -1) {
      this.position = this.circlesArr[this.circlesArr.length - 1];
    }
  }
}
