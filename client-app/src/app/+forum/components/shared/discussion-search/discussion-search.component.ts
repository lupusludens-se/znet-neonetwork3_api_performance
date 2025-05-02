import { Component, ElementRef, EventEmitter, Input, Output } from '@angular/core';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { SearchBarComponent } from '../../../../shared/components/search-bar/search-bar.component';
import { ForumSearchResultInterface } from '../../../interfaces/forum-search-result.interface';

@Component({
  selector: 'neo-discussion-search',
  templateUrl: './discussion-search.component.html',
  styleUrls: ['../../../../shared/components/search-bar/search-bar.component.scss']
})
export class DiscussionSearchComponent extends SearchBarComponent {
  @Input() results: ForumSearchResultInterface[];

  @Output() createDiscussion: EventEmitter<void> = new EventEmitter<void>();

  constructor(elRef: ElementRef, snackbarService: SnackbarService) {
    super(elRef, snackbarService);
  }
}
