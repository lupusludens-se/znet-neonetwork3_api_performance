import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { FilterStateInterface } from 'src/app/shared/modules/filter/interfaces/filter-state.interface';
import { PaginationInterface } from 'src/app/shared/modules/pagination/pagination.component';

export interface CommunityFiltersInterface {
  companiesSelected: TagInterface;
  peopleSelectedOptions: TagInterface;
  initialFiltersState: FilterStateInterface;
  disableIndustries: boolean;
  disableCompanies: boolean;
  disableRegions: boolean;
  disableProjectsUserFilter: boolean;
  disableProjectsCompanyFilter: boolean;
  disablePeopleFilter: boolean;
  listActive: boolean;
  followingFilterTitle: string;
  peopleFilterClassActive: boolean;
  companiesClassActive: boolean;
  title: string;
  forYouFilterTitle: string;
  paging: PaginationInterface;
  followingSelectedSortOrder: string;
}
