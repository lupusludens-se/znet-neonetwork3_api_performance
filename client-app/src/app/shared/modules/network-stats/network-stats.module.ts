import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { NetworkStatsComponent } from './components/network-stats/network-stats.component';

@NgModule({
  declarations: [NetworkStatsComponent],
  imports: [CommonModule, TranslateModule],
  exports: [NetworkStatsComponent]
})
export class NetworkStatsModule {}
