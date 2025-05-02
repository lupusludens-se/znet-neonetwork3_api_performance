import { NgModule } from '@angular/core';
import { SvgIconsModule } from '@ngneat/svg-icon';
import { TranslateModule } from '@ngx-translate/core';

import { FormFooterModule } from 'src/app/shared/modules/form-footer/form-footer.module';
import { MenuModule } from 'src/app/shared/modules/menu/menu.module';
import { ModalModule } from 'src/app/shared/modules/modal/modal.module';
import { PaginationModule } from 'src/app/shared/modules/pagination/pagination.module';
import { UserAvatarModule } from 'src/app/shared/modules/user-avatar/user-avatar.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { InitiativeSharedService } from '../shared/services/initiative-shared.service';
import { InitiativeManageCommunityRoutingModule } from './initiative-manage-community-routing.module';
import { InitiativeCommunityFormComponent } from './components/initiative-community-form/initiative-community-form.component';
import { InitiativeCommunityRecommendedComponent } from './components/initiative-community-recommended/initiative-community-recommended.component';
import { InitiativeCommunitySavedComponent } from './components/initiative-community-saved/initiative-community-saved.component';
import { InitiativeCommunityContentService } from './services/initiative-community-content.service';
import { CommunityItemModule } from '../shared/contents/community-item/community-item.module';

@NgModule({
  declarations: [
    InitiativeCommunityFormComponent,
    InitiativeCommunityRecommendedComponent,
    InitiativeCommunitySavedComponent
  ],
  imports: [
    InitiativeManageCommunityRoutingModule,
    FormFooterModule,
    MenuModule,
    CommunityItemModule,
    ModalModule,
    PaginationModule,
    SharedModule,
    SvgIconsModule,
    TranslateModule,
    UserAvatarModule
  ],
  providers: [InitiativeSharedService, InitiativeCommunityContentService]
})
export class InitiativeManageCommunityModule {}
