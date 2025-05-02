import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

import { PostInterface } from '../../interfaces/post.interface';
import { PaginateResponseInterface } from '../../../shared/interfaces/common/pagination-response.interface';
import { POSTS_PER_PAGE } from '../../constants/parameter.const';

@Component({
  selector: 'neo-posts-carousel',
  templateUrl: './posts-carousel.component.html',
  styleUrls: ['./posts-carousel.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PostsCarouselComponent {
  @Input() postData: PaginateResponseInterface<PostInterface>;
  @Input() title: string;
  @Input() learnIndex: number;
  @Output() forwardClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() setIndexClick: EventEmitter<number> = new EventEmitter<number>();

  public index: number = 0;

  forward(): void {
    this.index =
      this.index - POSTS_PER_PAGE + this.postData.count < POSTS_PER_PAGE ? this.index - 1 : this.index - POSTS_PER_PAGE;
    this.forwardClick.emit();
  }

  backward(): void {
    this.index = this.index + POSTS_PER_PAGE <= 0 ? this.index + POSTS_PER_PAGE : this.index + 1;
  }

  setIndex(){
    this.setIndexClick.emit(this.learnIndex);
  }
}
