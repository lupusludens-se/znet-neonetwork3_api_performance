import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CommunityDataService } from '../../services/community.data.service';
import { NetwrokStatsInterface } from 'src/app/shared/interfaces/netwrok-stats.interface';
import { SignTrackingSourceEnum } from '../../../core/enums/sign-tracking-source-enum';
@Component({
  selector: 'neo-community-public',
  templateUrl: './community-public.component.html',
  styleUrls: ['./community-public.component.scss']
})
export class CommunityPublicComponent implements OnInit {
  networkStats: NetwrokStatsInterface;
  signTrackingSourceEnum = SignTrackingSourceEnum.ZeigoNetwork;
  constructor(private readonly communityDataService: CommunityDataService) { }
  ngOnInit(): void {
    this.communityDataService.getNetorkStats().subscribe((result) => {
      this.networkStats = result;
    });
  }
}
