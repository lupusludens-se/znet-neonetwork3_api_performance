import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PostTypeEnum } from 'src/app/core/enums/post-type.enum';
import { MenuOptionInterface } from 'src/app/shared/modules/menu/interfaces/menu-option.interface';
import { TableCrudEnum } from 'src/app/shared/modules/table/enums/table-crud.enum';
import { Router } from '@angular/router';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { TaxonomyTypeEnum } from 'src/app/shared/enums/taxonomy-type.enum';
import { InitiativeArticleInterface } from '../../models/initiative-resources.interface';

@Component({
  selector: 'neo-learn-item',
  templateUrl: './learn-item.component.html',
  styleUrls: ['./learn-item.component.scss']
})
export class LearnItemComponent {
  showDeleteModal = false;
  @Output() selectedArticle = new EventEmitter<number>();
  @Input() isSavedArticle = false;
  @Input() article: InitiativeArticleInterface;
  @Input() initiativeId: number;

  options: MenuOptionInterface[] = [
    {
      icon: 'trash-can-red',
      name: 'initiative.viewInitiative.deleteSavedContentLabel',
      operation: TableCrudEnum.Delete,
      customClass: 'error-red-imp'
    }
  ];

  postType = PostTypeEnum;
  type = TaxonomyTypeEnum;

  constructor(private router: Router, private readonly activityService: ActivityService) {}

  getLearnType(postTypeEnum: PostTypeEnum): string {
    return postTypeEnum
      ? Object.keys(PostTypeEnum)[Object.values(PostTypeEnum).indexOf(postTypeEnum)].toLowerCase()
      : '';
  }

  openArticle(): void {
    if (this.isSavedArticle) {
      this.trackArticleActivity();
    }
  }

  optionClick(): void {
    this.selectedArticle.emit(this.article.id);
  }
  trackArticleActivity(): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.LearnView, {
        id: this.article.id,
        title: this.article.title,
        initiativeId: this.initiativeId
      })
      ?.subscribe();
    this.openArticleInNewTab();
  }
  openArticleInNewTab() {
    window.open(`learn/${this.article.id}`, '_blank');
  }
}
