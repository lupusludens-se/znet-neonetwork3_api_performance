import { Pipe, PipeTransform } from '@angular/core';
import { TopicTypeEnum } from '../../shared/enums/topic-type.enum';

@Pipe({
  name: 'topicTypeIcon'
})
export class TopicTypeToIconPipe implements PipeTransform {
  transform(value: number): string {
    switch (value) {
      case TopicTypeEnum.Project:
        return 'projects';
      case TopicTypeEnum.Learn:
        return 'learn';
      case TopicTypeEnum.Event:
        return 'events';
      case TopicTypeEnum.Forum:
        return 'forum';
      case TopicTypeEnum.Company:
        return 'building';
      default:
        return 'topic';
    }
  }
}
