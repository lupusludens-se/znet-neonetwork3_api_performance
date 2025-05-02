import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { CompanyManagementApiEnum } from 'src/app/+admin/modules/+company-management/enums/company-management-api.enum';
import { CompanyLogoInterface } from 'src/app/shared/interfaces/company-logo.interface';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'neo-thank-you',
  templateUrl: './thank-you.component.html',
  styleUrls: ['./thank-you.component.scss']
})
export class ThankYouComponent implements OnInit, OnDestroy {
  apiRoutes = CompanyManagementApiEnum;
  hardcodedCompanies: CompanyLogoInterface[] = [];

  private unsubscribe$: Subject<void> = new Subject<void>();
  private toolId: number;

  constructor(private readonly activatedRoute: ActivatedRoute, private readonly router: Router) {}

  ngOnInit(): void {
    this.activatedRoute.params.pipe(takeUntil(this.unsubscribe$)).subscribe(params => {
      this.toolId = params.id;
    });

    this.hardcodedCompanies = environment.quoteScreenCompanies.map(c => {
      return {
        id: c.id,
        image: {
          uri: `assets/images/quote-finish-screen-companies/${c.image}`
        }
      };
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  onCancel(): void {
    this.router.navigate(['/tools']);
  }

  submitAnotherRequest(): void {
    this.router.navigate([`/tools/${this.toolId}`]);
  }
}
