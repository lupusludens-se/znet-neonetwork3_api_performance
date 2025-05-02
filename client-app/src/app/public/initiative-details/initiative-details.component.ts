import { AfterViewInit, Component, OnInit } from '@angular/core';
import { PaginateResponseInterface } from 'src/app/shared/interfaces/common/pagination-response.interface';
import { PostInterface } from 'src/app/+learn/interfaces/post.interface';
import { InitiativeDetailsService } from '../services/initiative-details.service';
import { ActivityService } from 'src/app/core/services/activity.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'neo-initiative-details',
  templateUrl: './initiative-details.component.html',
  styleUrls: ['./initiative-details.component.scss']
})
export class InitiativeDetailsComponent implements OnInit, AfterViewInit {
  postData: PaginateResponseInterface<PostInterface>;
  post: PostInterface;
  isPublicUser = false;
  digArticleID = 0;
  videoContent = '';
  videoUnavailableText = 'Video is currently unavailable.';
  digArticleTitle = '';
  isArticleAvailable = false;
  constructor(private readonly initiativeDetailsService: InitiativeDetailsService,
    private readonly activityService: ActivityService,
    private readonly translateService: TranslateService) { }

  ngOnInit(): void {
    this.getDIGVideoContent();
  }
  getDIGVideoContent() {
    this.initiativeDetailsService.getDIGArticle().subscribe(data => {
      this.post = data;
      this.digArticleID = this.post?.id;
      this.digArticleTitle = this.post?.title;
      if (this.post !== null) {
        this.isArticleAvailable = true;
        this.videoContent = this.post.content.includes('[neo_video]') ? `<video id="digVideo" style="height:646px;" controls src="${this.post.videoUrl}" poster="${this.post.imageUrl}" class="w-100"></video>` : `<p style="color: #626469;font-size: 14px;">${this.videoUnavailableText}</p>`;
      }
      else {
        this.videoContent = `<p style="color: #626469;font-size: 14px;">${this.videoUnavailableText}</p>`
      }
    }
    );
  }
  onPreviousButtonClick() {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        buttonName: this.translateService.instant('general.previousStepLabel')
      })
      ?.subscribe();
  }
  onNextButtonClick() {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.InitiativesButtonClick, {
        buttonName: this.translateService.instant('general.nextStepLabel')
      })
      ?.subscribe();
  }
  ngAfterViewInit(): void {
    let hasPlayed = false;
    if (this.isArticleAvailable) {
      setTimeout(() => {
        var vid = document.getElementById("digVideo");
        vid.onplay = function () {
          if (!hasPlayed) {
            trackVideoClick();
            hasPlayed = true;
          }
        };
      }, 1000);
    }
    const trackVideoClick = () => {
      this.activityService
        .trackElementInteractionActivity(ActivityTypeEnum.LearnView, {
          id: this.digArticleID,
          title: this.digArticleTitle,
        })
        ?.subscribe();
    };
  }
}
