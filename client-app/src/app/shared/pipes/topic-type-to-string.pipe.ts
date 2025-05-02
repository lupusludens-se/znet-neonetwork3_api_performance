import { Pipe, PipeTransform } from '@angular/core';
import { TopicTypeEnum } from '../enums/topic-type.enum';

@Pipe({
  name: 'topicTypeString'
})
export class TopicTypeToStringPipe implements PipeTransform {
  transform(value: number): string {
    return Object.values(TopicTypeEnum).includes(value as TopicTypeEnum) ? TopicTypeEnum[value] : 'topic';
  }
}
