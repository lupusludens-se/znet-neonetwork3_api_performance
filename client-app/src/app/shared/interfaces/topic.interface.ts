import { TopicTypeEnum } from '../enums/topic-type.enum';
import { PaginateResponseInterface } from './common/pagination-response.interface';
import { ContentInterface } from './content.interface';

export interface TopicsPaginationInterface extends PaginateResponseInterface<any> {}

export interface TopicInterface extends PaginateResponseInterface<ContentInterface<TopicTypeEnum>> {
  counters: {
    forumsCount: number;
    articlesCount: number;
    projectsCount: number;
    companiesCount: number;
    eventsCount: number;
    toolsCount: number;
  };
}
