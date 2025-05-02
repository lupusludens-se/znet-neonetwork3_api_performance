import { Component, OnInit } from '@angular/core';
import { TitleService } from '../core/services/title.service';
import { AuthService } from '../core/services/auth.service';
import { PermissionService } from '../core/services/permission.service';
import { UserInterface } from '../shared/interfaces/user/user.interface';
import { AdminNavigationInterface } from '../+admin/interfaces/admin-navigation.interface';
import { MessageTabService } from '../shared/services/message-tab.service';
import { Router } from '@angular/router';

@Component({
  selector: 'neo-sp-admin',
  templateUrl: './sp-admin.component.html',
  styleUrls: ['./sp-admin.component.scss']
})
export class SpAdminComponent implements OnInit {
  adminNavigation: AdminNavigationInterface[] = [];

  currentUser: UserInterface;

  showModal: boolean;
  constructor(
    private readonly titleService: TitleService,
    private readonly authService: AuthService,
    private readonly permissionService: PermissionService,
    private readonly router: Router,
    private messagesTabService: MessageTabService
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle('spAdmin.adminManagementLabel');
    this.listenForCurrentUser();
  }

  hasPermission(permission: number): boolean {
    return this.permissionService.userHasPermission(this.currentUser, permission);
  }

  private listenForCurrentUser(): void {
    this.authService.currentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      if (currentUser) {
        this.initAdminNavigation();
      }
    });
  }

  private initAdminNavigation(): void {
    this.adminNavigation.push(
      {
        icon: 'user-account',
        title: 'userManagement.pageTitleLabel',
        addButtonName: 'spAdmin.viewUsersLabel',
        addButtonLink: '/manage/users'
      },
      {
        icon: 'building',
        title: 'companyManagement.pageTitleLabel',
        addButtonName: 'spAdmin.editCompanyLabel',
        addButtonLink: '/manage/company-profile'
      },
      {
        icon: 'communication-bubble',
        title: 'spAdmin.MessagesLabel',
        addButtonName: 'spAdmin.viewMessagesLabel',
        addButtonLink: '/messages'
      },
      {
        icon: 'sun',
        title: 'spAdmin.projectLabel',
        addButtonName: 'spAdmin.viewProjectsLabel',
        addButtonLink: '/projects-library'
      }
    );
  }

  goToMessages(): void {
    this.messagesTabService.setTabState('network');
    this.router.navigate(['/messages']);
  }
}
