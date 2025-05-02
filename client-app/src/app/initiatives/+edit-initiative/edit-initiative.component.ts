import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { Subject, takeUntil, switchMap, throwError, filter, combineLatest, catchError } from "rxjs";
import { InitialDataInterface } from "src/app/core/interfaces/initial-data.interface";
import { TagInterface } from "src/app/core/interfaces/tag.interface";
import { CommonService } from "src/app/core/services/common.service";
import { SnackbarService } from "src/app/core/services/snackbar.service";
import { TitleService } from "src/app/core/services/title.service";
import { CountryInterface } from "src/app/shared/interfaces/user/country.interface";
import { CustomValidator } from "src/app/shared/validators/custom.validator";
import { BaseInitiativeInterface } from "../+initatives/+view-initiative/interfaces/base-initiative.interface";
import { RegionScaleTypeEnum } from "../shared/enums/geographic-scale-type.enum";
import { EditInitiativeService } from "./services/edit-initiative.service";
import structuredClone from '@ungap/structured-clone';
import { InitiativeSharedService } from "../shared/services/initiative-shared.service";
import { UserCollaboratorInterface, UserInterface } from "src/app/shared/interfaces/user/user.interface";
import { AuthService } from "src/app/core/services/auth.service";
import { RolesEnum } from "src/app/shared/enums/roles.enum";

@Component({
  selector: 'neo-edit-initiative',
  templateUrl: './edit-initiative.component.html',
  styleUrls: ['./edit-initiative.component.scss']
})
export class EditInitiativeComponent implements OnInit, OnDestroy {
  initiativeForm: FormGroup;
  formSubmitted = false;
  initiativeRegions: TagInterface[] = [];
  initiativeScalesList: TagInterface[];
  showModal = false;
  projectTypesList: TagInterface[];
  initiativeId: number;
  maxInitiativeTitleLength: number = 100;
  continents: CountryInterface[];
  owner: UserCollaboratorInterface[];
  initialData: InitialDataInterface;
  initiativeScaleCustomTitle: string;
  unsubscribe$ = new Subject<void>();
  userList: UserCollaboratorInterface[];
  initiativeCollaborators: UserCollaboratorInterface[] = [];
  currentUser: UserInterface;

  constructor(
    private formBuilder: FormBuilder,
    private editInitiativeService: EditInitiativeService,
    private router: Router,
    private snackbarService: SnackbarService,
    private activatedRoute: ActivatedRoute,
    private commonService: CommonService,
    private titleService: TitleService,
    private initiativeSharedService: InitiativeSharedService,
    private readonly authService: AuthService
  ) {
    this.initiativeForm = this.formBuilder.group({
      projectType: ['', Validators.required],
      scale: ['', Validators.required],
      name: ['', [Validators.required, Validators.maxLength(100), CustomValidator.noWhitespaceValidator]],
      regions: [[], Validators.required],
      collaborators: []
    });
  }

  ngOnInit(): void {
    this.titleService.setTitle('initiative.editInitiative.editInitiativeLabel');
    this.authService.currentUser().subscribe(user => (this.currentUser = user));
    this.activatedRoute.params
      .pipe(
        takeUntil(this.unsubscribe$),
        switchMap(params => {
          if (params['id'] && params['id'] != 0) {
            this.initiativeId = params['id'];
            return this.commonService.initialData();
          } else {
            this.goToDashboard();
            return throwError('Invalid initiative ID');
          }
        }),
        filter(response => !!response),
        switchMap(initialData => {
          this.initialData = initialData;
          this.continents = structuredClone(initialData.regions.filter(r => r.parentId === 0));
          this.projectTypesList = structuredClone(initialData.categories).filter(item => item['solutionId'] !== null);
          return combineLatest([
            this.commonService.getRegionScaleTypes(),
            this.editInitiativeService.getInitiativeDetailsByInitiativeId(this.initiativeId),
            this.initiativeSharedService.getCompanyUsers()
          ]);
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 404) {
            this.goToDashboard();
          }
          return throwError(error);
        })
      )
      .subscribe({
        next: ([scaleTypes, initiativeDetails, users]) => {
          this.initiativeScalesList = scaleTypes;
          this.fetchInitiativeDetails(initiativeDetails);
          this.userList = users.dataList.filter(
            user => user.id !== this.currentUser.id && user.roles.some(role => role.id === RolesEnum.Corporation));
          this.userList.forEach(user => {
            user.name = `${user.firstName} ${user.lastName}`;
            user.selected =
              initiativeDetails.collaborators.filter(collaborator => collaborator.id === user.id).length > 0;
          });
        },
        error: (error) => {
          this.snackbarService.showError('general.defaultErrorLabel');
        }
      });
  }

  fetchInitiativeDetails(data: BaseInitiativeInterface): void {
    this.initiativeForm.patchValue({
      name: data.title,
      scale: this.initiativeScalesList.find(x => x.id == data.scaleId),
      projectType: data.category,
      regions: data.regions,
      collaborators: data.collaborators
    });
    this.initiativeRegions = data.regions;
    this.initiativeCollaborators = data.collaborators;
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

    const formData = {
      Title: this.initiativeForm.get('name').value,
      ScaleId: this.initiativeForm.get('scale').value.id,
      RegionIds: this.initiativeRegions.map(region => region.id),
      CollaboratorIds: this.initiativeCollaborators.map(collaborator => collaborator.id)
    };

    this.editInitiativeService.updateInitiative(formData, this.initiativeId)
      .pipe(
        catchError(error => {
          this.snackbarService.showError(error.status === 400 ? error.error : 'general.defaultErrorLabel');
          return throwError(error);
        })
      )
      .subscribe(data => {
        if (data?.id > 0) {
          this.snackbarService.showSuccess('initiative.editInitiative.successMessageLabel');
          this.router.navigate(['/decarbonization-initiatives', this.initiativeId]);
        } else {
          this.snackbarService.showError('general.defaultErrorLabel');
        }
      });
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
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
