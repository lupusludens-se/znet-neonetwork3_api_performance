import { NgModule } from "@angular/core";
import { SvgIconsModule } from "@ngneat/svg-icon";
import { TranslateModule } from "@ngx-translate/core";

import { FormFooterModule } from "src/app/shared/modules/form-footer/form-footer.module";
import { MenuModule } from "src/app/shared/modules/menu/menu.module";
import { ModalModule } from "src/app/shared/modules/modal/modal.module";
import { PaginationModule } from "src/app/shared/modules/pagination/pagination.module";
import { UserAvatarModule } from "src/app/shared/modules/user-avatar/user-avatar.module";
import { SharedModule } from "src/app/shared/shared.module";

import { InitiativeLearnComponent } from "./components/initiative-learn-form/initiative-learn.component";
import { InitiativeLearnRecommendedComponent } from "./components/initiative-learn-recommended-tab/initiative-learn-recommended.component";
import { InitiativeLearnSavedComponent } from "./components/initiative-learn-saved-tab/initiative-learn-saved.component";

import { InitiativeManageLearnRoutingModule } from "./initiative-manage-learn-routing.module";
import { InitiativeLearnContentService } from "./services/initiative-learn-content.service";
import { LearnItemModule } from "../shared/contents/learn-item/learn-item.module";
import { InitiativeSharedService } from "../shared/services/initiative-shared.service";

@NgModule({
  declarations: [
    InitiativeLearnComponent,
    InitiativeLearnRecommendedComponent,
    InitiativeLearnSavedComponent
  ],
  imports: [
    FormFooterModule,
    InitiativeManageLearnRoutingModule,
    MenuModule,
    LearnItemModule,
    ModalModule,
    PaginationModule,
    SharedModule,
    SvgIconsModule,
    TranslateModule,
    UserAvatarModule
  ],
  providers: [InitiativeLearnContentService, InitiativeSharedService]
})
export class InitiativeManageLearnModule { }
