import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

import { SearchBarComponent } from '../../components/search-bar/search-bar.component';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { CoreService } from '../../../core/services/core.service';

@Component({
  selector: 'neo-global-search',
  templateUrl: './global-search.component.html',
  styleUrls: ['../../components/search-bar/search-bar.component.scss', './global-search.component.scss']
})
export class GlobalSearchComponent extends SearchBarComponent implements AfterViewInit {
  menuBtnClick: boolean;

  @ViewChild('searchInput') searchInput;

  constructor(
    elRef: ElementRef,
    private readonly router: Router,
    private readonly coreService: CoreService,
    snackbarService: SnackbarService
  ) {
    super(elRef, snackbarService);
  }

  ngAfterViewInit(): void {
    setTimeout(() => this.searchInput.nativeElement?.focus());
  }

  preventCloseOnClick(): void {
    this.menuBtnClick = true;
  }

  toggleSearchBox(): void {
    this.clear();
    this.coreService.globalSearchActive$.next(false);
  }

  search(): void {
    if (this.value?.length > 0 && this.value?.length < 3) {
      this.snackbarService.showError('general.searchMinCharactersLabel');
      return;
    }

    this.router.navigate(['/search'], { queryParams: { data: this.value } }).then();
    this.toggleSearchBox();
  }
}
