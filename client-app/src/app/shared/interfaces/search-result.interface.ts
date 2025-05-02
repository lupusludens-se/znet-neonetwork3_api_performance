import { TopicTypeEnum } from '../enums/topic-type.enum';

export interface SearchResultInterface {
  id: number;
  name: string;
  displayName?: string;
  type?: TopicTypeEnum;
}
