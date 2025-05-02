import { Injectable } from '@angular/core';

import { Observable, Subject } from 'rxjs';

import { HttpService } from './http.service';

import { UserProfileApiEnum } from '../../shared/enums/api/user-profile-api.enum';
import { CompanyApiEnum } from '../../user-management/enums/company-api.enum';
import { ForumApiEnum } from '../../+forum/enums/forum-api.enum';

@Injectable({
  providedIn: 'root'
})
export class FollowingService {
  private userFollowingStatus$: Subject<{ userId: number; unFollow: boolean }> = new Subject<{
    userId: number;
    unFollow: boolean;
  }>();
  private companyFollowingStatus$: Subject<boolean> = new Subject<boolean>();
  private forumDiscussionFollowingStatus$: Subject<void> = new Subject<void>();

  constructor(private readonly httpService: HttpService) {}

  followedUser(): Observable<{ userId: number; unFollow: boolean }> {
    return this.userFollowingStatus$;
  }

  followedCompany(): Observable<boolean> {
    return this.companyFollowingStatus$;
  }

  followedForumDiscussion(): Observable<void> {
    return this.forumDiscussionFollowingStatus$;
  }

  followUser(userId: number, unFollow?: boolean): void {
    this.httpService[unFollow ? 'delete' : 'post']<unknown>(
      `${UserProfileApiEnum.Followers}/${userId}`,
      null
    ).subscribe(() => this.userFollowingStatus$.next({ userId, unFollow }));
  }

  followCompany(companyId: number, unFollow?: boolean): void {
    this.httpService[unFollow ? 'delete' : 'post']<unknown>(
      `${CompanyApiEnum.Companies}/${companyId}/${CompanyApiEnum.Followers}`,
      null
    ).subscribe(() => this.companyFollowingStatus$.next(unFollow));
  }

  followForumDiscussion(topicId: number, unFollow?: boolean): void {
    this.httpService[unFollow ? 'delete' : 'post']<unknown>(
      `${ForumApiEnum.Forum}/${topicId}/${ForumApiEnum.Followers}`,
      null
    ).subscribe(() => this.forumDiscussionFollowingStatus$.next());
  }
}
