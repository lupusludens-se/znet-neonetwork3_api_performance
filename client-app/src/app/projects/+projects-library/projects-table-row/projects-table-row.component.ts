import { OnInit, Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';

import { ProjectCatalogService } from '../../+projects/services/project-catalog.service';

import { ProjectInterface } from '../../../shared/interfaces/projects/project.interface';
import { MENU_OPTIONS, MenuOptionInterface } from '../../../shared/modules/menu/interfaces/menu-option.interface';
import { UserStatusEnum } from '../../../user-management/enums/user-status.enum';
import { TableCrudEnum } from '../../../shared/modules/table/enums/table-crud.enum';
import { ProjectStatusEnum } from '../../../shared/enums/projects/project-status.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ProjectSchema } from '../../shared/constants/project-schema-const';

@Component({
  selector: 'neo-projects-table-row',
  templateUrl: './projects-table-row.component.html',
  styleUrls: ['../projects-library.component.scss', './projects-table-row.component.scss']
})
export class ProjectsTableRowComponent implements OnInit {
  @Input() project: ProjectInterface;
  @Input() showCompany: boolean;
  @Input() currentUserId: number;

  @Output() updateProjectsList: EventEmitter<boolean> = new EventEmitter<boolean>();
  roles = RolesEnum;
  projectStatuses = ProjectStatusEnum;
  showOptions: boolean;
  showDeleteModal: boolean;
  showDuplicateModal: boolean;
  userStatuses = UserStatusEnum;
  menuOptions: MenuOptionInterface[] = MENU_OPTIONS;
  options: MenuOptionInterface[] = [];
  projectFormGroup: FormGroup = new FormGroup({
    text: new FormControl(null, [Validators.required, Validators.minLength(1), Validators.pattern(/^(\s+\S+\s*)*(?!\s).*$/)])
  });

  titleMaxLength = ProjectSchema.titleMaxLength;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private projectsService: ProjectCatalogService,
    private authService: AuthService
  ) { }

  ngOnInit() {

    this.options = JSON.parse(JSON.stringify(this.getOptions()));
  }

  generateLocationString(): string {
    let str = '';
    this.project.regions.forEach((r, index) => {
      if (index !== this.project.regions.length - 1) {
        str += `${r.name}, `;
      } else str += r.name;
    });

    return str;
  }

  goToEditProject(): void {
    this.router.navigate([`projects-library/edit-project/${this.project.id}`]).then();
  }

  goToProject() {
    this.router.navigate([`projects/${this.project.id}`]).then();
  }

  deleteProject(): void {
    this.projectsService.changeProjectStatus(this.project.id, this.projectStatuses.Deleted).subscribe(() => {
      this.showDeleteModal = false;
      this.updateProjectsList.emit(true);
    });
  }

  duplicateProject(): void {
    if (this.projectFormGroup.valid) {
      this.projectsService.duplicateProject(this.project.id, this.projectFormGroup.controls["text"]?.value?.trim()).subscribe(() => {
        this.showDuplicateModal = false;
        this.updateProjectsList.emit(true);
      });
    }
  }

  changeProjectStatus(statusVal: number): void {
    this.projectsService.changeProjectStatus(this.project.id, statusVal).subscribe(() => {
      this.updateProjectsList.emit(true);
    });
  }

  optionClick(option: MenuOptionInterface): void {
    switch (option.operation) {
      case TableCrudEnum.Edit:
        this.goToEditProject();
        break;
      case TableCrudEnum.Preview:
        this.goToProject();
        break;
      case TableCrudEnum.Status:
        this.changeProjectStatus(
          this.project.statusId === ProjectStatusEnum.Active ? ProjectStatusEnum.Inactive : ProjectStatusEnum.Active
        );
        break;
      case TableCrudEnum.Delete:
        this.showDeleteModal = true;
        break;
      case TableCrudEnum.Duplicate:
        this.showDuplicateModal = true;
        let _title = `Copy_${this.project.title}`;
        _title = _title.length <= 100 ? _title : _title.substring(0, 100);
        this.projectFormGroup.controls["text"]?.setValue(_title);
        break;
    }
  }

  getOptions(): MenuOptionInterface[] {
    const currentUser: UserInterface = this.authService.currentUser$.getValue();
    this.menuOptions = this.menuOptions.map(option => {
      if (this.project?.statusId === ProjectStatusEnum.Active) {
        this.menuOptions.map(opt => {
          if (opt.name === 'actions.deactivateLabel') opt.hidden = false;
          if (opt.name === 'actions.previewLabel') opt.hidden = false;
          if (opt.name === 'actions.activateLabel') opt.hidden = true;
        });
      } else if (this.project?.statusId === ProjectStatusEnum.Inactive) {
        this.menuOptions.map(opt => {
          if (opt.name === 'actions.deactivateLabel') opt.hidden = true;
          if (opt.name === 'actions.previewLabel') opt.hidden = false;
          if (opt.name === 'actions.activateLabel' && this.project.owner.statusId !== UserStatusEnum.Deleted)
            opt.hidden = false;
        });
      } else {
        this.menuOptions.map(opt => {
          if (opt.name === 'actions.previewLabel') opt.hidden = false;
          if (opt.name === 'actions.deactivateLabel') opt.hidden = true;
          if (opt.name === 'actions.activateLabel') opt.hidden = true;
        });
      }

      if (option.operation === TableCrudEnum.Delete) {
        option.hidden = this.project.statusId === ProjectStatusEnum.Deleted;
      }

      return option;
    });

    const preview: MenuOptionInterface = {
      icon: 'doc-magnify',
      name: 'actions.previewLabel',
      hidden: false,
      operation: TableCrudEnum.Preview,
      iconSize: '18px'
    };

    if (!this.menuOptions.some(option => option.operation === TableCrudEnum.Duplicate)) {
      const duplicate: MenuOptionInterface = {
        icon: 'copy',
        name: 'actions.duplicateLabel',
        operation: TableCrudEnum.Duplicate
      };

      this.menuOptions.splice(3, 0, duplicate);
    }
		if (
			currentUser?.id != this.project.ownerId &&
			(currentUser?.roles?.map(r => r.id).includes(this.roles.SolutionProvider))
		) {
      const duplicate: MenuOptionInterface = {
        icon: 'copy',
        name: 'actions.duplicateLabel',
        operation: TableCrudEnum.Duplicate
      };

      this.menuOptions.splice(0, this.menuOptions.length, duplicate);
      this.menuOptions.unshift(preview);
    }
    return this.menuOptions;
  }

  hasError(controlName: string, formGroup: string): boolean {
    const control = this[formGroup]?.get(controlName);

    return control?.invalid && control?.touched && control?.dirty;
  }
}
