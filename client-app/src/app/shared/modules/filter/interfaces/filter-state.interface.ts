import { FilterDataInterface } from './filter-data.interface';
import { FilterChildDataInterface } from './filter-child-data.interface';

export class FilterStateInterface {
  search?: string;
  parameter: Record<string, FilterDataInterface[] | FilterChildDataInterface[]> | string;
  filterby?: string;
  orderby?: string;
  skip?: number;
  take?: number;
}
