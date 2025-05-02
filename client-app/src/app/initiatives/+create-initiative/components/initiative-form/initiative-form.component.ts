import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { CreateInitiativeService } from '../../services/create-initiative.service';
import { Router } from '@angular/router';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { catchError, filter, forkJoin, Subject, takeUntil, throwError } from 'rxjs';
import { CountryInterface } from 'src/app/shared/interfaces/user/country.interface';
import { CommonService } from 'src/app/core/services/common.service';
import { InitialDataInterface } from 'src/app/core/interfaces/initial-data.interface';
import structuredClone from '@ungap/structured-clone';
import { ViewportScroller } from '@angular/common';
import { RegionScaleTypeEnum } from 'src/app/initiatives/shared/enums/geographic-scale-type.enum';
import { InitiativeSharedService } from 'src/app/initiatives/shared/services/initiative-shared.service';
import { UserCollaboratorInterface, UserInterface } from 'src/app/shared/interfaces/user/user.interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { CustomValidator } from 'src/app/shared/validators/custom.validator';

@Component({
  selector: 'neo-initiative-form',
  templateUrl: './initiative-form.component.html',
  styleUrls: ['./initiative-form.component.scss']
})
export class InitiativeFormComponent implements OnInit {
  @Output() initiativeResult = new EventEmitter<number>();

  initiativeForm: FormGroup;
  formSubmitted = false;
  initiativeRegions: TagInterface[] = [];
  initiativeCollaborators: UserCollaboratorInterface[] = [];
  projectTypesList: TagInterface[];
  initiativeScalesList: TagInterface[];
  maxInitiativeTitleLength = 100;
  initiativeScaleCustomTitle: string;
  showModal = false;
  initiativeId: number;
  scaleId: number;
  continents: CountryInterface[];
  initialData: InitialDataInterface;
  userList: UserCollaboratorInterface[];
  currentUser: UserInterface;
  private unsubscribe$ = new Subject<void>();

  constructor(
    private formBuilder: FormBuilder,
    private createInitiativeService: CreateInitiativeService,
    private router: Router,
    private snackbarService: SnackbarService,
    private commonService: CommonService,
    private initiativeSharedService: InitiativeSharedService,
    private viewPort: ViewportScroller,
    private readonly authService: AuthService
  ) {
    this.initiativeForm = this.formBuilder.group({
      projectType: ['', Validators.required],
      scale: ['', Validators.required],
      name: ['', [CustomValidator.required, Validators.maxLength(100)]],
      regions: [[], Validators.required],
      collaborators: []
    });
  }

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      if (user != null) {
        this.currentUser = user;
        this.loadData();
        this.commonService
          .initialData()
          .pipe(
            takeUntil(this.unsubscribe$),
            filter(response => !!response)
          )
          .subscribe(initialData => {
            this.initialData = initialData;
            this.continents = structuredClone(initialData.regions.filter(r => r.parentId === 0));
            this.projectTypesList = structuredClone(initialData.categories).filter(item => item['solutionId'] !== null);
          });
      }
    });
  }

  loadData(): void {
    forkJoin({
      scaleData: this.commonService.getRegionScaleTypes(),
      userData: this.initiativeSharedService.getCompanyUsers()
    }).subscribe({
      next: (data: any) => {
        this.initiativeScalesList = data.scaleData;
        this.userList = data.userData.dataList.filter(
          user => user.id !== this.currentUser.id && user.roles.some(role => role.id === RolesEnum.Corporation)
        );
      }
    });
  }

  updateRegions(regions: TagInterface[]): void {
    this.initiativeForm.get('regions').setValue(regions);
    this.initiativeRegions = regions;
  }

  updateCollaborator(users: UserCollaboratorInterface[]): void {
    this.initiativeForm.get('collaborators').setValue(users);
    this.initiativeCollaborators = users;
  }

  save(): void {
    this.formSubmitted = true;
    if (this.initiativeForm.invalid) return;
    if (this.initiativeForm.valid) {
      const formData = {
        Title: this.initiativeForm.get('name').value,
        ScaleId: this.initiativeForm.get('scale').value.id,
        ProjectTypeId: this.initiativeForm.get('projectType').value.id,
        RegionIds: this.initiativeRegions.map(region => region.id),
        CollaboratorIds: this.initiativeCollaborators.map(collaborator => collaborator.id)
      };
      this.createInitiativeService
        .saveInitiative(formData)
        .pipe(
          catchError(error => {
            error.status === 400
              ? this.snackbarService.showError(error.error)
              : this.snackbarService.showError('general.defaultErrorLabel');
            return;
            return throwError(error);
          })
        )
        .subscribe(data => {
          this.initiativeResult.emit(data);
        });
    }
  }

  cancel(): void {
    this.showModal = true;
  }

  goToDashboard(): void {
    this.showModal = false;
    this.router.navigate(['/dashboard']);
  }

  close(): void {
    this.showModal = false;
  }

  onScaleChange(event: TagInterface): void {
    this.initiativeScaleCustomTitle =
      event.id === RegionScaleTypeEnum.State
        ? 'initiative.createInitiative.formContent.chooseStateLeabel'
        : event.id === RegionScaleTypeEnum.National
          ? 'initiative.createInitiative.formContent.selectRegionCountryLabel'
          : 'initiative.createInitiative.formContent.selectRegionLabel';
    setTimeout(() => this.viewPort.scrollToAnchor('regionsListControl'));
  }
}
