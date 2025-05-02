export interface PaginateResponseInterface<T> {
  skip: number;
  take: number;
  dataList: T[];
  count?: number;
}
