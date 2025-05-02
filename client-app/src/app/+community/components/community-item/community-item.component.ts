import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommunityType } from '../../constants/parameter.const';
import { CommunityInterface } from '../../interfaces/community.interface';

@Component({
  selector: 'neo-community-item',
  templateUrl: './community-item.component.html',
  styleUrls: ['./community-item.component.scss']
})
export class CommunityItemComponent {
  @Input() communityElement: CommunityInterface;
  @Output() itemClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() followClick: EventEmitter<void> = new EventEmitter<void>();
  communityTypes = CommunityType;
}
