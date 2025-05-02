/* eslint-disable prettier/prettier */
import { ValidationErrors } from '@angular/forms';

export const ValidationErrorsMsg: Map<string, (err: ValidationErrors, ctrName: string) => string> = new Map([
  ['email', (err, cn) => `${cn} is not valid`],
  ['required', (err, cn) => `${cn} is required`],
  ['minlength', (err, cn) => `${cn} must be at least ${err.minlength.requiredLength} characters`],
  ['maxlength', (err, cn) => `${cn} must be not more than ${err.maxlength.requiredLength} characters`],
  ['pattern', (err, cn) => `${cn} is not valid`],
  ['max', (err, cn) => `${cn} must not be more than  ${err.max.max}`],
  ['min', (err, cn) => `${cn} must be at least ${err.min.min}`],
  ['total', (err, cn) => `${cn} must be ${err.total} in total`],
  ['url', (err, cn) => `${cn} must be a valid URL`],
  ['incorrect', (err, cn) => `${cn} is incorrect`],
  ['mdmKey', (err, cn) => `${cn} must be in format ORG-X(X)`],
  ['invalidLetters', (err, cn) => `${cn} can't include any digit and those characters !@#$%^&*()[]:\`~{{}}<>=\\";?/|+`],
  ['companyNameInvalidLetters', (err, cn) => `${cn} can't be URL or include characters @#$%^*[]:\`~{{}}<>=\\";/|`],
  ['nameInvalidLetters', (err, cn) => `${cn} can contain only letters and text symbols .-,'`],
  ['deletedUser', () => `Project must be assigned to an active user`],
  ['whitespace', (err, cn) => `${cn} should not contain only whitespaces`],
]);
