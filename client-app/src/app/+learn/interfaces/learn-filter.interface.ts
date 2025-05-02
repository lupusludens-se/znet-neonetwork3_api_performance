import { FilterStateInterface } from 'src/app/shared/modules/filter/interfaces/filter-state.interface';

export interface LearnFiltersInterface {
  title: string;
  searchStr: string;
  initialFiltersState: FilterStateInterface;
}
