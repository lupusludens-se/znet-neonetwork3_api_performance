import { PaginationInterface } from '../../shared/modules/pagination/pagination.component';

export interface ForumRequestInterface extends PaginationInterface {
  filterBy?: string;
  search?: string;
  expand?: string;
  includeCount?: boolean;
}
