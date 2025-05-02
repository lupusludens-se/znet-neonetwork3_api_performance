import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Form, FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { untilDestroyed } from '@ngneat/until-destroy';
import { SkillsByCategoryInterface } from 'src/app/shared/interfaces/user/skills-by-category.interface';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { SkillsInterface } from 'src/app/shared/interfaces/user/Skills.interface';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { CommonService } from 'src/app/core/services/common.service';
import { Subject, takeUntil } from 'rxjs';
import { InitialDataInterface } from 'src/app/core/interfaces/initial-data.interface';
import { UserProfileApiEnum } from 'src/app/shared/enums/api/user-profile-api.enum';
import { GeneralApiEnum } from 'src/app/shared/enums/api/general-api.enum';
import { UserManagementApiEnum } from 'src/app/user-management/enums/user-management-api.enum';
import { HttpService } from '../../../core/services/http.service';
import { UpdateUserprofileInterface } from 'src/app/+edit-profile/interfaces/update-userprofile.interface';
import { UserProfileInterface } from '../../interfaces/user-profile.interface';
import { Router } from '@angular/router';
import { SnackbarService } from '../../../core/services/snackbar.service';
@Component({
  selector: 'neo-skills-modal',
  templateUrl: './skills-modal.component.html',
  styleUrls: ['./skills-modal.component.scss']
})
export class SkillsModalComponent implements OnInit {
  @Output() closeModal: EventEmitter<void> = new EventEmitter<void>();
  @Output() saveChanges: EventEmitter<void> = new EventEmitter<void>();
  apiRoutes = { ...UserManagementApiEnum, ...GeneralApiEnum, ...UserProfileApiEnum };
  addSkillForm: FormGroup;
  categories: TagInterface[] = [];
  skills: SkillsByCategoryInterface[] = [];
  showSkillsError: boolean;
  isSPRole: boolean;
  isCorporateRole: boolean;
  filteredSkills: SkillsInterface[] = [];
  maxSkills: number = 5;
  setDuplicateSkillsError = false;
  form: FormGroup = this.formBuilder.group({ skillsByCategory: this.formBuilder.array([]) });
  userRoles = RolesEnum;
  initialData: InitialDataInterface;
  private unsubscribe$: Subject<void> = new Subject<void>();
  userProfile: UserProfileInterface;
  isSkillRequired: boolean = false;
  isSkillandCatetegoryRequired: boolean = false;
  constructor(private formBuilder: FormBuilder, private authService: AuthService, private commonService: CommonService, private httpService: HttpService, private router: Router, private snackbarService: SnackbarService) { }

  get skillsByCategory(): FormArray {
    return this.form.get('skillsByCategory') as FormArray;
  }

  ngOnInit(): void {
    this.authService
      .currentUser()
      .subscribe(currentUser => {
        if (currentUser) {
          this.isCorporateRole = currentUser.roles.some(r => r.id === this.userRoles.Corporation);
          this.isSPRole = currentUser.roles.some(r => r.id === this.userRoles.SolutionProvider || r.id === this.userRoles.SPAdmin);
          this.initializeSkills(currentUser.userProfile.skillsByCategory);
          this.userProfile = currentUser.userProfile;
        }
        if (this.isCorporateRole) {
          this.commonService
            .initialData()
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe(initialData => (this.initialData = { ...initialData }));
          this.categories = this.initialData.categories;
        }
        this.skillsByCategory.valueChanges.subscribe(() => {
          this.validateDuplicates();
        });
      });

    this.authService.getSkillsByCategories().subscribe((data: SkillsByCategoryInterface[]) => {
      if (data)
        this.populateCategoryandSkills(data);
    })

  }

  addSkill(): void {
    const skillGroup = this.formBuilder.group({
      projectTypeId: [''],
      skillId: ['']
    });
    this.skillsByCategory.push(skillGroup);
  }
  private initializeSkills(skillsByCategory: any[]) {
    const skillsArray = this.form.get('skillsByCategory') as FormArray;
    skillsByCategory.forEach(skill => {
      skillsArray.push(this.formBuilder.group({
        projectTypeId: [{ id: skill.categoryId, name: skill.categoryName }],
        skillId: [{ id: skill.skillId, name: skill.skillName }]
      }));
    });
  }
  private populateCategoryandSkills(data: SkillsByCategoryInterface[]) {
    if (this.isSPRole) {
      const uniqueCategories = new Map();
      data.forEach(item => {
        uniqueCategories.set(item.categoryId, { id: item.categoryId, name: item.categoryName });
      });
      this.skills = data;
      this.categories = Array.from(uniqueCategories.values());
    }
    else {
      this.skills = data;
      this.filteredSkills = this.skills.map(skill => ({
        id: skill.skillId,
        name: skill.skillName
      }));

    }
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
      this.filteredSkills = this.skills.filter(skill => skill.categoryId === projectType.id).map(skill => ({
        id: skill.skillId,
        name: skill.skillName
      }));
    }
  }
  removeSkill(index: number): void {
    const currentDropdown = this.form.get('skillsByCategory') as FormArray;
    currentDropdown.removeAt(index);
    this.checkForMissingSkills();
    this.checkForMissingSkillsAndCategories();
  }
  onSave() {
    this.checkForMissingSkills();
    this.checkForMissingSkillsAndCategories();
    if (this.setDuplicateSkillsError || this.isSkillRequired || this.form.invalid || this.isSkillandCatetegoryRequired) return;
    const skillsData: SkillsByCategoryInterface[] = this.skillsByCategory.value.map(skill => ({
      skillId: skill.skillId.id,
      skillName: skill.skillId.name,
      categoryId: skill.projectTypeId.id,
      categoryName: skill.projectTypeId.name
    }));

    const userProfilePayload: UpdateUserprofileInterface = {
      ...this.userProfile,
      ...this.form.getRawValue(),
      skillsByCategory: skillsData
    };
    this.httpService.put(`${this.apiRoutes.UserProfilesCurrent}`, userProfilePayload).subscribe(
      (res) => {
        if (res) {
          this.saveChanges.emit();
        }
      })
  }
  cancel() {
    this.closeModal.emit();
  }
  validateDuplicates() {
    const skillValues = this.skillsByCategory.controls.map(control => ({
      projectTypeId: control.get('projectTypeId')?.value,
      skillId: control.get('skillId')?.value,
    }));
    const duplicates = skillValues.filter((value, index, self) => {
      if (!value.projectTypeId.id || !value.skillId.id) {
        return false;
      }
      return index !== self.findIndex((v) => v.projectTypeId.id === value.projectTypeId.id && v.skillId.id === value.skillId.id)
    }
    );

    this.setDuplicateSkillsError = duplicates.length > 0;
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
 onProjectTypeChange(skillsGroup:FormGroup){
  skillsGroup.get('skillId').setValue('');
  skillsGroup.get('skillId').enable();
 }
}
