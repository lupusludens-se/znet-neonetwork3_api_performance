import { FeedbackInterface } from '../interfaces/feedback.interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { ActivatedRoute } from '@angular/router';
import { TitleService } from 'src/app/core/services/title.service';
import { Subject, switchMap, takeUntil } from 'rxjs';
import { FeedbackService } from '../services/feedback.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { UserStatusEnum } from 'src/app/user-management/enums/user-status.enum';
import { FormBuilder, FormGroup } from '@angular/forms';
import { UserRoleInterface } from 'src/app/shared/interfaces/user/user-role.interface';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { CommonService } from 'src/app/core/services/common.service';
import { CoreService } from 'src/app/core/services/core.service';
import { FeedbackRoutesEnum } from '../enums/feedback.enum';

@UntilDestroy()
@Component({
  selector: 'neo-user-feedback-details',
  templateUrl: './user-feedback-details.component.html',
  styleUrls: ['./user-feedback-details.component.scss']
})
export class UserFeedbackDetailsComponent implements OnInit, OnDestroy {
  feedback: FeedbackInterface;
  feedbackId: number;
  subscription: any;
  currentUser: UserInterface;
  userStatuses = UserStatusEnum;
  commentsMaxLength: number = 1000;
  private loadData$: Subject<void> = new Subject<void>();
  private unsubscribe$: Subject<void> = new Subject<void>();
  form: FormGroup = this.formBuilder.group({
    firstName: [''],
    lastName: [''],
    company: [''],
    role: [''],
    comments: [''],
    rating: [null]
  });
  routesToNotClearFilters: string[] = [`${FeedbackRoutesEnum.UserFeedbacksComponent}`];
  constructor(
    private authService: AuthService,
    private feedbackService: FeedbackService,
    private titleService: TitleService,
    private readonly activatedRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    private commonService: CommonService,
    private coreService: CoreService
  ) {}

  ngOnInit(): void {
    this.subscription = this.authService
      .currentUser()
      .pipe(untilDestroyed(this))
      .subscribe((user: UserInterface) => {
        this.currentUser = user;
        this.activatedRoute.params.subscribe(() => {
          let feedbackId: string = this.activatedRoute.snapshot.paramMap.get('id');
          this.feedbackId = feedbackId ? parseInt(feedbackId) : 0;
          if (!isNaN(this.feedbackId) && this.feedbackId > 0) {
            this.listenToLoadData();
            this.loadData$.next();
          }
        });
      });

    this.titleService.setTitle('feedbackManagement.viewUserFeebackTitleLabel');
  }

  listenToLoadData() {
    this.loadData$
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(() => this.feedbackService.fetchFeedback(this.feedbackId))
      )
      .subscribe(feedback => {
        this.feedback = JSON.parse(JSON.stringify(feedback));
        this.feedback.roles = feedback.feedbackUser.roles.filter((role: any) => role.id != RolesEnum.All);
        this.form.patchValue({
          firstName: this.feedback.feedbackUser.firstName,
          lastName: this.feedback.feedbackUser.lastName,
          company: this.feedback.feedbackUser.company,
          role: this.feedback.feedbackUser.roles[0].id,
          comments: this.feedback.comments,
          rating: this.feedback.rating
        });
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    this.loadData$.next();
    this.loadData$.complete();

    const routesFound = this.routesToNotClearFilters.some(val => this.coreService.getOngoingRoute().includes(val));
    if (!routesFound) {
      this.feedbackService.clearAll();
    }
  }

  getClassNamesBasedonRole(role: UserRoleInterface) {
    return role.id == RolesEnum.SPAdmin ? role.name.toLowerCase().replace(/\s/g, '') : role.name.toLowerCase();
  }

  goBack() {
    this.commonService.goBack();
  }
}
