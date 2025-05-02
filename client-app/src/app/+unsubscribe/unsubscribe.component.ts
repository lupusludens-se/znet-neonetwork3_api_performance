import { Component, OnInit } from '@angular/core';
import { UnsubscribeApiEnum } from '../shared/enums/api/general-api.enum';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { RadioControlInterface } from '../shared/modules/radio-control/radio-control.component';
import { EmailAlertFrequencyEnum } from '../+admin/modules/+email-alerts/models/email-alert';
import { UnsubscribeService } from '../shared/services/unsubscribe.service';

@Component({
  selector: 'neo-unsubscribe',
  templateUrl: './unsubscribe.component.html',
  styleUrls: ['./unsubscribe.component.scss']
})
export class UnsubscribeComponent implements OnInit {
  hasLoaded: boolean = false;
  email: string;
  apiRoutes = UnsubscribeApiEnum;
  showSuccess: boolean;

  termOfUseModal: boolean;
  termsAndConditions: boolean;
  emailAlertFrequencyEnum = EmailAlertFrequencyEnum;
  types: RadioControlInterface[] = [
    {
      id: EmailAlertFrequencyEnum.Off,
      name: this.translateService.instant('unsubscribe.type.Off')
    },
    {
      id: EmailAlertFrequencyEnum.Monthly,
      name: this.translateService.instant('unsubscribe.type.Monthly')
    },
    {
      id: EmailAlertFrequencyEnum.Weekly,
      name: this.translateService.instant('unsubscribe.type.Weekly')
    },
    {
      id: EmailAlertFrequencyEnum.Daily,
      name: this.translateService.instant('unsubscribe.type.Daily')
    }
  ];
  formSubmitted: boolean;

  formGroup: FormGroup = this.formBuilder.group({
    typeId: new FormControl(null, Validators.required)
  });
  TypeSelected: number = -1;
  showTypeError: boolean = false;
  showAlreadyUnsubscribedMsg: boolean;

  constructor(
    private formBuilder: FormBuilder,
    private readonly translateService: TranslateService,
    private readonly unsubscribeService: UnsubscribeService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly router: Router) { }

  ngOnInit(): void {
    this.unsubscribeService.getUserEmailFromToken(this.activatedRoute.snapshot.params?.token).subscribe((response: any) => {
      this.hasLoaded = true;
      this.email = response?.email;
      this.formGroup?.get('typeId').setValue(response?.emailPreference);
    });
  }

  unsubscribe(): void {
    const control = this.formGroup?.get('typeId');
    if (!control?.invalid) {
      this.unsubscribeService.updateUserEmailPreferences(this.activatedRoute.snapshot.params?.token, control.value).subscribe((response: any) => {
        if (response?.message == "Success") {
          this.showSuccess = true;
        }
        else if (response?.message == "AlreadyUnsubscribed") {
          this.showAlreadyUnsubscribedMsg = true;
          this.showSuccess = false;
        }
      });
    }
    else {
      this.showTypeError = true;
    }
  }

  login(): void {
		this.router.navigate(['./settings/2']);
  }

  changeType(control: RadioControlInterface): void {
    this.TypeSelected = Number(control.id);
    this.showTypeError = false;
  }
}
