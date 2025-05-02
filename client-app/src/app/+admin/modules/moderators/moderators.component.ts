import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { ControlContainer, FormArray, FormBuilder, FormGroupDirective } from '@angular/forms';
import { UserInterface } from '../../../shared/interfaces/user/user.interface';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { EventsService } from '../../services/events.service';

@UntilDestroy()
@Component({
  selector: 'neo-moderators',
  templateUrl: 'moderators.component.html',
  styleUrls: ['../+create-event/create-event.component.scss', 'moderators.component.scss'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ModeratorsComponent implements OnInit {
  @Input() formSubmitted: boolean;

  constructor(
    private formBuilder: FormBuilder,
    public controlContainer: ControlContainer,
    private eventsService: EventsService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  get moderators(): FormArray {
    return this.controlContainer.control.get('moderators') as FormArray;
  }

  ngOnInit(): void {
    this.eventsService.currentFormValue$.pipe(untilDestroyed(this)).subscribe(formVal => {
      if (formVal?.moderators) {
        this.moderators.clear();

        formVal.moderators.forEach(mod => {
          this.addModerator(
            mod.name || `${mod.user?.firstName} ${mod.user?.lastName}`,
            mod.company,
            mod.user?.image?.uri,
            mod.userId
          );
        });
      } else {
        this.addModerator();
      }

      this.cdr.detectChanges();
    });
  }

  addModerator(
    name: string = '',
    company: string = '',
    displayImage: string = undefined,
    userId: number = undefined
  ): void {
    const moderatorsForm = this.formBuilder.group({
      name,
      company,
      displayImage,
      userId
    });
    this.moderators.push(moderatorsForm);
  }

  validateModeratorRow(index: number): boolean {
    return (
      (this.controlContainer.control.get('moderators').value.length === 1 &&
        !this.controlContainer.control.get('moderators').value[index].name &&
        !this.controlContainer.control.get('moderators').value[index].userId) ||
      (!this.controlContainer.control.get('moderators').value[index].name &&
        !this.controlContainer.control.get('moderators').value[index].userId &&
        this.controlContainer.control.get('moderators').value[index].company) ||
      (this.controlContainer.control.get('moderators').value.length > 1 &&
        this.controlContainer.control.get('moderators').value.every(m => !m.name && !m.userId))
    );
  }

  removeModerator(index: number) {
    const currentModerators = this.controlContainer.control.get('moderators') as FormArray;
    currentModerators.removeAt(index);
  }

  setModeratorName(str: string, index: number) {
    this.moderators.at(index).patchValue({ name: str });
  }

  setModeratorCompany(str: string, index: number) {
    this.moderators.at(index).patchValue({ company: str });
  }

  addModeratorToForm(moderator: UserInterface, index: number) {
    this.moderators.at(index).patchValue({
      name: `${moderator.firstName} ${moderator.lastName}`,
      displayImage: moderator.image?.uri,
      userId: moderator.id,
      company: moderator.company.name
    });
  }

  clearModeratorInput(index: number) {
    this.moderators.at(index).patchValue({
      name: '',
      displayImage: '',
      userId: '',
      company: ''
    });
  }
}
