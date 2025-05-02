import { Injectable } from '@angular/core';
import { NavigationStart, Router, RouterEvent } from '@angular/router';
import { filter, Observable } from 'rxjs';
import { LogService } from 'src/app/shared/services/log.service';
import { environment } from 'src/environments/environment';
import { ActivityApiEnum } from '../enums/activity/activity-api.enum';
import { ActivityLocation } from '../enums/activity/activity-location.enum';
import { ActivityTypeEnum } from '../enums/activity/activity-type.enum';
import { HTTPType } from '../enums/http-type.enum';
import { HttpService } from './http.service';
import { CoreService } from './core.service';
import { DiscussionSourceTypeEnum } from 'src/app/shared/enums/discussion-source-type.enum';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {
  private currentActivityLocation: ActivityLocation;
  private requestUrlsList: string[] = [];
  authService = AuthService;

  constructor(
    private readonly httpService: HttpService,
    private readonly router: Router,
    private readonly logService: LogService,
    private readonly coreService: CoreService
  ) {
    this.router.events
      .pipe(filter((event: any): event is RouterEvent => event instanceof RouterEvent))
      .subscribe((event: RouterEvent) => {
        if (event instanceof NavigationStart) {
          if (this.coreService.elementNotFoundData$.getValue() !== null) {
            this.coreService.elementNotFoundData$.next(null);
          }

          if (event.url.indexOf('/start-a-discussion') >= 0) {
            this.trackElementInteractionActivity(ActivityTypeEnum.NewDiscussionClick)?.subscribe();
          } else if (event.url.indexOf('/add-project') >= 0) {
            this.trackElementInteractionActivity(ActivityTypeEnum.NewProjectClick)?.subscribe();
          }
        }
      });
  }

  trackElementInteractionActivity(activityType: ActivityTypeEnum, data?: any): Observable<unknown> {
    try {
      const activityLocation = ActivityService.getActivityLocation(location.pathname);
      if (!this.authService.isLoggedIn()) {
        data['isPublicUser'] = true;
      }
      const detailsObject = this.getActivityDetails(activityType, activityLocation, { ...data });
      const details = JSON.stringify(detailsObject);
      if (!this.authService.isLoggedIn()) {
        return this.httpService.post(`${ActivityApiEnum.PublicActivity}`, {
          typeId: activityType,
          locationId: activityLocation,
          details
        });
      } else {
        return this.httpService.post(`${ActivityApiEnum.Activity}`, {
          typeId: activityType,
          locationId: activityLocation,
          details
        });
      }
    } catch (e) {
      this.logActivityWarning(e);
    }
  }

  trackRequestActivity(url: string, requestType: string, requestBody: any, responseBody: any): Observable<unknown> {
    try {
      const activityType: ActivityTypeEnum = this.getActivityType(
        url.toLowerCase(),
        requestBody,
        requestType.toLowerCase()
      );
      const activityLocation = ActivityService.getActivityLocation(location.pathname);
      const isLocationChanged = activityLocation !== this.currentActivityLocation;
      if (isLocationChanged) {
        this.currentActivityLocation = activityLocation;
        this.requestUrlsList = [];
      } else {
        this.requestUrlsList.push(url);
      }
      const detailsObject = this.getActivityDetails(
        activityType,
        activityLocation,
        { ...requestBody, ...responseBody },
        url
      );
      const details = JSON.stringify(detailsObject);

      if (!this.authService.isLoggedIn()) {
        return this.httpService.post(`${ActivityApiEnum.PublicActivity}`, {
          typeId: activityType,
          locationId: activityLocation,
          details
        });
      } else {
        return this.httpService.post(`${ActivityApiEnum.Activity}`, {
          typeId: activityType,
          locationId: activityLocation,
          details
        });
      }
    } catch (e) {
      this.logActivityWarning(e);
    }
  }

  private logActivityWarning(e): void {
    if (!environment.production) {
      this.logService.warning(null, e);
    }
  }

  private getActivityType(route: string, body: unknown, type: string): ActivityTypeEnum {
    const requestUrlPath: string = route.split('?')[0];
    const requestUrlPathParts: string[] = ActivityService.getUrlParts(route);
    const lastRequestUrlPathPart = requestUrlPathParts[requestUrlPathParts.length - 1];
    const lastRequestUrlIndex: number = this.requestUrlsList.lastIndexOf(requestUrlPath);

    let prevSearchValue: string;
    let prevFilterByValue: string;

    if (lastRequestUrlIndex >= 0) {
      const prevRequestUrl = this.requestUrlsList[lastRequestUrlIndex];
      const prevParams = this.getParams(prevRequestUrl);
      prevSearchValue = prevParams['search'];
      prevFilterByValue = prevParams['filterby'];
    }

    const params = this.getParams(route);
    const searchValue = params['search'];
    const filterByValue = params['filterby'];

    if (!!searchValue && searchValue !== prevSearchValue) {
      return ActivityTypeEnum.SearchApply;
    }

    if (!!filterByValue && filterByValue !== prevFilterByValue) {
      return ActivityTypeEnum.FilterApply;
    }

    if (
      requestUrlPath.includes('/api/companies/') &&
      !isNaN(parseInt(lastRequestUrlPathPart)) &&
      type === HTTPType.GET
    ) {
      return ActivityTypeEnum.CompanyProfileView;
    }

    if (
      requestUrlPath.includes('/api/solutions') &&
      !isNaN(parseInt(lastRequestUrlPathPart)) &&
      type === HTTPType.GET
    ) {
      return ActivityTypeEnum.SolutionDetailsView;
    }

    if (requestUrlPath.includes('/api/companies/') && requestUrlPath.includes('/followers') && type === HTTPType.POST) {
      return ActivityTypeEnum.CompanyFollow;
    }

    if (
      requestUrlPath.includes('/api/conversations') &&
      body['sourceTypeId'] === DiscussionSourceTypeEnum.ProviderContact &&
      type === HTTPType.POST
    ) {
      return ActivityTypeEnum.ContactProviderConfirmButtonClick;
    }

    if (requestUrlPath.includes('/api/events/') && !requestUrlPath.includes('past-events') && type === HTTPType.GET) {
      return ActivityTypeEnum.EventDetailsView;
    }

    if (
      requestUrlPath.includes('/api/events/') &&
      requestUrlPath.includes('/attendees/current') &&
      type === HTTPType.PUT
    ) {
      return ActivityTypeEnum.EventAttendingButtonClick;
    }

    // TODO: ActivityTypeEnum.FirstDashboardClick - need to subscribe to link/button click

    if (requestUrlPath.includes('/api/users/current/followers/') && type === HTTPType.POST) {
      return ActivityTypeEnum.UserFollow;
    }

    if (requestUrlPath.includes('/api/forums/') && requestUrlPath.includes('/followers') && type === HTTPType.POST) {
      return ActivityTypeEnum.ForumFollow;
    }

    if (requestUrlPath.includes('/api/saved-content/forums') && type === HTTPType.POST) {
      return ActivityTypeEnum.ForumSave;
    }

    if (requestUrlPath.includes('/api/forums/') && !requestUrlPath.includes('/messages') && type === HTTPType.GET) {
      return ActivityTypeEnum.ForumView;
    }

    if (requestUrlPath.includes('/api/saved-content/projects') && type === HTTPType.POST) {
      return ActivityTypeEnum.ProjectSave;
    }

    if (requestUrlPath.includes('/api/projects/') && type === HTTPType.GET) {
      return ActivityTypeEnum.ProjectView;
    }

    if (
      requestUrlPath.includes('/api/userprofiles/') &&
      !isNaN(parseInt(lastRequestUrlPathPart)) &&
      type === HTTPType.GET
    ) {
      return ActivityTypeEnum.UserProfileView;
    }

    if (requestUrlPath.includes('/api/forums/') && requestUrlPath.includes('/messages') && type === HTTPType.POST) {
      if (requestUrlPath.includes('/likes')) {
        return ActivityTypeEnum.ForumMessageLike;
      }
      return ActivityTypeEnum.ForumMessageResponse;
    }

    if (
      requestUrlPath.includes('/api/articles/') &&
      type === HTTPType.GET &&
      !isNaN(parseInt(lastRequestUrlPathPart))
    ) {
      return ActivityTypeEnum.LearnView;
    }

    throw new Error(`API request ${type} ${route} does not match any existed Activity Type.`);
  }

  private static getActivityLocation(url: string): ActivityLocation {
    const urlParts: string[] = ActivityService.getUrlParts(url);
    const lastUrlPart = urlParts[urlParts.length - 1];
    const oneBeforeLastUrlPart = urlParts[urlParts.length - 2];
    const twoBeforeLastUrlPart = urlParts[urlParts.length - 3];
    // TODO: move strings to enum and use the enum also in all urls, which defines the urls

    if (lastUrlPart === 'admin') {
      return ActivityLocation.Admin;
    }

    if (lastUrlPart === 'admit-users') {
      return ActivityLocation.AdmitUsers;
    }

    if (oneBeforeLastUrlPart === 'review-user') {
      return ActivityLocation.ReviewUser;
    }

    if (
      (lastUrlPart === 'learn' ||
        lastUrlPart === 'projects' ||
        lastUrlPart === 'community' ||
        lastUrlPart === 'tools' ||
        lastUrlPart === 'messages') &&
      twoBeforeLastUrlPart === 'decarbonization-initiatives'
    ) {
      return ActivityLocation.InitiativeManageModulePage;
    }

    if (lastUrlPart === 'user-management') {
      return ActivityLocation.UserManagement;
    }

    if (oneBeforeLastUrlPart === 'user-management' && lastUrlPart === 'add') {
      return ActivityLocation.AddUser;
    }

    if (twoBeforeLastUrlPart === 'user-management' && oneBeforeLastUrlPart === 'edit') {
      return ActivityLocation.EditUser;
    }

    if (lastUrlPart === 'company-management') {
      return ActivityLocation.CompanyManagement;
    }

    if (oneBeforeLastUrlPart === 'company-management' && lastUrlPart === 'add') {
      return ActivityLocation.AddCompany;
    }

    if (twoBeforeLastUrlPart === 'company-management' && oneBeforeLastUrlPart === 'edit') {
      return ActivityLocation.EditCompany;
    }

    // TODO: add groups as soon as it will be implemented - GroupManagement, AddGroup, EditGroup

    if (lastUrlPart === 'learn' && oneBeforeLastUrlPart === '') {
      return ActivityLocation.Learn;
    }

    if (oneBeforeLastUrlPart === 'learn') {
      return ActivityLocation.LearnDetails;
    }

    if (lastUrlPart === 'events') {
      return ActivityLocation.Events;
    }

    if (oneBeforeLastUrlPart === 'events') {
      return ActivityLocation.EventDetails;
    }

    if (lastUrlPart === 'create-an-event') {
      return ActivityLocation.AddEvent;
    }

    if (lastUrlPart === 'forum') {
      return ActivityLocation.Forums;
    }

    if (oneBeforeLastUrlPart === 'topic') {
      return ActivityLocation.ForumDetails;
    }

    if (lastUrlPart === 'start-a-discussion') {
      return ActivityLocation.AddForum;
    }

    if (lastUrlPart === 'tool-management') {
      return ActivityLocation.ToolManagement;
    }

    if (lastUrlPart === 'tools') {
      return ActivityLocation.Tools;
    }

    if (oneBeforeLastUrlPart === 'tool-management' && lastUrlPart === 'add') {
      return ActivityLocation.AddTool;
    }

    if (twoBeforeLastUrlPart === 'tool-management' && oneBeforeLastUrlPart === 'edit') {
      return ActivityLocation.EditTool;
    }

    if (oneBeforeLastUrlPart === 'tools') {
      return ActivityLocation.ViewTool;
    }

    if (lastUrlPart === 'email-alerts') {
      return ActivityLocation.EmailAlertSettings;
    }

    if (lastUrlPart === 'announcement') {
      return ActivityLocation.AnnouncementManagement;
    }

    if (oneBeforeLastUrlPart === 'announcement' && lastUrlPart === 'add') {
      return ActivityLocation.AddAnnouncement;
    }

    if (twoBeforeLastUrlPart === 'announcement' && oneBeforeLastUrlPart === 'edit') {
      return ActivityLocation.EditAnnouncement;
    }

    if (lastUrlPart === 'dashboard') {
      return ActivityLocation.Dashboard;
    }

    if (lastUrlPart === 'projects') {
      return ActivityLocation.ProjectCatalog;
    }

    if (oneBeforeLastUrlPart === 'projects') {
      return ActivityLocation.ProjectDetails;
    }

    if (lastUrlPart === 'projects-library') {
      return ActivityLocation.ProjectLibrary;
    }

    if (lastUrlPart === 'add-project') {
      return ActivityLocation.AddProject;
    }

    if (oneBeforeLastUrlPart === 'edit-project') {
      return ActivityLocation.EditProject;
    }

    if (lastUrlPart === 'community') {
      return ActivityLocation.Community;
    }

    if (lastUrlPart === 'search') {
      return ActivityLocation.SearchResult;
    }

    if (lastUrlPart === 'notifications') {
      return ActivityLocation.Notifications;
    }

    if (lastUrlPart === 'messages') {
      return ActivityLocation.Messages;
    }

    if (lastUrlPart === 'new-message') {
      return ActivityLocation.AddMessage;
    }

    if (oneBeforeLastUrlPart === 'messages') {
      return ActivityLocation.ViewMessage;
    }

    if (lastUrlPart === 'saved-content') {
      return ActivityLocation.SavedContent;
    }

    if (lastUrlPart === 'topics') {
      return ActivityLocation.Topic;
    }

    if (oneBeforeLastUrlPart === 'user-profile') {
      return ActivityLocation.ViewUserProfile;
    }

    if (twoBeforeLastUrlPart === 'user-profile' && lastUrlPart === 'edit') {
      return ActivityLocation.EditUserProfile;
    }

    if (oneBeforeLastUrlPart === 'company-profile') {
      return ActivityLocation.ViewCompanyProfile;
    }

    if (lastUrlPart === 'settings') {
      return ActivityLocation.AccountSettings;
    }

    if (lastUrlPart === 'solutions') {
      return ActivityLocation.SolutionDetails;
    }
    if (oneBeforeLastUrlPart === 'solutions') {
      return ActivityLocation.SolutionTypes;
    }
    if (
      oneBeforeLastUrlPart === 'decarbonization-initiatives' ||
      twoBeforeLastUrlPart === 'decarbonization-initiatives'
    ) {
      return ActivityLocation.ViewInitiative;
    }
    if (lastUrlPart === 'initiatives' && oneBeforeLastUrlPart !== 'admin') {
      return ActivityLocation.ViewInitiative;
    }
    if (lastUrlPart === 'create-initiative') {
      return ActivityLocation.CreateInitiative;
    }
    throw new Error(`Window location ${url} does not match any existed Activity Location.`);
  }

  private getActivityDetails(
    type: ActivityTypeEnum,
    location: ActivityLocation,
    data: object,
    requestUrl?: string
  ): unknown {
    let requestUrlParts: string[];
    const params = requestUrl ? this.getParams(requestUrl) : null;
    switch (type) {
      case ActivityTypeEnum.NavMenuItemClick: {
        return { navMenuItemName: data['navMenuItemName'] };
      }
      case ActivityTypeEnum.SearchApply: {
        const searchTerm = params && params['search'];
        if (!searchTerm) {
          throw new Error(`search parameter is required.`);
        }
        return { searchTerm: searchTerm };
      }
      case ActivityTypeEnum.FilterApply: {
        if (
          location == ActivityLocation.UserManagement ||
          location == ActivityLocation.Learn ||
          location == ActivityLocation.ProjectCatalog ||
          location == ActivityLocation.Community ||
          location == ActivityLocation.Forums
        ) {
          const filterBy = params && params['filterby'];
          if (!filterBy) {
            throw new Error(`filterby parameter is required.`);
          }
          return { filterDetails: filterBy };
        } else break;
      }
      case ActivityTypeEnum.CompanyProfileView: {
        if (this.authService.isLoggedIn() || this.authService.needSilentLogIn()) {
          requestUrlParts = ActivityService.getUrlParts(requestUrl);
          return { companyId: requestUrlParts[requestUrlParts.length - 1] };
        } else {
          return data;
        }
      }
      case ActivityTypeEnum.CompanyFollow: {
        requestUrlParts = ActivityService.getUrlParts(requestUrl);
        return { companyId: requestUrlParts[requestUrlParts.length - 2] };
      }
      case ActivityTypeEnum.ContactProviderClick:
      case ActivityTypeEnum.ContactProviderNevermindButtonClick:
      case ActivityTypeEnum.ContactProviderConfirmButtonClick: {
        return { companyId: data['companyId'] };
      }
      case ActivityTypeEnum.EventDetailsView: {
        requestUrlParts = ActivityService.getUrlParts(requestUrl);
        return { eventId: requestUrlParts[requestUrlParts.length - 1] };
      }
      case ActivityTypeEnum.EventRegistration: {
        return data;
      }
      case ActivityTypeEnum.EventAttendingButtonClick: {
        requestUrlParts = ActivityService.getUrlParts(requestUrl);
        return { eventId: requestUrlParts[requestUrlParts.length - 3], doesAttend: data['isAttending'] };
      }
      case ActivityTypeEnum.FirstDashboardClick: {
        if (location === ActivityLocation.Dashboard) {
          return data;
        } else break;
      }
      case ActivityTypeEnum.AnnouncementButtonClick: {
        if (location === ActivityLocation.Dashboard) {
          return data;
        } else break;
      }
      case ActivityTypeEnum.UserFollow: {
        requestUrlParts = ActivityService.getUrlParts(requestUrl);
        return { followerId: requestUrlParts[requestUrlParts.length - 1] };
      }
      case ActivityTypeEnum.ForumFollow: {
        requestUrlParts = ActivityService.getUrlParts(requestUrl);
        return { forumId: requestUrlParts[requestUrlParts.length - 2] };
      }
      case ActivityTypeEnum.ForumSave: {
        return { forumId: data['forumId'] };
      }
      case ActivityTypeEnum.ForumView: {
        requestUrlParts = ActivityService.getUrlParts(requestUrl);
        return { forumId: requestUrlParts[requestUrlParts.length - 1] };
      }
      case ActivityTypeEnum.LinkClick: {
        return { name: data['name'], url: data['url'] };
      }
      case ActivityTypeEnum.ProjectSave: {
        return { projectId: data['projectId'] };
      }
      case ActivityTypeEnum.ProjectView: {
        if (requestUrl != undefined || requestUrl != null) {
          requestUrlParts = ActivityService.getUrlParts(requestUrl);
          if (requestUrlParts != null && requestUrlParts.length)
            return { projectId: requestUrlParts[requestUrlParts.length - 1] };
        }

        if (data['companyId'] != null) {
          return { projectId: data['projectId'], companyId: data['companyId'] };
        }
        return { projectId: data['projectId'], initiativeId: data['initiativeId'] };
      }

      case ActivityTypeEnum.ToolClick: {
        return { toolId: data['toolId'] ?? 0, initiativeId: data['initiativeId'] };
      }
      case ActivityTypeEnum.UserProfileView: {
        if (requestUrl != undefined || requestUrl != null) {
          requestUrlParts = ActivityService.getUrlParts(requestUrl);
          if (requestUrlParts != null && requestUrlParts.length)
            return { userId: requestUrlParts[requestUrlParts.length - 1] };
        }
        return { userId: data['userId'], initiativeId: data['initiativeId'] };
      }

      case ActivityTypeEnum.NewDiscussionClick:
      case ActivityTypeEnum.ViewMapClick:
      case ActivityTypeEnum.ConnectWithNEOClick:
      case ActivityTypeEnum.NewProjectClick:
      case ActivityTypeEnum.LinkButtonClick:
      case ActivityTypeEnum.InitiativeCreate: {
        return {};
      }
      case ActivityTypeEnum.ForumMessageResponse: {
        return { messageId: data['parentMessageId'] };
      }
      case ActivityTypeEnum.ForumMessageLike: {
        requestUrlParts = ActivityService.getUrlParts(requestUrl);
        return { messageId: requestUrlParts[requestUrlParts.length - 2] };
      }
      case ActivityTypeEnum.PrivateLearnClick: {
        return { articleId: data['id'], articleName: data['title'] };
      }
      case ActivityTypeEnum.LearnView: {
        return { articleId: data['id'], articleName: data['title'], initiativeId: data['initiativeId'] };
      }
      case ActivityTypeEnum.TechnologiesSolutionsClick: {
        return { buttonName: data['buttonName'] };
      }
      case ActivityTypeEnum.ViewAllClick: {
        return { viewAllResourceName: data['viewAllType'] };
      }
      case ActivityTypeEnum.ProjectResourceClick: {
        return { resourceId: data['resourceId'] };
      }
      case ActivityTypeEnum.SolutionDetailsView: {
        return { solutionId: data['solutionId'] };
      }
      case ActivityTypeEnum.SolutionTypes: {
        return { technologyId: data['technologyId'] };
      }
      case ActivityTypeEnum.InitiativeModuleViewAllClick: {
        return {
          initiativeId: data['id'],
          initiativeTitle: data['title'],
          resourceType: data['resourceType']
        };
      }
      case ActivityTypeEnum.InitiativeDetailsView: {
        return { initiativeId: data['id'], initiativeTitle: data['title'] };
      }
      case ActivityTypeEnum.InitiativeSubstepClick: {
        return { initiativeId: data['id'], subStepId: data['subStepId'], stepDescription: data['stepDesc'] };
      }
      case ActivityTypeEnum.MessageDetailsView: {
        return { messageId: data['messageId'], initiativeId: data['initiativeId'] };
      }
      case ActivityTypeEnum.InitiativesButtonClick: {
        return { initiativeId: data['id'], buttonName: data['buttonName'], moduleName: data['moduleName'] };
      }
      case ActivityTypeEnum.SaveContentFromAttachContentPopUp: {
        return { initiativeIds: data['initiativeIds'], moduleName: data['moduleName'], contentId: data['contentId'] };
      }
      default:
        throw new Error(`${type} is unsupported Activity Type.`);
    }
    throw new Error(
      `Activity with type: ${ActivityTypeEnum[type]}, location: ${ActivityLocation[location]} is unsupported.`
    );
  }

  private static getUrlParts(url?: string): string[] {
    if (!url) {
      throw new Error(`url parameter is required.`);
    }
    const urlPath = url.split('?')[0];
    return urlPath.split('/');
  }

  private getParams(url?: string): {} {
    if (!url) {
      throw new Error(`url parameter is required.`);
    }
    let params = {};
    const paramsString = url.split('?')[1];
    if (paramsString) {
      paramsString.split('&').forEach(p => {
        const key = p.split('=')[0];
        const value = p.substring(p.indexOf('=') + 1);
        params[key] = value;
      });
    }
    return params;
  }
}
