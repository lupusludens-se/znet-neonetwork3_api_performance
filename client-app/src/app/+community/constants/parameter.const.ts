import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { SortingOptionsKeyValuePair } from 'src/app/shared/modules/sort-dropdown/sort-dropdown.component';

export enum CommunityType {
  Company = 0,
  User = 1
}

export enum FollowingTypes {
  All = -1,
  Companies = 0,
  People = 1
}

export const COMPANIES_OPTIONS_ALL: number = 0;
export const COMPANIES_OPTIONS_CORPORATIONS: number = 1;

export const FOLLOWING_OPTIONS: TagInterface[] = [
  { id: FollowingTypes.All, name: 'All' },
  { id: FollowingTypes.Companies, name: 'Companies' },
  { id: FollowingTypes.People, name: 'People' }
];

export const COMPANIES_OPTIONS: TagInterface[] = [
  { id: 0, name: 'All Companies' },
  { id: 1, name: 'Corporations' },
  { id: 2, name: 'Solution Providers' }
];

export const PEOPLE_OPTIONS: TagInterface[] = [
  { id: RolesEnum.All, name: 'All Users' },
  { id: RolesEnum.Corporation, name: 'Corporations' },
  { id: RolesEnum.SolutionProvider, name: 'Solution Providers' }
];

export const FOLLOWING_SORT_OPTIONS: SortingOptionsKeyValuePair[] = [
  { key: 'all', value: 'All' },
  { key: 'companies', value: 'Companies' },
  { key: 'people', value: 'People' }
];
