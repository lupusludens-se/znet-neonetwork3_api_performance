import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { SignTrackingSourceEnum } from 'src/app/core/enums/sign-tracking-source-enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { HttpService } from 'src/app/core/services/http.service';

@Component({
	selector: 'neo-thank-you-interest-modal-popup',
	templateUrl: './thank-you-interest-modal-popup.component.html',
	styleUrls: ['./thank-you-interest-modal-popup.component.scss']
})
export class ThankYouInterestModalPopupComponent implements OnInit {

	signTrackingSourceEnum: string = "";
	constructor(
		private readonly authService: AuthService
	) { }
	ngOnInit(): void {
		this.signTrackingSourceEnum = SignTrackingSourceEnum.ZeigoNetwork;


	}

	logIn(): void {
		this.authService.loginRedirect();
	}
}
