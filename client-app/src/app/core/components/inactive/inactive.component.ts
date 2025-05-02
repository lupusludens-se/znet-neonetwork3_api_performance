import { Component } from '@angular/core';

import { AuthService } from '../../services/auth.service';

@Component({
  templateUrl: './inactive.component.html',
  styleUrls: ['./inactive.component.scss']
})
export class InactiveComponent {
  constructor(private readonly authService: AuthService) {}

  back(): void {
    this.authService.logout();
  }
}
