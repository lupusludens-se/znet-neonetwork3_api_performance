import { Injectable } from '@angular/core';

@Injectable()
export class SearchValidatorService {
  private static defaultSearchRegExp: RegExp = new RegExp('^[^<>]*$');

  static validateSearch(value: string, customRegExp?: RegExp): boolean {
    const regExp = customRegExp || SearchValidatorService.defaultSearchRegExp;
    return !value || regExp.test(value);
  }
}
