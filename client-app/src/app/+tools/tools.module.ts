// modules
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ToolsRoutingModule } from './tools-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ToolModule } from '../shared/modules/tool/tool.module';
import { BlueCheckboxModule } from '../shared/modules/blue-checkbox/blue-checkbox.module';
import { RadioControlModule } from '../shared/modules/radio-control/radio-control.module';
import { SquareRadioControlModule } from '../shared/modules/square-radio-control/square-radio-control.module';
import { FormFooterModule } from '../shared/modules/form-footer/form-footer.module';
import { PaginationModule } from '../shared/modules/pagination/pagination.module';
import { NoResultsModule } from '../shared/modules/no-results/no-results.module';

// components
import { ToolsComponent } from './tools.component';
import { ToolContentComponent } from './components/tool-content/tool-content.component';
import { IndicativeQuoteRequestFormComponent } from './components/solar-quote/indicative-quote-request-form/indicative-quote-request-form.component';
import { SubmitAddressFormComponent } from './components/solar-quote/submit-address-form/submit-address-form.component';
import { RequestPortfolioReviewFormComponent } from './components/solar-quote/request-portfolio-review-form/request-portfolio-review-form.component';
import { ThankYouComponent } from './components/solar-quote/thank-you/thank-you.component';
import { PinnedToolsRequestsService } from '../+dashboard/services/pinned-tools-requests.service';
import { TextInputModule } from '../shared/modules/controls/text-input/text-input.module';
import { TextareaControlModule } from '../shared/modules/controls/textarea-control/textarea-control.module';
import { ControlErrorModule } from '../shared/modules/controls/control-error/control-error.module';
import { NumberInputModule } from '../shared/modules/controls/number-input/number-input.module';
import { EmptyPageModule } from '../shared/modules/empty-page/empty-page.module';
import { CompanyLogoModule } from '../shared/modules/company-logo/company-logo.module';
import { TopPanelModule } from "../shared/modules/top-panel/top-panel.module";
import { TranslateModule } from '@ngx-translate/core';
import { ModalModule } from '../shared/modules/modal/modal.module';
import { InitiativeSharedService } from '../initiatives/shared/services/initiative-shared.service';

@NgModule({
    declarations: [
        ToolsComponent,
        ToolContentComponent,
        IndicativeQuoteRequestFormComponent,
        SubmitAddressFormComponent,
        RequestPortfolioReviewFormComponent,
        ThankYouComponent
    ],
    providers: [PinnedToolsRequestsService, InitiativeSharedService],
    imports: [
        SharedModule,
        ToolsRoutingModule,
        ToolModule,
        ReactiveFormsModule,
        BlueCheckboxModule,
        RadioControlModule,
        SquareRadioControlModule,
        FormFooterModule,
        PaginationModule,
        NoResultsModule,
        TextInputModule,
        TextareaControlModule,
        CompanyLogoModule,
        ControlErrorModule,
        NumberInputModule,
        EmptyPageModule,
        TopPanelModule,
        TranslateModule,
        ModalModule
    ]
})
export class ToolsModule { }
