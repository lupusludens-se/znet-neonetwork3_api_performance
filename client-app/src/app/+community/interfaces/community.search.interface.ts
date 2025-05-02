export interface CommunitySearchInterface {
  search?: string;
  filterBy?: any;
  onlyFollowed?: boolean;
  orderBy: string;
  forYou?: boolean;
  type?: number;
  skip?: number;
  take?: number;
  includeCount?: boolean;
}
