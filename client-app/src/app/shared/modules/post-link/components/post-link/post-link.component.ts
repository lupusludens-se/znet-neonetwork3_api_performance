import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';

import { CoreService } from '../../../../../core/services/core.service';

import { PostInterface } from '../../../../../+learn/interfaces/post.interface';

import { PostTypeEnum } from '../../../../../core/enums/post-type.enum';
import { SaveContentService } from '../../../../services/save-content.service';
import { CATEGORIES, CONTENTTAGS, REGIONS, SOLUTIONS, TECHNOLOGIES } from '../../../../constants/taxonomy-names.const';
import { Router } from '@angular/router';
import { LocationStrategy } from '@angular/common';
import { AuthService } from 'src/app/core/services/auth.service';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { ActivityService } from 'src/app/core/services/activity.service';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';

@Component({
  selector: 'neo-post-link',
  templateUrl: './post-link.component.html',
  styleUrls: ['./post-link.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PostLinkComponent implements OnInit, OnChanges {
  @Input() post: PostInterface;
  @Input() icon: string;

  // simplified
  @Input() image: string;
  @Input() title: string;

  @Output() postClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() tagClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() saveClick: EventEmitter<boolean> = new EventEmitter<boolean>();

  postType = PostTypeEnum;
  tagsNumber: number;
  auth = AuthService;
  activityTypeEnum: any;
  currentUser: UserInterface;

  constructor(
    private readonly coreService: CoreService,
    private readonly authService: AuthService,
    private readonly saveContentService: SaveContentService,
    public router: Router,
    private readonly activityService: ActivityService,
    private locationStrategy: LocationStrategy,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.activityTypeEnum = ActivityTypeEnum.PrivateLearnClick;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes?.post?.currentValue?.id !== changes?.post?.previousValue?.id) {
      this.post.postTags = [
        ...this.coreService.getTaxonomyTag(this.post, CATEGORIES),
        ...this.coreService.getTaxonomyTag(this.post, SOLUTIONS),
        ...this.coreService.getTaxonomyTag(this.post, TECHNOLOGIES),
        ...this.coreService.getTaxonomyTag(this.post, REGIONS),
        ...this.coreService.getTaxonomyTag(this.post, CONTENTTAGS)
      ];

      this.tagsNumber = this.post.postTags.length - 2;
      this.post.postTags.splice(2, this.post.postTags.length - 1);
    

      this.authService.currentUser().subscribe(currentUser => {
        if (currentUser != null) {
          this.currentUser = currentUser;
          this.cdr.detectChanges();
        }
      });
    }
  }

  getNeoType(postTypeEnum: PostTypeEnum): string {
    if (!postTypeEnum) return;

    const typeEnumKey = Object.keys(PostTypeEnum)[Object.values(PostTypeEnum).indexOf(postTypeEnum)].toLowerCase();

    return typeEnumKey ? typeEnumKey : '';
  }

  save(postId: number): void {
    if (this.post.isSaved) {
      this.saveContentService.deleteArticle(postId).subscribe(() => {
        this.post.isSaved = false;
        this.saveClick.emit(false);
      });
    } else {
      this.saveContentService.saveArticle(postId).subscribe(() => {
        this.post.isSaved = true;
        this.saveClick.emit(true);
      });
    }
  }

  routeToLearnPage(ctrlKeyPressed: boolean, path: string) {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.LearnView, { id: this.post?.id, title: this.post?.title })
      ?.subscribe();
    if (ctrlKeyPressed) {
      const getBaseHref = location.origin + this.locationStrategy.getBaseHref();
      const serializedUrl = getBaseHref + this.router.serializeUrl(this.router.createUrlTree([`${path}`]));
      window.open(serializedUrl, '_blank');
    } else {
      this.router.navigate([path]);
    }
    this.postClick.emit();
  }
  handleClick(event: Event) {
    if (this.currentUser == null) {
      event.stopPropagation();
      event.preventDefault();
    }
  }
}
