import { NgModule } from "@angular/core";
import { SvgIconsModule } from "@ngneat/svg-icon";
import { TranslateModule } from "@ngx-translate/core";
import { FormFooterModule } from "src/app/shared/modules/form-footer/form-footer.module";
import { MenuModule } from "src/app/shared/modules/menu/menu.module";
import { ModalModule } from "src/app/shared/modules/modal/modal.module";
import { PaginationModule } from "src/app/shared/modules/pagination/pagination.module";
import { UserAvatarModule } from "src/app/shared/modules/user-avatar/user-avatar.module";
import { SharedModule } from "src/app/shared/shared.module";
import { InitiativeManageToolsRoutingModule } from "./initiative-manage-tools-routing.module";
import { InitiativeToolContentService } from "./services/initiative-tool-content.service";
import { InitiativeToolsComponent } from "./components/initiative-tools-form/initiative-tools.component";
import { InitiativeToolsRecommendedComponent } from "./components/initiative-tools-recommended-tab/initiative-tools-recommended.component";
import { InitiativeToolsSavedComponent } from "./components/initiative-tools-saved-tab/initiative-tools-saved-tab.component";
import { ToolItemModule } from "../shared/contents/tool-item/tool-item.module";
import { InitiativeSharedService } from "../shared/services/initiative-shared.service";

@NgModule({
  declarations: [
    InitiativeToolsComponent,
    InitiativeToolsRecommendedComponent,
    InitiativeToolsSavedComponent
  ],
  imports: [
    FormFooterModule,
    InitiativeManageToolsRoutingModule,
    MenuModule,
    ModalModule,
    PaginationModule,
    SharedModule,
    SvgIconsModule,
    ToolItemModule,
    TranslateModule,
    UserAvatarModule
  ],
  providers: [
    InitiativeSharedService,
    InitiativeToolContentService
  ]
})
export class InitiativeManageToolsModule { }
