import { LogLevel } from '../../enums/log-level-enum';
import { LogEntry } from './log-entry';
import { LogPublisherInterface } from './log-publisher.interface';

export class ConsoleLogPublisher implements LogPublisherInterface {
  log(entry: LogEntry, isUnhandled?: boolean) {
    if (isUnhandled) {
      console.error(this.buildLogString(entry));
    } else {
      console.warn(this.buildLogString(entry));
    }
  }

  private buildLogString(entry: LogEntry): any {
    let logString: string = `[${entry.dateTime.toUTCString()}] | ${LogLevel[entry.level]}`;

    if (entry.message) {
      logString += ` | ${entry.message}`;
    }

    if (entry.extraInfo?.length) {
      logString += ` | Extra Info: ${this.formatExtraInfo(entry.extraInfo)}`;
    }

    if (entry.error) {
      if (entry.error.stack) {
        logString += ` | ${entry.error.stack}`;
      } else if (entry.error.message) {
        logString += ` | ${entry.error.message}`;
      }
    }

    return logString;
  }

  private formatExtraInfo(extraInfo: any[]): string {
    let formattedString: string;

    // is there at least one object in the array?
    if (extraInfo.some(p => typeof p == 'object')) {
      extraInfo = extraInfo.map(item => JSON.stringify(item));
    }

    formattedString = extraInfo.join(',');

    return formattedString;
  }
}
