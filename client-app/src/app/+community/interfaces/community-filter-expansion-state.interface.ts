import { ExpandStateEnum } from 'src/app/shared/modules/filter/enums/expand-state.enum';

export interface CommunityFilterExpansionState {
  regionFilterexpansionState: ExpandStateEnum;
  industryFilterexpansionState: ExpandStateEnum;
  categoryFilterexpansionState: ExpandStateEnum;
}
