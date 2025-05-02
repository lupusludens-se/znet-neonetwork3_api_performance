import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { SearchType } from '../projects.component';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';

@Injectable()
export class ProjectService {
  private skip$: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  private searchType$: BehaviorSubject<SearchType> = new BehaviorSubject<SearchType>(SearchType.ForYou);
  private showMap$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  mapProjectsList$: BehaviorSubject<ProjectInterface[]> = new BehaviorSubject<ProjectInterface[]>([]);
  hoverProject$: BehaviorSubject<ProjectInterface> = new BehaviorSubject<ProjectInterface>(null);
  constructor() {}

  getSkip$(): Observable<number> {
    return this.skip$.asObservable();
  }

  setSkip(value: number) {
    this.skip$.next(value);
  }

  getMapProjectList$(): Observable<ProjectInterface[]> {
    return this.mapProjectsList$.asObservable();
  }

  setMapProjectList(value: ProjectInterface[]) {
    this.mapProjectsList$.next(value);
  }

  getHoverProject$(): Observable<ProjectInterface> {
    return this.hoverProject$.asObservable();
  }

  setHoverProject(value: ProjectInterface) {
    this.hoverProject$.next(value);
  }

  getSearchType$(): Observable<SearchType> {
    return this.searchType$.asObservable();
  }

  getSearchTypeValue(): SearchType {
    return this.searchType$.getValue();
  }

  setSearchType(searchType: SearchType) {
    this.searchType$.next(searchType);
  }

  getshowMap(): Observable<boolean> {
    return this.showMap$.asObservable();
  }

  setshowMap(value: boolean) {
    this.showMap$.next(value);
  }

  clearPaging() {
    this.setSkip(0);
    this.setSearchType(SearchType.ForYou);
    this.setshowMap(false);
  }
}
