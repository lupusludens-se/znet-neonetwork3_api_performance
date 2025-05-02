import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { HttpService } from 'src/app/core/services/http.service';
import { LogEntry } from './log-entry';
import { LogPublisherInterface } from './log-publisher.interface';

export class WebApiLogPublisher implements LogPublisherInterface {
  private readonly ANGULAR_LOGGER_SOURCE_TYPE: number = 2;

  constructor(private readonly httpService: HttpService) {}

  log(entry: LogEntry) {
    if (!AuthService.isLoggedIn()) {
      /* avoid the redundant API call which will fail */
      return;
    }

    const apiEntry = {
      source: this.ANGULAR_LOGGER_SOURCE_TYPE,
      level: entry.level,
      message: entry.message,
      error: entry.error
        ? {
            message: entry.error.message,
            stack: entry.error.stack,
            name: entry.error.name
          }
        : null,
      extraInfo: entry.extraInfo,
      dateTimeUtc: entry.dateTime.toISOString()
    };

    this.httpService.post<LogEntry>(CommonApiEnum.Logs, apiEntry, false).subscribe();
  }
}
