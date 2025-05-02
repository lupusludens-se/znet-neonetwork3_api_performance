import { ColumnConfigurationInterface } from './column-configuration.interface';
import { PaginateResponseInterface } from '../common/pagination-response.interface';

export interface TableConfigurationInterface<T> {
  data: PaginateResponseInterface<T>;
  optionCell: boolean;
  columns: ColumnConfigurationInterface[];
}
