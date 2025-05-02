import { LogLevel } from '../../enums/log-level-enum';

export class LogEntry {
  readonly message: string | null = null;
  readonly level: LogLevel;
  readonly dateTime: Date;
  readonly extraInfo: any[] | null = null;
  readonly error: Error;

  constructor(message: string | null, level: LogLevel, dateTime: Date, extraInfo: any[] | null, error: Error | null) {
    this.message = message;
    this.level = level;
    this.dateTime = dateTime;
    this.extraInfo = extraInfo;
    this.error = error;
  }
}
