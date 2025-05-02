import { Component, OnInit } from '@angular/core';

import { UntilDestroy } from '@ngneat/until-destroy';

import { AuthService } from '../core/services/auth.service';
import { Router } from '@angular/router';

@UntilDestroy()
@Component({
  selector: 'neo-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss', '../../../src/app/core/components/spinner/spinner.component.scss']
})
export class LandingComponent implements OnInit {
  constructor(private readonly authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    if (this.authService.currentUser$.getValue() == null) {
      this.router.navigate(['dashboard']);
    } else {
      this.authService.loginRedirect();
    }
  }
}
