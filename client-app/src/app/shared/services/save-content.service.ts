import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ContentInterface } from '../interfaces/content.interface';
import { PaginateResponseInterface } from '../interfaces/common/pagination-response.interface';
import { HttpService } from '../../core/services/http.service';
import { SavedContentTypeEnum } from '../modules/saved-content/enums/saved-content-type.enum';

@Injectable()
export class SaveContentService {
  saveContentApiRoutes = {
    savedContent: 'saved-content/',
    projects: 'saved-content/projects',
    articles: 'saved-content/articles',
    forums: 'saved-content/forums'
  };

  constructor(private httpService: HttpService) {}

  getSavedContent(
    skip: number = 0,
    take: number = 25,
    search?: string,
    type?: 0 | 1 | 3
  ): Observable<PaginateResponseInterface<ContentInterface<SavedContentTypeEnum>>> {
    return this.httpService.get<PaginateResponseInterface<ContentInterface<SavedContentTypeEnum>>>(
      this.saveContentApiRoutes.savedContent,
      {
        includeCount: true,
        skip,
        take,
        search,
        type
      }
    );
  }

  saveProject(projectId: number): Observable<void> {
    return this.httpService.post(this.saveContentApiRoutes.projects, {
      projectId
    });
  }

  deleteProject(projectId: number): Observable<void> {
    return this.httpService.delete(this.saveContentApiRoutes.projects + `/${projectId}`);
  }

  saveArticle(articleId: number): Observable<void> {
    return this.httpService.post(this.saveContentApiRoutes.articles, {
      articleId
    });
  }

  deleteArticle(articleId: number): Observable<void> {
    return this.httpService.delete(this.saveContentApiRoutes.articles + `/${articleId}`);
  }

  saveForum(forumId: number): Observable<void> {
    return this.httpService.post(this.saveContentApiRoutes.forums, {
      forumId
    });
  }

  deleteForum(forumId: number): Observable<void> {
    return this.httpService.delete(this.saveContentApiRoutes.forums + `/${forumId}`);
  }
}
