import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { ControlContainer, FormArray, FormBuilder, FormGroupDirective } from '@angular/forms';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CustomValidator } from '../../../shared/validators/custom.validator';
import { EventsService } from '../../services/events.service';

@UntilDestroy()
@Component({
  selector: 'neo-content-links',
  templateUrl: 'content-links.component.html',
  styleUrls: ['../+create-event/create-event.component.scss'],
  styles: [
    `
      input {
        height: 48px;
        margin-top: 0 !important;
      }
    `
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class ContentLinksComponent implements OnInit {
  @Input() linksError: boolean;
  @Input() formSubmitted: boolean;

  constructor(
    private formBuilder: FormBuilder,
    private controlContainer: ControlContainer,
    private eventsService: EventsService,
    private changeDetRef: ChangeDetectorRef
  ) {}

  /* RECORDINGS */
  get recordings(): FormArray {
    return this.controlContainer.control.get('recordings') as FormArray;
  }

  /* LINKS */
  get links(): FormArray {
    return this.controlContainer.control.get('links') as FormArray;
  }

  ngOnInit() {
    this.eventsService.currentFormValue$.pipe(untilDestroyed(this)).subscribe(formVal => {
      if (formVal) {
        this.links.clear();
        if (formVal?.links) {
          formVal.links.forEach(link => {
            this.addLink(link.url, link.name);
          });
        }

        if (formVal?.recordings) {
          this.recordings.clear();
          formVal.recordings.forEach(link => {
            this.addRecording(link.url);
          });
        }

        this.changeDetRef.markForCheck();
      } else {
        this.addLink();
        this.addRecording();
      }
    });
  }

  addRecording(url: string = ''): void {
    const recordingForm = this.formBuilder.group({ url });

    recordingForm.get('url').setValidators(CustomValidator.url);
    this.recordings.push(recordingForm);
  }

  removeRecording(index: number) {
    const currentRecordings = this.controlContainer.control.get('recordings') as FormArray;
    currentRecordings.removeAt(index);
  }

  /**/

  addLink(url: string = '', name: string = ''): void {
    const linkForm = this.formBuilder.group({ url, name });

    linkForm.get('url').setValidators(CustomValidator.url);
    linkForm.get('url').updateValueAndValidity();

    this.links.push(linkForm);
  }

  removeLink(index: number) {
    const currentLinks = this.controlContainer.control.get('links') as FormArray;
    currentLinks.removeAt(index);

    if (!currentLinks.length) {
      this.linksError = null;
    }
  }

  /**/
}
