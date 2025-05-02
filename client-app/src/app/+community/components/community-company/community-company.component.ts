import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { CommunityInterface } from '../../interfaces/community.interface';

import { CommunityDataService } from '../../services/community.data.service';

@Component({
  selector: 'neo-community-company',
  templateUrl: './community-company.component.html',
  styleUrls: ['./community-company.component.scss']
})
export class CommunityCompanyComponent implements OnInit {
  @Input() communityCompany: CommunityInterface;

  @Output() companyClick: EventEmitter<void> = new EventEmitter<void>();
  @Output() followClick: EventEmitter<void> = new EventEmitter<void>();
  logoExist: boolean;

  constructor(private readonly communityDataService: CommunityDataService) {}

  ngOnInit(): void {
    this.logoExist = this.communityCompany.image != null && this.communityCompany.image.uri != null;
  }

  follow(httpMethod: string): void {
    this.communityDataService.followCompany(this.communityCompany.id, httpMethod).subscribe(res => {
      if (!res.errorMessages) this.communityCompany.isFollowed = !this.communityCompany.isFollowed;
      this.followClick.emit();
    });
  }
}
