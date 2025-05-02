import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { SnackbarMessageInterface } from '../interfaces/snackbar-message.interface';
import { SnackbarTypeEnum } from '../enums/snackbar-type.enum';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {
  private snackbarSubject = new Subject<SnackbarMessageInterface>();
  public snackbarState = this.snackbarSubject.asObservable();

  constructor(private readonly translateService: TranslateService) {}

  public showError(message: string): void {
    this.show(SnackbarTypeEnum.Error, message);
  }

  public showSuccess(message: string, subMessage?: string): void {
    this.show(SnackbarTypeEnum.Success, message, subMessage);
  }

  public showInfo(message: string, subMessage?: string): void {
    this.show(SnackbarTypeEnum.Info, message, subMessage);
  }

  private parseTranslation(strToTranslate: string): string {
    return strToTranslate?.includes('Label') ? this.translateService.instant(strToTranslate) : strToTranslate;
  }

  private show(type: SnackbarTypeEnum, message: string, subMessage?: string): void {
    const messageToShow: string = this.parseTranslation(message);
    const subMessageToShow: string = subMessage ? this.parseTranslation(subMessage) : null;

    this.snackbarSubject.next({
      show: true,
      type,
      message: subMessage ? `${messageToShow} ${subMessageToShow}` : messageToShow
    });
  }
}
