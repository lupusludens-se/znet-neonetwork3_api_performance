export interface InitiativeContentsPaginationResponseInterface<T> {
    skip: number;
    take: number;
    dataList: T[];
    count?: number;
    newRecommendationsCount: number;
}