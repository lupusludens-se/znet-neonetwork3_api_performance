import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonService } from '../../../core/services/common.service';
import { UserProfileInterface } from '../../interfaces/user-profile.interface';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { HttpService } from '../../../core/services/http.service';
import { TaxonomiesPayloadInterface } from '../../../shared/modules/interests-topic/interfaces/taxonomies-payload.interface';
import { UserProfileApiEnum } from '../../../shared/enums/api/user-profile-api.enum';
import { filter } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import structuredClone from '@ungap/structured-clone';

@UntilDestroy()
@Component({
  selector: 'neo-interests-modal',
  templateUrl: 'interests-modal.component.html',
  styleUrls: ['./interests-modal.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class InterestsModalComponent implements OnInit {
  @Input() userProfile: UserProfileInterface;

  @Output() closeModal: EventEmitter<void> = new EventEmitter<void>();
  @Output() saveChanges: EventEmitter<void> = new EventEmitter<void>();

  categoriesList: TagInterface[];
  userProfileApiRoutes = UserProfileApiEnum;
  updatedTopics: TagInterface[];

  constructor(private commonService: CommonService, private httpService: HttpService) {}

  ngOnInit(): void {
    this.commonService
      .initialData()
      .pipe(
        filter(v => !!v),
        untilDestroyed(this)
      )
      .subscribe(data => {
        this.categoriesList = structuredClone(data)?.categories || [];

        this.categoriesList.forEach(cat => {
          this.userProfile.categories.forEach(userCat => {
            if (userCat.id === cat.id) cat.selected = true;
          });
        });
      });
  }

  updateTopics(selectedTopic: TagInterface[]): void {
    this.updatedTopics = [...this.userProfile.categories, ...selectedTopic].filter(top => top.selected === true);
  }

  save(): void {
    if (!this.updatedTopics) {
      return this.closeModal.emit();
    }

    const selectedTopics: TagInterface[] = [...this.updatedTopics];
    const updatedTaxonomies: TaxonomiesPayloadInterface = {
      categories: selectedTopics,
      solutions: [],
      technologies: [],
      regions: []
    };

    this.httpService.post(this.userProfileApiRoutes.UserInterests, updatedTaxonomies).subscribe(() => {
      this.saveChanges.emit();
    });
  }

  cancel(): void {
    this.updateTopics(this.userProfile.categories);
    this.closeModal.emit();
  }

  checkSelectedTopics(): boolean {
    return !this.categoriesList?.some(c => c.selected === true) && !this.updatedTopics?.length;
  }
}
