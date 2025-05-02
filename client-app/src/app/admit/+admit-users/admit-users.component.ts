import { AdmitUserEnum } from '../emuns/admit-user.enum';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { DEFAULT_PER_PAGE, PaginationInterface } from '../../shared/modules/pagination/pagination.component';
import { PaginateResponseInterface } from '../../shared/interfaces/common/pagination-response.interface';
import { PendingUserInterface } from '../interfaces/pending-user.interface';
import { CompanyStatusEnum } from '../../shared/enums/company-status.enum';
import { CompanyRolesEnum } from '../../shared/enums/company-roles.enum';
import { SnackbarService } from '../../core/services/snackbar.service';
import { TitleService } from '../../core/services/title.service';
import { HttpService } from '../../core/services/http.service';
import { RolesEnum } from '../../shared/enums/roles.enum';
import { TranslateService } from '@ngx-translate/core';
import { UserDataService } from 'src/app/user-management/services/user.data.service';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { UserRoleInterface } from 'src/app/shared/interfaces/user/user-role.interface';
import { ViewportScroller } from '@angular/common';

@Component({
  selector: 'neo-admit-users',
  templateUrl: 'admit-users.component.html',
  styleUrls: ['./admit-users.component.scss']
})
export class AdmitUsersComponent implements OnInit {
  apiList = AdmitUserEnum;
  paging: PaginationInterface;
  defaultItemPerPage = DEFAULT_PER_PAGE;
  usersList: PendingUserInterface[];
  nameAsc: boolean;
  companyAsc: boolean;
  roleAsc: boolean;
  createddateAsc: boolean = false;
  sortingCriteria: Record<string, string> = {
    name: 'name',
    company: 'company',
    role: 'role',
    createddate: 'createddate'
  };
  companyStatuses = CompanyStatusEnum;
  showConfirmModal: boolean;
  userToApprove: PendingUserInterface;
  rolesList = RolesEnum;
  companyRolesList = CompanyRolesEnum;
  tdTitleClick: string;
  spAdminConfirmationTitle: any;

  constructor(
    private httpService: HttpService,
    private router: Router,
    private titleService: TitleService,
    private snackbarService: SnackbarService,
    private translateService: TranslateService,
    private userDataService: UserDataService,
    private viewPort: ViewportScroller
  ) { }

  ngOnInit(): void {
    this.titleService.setTitle('admitUsers.pageTitle');
    this.getUsersList();
  }

  getUsersList(Skip: number = 0, OrderBy: string = '', Take: number = this.defaultItemPerPage): void {
    this.httpService
      .get<PaginateResponseInterface<PendingUserInterface>>(this.apiList.UserPendings, {
        IncludeCount: true,
        expand: 'company,role',
        Skip,
        OrderBy,
        Take
      })
      .subscribe(us => {
        this.usersList = us.dataList;

        this.paging = {
          ...this.paging,
          skip: Skip,
          take: Take,
          total: us.count
        };
        
      this.viewPort.scrollToPosition([0, 0]);
      });
  }

  goToUser(id: number): void {
    this.router.navigate([`admin/admit-users/review-user/${id}`]);
  }

  denyUser(user: PendingUserInterface, isDenied: boolean): void {
    const denyMessage: string = `${user.lastName}, ${user.firstName}
          ${this.translateService.instant(
      isDenied ? 'admitUsers.denySuccessMessageLabel' : 'admitUsers.unDenySuccessMessageLabel'
    )}
          `;

    this.httpService
      .patch(this.apiList.UserPendings + `/${user.id}/` + this.apiList.Denial + `?isDenied=${isDenied}`, {
        id: user.id
      })
      .subscribe(() => {
        this.getUsersList();
        if (isDenied) {
          this.snackbarService.showError(denyMessage);
        } else this.snackbarService.showSuccess(denyMessage);
      });
  }

  approveUser(user: PendingUserInterface): void {
    if (user.id > 0) {
      this.httpService
        .post(this.apiList.UserPendings + `/${user.id}/` + this.apiList.Approval, {
          id: user.id
        })
        .subscribe(() => {
          this.showConfirmModal = false;
          this.userToApprove = null;
          this.getUsersList();
          this.snackbarService.showSuccess(
            `${user.lastName}, ${user.firstName}`,
            'admitUsers.approvedSuccessMessageLabel'
          );
        });
    }
  }

  sortCriteriaSelection(
    sortDirection: string,
    sortKey: string,
    secondSortCol: string,
    thirdSortCol: string,
    fourthSortCol: string
  ): void {
    this.tdTitleClick = sortKey;
    if (this[sortDirection]) {
      this.getUsersList(0, `${this.sortingCriteria[sortKey]}.desc`);
    } else {
      this.getUsersList(0, `${this.sortingCriteria[sortKey]}.asc`);
    }

    this[secondSortCol] = false;
    this[thirdSortCol] = false;
    this[fourthSortCol] = false;

    this[sortDirection] = !this[sortDirection];
  }

  updatePaging(page: number): void {
    const skip: number = Math.round((page - 1) * this.defaultItemPerPage);
    this.getUsersList(skip);
  }

  getClassNamesBasedonRole(roleId: number, roleName : string) {
    return roleId == RolesEnum.SPAdmin ? roleName.toLowerCase().replace(/\s/g, '') : roleName.toLowerCase();
  }

  approveUserConfirmation(user) {
    this.spAdminConfirmationTitle = "";
    if (user.roleId == RolesEnum.SPAdmin) {
      this.userDataService.getSPAdminByCompany(Number(user.companyId), 0).subscribe((result: UserInterface) => {
        this.userToApprove = user;
        if (result?.id > 0) {
          this.showConfirmModal = true;
          const spAdminConfirmationTitle = this.translateService.instant('userManagement.form.SPAdminConfirmationTitle');
          this.spAdminConfirmationTitle = spAdminConfirmationTitle?.replace("_username_", `${result.lastName}, ${result.firstName}`);
        }
        else {
          this.showConfirmModal = true;
        }
      });
    }
    else {
      this.showConfirmModal = true;
      this.userToApprove = user;
    }
  }
}
