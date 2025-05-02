import { Component } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'neo-navbar-skeleton',
  templateUrl: './navbar-skeleton.component.html',
  styleUrls: ['./navbar-skeleton.component.scss']
})
export class NavbarSkeletonComponent { 
  isPublicUser: boolean;
  constructor() { }

  ngOnInit() {
    this.isPublicUser = !( AuthService.isLoggedIn() || AuthService.needSilentLogIn());
  }
}
