import { Component } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'neo-reset-complete',
  templateUrl: './reset-complete.component.html',
  styleUrls: ['./reset-complete.component.scss']
})
export class ResetCompleteComponent {
  termOfUseModal: boolean;

  constructor(private readonly authService: AuthService) {}
  goToDashboard(): void {
    this.authService.loginRedirect();
  }
}