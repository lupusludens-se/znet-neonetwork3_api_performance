import { ControlContainer, FormArray, FormBuilder, FormGroup, FormGroupDirective } from '@angular/forms';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { OnboardingStepsEnum } from '../../enums/onboarding-steps.enum';
import { OnboardingWizardService } from '../../services/onboarding-wizard.service';
import { SkillsInterface } from 'src/app/shared/interfaces/user/Skills.interface';
import { SkillsByCategoryInterface } from 'src/app/shared/interfaces/user/skills-by-category.interface';
import { TagInterface } from 'src/app/core/interfaces/tag.interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { CommonService } from 'src/app/core/services/common.service';
import { Subject, takeUntil } from 'rxjs';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';

@Component({
  selector: 'neo-onboarding-personal-info',
  templateUrl: 'onboarding-personal-info.component.html',
  styleUrls: ['../../onboarding-wizard.component.scss', 'onboarding-personal-info.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OnboardingPersonalInfoComponent implements OnInit {
  stepsList = OnboardingStepsEnum;
  categories: TagInterface[] = [];
  formSubmitted: boolean;
  maxSkills: number = 5;
  setDuplicateSkillsError = false;
  filteredSkills: SkillsInterface[] = [];
  skills: SkillsByCategoryInterface[] = [];
  @Input() skillsByCategory: FormArray;
  isUserCorporate: boolean;
  isUserSolutionProvider: boolean;
  roleIds: number[];
  userId: number;
  private unsubscribe$: Subject<void> = new Subject<void>();
  userRoles = RolesEnum;
  isSkillRequired: boolean = false;
  isSkillandCatetegoryRequired: boolean = false;

  constructor(
    public controlContainer: ControlContainer,
    private formBuilder: FormBuilder,
    public onboardingWizardService: OnboardingWizardService,
    private authService: AuthService,
    private commonService: CommonService
  ) {}

  ngOnInit(): void {
    this.authService
      .currentUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(user => {
        this.userId = user.id;
        this.roleIds = user.roles.map(r => r.id);
        this.isUserCorporate = user.roles.some(r => r.id === this.userRoles.Corporation);
        this.isUserSolutionProvider = user.roles.some(
          r => r.id === this.userRoles.SolutionProvider || r.id === this.userRoles.SPAdmin
        );
        this.authService
          .getSkillsByCategoriesForOnboardUser(this.roleIds, this.userId)
          .subscribe((data: SkillsByCategoryInterface[]) => {
            if (data) this.populateCategoryandSkills(data);
            this.skillsByCategory.valueChanges.subscribe(() => {
              this.validateDuplicates();
            });
          });
      });

    this.commonService
      .initialData()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(initialData => {
        this.categories = initialData.categories;
      });
  }

  private populateCategoryandSkills(data: SkillsByCategoryInterface[]) {
    if (this.isUserSolutionProvider) {
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

  goForward(step: OnboardingStepsEnum): void {
    this.formSubmitted = true;
    this.checkForMissingSkillsAndCategories();
    this.checkForMissingSkills();
    if (
      this.controlContainer.control.get('linkedInUrl').invalid ||
      this.controlContainer.control.get('skillsByCategory').invalid ||
      this.isSkillRequired ||
      this.isSkillandCatetegoryRequired
    )
      return;

    this.changeStep(step);
  }

  addSkill(): void {
    const skillGroup = this.formBuilder.group({
      projectType: [''],
      skill: ['']
    });

    if (this.skillsByCategory.length < this.maxSkills) this.skillsByCategory.push(skillGroup);
  }

  removeSkill(index: number): void {
    const currentDropdown = this.skillsByCategory;
    currentDropdown.removeAt(index);
  }

  validateDuplicates() {
    const skillValues = this.skillsByCategory.controls.map(control => ({
      projectType: control.get('projectType')?.value,
      skill: control.get('skill')?.value
    }));
    const duplicates = skillValues.filter((value, index, self) => {
      if (!value.projectType.id || !value.skill.id) {
        return false;
      }
      return index !== self.findIndex(v => v.projectType.id === value.projectType.id && v.skill.id === value.skill.id);
    });

    this.setDuplicateSkillsError = duplicates.length > 0;
  }

  changeStep(step: OnboardingStepsEnum): void {
    this.onboardingWizardService.changeStep(step);
  }

  onSkillsChange(index?: number) {
    if (this.isSkillRequired) {
      const skillGroups = this.skillsByCategory.controls;
      const hasEmptySkillId = skillGroups.some(group => {
        return group.get('projectType').value && !group.get('skill').value;
      });
      this.isSkillRequired = hasEmptySkillId;
    }
    if (this.isSkillandCatetegoryRequired) {
      const skillGroups = this.skillsByCategory.controls;
      const hasEmptySkill = skillGroups.some(group => {
        return !group.get('projectType').value && !group.get('skill').value;
      });
      this.isSkillandCatetegoryRequired = hasEmptySkill;
    }
    const projectType = this.skillsByCategory.controls[index].get('projectType').value;
    if (this.isUserSolutionProvider) {
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
      return group.get('projectType').value && !group.get('skill').value;
    });
    this.isSkillRequired = hasEmptySkillId;
    return this.isSkillRequired;
  }

  checkForMissingSkillsAndCategories(): boolean {
    const skillGroups = this.skillsByCategory.controls;
    const hasEmptySkill = skillGroups.some(group => {
      return !group.get('projectType').value && !group.get('skill').value;
    });
    this.isSkillandCatetegoryRequired = hasEmptySkill;
    return this.isSkillandCatetegoryRequired;
  }

  onProjectTypeChange(skillsGroup: FormGroup) {
    skillsGroup.get('skill').setValue('');
    skillsGroup.get('skill').enable();
  }
}
