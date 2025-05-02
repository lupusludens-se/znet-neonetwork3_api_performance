import { FilterDataInterface } from './filter-data.interface';

export interface FilterChildDataInterface extends FilterDataInterface {
  childElements: FilterDataInterface[];
  filterSearch?: string;
}
