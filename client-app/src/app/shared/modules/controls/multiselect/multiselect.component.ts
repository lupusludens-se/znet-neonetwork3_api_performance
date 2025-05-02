import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  Output,
  Renderer2,
  SimpleChanges,
  ViewChild
} from '@angular/core';

import { debounceTime, fromEvent, Subject, takeUntil } from 'rxjs';

import { MultiselectOptionInterface } from './interfaces/multiselect-option.interface';
import { UserInterface } from '../../../interfaces/user/user.interface';
import { SearchValidatorService } from 'src/app/shared/services/search-validator.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';

@Component({
  selector: 'neo-multiselect',
  templateUrl: './multiselect.component.html',
  styleUrls: ['./multiselect.component.scss']
})
export class MultiselectComponent implements OnChanges, AfterViewInit, OnDestroy {
  @ViewChild('searchInput') searchInput: ElementRef;

  @Input() error: boolean;
  @Input() value: string;
  @Input() maxSelection: number;
  @Input() results: MultiselectOptionInterface[];
  @Input() placeholder: string = 'general.searchLabel';
  @Input() selectedResults: MultiselectOptionInterface[] = [];

  @Output() inputChange: EventEmitter<string> = new EventEmitter<string>();
  @Output() selectionChange: EventEmitter<MultiselectOptionInterface[]> = new EventEmitter<
    MultiselectOptionInterface[]
  >();

  resultsListOpen: boolean;

  private unsubscribe$: Subject<void> = new Subject<void>();

  constructor(private readonly renderer: Renderer2, private readonly snackbarService: SnackbarService) {}

  //#region lifecycle hooks
  ngOnChanges(changes: SimpleChanges): void {
    if (changes?.results?.currentValue !== changes?.results?.previousValue) {
      this.results?.map(result => {
        result.displayName = result?.name?.replace(
          this.value,
          `<span class="fw-700 text-dark-gray-750">${this.value}</span>`
        );
      });

      this.resultsListOpen = true;
    }

    if (changes?.maxSelection?.currentValue !== changes?.maxSelection?.previousValue) {
      this.selectedResults = [];
    }
  }

  ngAfterViewInit(): void {
    this.listenForWindowClick();
    this.listenForInputChange();
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  //#endregion

  optionClick(option: MultiselectOptionInterface): void {
    this.value = null;
    this.results = [];
    this.resultsListOpen = false;

    if (this.selectedResults?.length === this.maxSelection) return;

    this.selectedResults.push(option);
    this.selectionChange.emit(this.selectedResults);
  }

  getImage(selectedResult: MultiselectOptionInterface) {
    return {
      image: selectedResult.image,
      firstName: selectedResult.firstName,
      lastName: selectedResult.lastName
    } as UserInterface;
  }

  removeUser(index: number): void {
    this.selectedResults.splice(index, 1);
    this.selectionChange.emit(this.selectedResults);
  }

  private listenForInputChange(): void {
    fromEvent(this.searchInput.nativeElement, 'keyup')
      .pipe(takeUntil(this.unsubscribe$), debounceTime(400))
      .subscribe((keyUpEvent: Record<string, Record<string, string>>) => {
        if (!SearchValidatorService.validateSearch(keyUpEvent.target.value)) {
          this.snackbarService.showError('general.searchErrorLabel');
          return;
        }

        this.inputChange.emit(keyUpEvent.target.value);
      });
  }

  private listenForWindowClick(): void {
    this.renderer.listen('window', 'click', () => {
      if (!this.resultsListOpen) {
        this.resultsListOpen = false;
      }
      this.resultsListOpen = false;
    });
  }
}
