import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'neo-cookies',
  templateUrl: './cookies.component.html',
  styleUrls: ['./cookies.component.scss']
})
export class CookiesComponent {
  @Output() privacyPolicyClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() acceptCookiesClick: EventEmitter<void> = new EventEmitter<void>();
}
