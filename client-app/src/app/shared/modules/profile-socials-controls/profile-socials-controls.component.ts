import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'neo-profile-socials-controls',
  templateUrl: 'profile-socials-controls.component.html',
  styleUrls: ['./profile-socials-controls.component.scss']
})
export class ProfileSocialsControlsComponent {
  @Output() followClick: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() userId: number;
  @Input() message: boolean = true;
  @Input() linkedInLink: string;
  @Input() following: boolean;
}
