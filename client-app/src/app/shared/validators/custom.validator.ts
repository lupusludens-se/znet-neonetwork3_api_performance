import { AbstractControl, FormGroup, ValidationErrors, Validator, ValidatorFn, Validators } from '@angular/forms';
import { EventModeratorInterface } from '../interfaces/event/event-moderator.interface';

export const URL_REGEXP: RegExp =
  /^(?:https:\/\/?|http:\/\/?|ftp:\/\/|www\.)(?:\S+(?::\S*)?@)?(?:(?!10(?:\.\d{1,3}){3})(?!127(?:\.\d{1,3}){3})(?!169\.254(?:\.\d{1,3}){2})(?!192\.168(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]+-?)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]+-?)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:\/[^\s]*)?$/i;

const EMAIL_REGEXP: RegExp = /^\w+([-.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
const NAME_EXCLUDE_SYMBOLS_REGEXP: RegExp = /[\d!@#$%^&*()[\]:`~{}<>=";?/\\|+]/;
const COMPANY_EXCLUDE_SYMBOLS_REGEXP: RegExp = /[@#$%^*[\]:`~{}<>=";/\\|]/;
const MDMKEY_REGEXP: RegExp = /^(ORG|MDM)-[0-9]+$/i;
const NUMBER_FLOAT: RegExp = /^(?!0\d)\d*(\.\d+)?$/;
const LINKEDINURL_REGEXP: RegExp = /^(http(s)?:\/\/)?([a-z]{2,3}\.)?linkedin\.com\/(pub\/|in\/|profile\/|company\/)/;

export abstract class CustomValidator {
  static validateUrl(record: { urlName: string; urlLink: string }): ValidationErrors | null {
    const errors: { [key: string]: any } = {};
    if (!record.urlName && record.urlLink) {
      errors['noUrlName'] = 'Link Name is required.';
    } else if (record.urlName && !record.urlLink) {
      errors['noUrlLink'] = 'URL is required.';
    }
    return Object.keys(errors).length ? errors : null;
  }

  static urlValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const isInvalid: ValidationErrors | null = this.validateUrl(control.value);

      return isInvalid;
    };
  }

  static url(c: FormGroup): ValidationErrors | null {
    const isInvalid: ValidationErrors | null = Validators.pattern(URL_REGEXP)(c);
    return isInvalid ? { url: true } : null;
  }

  static required(c: AbstractControl): ValidationErrors | null {
    let error: ValidationErrors | null;
    if (typeof c.value === 'string') {
      error = c.value.trim() ? null : { required: true };
    } else if (Array.isArray(c.value)) {
      error = c.value.length ? null : { required: true };
    } else if (!c.value) {
      error = { required: true };
    }

    return error;
  }

  static email(c: AbstractControl): ValidationErrors | null {
    const isInvalid: ValidationErrors | null = Validators.pattern(EMAIL_REGEXP)(c);
    return isInvalid ? { email: true } : null;
  }

  static preventWhitespacesOnly(c: AbstractControl): ValidationErrors | null {
    const isInvalid = (c.value || '').trim().length === 0;
    return isInvalid ? { whitespaces: true } : null;
  }

  static companyNameExcludeSymbols(c: AbstractControl): ValidationErrors | null {
    const isMatching: ValidationErrors | null = Validators.pattern(COMPANY_EXCLUDE_SYMBOLS_REGEXP)(c);
    const isInvalid: ValidationErrors | null = Validators.pattern(URL_REGEXP)(c);
    return isMatching && isInvalid ? null : { companyNameInvalidLetters: true };
  }

  static nameExcludeSymbols(c: AbstractControl): ValidationErrors | null {
    const isMatching: ValidationErrors | null = Validators.pattern(NAME_EXCLUDE_SYMBOLS_REGEXP)(c);
    return isMatching ? null : { invalidLetters: true };
  }

  static userName(c: FormGroup): ValidationErrors | null {
    const isMatching: ValidationErrors | null = Validators.pattern(NAME_EXCLUDE_SYMBOLS_REGEXP)(c);
    return isMatching ? null : { nameInvalidLetters: true };
  }

  static mdmKey(c: FormGroup): ValidationErrors | null {
    const isInvalid: ValidationErrors | null = Validators.pattern(MDMKEY_REGEXP)(c);
    return isInvalid ? { mdmKey: true } : null;
  }

  static checkOccurrences(control: AbstractControl): ValidationErrors {
    const occurrences: { fromDateBrowser: string; toDateBrowser: string }[] = control.value;

    if (!!occurrences && (!occurrences.length || occurrences.some(oc => oc.fromDateBrowser >= oc.toDateBrowser))) {
      return { incorrect: true };
    }

    return null;
  }

  static checkModerators(control: AbstractControl): ValidationErrors {
    const moderators: EventModeratorInterface[] = control.value;
    if (
      (!!moderators && (!moderators.length || moderators.every(m => !m.name && !m.userId))) ||
      moderators.some(m => m.company && !m.name && !m.userId)
    ) {
      return { required: true };
    }
    return null;
  }

  static floatNumber(c: AbstractControl): ValidationErrors | null {
    const isInvalid: ValidationErrors | null = Validators.pattern(NUMBER_FLOAT)(c);
    return isInvalid ? { pattern: true } : null;
  }

  static linkedInUrl(c: FormGroup): ValidationErrors | null {
    const isInvalid: ValidationErrors | null = Validators.pattern(LINKEDINURL_REGEXP)(c);
    return isInvalid ? { pattern: true } : null;
  }

  static noWhitespaceValidator(control: AbstractControl) {
    return (control.value || '').trim().length ? null : { whitespace: true };
  }

  static checkIfCharacterCountIsGreaterThanLimit(control: AbstractControl, maxLength: number, message: string): ValidationErrors | null {
    return !control.value ? null : control.value?.length > maxLength ? { characterCount: message } : null;
  }
}
