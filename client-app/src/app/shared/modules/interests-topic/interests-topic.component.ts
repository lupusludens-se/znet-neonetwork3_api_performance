import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { EventCategoryInterface } from '../../../+admin/modules/+edit-event/interfaces/event-category.interface';

@Component({
  selector: 'neo-interests-topic',
  templateUrl: 'interests-topic.component.html',
  styleUrls: ['./interests-topic.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class InterestsTopicComponent {
  @Output() selectedTopics: EventEmitter<TagInterface[]> = new EventEmitter<TagInterface[]>();
  @Input() topics: TagInterface[] | EventCategoryInterface[];

  updateSelection(topicIndex: number): void {
    this.topics.find((top: TagInterface, ind: number) => {
      if (ind === topicIndex) top.selected = !top.selected;
    });

    this.selectedTopics.emit(this.topics);
  }
}
