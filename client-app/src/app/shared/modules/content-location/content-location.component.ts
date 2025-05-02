import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { TagInterface } from '../../../core/interfaces/tag.interface';
import { CoreService } from '../../../core/services/core.service';
import { TaxonomyTypeEnum } from '../../enums/taxonomy-type.enum';
import { UserInterface } from '../../interfaces/user/user.interface';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'neo-content-location',
  templateUrl: 'content-location.component.html',
  styleUrls: ['content-location.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ContentLocationComponent implements OnInit {
  currentUser: UserInterface;
  @Input() regions: TagInterface[] | { id: number; name: string }[];
  @Input() isOpenInNewTab?: boolean = false;

  @Output() clicked: EventEmitter<void> = new EventEmitter<void>();

  constructor(private readonly coreService: CoreService, private cdr: ChangeDetectorRef, private readonly authService: AuthService) { }

  ngOnInit(): void {
    this.listenForCurrentUser();
  }

  private listenForCurrentUser(): void {
    this.authService.currentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.cdr.detectChanges();
    });
  }

  goToTopics(region: TagInterface): void {
    if (this.currentUser != null) {
      this.clicked.emit();
      this.coreService.goToTopics(region.id, region.name, TaxonomyTypeEnum.Region, this.isOpenInNewTab);
    }
  }
}
