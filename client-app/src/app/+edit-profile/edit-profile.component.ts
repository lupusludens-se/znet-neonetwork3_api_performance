import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserProfileInterface } from '../+user-profile/interfaces/user-profile.interface';
import { UpdateUserprofileInterface } from './interfaces/update-userprofile.interface';
import { UserProfileApiEnum } from '../shared/enums/api/user-profile-api.enum';
import { CountryInterface } from '../shared/interfaces/user/country.interface';
import { ImageUploadService } from '../shared/services/image-upload.service';
import { UserInterface } from '../shared/interfaces/user/user.interface';
import { UpdateUserInterface } from './interfaces/update-user.interface';
import { TagInterface } from '../core/interfaces/tag.interface';
import { TitleService } from '../core/services/title.service';
import { AuthService } from '../core/services/auth.service';
import { HttpService } from '../core/services/http.service';
import { BlobTypeEnum, GeneralApiEnum } from '../shared/enums/api/general-api.enum';
import { SnackbarService } from '../core/services/snackbar.service';
import { MAX_IMAGE_SIZE } from '../shared/constants/image-size.const';
import { RolesEnum } from '../shared/enums/roles.enum';
import { USER_RESPONSIBILITIES } from '../user-management/constants/responsibility.const';
import { UserPermissionInterface } from '../shared/interfaces/user/user-permission.interface';
import { CustomValidator } from '../shared/validators/custom.validator';
import { UserRoleInterface } from '../shared/interfaces/user/user-role.interface';
import { filter, map, takeUntil } from 'rxjs/operators';
import { combineLatest, Subject } from 'rxjs';
import { UserManagementApiEnum } from '../user-management/enums/user-management-api.enum';
import { ImageInterface } from '../shared/interfaces/image.interface';
import { CommonService } from '../core/services/common.service';
import { InitialDataInterface } from '../core/interfaces/initial-data.interface';

import { SkillsByCategoryInterface } from '../shared/interfaces/user/skills-by-category.interface';
import { SkillsInterface } from '../shared/interfaces/user/Skills.interface';

@UntilDestroy()
@Component({
  selector: 'neo-edit-profile',
  templateUrl: 'edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss']
})
export class EditProfileComponent implements OnInit {
  countriesList: CountryInterface[];
  statesList: TagInterface[];
  apiRoutes = { ...UserManagementApiEnum, ...GeneralApiEnum, ...UserProfileApiEnum };
  user: UserInterface;
  userProfile: UserProfileInterface;
  blobType = BlobTypeEnum;
  showStatesControl: boolean;
  maxImageSize = MAX_IMAGE_SIZE;
  responsibilityList = USER_RESPONSIBILITIES;
  permissionsData: UserPermissionInterface[];
  formSubmitted: boolean;
  categories: TagInterface[] = [];
  skills: SkillsByCategoryInterface[] = [];
  showSkillsError: boolean;
  isSPRole: boolean;
  filteredSkills: SkillsInterface[] = [];
  maxSkills: number = 5;
  setDuplicateSkillsError = false;
  form: FormGroup = this.formBuilder.group({
    firstName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(64), CustomValidator.userName]],
    lastName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(64), CustomValidator.userName]],
    jobTitle: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(250)]],
    company: [{ value: '', disabled: true }],
    email: [{ value: '', disabled: true }],
    countryId: ['', Validators.required],
    stateId: [''],
    linkedInUrl: ['', [CustomValidator.linkedInUrl, Validators.maxLength(2048)]],
    about: [''],
    responsibilityId: [''],
    urlLinks: this.formBuilder.array([]),
    skillsByCategory: this.formBuilder.array([])
  });
  imageFormData: File;
  linksError: boolean;
  userRoles = RolesEnum;
  corporationRole: boolean;
  userIsAdmin: boolean;
  initialData: InitialDataInterface;
  private unsubscribe$: Subject<void> = new Subject<void>();
  isSkillRequired: boolean = false;
  isSkillandCatetegoryRequired: boolean = false;
  constructor(
    public authService: AuthService,
    private formBuilder: FormBuilder,
    private httpService: HttpService,
    private router: Router,
    private titleService: TitleService,
    private imageUploadService: ImageUploadService,
    private snackbarService: SnackbarService,
    private commonService: CommonService
  ) {}

  get urlLinks(): FormArray {
    return this.form.get('urlLinks') as FormArray;
  }
  get skillsByCategory(): FormArray {
    return this.form.get('skillsByCategory') as FormArray;
  }

  ngOnInit(): void {
    this.authService
      .currentUser()
      .pipe(untilDestroyed(this))
      .subscribe(currentUser => {
        // !! check condition
        if (currentUser) {
          this.titleService.setTitle(
            'userProfile.edit.editProfileLabel',
            `${currentUser.firstName} ${currentUser.lastName}`
          );
          this.corporationRole = currentUser.roles.some(r => r.id === this.userRoles.Corporation);
          this.isSPRole = currentUser.roles.some(
            r => r.id === this.userRoles.SolutionProvider || r.id === this.userRoles.SPAdmin
          );
          this.form.patchValue({
            firstName: currentUser.firstName,
            lastName: currentUser.lastName,
            jobTitle: currentUser.userProfile.jobTitle,
            company: currentUser.company.name,
            email: currentUser.email,
            countryId: currentUser.country.id,
            stateId: currentUser.userProfile.state?.id,
            linkedInUrl: currentUser.userProfile.linkedInUrl,
            about: currentUser.userProfile.about,
            responsibilityId: {
              id: currentUser.userProfile.responsibilityId,
              name: currentUser.userProfile.responsibilityName
            }
          });
          this.initializeSkills(currentUser.userProfile.skillsByCategory);
          currentUser.userProfile.urlLinks.forEach(link => {
            this.addLink(link.urlName, link.urlLink);
          });
          if (this.isSPRole || this.corporationRole)
            this.authService.getSkillsByCategories().subscribe((data: SkillsByCategoryInterface[]) => {
              if (data) this.populateCategoryandSkills(data);
            });

          if (this.corporationRole) {
            this.commonService
              .initialData()
              .pipe(takeUntil(this.unsubscribe$))
              .subscribe(initialData => (this.initialData = { ...initialData }));
            this.categories = this.initialData.categories;
          }
          this.skillsByCategory.valueChanges.subscribe(() => {
            this.validateDuplicates();
          });
          this.userProfile = currentUser.userProfile;
          this.user = { ...currentUser };
          this.chooseCountry(currentUser.country);

          if (this.user.roles.some(r => r.id === this.userRoles.Admin)) {
            this.userIsAdmin = true;
            this.generatePermissionsData();
          }
        }
      });
  }

  generatePermissionsData(): void {
    const roles$ = this.httpService
      .get<UserRoleInterface[]>(this.apiRoutes.UserRoles, {
        expand: 'permissions'
      })
      .pipe(
        map(roles => {
          return roles.filter(r => r.name !== 'All');
        })
      );

    const permissionsObs$ = this.httpService.get<UserPermissionInterface[]>(this.apiRoutes.Permissions);

    combineLatest([roles$, permissionsObs$]).subscribe(res => {
      this.permissionsData = res[1] as UserPermissionInterface[];
      this.permissionsData.forEach((pd: UserPermissionInterface) => {
        pd['checked'] = false;

        this.user.permissions?.forEach(up => {
          if (up.id === pd.id) pd['checked'] = true;
        });

        this.user.roles.forEach((ur: UserRoleInterface) => {
          ur.permissions.forEach((r: UserPermissionInterface) => {
            if (r.id === pd.id) {
              pd['checked'] = true;
            }
          });
        });
      });
    });
  }

  onFileSelect(event): void {
    const supported: boolean =
      event.target.files[0].type.includes('png') ||
      event.target.files[0].type.includes('jpeg') ||
      event.target.files[0].type.includes('jpg');

    if (!supported) {
      this.snackbarService.showError('general.wrongFileTypeLabel');
      return;
    }

    const isLarge = event.target.files[0].size > this.maxImageSize;
    if (isLarge) {
      this.snackbarService.showError('general.wrongMediumFileSizeLabel');
      return;
    }

    if (event.target.files.length > 0) {
      this.imageUploadService.renderPreview(event).then(r => {
        this.removeImg();
        if (!this.user.image) {
          this.user.image = { uri: '', name: '', blobType: null };
        }
        this.imageFormData = event.target.files[0];
        this.user.image.uri = r;
        this.user.image.name = null; // * update name as indicator that image was updated
      });
    }
  }

  addLink(name: string = '', url: string = ''): void {
    const linkForm = this.formBuilder.group({
      urlLink: [url, [CustomValidator.url, Validators.maxLength(2048)]],
      urlName: [name, Validators.maxLength(250)]
    });

    this.urlLinks.push(linkForm);
  }

  removeLink(index: number): void {
    const currentLinks = this.form.get('urlLinks') as FormArray;
    currentLinks.removeAt(index);
  }

  saveData(): void {
    this.checkForMissingSkills();
    this.checkForMissingSkillsAndCategories();
    this.linksError = false;
    this.formSubmitted = true;
    this.showLinksError();
    if (
      this.form.invalid ||
      this.linksError ||
      this.setDuplicateSkillsError ||
      this.isSkillandCatetegoryRequired ||
      this.isSkillRequired
    )
      return;
    const skillsData: SkillsByCategoryInterface[] = this.skillsByCategory.value.map(skill => ({
      skillId: skill.skillId.id,
      skillName: skill.skillId.name,
      categoryId: skill.projectTypeId.id,
      categoryName: skill.projectTypeId.name
    }));

    if (this.form.invalid || this.linksError) return;

    const userPayload: UpdateUserInterface = {
      ...this.user,
      ...this.form.getRawValue(),
      timeZone: Intl.DateTimeFormat().resolvedOptions().timeZoneName
    };

    const userProfilePayload: UpdateUserprofileInterface = {
      ...this.userProfile,
      ...this.form.getRawValue(),
      responsibilityId: this.form.getRawValue().responsibilityId.id,
      skillsByCategory: skillsData
    };

    if (
      (this.imageFormData && this.user.image?.name !== this.user?.imageName) ||
      (!this.user.imageName && this.imageFormData)
    ) {
      const formData: FormData = new FormData();
      formData.append('file', this.imageFormData);

      this.imageUploadService.uploadImage(this.blobType.Users, formData).subscribe((image: ImageInterface) => {
        this.user.imageName = image.name;

        if (this.user.image) {
          this.user.image.uri = image.uri;
        } else {
          this.user.image = {
            blobType: this.blobType.Users,
            name: image.name,
            uri: image.uri
          };
        }
        /**/
        this.updateUserRequest(userProfilePayload, userPayload);
      });
    } else {
      this.updateUserRequest(userProfilePayload, userPayload);
    }
  }

  showLinksError(): void {
    if (
      this.form.get('urlLinks').value.length > 1 &&
      this.form.get('urlLinks').value.some(r => !r.urlName || !r.urlLink)
    ) {
      this.linksError = true;
    }
  }

  updateUserRequest(userProfilePayload: UpdateUserprofileInterface, userPayload: UpdateUserInterface): void {
    this.httpService.put(`${this.apiRoutes.UserProfilesCurrent}`, userProfilePayload).subscribe(
      () => {
        if (
          this.user.countryId !== userPayload.countryId ||
          this.user.firstName !== userPayload.firstName ||
          this.user.lastName !== userPayload.lastName ||
          this.imageFormData || // * indication that image was uploaded
          this.user.image === null
        ) {
          this.httpService
            .put(`${this.apiRoutes.Users}/${this.user.id}`, {
              ...userPayload,
              imageName: this.user.imageName
            })
            .subscribe(
              () => {
                this.handleUpdateSuccessResp();
              },
              error => this.handleErrorResponse(error)
            );
        } else {
          this.handleUpdateSuccessResp();
        }
      },
      error => this.handleErrorResponse(error)
    );
  }

  handleErrorResponse(error): void {
    const errorsKeys = Object.keys(error.error.errors);
    this.snackbarService.showError(error.error.errors[errorsKeys[0]][0]);
  }

  handleUpdateSuccessResp(): void {
    this.router.navigate([`user-profile/${this.userProfile.userId}`]);
    this.authService.getCurrentUser$.next(true);
    this.snackbarService.showSuccess('userProfile.edit.profileUpdatedLabel');
  }

  removeImg(): void {
    this.user.image = null;
    this.user.imageName = null;
    this.imageFormData = null;
  }

  goBack(): void {
    this.router.navigate([`./user-profile/${this.user.id}`]);
  }

  getCountries(search?: string): void {
    this.httpService
      .get<CountryInterface[]>('countries', {
        search
      })
      .subscribe(c => {
        this.countriesList = [...c];
        this.form.get('countryId').setValue('');
      });
  }

  getStates(search?: string): void {
    this.httpService
      .get<TagInterface[]>('states', {
        filterBy: `countryId=${this.form.get('countryId').value}`,
        search: search
      })
      .subscribe(states => {
        this.statesList = [...states];
        this.form.get('stateId').setValue('');
      });
  }

  chooseCountry(country: CountryInterface): void {
    if (!country) {
      this.form.patchValue({ state: '' });
      this.form.patchValue({ countryId: null });
      this.showStatesControl = false;
      this.statesList = [];
      return;
    }

    if (country.code.toLowerCase() === 'us') {
      this.showStatesControl = true;

      this.form.get('stateId').setValidators(Validators.required);
      this.form.get('stateId').updateValueAndValidity();
    } else {
      this.showStatesControl = false;
      this.statesList = [];

      if (this.user?.userProfile?.state) {
        this.user.userProfile.state = null;
        this.form.get('stateId').setValue('');
      }

      this.form.get('stateId').clearValidators();
      this.form.get('stateId').updateValueAndValidity();
    }

    this.form.get('countryId').setValue(country.id);
  }

  selectState(state: TagInterface): void {
    !state ? this.form.get('stateId').setValue('') : this.form.get('stateId').setValue(state.id);
  }
  addSkill(): void {
    const skillGroup = this.formBuilder.group({
      projectTypeId: [''],
      skillId: ['']
    });

    if (this.skillsByCategory.length < this.maxSkills) this.skillsByCategory.push(skillGroup);
  }
  removeSkill(index: number): void {
    const currentDropdown = this.form.get('skillsByCategory') as FormArray;
    currentDropdown.removeAt(index);
    this.checkForMissingSkills();
    this.checkForMissingSkillsAndCategories();
  }

  private populateCategoryandSkills(data: SkillsByCategoryInterface[]) {
    if (this.isSPRole) {
      const uniqueCategories = new Map();
      data.forEach(item => {
        uniqueCategories.set(item.categoryId, { id: item.categoryId, name: item.categoryName });
      });
      this.skills = data;
      this.categories = Array.from(uniqueCategories.values());
    } else {
      this.skills = data;
      this.filteredSkills = this.skills.map(skill => ({
        id: skill.skillId,
        name: skill.skillName
      }));
    }
  }

  private initializeSkills(skillsByCategory: any[]) {
    const skillsArray = this.form.get('skillsByCategory') as FormArray;
    skillsByCategory.forEach(skill => {
      skillsArray.push(
        this.formBuilder.group({
          projectTypeId: [{ id: skill.categoryId, name: skill.categoryName }],
          skillId: [{ id: skill.skillId, name: skill.skillName }]
        })
      );
    });
  }
  validateDuplicates() {
    const skillValues = this.skillsByCategory.controls.map(control => ({
      projectTypeId: control.get('projectTypeId')?.value,
      skillId: control.get('skillId')?.value
    }));
    const duplicates = skillValues.filter((value, index, self) => {
      if (!value.projectTypeId.id || !value.skillId.id) {
        return false;
      }
      return (
        index !==
        self.findIndex(v => v.projectTypeId.id === value.projectTypeId.id && v.skillId.id === value.skillId.id)
      );
    });

    this.setDuplicateSkillsError = duplicates.length > 0;
  }
  onSkillsChange(index?: number, event?: any) {
    if (this.isSkillRequired) {
      const skillGroups = this.skillsByCategory.controls;
      const hasEmptySkillId = skillGroups.some(group => {
        return group.get('projectTypeId').value && !group.get('skillId').value;
      });
      this.isSkillRequired = hasEmptySkillId;
    }
    if (this.isSkillandCatetegoryRequired) {
      const skillGroups = this.skillsByCategory.controls;
      const hasEmptySkill = skillGroups.some(group => {
        return !group.get('projectTypeId').value && !group.get('skillId').value;
      });
      this.isSkillandCatetegoryRequired = hasEmptySkill;
    }
    const projectType = this.skillsByCategory.controls[index].get('projectTypeId').value;
    if (this.isSPRole) {
      this.filteredSkills = this.skills
        .filter(skill => skill.categoryId === projectType.id)
        .map(skill => ({
          id: skill.skillId,
          name: skill.skillName
        }));
    }
  }
  checkForMissingSkills(): boolean {
    const skillGroups = this.skillsByCategory.controls;
    const hasEmptySkillId = skillGroups.some(group => {
      return group.get('projectTypeId').value && !group.get('skillId').value;
    });
    this.isSkillRequired = hasEmptySkillId;
    return this.isSkillRequired;
  }
  checkForMissingSkillsAndCategories(): boolean {
    const skillGroups = this.skillsByCategory.controls;
    const hasEmptySkill = skillGroups.some(group => {
      return !group.get('projectTypeId').value && !group.get('skillId').value;
    });
    this.isSkillandCatetegoryRequired = hasEmptySkill;
    return this.isSkillandCatetegoryRequired;
  }
  onProjectTypeChange(skillsGroup: FormGroup) {
    skillsGroup.get('skillId').setValue('');
    skillsGroup.get('skillId').enable();
  }
}
