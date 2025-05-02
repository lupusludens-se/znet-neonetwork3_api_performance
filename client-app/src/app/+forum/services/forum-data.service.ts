import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { HttpService } from 'src/app/core/services/http.service';
import { ForumApiEnum } from '../enums/forum-api.enum';
import { ForumTopicInterface } from '../interfaces/forum-topic.interface';

@Injectable({
  providedIn: 'root'
})
export class ForumDataService {
  constructor(private readonly httpService: HttpService) {}

  updateDiscussion(discussion: any): Observable<any> {
    return this.httpService.put<any>(ForumApiEnum.Forum + '/edit', discussion).pipe(take(1));
  }

  getDiscussionById(id: number, filter: string): Observable<ForumTopicInterface> {
    return this.httpService.get<ForumTopicInterface>(`${ForumApiEnum.Forum}/${id}`, {
      expand: filter
    });
  }
}
