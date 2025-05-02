import { Component, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { filter } from 'rxjs';
import { CoreService } from '../core/services/core.service';
import { TitleService } from '../core/services/title.service';
import { AuthService } from '../core/services/auth.service';

@UntilDestroy()
@Component({
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss']
})
export class EventsComponent implements OnInit {
  selectedTab: 'list' | 'calendar' | null = null;
  search: string;
  eventsCount: number | null = null;
  auth = AuthService;
  constructor(
    private readonly titleService: TitleService,
    public authService: AuthService,
    private readonly coreService: CoreService
  ) {}

  ngOnInit(): void {
    this.coreService.elementNotFoundData$
      .pipe(
        untilDestroyed(this),
        filter(data => !data)
      )
      .subscribe(() => {
        this.selectedTab = 'list';
        this.titleService.setTitle('settings.eventsLabel');
      });
  }

  onEventsLoaded(eventsCount: number): void {
    if (this.eventsCount !== eventsCount && (!this.eventsCount || eventsCount === 0)) this.eventsCount = eventsCount;
  }
}
