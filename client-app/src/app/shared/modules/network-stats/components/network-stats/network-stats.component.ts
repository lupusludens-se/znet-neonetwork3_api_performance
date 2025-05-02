import { Component, Input, OnInit } from '@angular/core';
import { NetwrokStatsInterface } from 'src/app/shared/interfaces/netwrok-stats.interface';

@Component({
  selector: 'neo-network-stats',
  templateUrl: './network-stats.component.html',
  styleUrls: ['./network-stats.component.scss']
})
export class NetworkStatsComponent implements OnInit {
  @Input() networkStats: NetwrokStatsInterface;
  @Input() title?: string;
  corporateCompanyCount: number = 0;
  corporateCompanyCountStop: any;

  projectCount: number = 0;
  projectCountStop: any;

  articleMarketBriefCount: number = 0;
  articleMarketBriefCountStop: any;

  solutionProviderCompanyCount: number = 0;
  solutionProviderCompanyCountStop: any;

  ngOnInit(): void {
    this.startCounters();
  }

  startCounters() {
    this.initializeCount('corporateCompanyCountStop', 'corporateCompanyCount');
    this.initializeCount('projectCountStop', 'projectCount');
    this.initializeCount('articleMarketBriefCountStop', 'articleMarketBriefCount');
    this.initializeCount('solutionProviderCompanyCountStop', 'solutionProviderCompanyCount');
  }

  initializeCount(intervalCounterKey: string, countKey: string) {
    this[countKey] = Math.ceil(this.networkStats?.[countKey] - this.networkStats?.[countKey] * 0.25);
    var waitTime = this.networkStats != null ? this.calculateWaitTime(this.networkStats?.[countKey]) : 20;
    this[intervalCounterKey] = setInterval(() => {
      this[countKey]++;
      if (this[countKey] >= this.networkStats?.[countKey]) {
        clearInterval(this[intervalCounterKey]);
        this[countKey] = this.networkStats?.[countKey];
      }
    }, waitTime);
  }

  calculateWaitTime(count: number) {
    switch (count > 0) {
      case count <= 100:
        return 100;
      case count > 100 && count <= 200:
        return 50;
      case count > 200 && count <= 300:
        return 40;
      case count > 300 && count <= 400:
        return 35;
      case count > 400 && count <= 500:
        return 30;
      case count > 500 && count <= 1000:
        return 10;
      case count > 1000 && count <= 10000:
        return 5;
      case count > 10000:
        return 2;
    }
  }
}
