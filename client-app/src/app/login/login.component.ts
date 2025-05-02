import { Component, OnInit } from '@angular/core';
import { AuthService } from '../core/services/auth.service';
import { SpinnerService } from '../core/services/spinner.service';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';
import { HttpService } from '../core/services/http.service';

@Component({
  selector: 'neo-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
  private unsubscribe$: Subject<void> = new Subject<void>();
  public hideLoader: boolean = false;

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly httpService: HttpService,
    private readonly spinnerService: SpinnerService
  ) {}

  ngOnInit(): void {
    // this.authService.loginRedirect();
  }
}
