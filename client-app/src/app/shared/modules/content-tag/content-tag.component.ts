import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output
} from '@angular/core';
import { TagInterface } from '../../../core/interfaces/tag.interface';
import { CoreService } from '../../../core/services/core.service';
import { TaxonomyTypeEnum } from '../../enums/taxonomy-type.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserInterface } from '../../interfaces/user/user.interface';

@Component({
  selector: 'neo-content-tag',
  template: `
    <div
      class="disabled default-cursor"
      *ngIf="currentUser === null && !auth.isLoggedIn()"
      [src]="'article'"
      neoLockClick>
      <div
        class="flex-center pl-8 pr-8 pt-6 pb-6 {{ classCustom }} default-cursor"
        [style.fontSize]="fontSize + 'px'">
        {{ tagText ? tagText : tag?.name }}
      </div>
    </div>
    <div
      *ngIf="currentUser !== null"
      [src]="'article'">
      <div
        class="flex-center pl-8 pr-8 pt-6 pb-6 {{ classCustom }} c-pointer"
        [style.fontSize]="fontSize + 'px'"
        (click)="$event.stopPropagation(); tagClicked.emit(tag); goToTopics()">
        {{ tagText ? tagText : tag?.name }}
      </div>
    </div>
  `,
  styleUrls: ['content-tag.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ContentTagComponent implements OnInit {
  // TODO: make consistent components input
  currentUser: UserInterface;
  @Input() tag: TagInterface;
  @Input() type: TaxonomyTypeEnum;
  @Input() tagText: string;
  @Input() fontSize: string = '16';
  @Input() classCustom?: string = 'tag';
  @Input() disable: boolean = false;
  @Input() isOpenInNewTab?: boolean = false;
  @Input() skipGoToTopics: boolean = false;
  auth = AuthService;

  @Output() tagClicked: EventEmitter<TagInterface> = new EventEmitter<TagInterface>();

  constructor(
    private readonly coreService: CoreService,
    private readonly authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.listenForCurrentUser();
  }

  private listenForCurrentUser(): void {
    this.authService.currentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      if (currentUser) {
        this.cdr.detectChanges();
      }
    });
  }

  goToTopics(): void {
    if (this.tag && !this.skipGoToTopics && this.currentUser != null) {
      this.coreService.goToTopics(this.tag.id, this.tag.name, this.type, this.isOpenInNewTab);
    }
  }
}
