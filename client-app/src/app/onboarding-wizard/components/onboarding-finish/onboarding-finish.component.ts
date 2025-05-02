import { ControlContainer, FormGroupDirective } from '@angular/forms';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Component, Input, OnInit } from '@angular/core';
import { filter } from 'rxjs/operators';

import { UserProfileApiEnum } from '../../../shared/enums/api/user-profile-api.enum';
import { OnboardingWizardService } from '../../services/onboarding-wizard.service';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { OnboardingStepsEnum } from '../../enums/onboarding-steps.enum';
import { AuthService } from '../../../core/services/auth.service';
import { HttpService } from '../../../core/services/http.service';

@UntilDestroy()
@Component({
  selector: 'neo-onboarding-finish',
  templateUrl: 'onboarding-finish.component.html',
  styleUrls: ['../../onboarding-wizard.component.scss', 'onboarding-finish.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class OnboardingFinishComponent implements OnInit {
  @Input() userId: number | undefined;
  stepsList = OnboardingStepsEnum;
  userApi = UserProfileApiEnum;
  user: UserInterface;

  constructor(
    public controlContainer: ControlContainer,
    private onboardingWizardService: OnboardingWizardService,
    private readonly authService: AuthService,
    private httpService: HttpService
  ) {}

  ngOnInit(): void {
    this.authService
      .currentUser()
      .pipe(
        filter(v => !!v),
        untilDestroyed(this)
      )
      .subscribe(us => (this.user = us));
  }

  changeStep(step: OnboardingStepsEnum): void {
    this.onboardingWizardService.changeStep(step);
  }

  finishOnboarding(): void {
    this.httpService.post(this.userApi.UserProfiles, this.getPayload()).subscribe(() => {
      if (this.controlContainer.control?.get('countryId')?.value !== this.user.countryId) {
        this.httpService
          .patch(this.userApi.UsersCurrent, {
            jsonPatchDocument: [
              {
                op: 'replace',
                path: '/CountryId',
                value: this.controlContainer.control?.get('countryId')?.value
              }
            ]
          })
          .subscribe(() => {
            this.authService.getCurrentUser$.next(true);
          });
      } else {
        this.authService.getCurrentUser$.next(true);
      }
    });
  }

  goBack(): void {
    this.onboardingWizardService.changeStep(this.stepsList.DecarbonizationSolutions);
  }

  private getPayload() {
    const payload = { ...this.controlContainer.value, userId: this.userId };
    delete payload.country;
    delete payload.state;
    if (payload.skillsByCategory) {
      payload.skillsByCategory = payload.skillsByCategory.map(skill => ({
        skillId: skill.skill.id,
        skillName: skill.skill.name,
        categoryId: skill.projectType.id,
        categoryName: skill.projectType.name
      }));
    }
    payload.categories = payload.categories?.map(category => ({
      id: category.id
    }));
    payload.regions = payload.regions?.map(region => ({
      id: region.id
    }));
    payload.AcceptWelcomeSeriesEmail = payload.subscribeToWelcomeSeries == 1;
    if (!payload.linkedInUrl) delete payload.linkedInUrl;

    return payload;
  }
}
