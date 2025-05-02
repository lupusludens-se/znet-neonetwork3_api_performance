import { Component, OnInit } from '@angular/core';
import { TitleService } from 'src/app/core/services/title.service';
import { InitiativeViewSource } from './enums/initiative-view-source';
import { DecarbonizationInitiativeService } from './services/decarbonization-initiatives.service';
import { Subject, takeUntil } from 'rxjs';
import { PaginationIncludeCountInterface } from 'src/app/shared/modules/pagination/pagination.component';
import { CoreService } from 'src/app/core/services/core.service';

@Component({
  selector: 'neo-decarbonization-initiatives',
  templateUrl: './decarbonization-initiatives.component.html',
  styleUrls: ['./decarbonization-initiatives.component.scss']
})
export class DecarbonizationInitiativesComponent implements OnInit {
  private unsubscribe$: Subject<void> = new Subject<void>();
  defaultItemPerPage = 6;
  paging: PaginationIncludeCountInterface = {
    take: this.defaultItemPerPage,
    skip: 0,
    total: null,
    includeCount: true
  };

  routesToNotClearFilters: string[] = [`/decarbonization-initiatives`];

  selectedTab: InitiativeViewSource = InitiativeViewSource['YourInitiatives'];

  constructor(
    private readonly titleService: TitleService,
    private readonly decarbonizationInitiativeService: DecarbonizationInitiativeService,
    private coreService: CoreService
  ) {
    this.decarbonizationInitiativeService
      .getPaging()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(page => {
        this.paging = page;
      });

    this.decarbonizationInitiativeService.getSource().subscribe(sourceId => {
      this.selectedTab = sourceId;
    });
  }

  ngOnInit() {
    this.titleService.setTitle('initiative.dashboard.initiativeTitle');
  }

  changeTab(tabName: InitiativeViewSource): void {
    this.decarbonizationInitiativeService.setSource(tabName);
  }

  updateSourceAndPagingDetails(paging: PaginationIncludeCountInterface): void {
    this.decarbonizationInitiativeService.setPaging(paging);
  }

  ngOnDestroy(): void {
    const routesFound = this.routesToNotClearFilters.some(val => this.coreService.getOngoingRoute().includes(val));
    if (!routesFound) {
      this.decarbonizationInitiativeService.clearPaging();
      this.decarbonizationInitiativeService.setSource(InitiativeViewSource['YourInitiatives']);
    }
  }
}
