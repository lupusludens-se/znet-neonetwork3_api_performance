import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  OnChanges,
  OnDestroy,
  Output,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import { debounceTime, fromEvent, Subscription } from 'rxjs';
import { SearchResultInterface } from '../../interfaces/search-result.interface';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { filter } from 'rxjs/operators';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { SearchValidatorService } from '../../services/search-validator.service';

@Component({
  selector: 'neo-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SearchBarComponent implements OnChanges, AfterViewInit, OnDestroy {
  @ViewChild('searchEl') searchEl: ElementRef;
  @Input() size: 'small' | 'medium' | 'large' = 'medium';
  @Input() results: SearchResultInterface[] | TagInterface[];
  @Input() placeholder: string = 'general.searchLabel';
  @Input() value: string;
  @Input() searchOnEnter: boolean;
  @Input() pageSearchControl: boolean;
  @Input() disabled: boolean;
  @Input() clearOnSelect: boolean;
  @Input() submitted: boolean;
  @Input() error: boolean;
  @Input() allowedCharactersRegExp: RegExp;
  @Input() errorIcon: string;
  @Input() resultsHeight: string = '230px';
  @Input() showResults: boolean = true;
  @Input() isRemoveBg?: boolean = false;
  @Input() showErrorIcon: boolean = true;


  @Output() enterEvent: EventEmitter<void> = new EventEmitter<void>();
  @Output() inputChange: EventEmitter<string> = new EventEmitter<string>();
  @Output() clearInput: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() selectedResult: EventEmitter<any> = new EventEmitter<any>(); // !! possibly will be in other comp

  inputSubscription: Subscription;
  clickOutside: boolean;
  showArrow: number;
  searchString: string;
  isActive: boolean;

  @HostListener('document:click', ['$event'])
  documentClick(event: Event) {
    this.clickOutside = !this.elRef.nativeElement.contains(event.target);

    if (this.clickOutside && this.results?.length > 0) {
      this.results = [];
    }
  }

  constructor(private elRef: ElementRef, protected snackbarService: SnackbarService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes?.results?.currentValue !== changes?.results?.previousValue) {
      this.setupResultsDisplayName();
    }

    if (changes?.value?.currentValue !== changes?.value?.previousValue) {
      this.searchString = changes?.value?.currentValue;
    }
  }

  ngAfterViewInit(): void {
    this.inputSubscription = fromEvent(this.searchEl?.nativeElement, 'keyup')
      .pipe(
        debounceTime(1000),
        filter(
          (keyup: Record<string, Record<string, string>>) =>
            keyup.target.value !== this.searchString && !this.searchOnEnter
        )
      )
      .subscribe((keyUpEvent: Record<string, Record<string, string>>) => {
        this.searchString = keyUpEvent.target.value;
        this.searchValue(keyUpEvent.target.value.trim());

        if (!keyUpEvent.target.value) this.clear();
      });
  }

  ngOnDestroy(): void {
    if (this.inputSubscription) {
      this.inputSubscription.unsubscribe();
    }
  }

  clear(): void {
    this.value = '';
    this.searchString = '';
    this.clearInput.emit(true);
  }

  selectItem(item: any): void {
    // !! possibly will be in a separate comp
    this.selectedResult.emit(item);
    this.value = item.name;
    this.results = [];

    if (this.clearOnSelect) {
      this.clear();
    }
  }

  emitSearch(): void {
    if (this.searchOnEnter) {
      this.searchValue(this.value.trim());
    }
    this.enterEvent.emit();
  }

  searchValue(value: string): void {
    if (!SearchValidatorService.validateSearch(value, this.allowedCharactersRegExp)) {
      this.displayError();
      return;
    }

    this.error = false;
    this.inputChange.emit(value);
  }

  protected displayError(): void {
    this.error = true;
    this.snackbarService.showError('general.searchErrorLabel');
  }

  private setupResultsDisplayName(): void {
    this.results?.map(result => {
      if (!this.value || !result) return;

      const searchMask = this.value;
      result.displayName = result.name;
      let i = result.displayName.length - 1;

      while (i >= 0) {
        const index = result.displayName.toLowerCase().lastIndexOf(searchMask.toLowerCase(), i);

        if (index === -1) break;

        let partToReplace = result.displayName.substr(index, searchMask.length);

        //adding this css chnage for forum start a disucssion listing while search text
        const replaceMask = `<span class="fw-700 forum-search-item text-m neo-arial">${partToReplace}</span>`;

        result.displayName =
          result.displayName.substring(0, index) +
          replaceMask +
          result.displayName.substring(index + partToReplace.length);
        i = index - 1;
      }
    });
  }
}
