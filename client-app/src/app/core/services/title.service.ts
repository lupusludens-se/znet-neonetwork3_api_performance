import { Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TitleService {
  constructor(private readonly title: Title, private readonly translateService: TranslateService) {}

  setTitle(title: string, subTitle?: string): void {
    const titleToSet =
      title.includes('Label') || title.includes('Title') ? this.translateService.instant(title) : title;

    this.title.setTitle(
      subTitle
        ? `${titleToSet}: ${subTitle} | ${environment.projectName}`
        : `${titleToSet} | ${environment.projectName}`
    );
  }
}
