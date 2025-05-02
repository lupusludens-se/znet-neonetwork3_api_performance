import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TitleService } from '../core/services/title.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
	selector: 'neo-components',
	templateUrl: './settings.component.html',
	styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {
	selectedTab: 'general' | 'notification' = 'general';

	constructor(private readonly titleService: TitleService,
		private readonly activatedRoute: ActivatedRoute,
		private readonly cdr: ChangeDetectorRef) { }

	ngOnInit(): void {
		this.titleService.setTitle('settings.accountSettingLabel');

		this.activatedRoute.params.subscribe((result) => {
			if (result.tab == "2") {
				this.changeTab('notification');
				this.cdr.detectChanges();
				return;
			}
			this.changeTab('general');
			this.cdr.detectChanges();
		});
	}

	changeTab(tabName: 'general' | 'notification'): void {
		this.selectedTab = tabName;
	}
}
