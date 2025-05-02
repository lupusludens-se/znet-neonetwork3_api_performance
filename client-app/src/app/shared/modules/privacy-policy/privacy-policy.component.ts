import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'neo-privacy-policy',
  templateUrl: './privacy-policy.component.html',
  styleUrls: ['./privacy-policy.component.scss']
})
export class PrivacyPolicyComponent {
  @Output() closed = new EventEmitter<Record<string, string>>();
}
