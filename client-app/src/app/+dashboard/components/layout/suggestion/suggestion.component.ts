import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';

import { Observable } from 'rxjs';

import { Suggestions } from '../../../../shared/interfaces/user/user-suggestion.intarface';
import { AuthService } from '../../../../core/services/auth.service';
import { UserInterface } from '../../../../shared/interfaces/user/user.interface';

@Component({
  selector: 'neo-suggestion',
  templateUrl: './suggestion.component.html',
  styleUrls: ['./suggestion.component.scss']
})
export class SuggestionComponent implements OnChanges {
  @Input() missingInfo: string;
  @Input() usersSuggestions: Suggestions[];

  @Output() skipClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() hideClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() yesClick: EventEmitter<void> = new EventEmitter<void>();

  suggestionText: string;
  suggestions: Suggestions[];

  currentUser$: Observable<UserInterface> = this.authService.currentUser();

  constructor(private readonly authService: AuthService) {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes?.usersSuggestions?.currentValue !== changes?.usersSuggestions?.previousValue) {
      this.fillSuggestionInfo();
    }
  }

  fillSuggestionInfo(): void {
    const skippedSuggestions = localStorage.getItem('skippedSuggestions');

    const skippedSuggestionsList: string[] = skippedSuggestions ? JSON.parse(skippedSuggestions) : [];

    if (skippedSuggestions?.length) {
      this.suggestions = this.usersSuggestions?.filter(
        suggestion => !skippedSuggestionsList?.some(item => item === suggestion?.name)
      );
    } else {
      this.suggestions = this.usersSuggestions;
    }

    if (this.suggestions.length) {
      this.suggestionText =
        this.suggestions[0].name === 'About'
          ? 'dashboard.completeUserAboutLabel'
          : 'dashboard.completeUserLinkedInLabel';
    }
  }

  skipSuggestionInfo(): void {
    let selectSkippedSuggestions: string[] = JSON.parse(localStorage.getItem('skippedSuggestions'));

    if (selectSkippedSuggestions?.length) {
      selectSkippedSuggestions.push(this.suggestions[0].name);
    } else {
      selectSkippedSuggestions = [this.suggestions[0].name];
    }

    localStorage.setItem('skippedSuggestions', JSON.stringify(selectSkippedSuggestions));

    this.fillSuggestionInfo();
    this.skipClick.emit();
  }

  closeSuggestion(): void {
    localStorage.setItem('skippedSuggestions', JSON.stringify(this.usersSuggestions.map(item => item.name)));
    this.fillSuggestionInfo();
    this.hideClick.emit();
  }
}
