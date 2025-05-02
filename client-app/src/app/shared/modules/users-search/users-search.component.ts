import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges
} from '@angular/core';
import { debounceTime, fromEvent } from 'rxjs';

import { PaginateResponseInterface } from '../../interfaces/common/pagination-response.interface';
import { SearchBarComponent } from '../../components/search-bar/search-bar.component';
import { UserProfileApiEnum } from '../../enums/api/user-profile-api.enum';
import { UserStatusEnum } from '../../../user-management/enums/user-status.enum';
import { UserInterface } from '../../interfaces/user/user.interface';
import { HttpService } from '../../../core/services/http.service';
import { CompanyType } from 'src/app/+company-profile/interfaces/company-profile.interface';
import { PermissionTypeEnum } from 'src/app/core/enums/permission-type.enum';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { SearchValidatorService } from '../../services/search-validator.service';
import { UserManagementApiEnum } from 'src/app/user-management/enums/user-management-api.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { RolesEnum } from '../../enums/roles.enum';

interface ModeratorsSearchResultInterface extends UserInterface {
  name?: string;
  displayImage?: string;
  userId?: number;
}

@Component({
  selector: 'neo-users-search',
  templateUrl: './users-search.component.html',
  styleUrls: ['../../components/search-bar/search-bar.component.scss', './users-search.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UsersSearchComponent extends SearchBarComponent implements OnChanges, OnInit, AfterViewInit {
  @Input() preSelectedModerator: ModeratorsSearchResultInterface;
  @Input() displayName: string;
  @Input() displayImage: string;
  @Input() error: boolean;
  @Input() searchWithMention: boolean = true;
  @Input() showClearButton: boolean = true;
  @Input() showOnlyProjectPublishers: boolean;
  @Input() companyCategoryId: number;

  apiRoutes = UserProfileApiEnum;
  userManagementApiRoutes = UserManagementApiEnum;
  resultsList: ModeratorsSearchResultInterface[];
  userStatuses = UserStatusEnum;
  companyTypes = CompanyType;
  permissionTypes = PermissionTypeEnum;
  currentUser: UserInterface;

  constructor(
    elRef: ElementRef,
    private httpService: HttpService,
    private changeDetRef: ChangeDetectorRef,
    snackbarService: SnackbarService,
    public authService: AuthService
  ) {
    super(elRef, snackbarService);
  }

  ngOnInit(): void {
    if (this.displayName) {
      this.preSelectedModerator = {
        name: this.displayName,
        image: { uri: this.displayImage }
      } as ModeratorsSearchResultInterface;
    } else {
      this.displayName = this.preSelectedModerator?.name;
    }

    this.currentUser = this.authService.currentUser$.getValue();
  }

  ngAfterViewInit(): void {
    this.inputSubscription = fromEvent(this.searchEl.nativeElement, 'keyup')
      .pipe(debounceTime(400))
      .subscribe((keyUpEvent: Record<string, Record<string, string>>) => {
        if (this.searchWithMention && keyUpEvent.target.value.indexOf('@') === 0) {
          const searchStr: string = keyUpEvent.target.value.slice(1);
          this.performSearch(keyUpEvent, searchStr);
        } else if (this.searchWithMention && !keyUpEvent.target.value) {
          this.resultsList = [];
        } else if (!this.searchWithMention) {
          this.performSearch(keyUpEvent, keyUpEvent.target.value);
        } else this.searchValue(keyUpEvent.target.value);
      });
  }

  performSearch(keyUpEvent, searchStr): void {
    if (!SearchValidatorService.validateSearch(searchStr)) {
      this.displayError();
      return;
    }

    this.error = false;
    let filterBy = `statusids=${this.userStatuses.Active}`;
    if (this.showOnlyProjectPublishers) {
      filterBy += `&companytypesids=${this.companyTypes.SolutionProvider}&permissionids=${this.permissionTypes.ProjectsManageOwn}&companycategoryids=${this.companyCategoryId}`;
    }
    const url = this.currentUser.roles.find(x => x.id == RolesEnum.SPAdmin) ? this.userManagementApiRoutes.CompanyUsers : this.apiRoutes.Users;
    this.httpService
      .get<PaginateResponseInterface<UserInterface>>(url, {
        FilterBy: filterBy,
        Search: searchStr,
        expand: 'images,company'
      })
      .subscribe(res => {
        this.resultsList = [...res.dataList];
        this.resultsList.forEach(result => {
          result['name'] = result?.lastName?.replace(
            this.displayName,
            `<span class="fw-700 text-neo-blue">${this.displayName}</span>`
          );
        });

        this.changeDetRef.markForCheck();
      });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes?.resultsList?.currentValue !== changes?.resultsList?.previousValue) {
      this.resultsList.map(result => {
        result.name = result?.lastName?.replace(
          this.displayName,
          `<span class="fw-700 text-neo-blue">${this.displayName}</span>`
        );
      });
    }
  }

  selectItem(item: ModeratorsSearchResultInterface): void {
    this.selectedResult.emit(item);
    this.displayName = `${item.firstName} ${item.lastName} `;
    this.preSelectedModerator = item;
    this.resultsList = [];
  }

  clear(): void {
    this.displayName = '';
    this.displayImage = '';
    this.preSelectedModerator = null;
    this.clearInput.emit(true);
  }
}
