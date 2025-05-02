import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';

import { HttpService } from 'src/app/core/services/http.service';

import { PatchPayloadInterface } from 'src/app/shared/interfaces/common/patch-payload.interface';
import { CompanyInterface } from 'src/app/shared/interfaces/user/company.interface';

import { CompanyStatusEnum } from 'src/app/shared/enums/company-status.enum';
import { CompanyManagementApiEnum } from '../../enums/company-management-api.enum';
import { TableCrudEnum } from '../../../../../shared/modules/table/enums/table-crud.enum';

import { PATCH_PAYLOAD } from 'src/app/shared/constants/patch-payload.const';
import { MENU_OPTIONS, MenuOptionInterface } from '../../../../../shared/modules/menu/interfaces/menu-option.interface';

@Component({
  selector: 'neo-company-table-row',
  templateUrl: './company-table-row.component.html',
  styleUrls: ['../../company-management.component.scss', './company-table-row.component.scss']
})
export class CompanyTableRowComponent {
  @Input() company: CompanyInterface;
  @Input() showMenu: boolean;
  @Output() updateCompanies: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() deleteCompanies: EventEmitter<boolean> = new EventEmitter<boolean>();
  companyStatuses = CompanyStatusEnum;
  showModal: boolean;
  menuOptions: MenuOptionInterface[] = MENU_OPTIONS;
  private patchPayload: PatchPayloadInterface = PATCH_PAYLOAD;

  constructor(private httpService: HttpService, private router: Router) {}

  deleteCompany(): void {
    this.patchPayload.jsonPatchDocument[0].value = this.companyStatuses.Deleted;
    this.patchPayload.jsonPatchDocument[0].op = 'replace';
    this.patchPayload.jsonPatchDocument[0].path = '/StatusId';

    this.httpService
      .patch(CompanyManagementApiEnum.Companies + `/${this.company.id}`, this.patchPayload)
      .subscribe(() => {
        this.deleteCompanies.emit(true);
      });
  }

  getOptions(): MenuOptionInterface[] {
    this.menuOptions = this.menuOptions.map(option => {
      if (this.company?.statusId === CompanyStatusEnum.Active) {
        this.menuOptions.map(opt => {
          if (opt.name === 'actions.deactivateLabel') opt.hidden = false;
          if (opt.name === 'actions.activateLabel') opt.hidden = true;
          if (opt.name === 'actions.previewLabel') opt.hidden = true;
        });
      } else {
        this.menuOptions.map(opt => {
          if (opt.name === 'actions.deactivateLabel') opt.hidden = true;
          if (opt.name === 'actions.activateLabel') opt.hidden = false;
          if (opt.name === 'actions.previewLabel') opt.hidden = true;
        });
      }

      if (option.operation === TableCrudEnum.Delete) {
        option.hidden = this.company.statusId === CompanyStatusEnum.Deleted;
      }

      return option;
    });

    return this.menuOptions;
  }

  optionClick(option: MenuOptionInterface): void {
    switch (option.operation) {
      case TableCrudEnum.Edit:
        this.goToCompany(this.company.id);
        break;
      case TableCrudEnum.Delete:
        this.showModal = true;
        break;
      case TableCrudEnum.Status:
        this.updateCompanyStatus(this.company.statusId);
        break;
    }
  }

  private goToCompany(id: number): void {
    this.router.navigate([`admin/company-management/edit/${id}`]).then();
  }

  private updateCompanyStatus(status: CompanyStatusEnum): void {
    this.patchPayload.jsonPatchDocument[0].value =
      status === CompanyStatusEnum.Active ? CompanyStatusEnum.Inactive : CompanyStatusEnum.Active;
    this.patchPayload.jsonPatchDocument[0].op = 'replace';
    this.patchPayload.jsonPatchDocument[0].path = '/StatusId';

    this.httpService
      .patch(CompanyManagementApiEnum.Companies + `/${this.company.id}`, this.patchPayload)
      .subscribe(() => {
        this.updateCompanies.emit(true);
      });
  }
}
