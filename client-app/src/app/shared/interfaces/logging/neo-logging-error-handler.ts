import { ErrorHandler, Injectable } from '@angular/core';
import { LogService } from '../../services/log.service';

@Injectable()
export class NeoLoggingErrorHandler implements ErrorHandler {
  constructor(private readonly logService: LogService) {}

  handleError(error) {
    this.logService.error('Unhandled error occurred.', error);
  }
}
