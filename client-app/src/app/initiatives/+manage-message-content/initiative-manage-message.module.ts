import { NgModule } from "@angular/core";
import { SvgIconsModule } from "@ngneat/svg-icon";
import { TranslateModule } from "@ngx-translate/core";

import { FormFooterModule } from "src/app/shared/modules/form-footer/form-footer.module";
import { MenuModule } from "src/app/shared/modules/menu/menu.module";
import { ModalModule } from "src/app/shared/modules/modal/modal.module";
import { PaginationModule } from "src/app/shared/modules/pagination/pagination.module";
import { UserAvatarModule } from "src/app/shared/modules/user-avatar/user-avatar.module";
import { SharedModule } from "src/app/shared/shared.module";

import { InitiativeManageMessageRoutingModule } from "./initiative-manage-message-routing.module";
import { InitiativeMessageComponent } from "./components/initiative-message-form/initiative-message.component";
import { InitiativeMessageRecommendedComponent } from "./components/initiative-message-recommended-tab/initiative-message-recommended.component";
import { InitiativeMessageSavedComponent } from "./components/initiative-message-saved-tab/initiative-message-saved-tab.component";
import { InitiativeMessageContentService } from "./services/initiative-message-content.service";
import { MessageItemModule } from "../shared/contents/message-item/message-item.module";
import { InitiativeSharedService } from "../shared/services/initiative-shared.service";

@NgModule({
  declarations: [
    InitiativeMessageComponent,
    InitiativeMessageRecommendedComponent,
    InitiativeMessageSavedComponent
  ],
  imports: [
    FormFooterModule,
    InitiativeManageMessageRoutingModule,
    MenuModule,
    MessageItemModule,
    ModalModule,
    PaginationModule,
    SharedModule,
    SvgIconsModule,
    TranslateModule,
    UserAvatarModule
  ],
  providers: [
    InitiativeMessageContentService,
    InitiativeSharedService
  ]
})
export class InitiativeManageMessageModule { }
