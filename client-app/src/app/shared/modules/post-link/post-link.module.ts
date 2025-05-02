// modules
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { RouterModule } from '@angular/router';

// components
import { PostLinkComponent } from './components/post-link/post-link.component';
import { PublicModule } from 'src/app/public/public.module';

@NgModule({
  declarations: [PostLinkComponent],
  imports: [SharedModule, RouterModule, PublicModule],
  exports: [PostLinkComponent]
})
export class PostLinkModule {}
