import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'neo-confirm-announcement',
  templateUrl: 'confirm-announcement.component.html',
  styleUrls: ['confirm-announcement.component.scss']
})
export class ConfirmAnnouncementComponent {
  @Input() announcementId: number;
  @Output() closeModal: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() confirmChanges: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private router: Router) {}

  onEditClick(): void {
    if (this.router.url.includes('edit') || this.router.url.includes('add')) {
      this.closeModal.emit(true);
    } else {
      this.closeModal.emit(true);
      this.router.navigate([`admin/announcements/edit/${this.announcementId}`]);
    }
  }
}
