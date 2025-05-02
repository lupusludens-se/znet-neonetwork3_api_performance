import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CoreService } from '../../services/core.service';

@Component({
  templateUrl: './forbidden.component.html',
  styleUrls: ['./forbidden.component.scss']
})
export class ForbiddenComponent {
  constructor(private readonly router: Router, private coreService: CoreService) {}
  back(): void {
    //navigates to dashboard, if previous page is part of SP Admin Routes or no route
    if (this.coreService.isSPAdminRoutes()) {
      this.router.navigate(['./dashboard']);
      return;
    }
    // history.back(); - returns us to a previous page, which leads again to 403
    if (history.length > 2) history.go(-2);
    else this.router.navigate(['./dashboard']);
  }
}
