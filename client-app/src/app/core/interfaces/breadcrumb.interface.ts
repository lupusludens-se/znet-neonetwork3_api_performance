import { Params } from '@angular/router';

export interface BreadcrumbInterface {
  label: string;
  url: string;
  queryParams?: Params;
}
