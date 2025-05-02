import { Injectable } from '@angular/core';
import { LogLevel } from '../enums/log-level-enum';
import { LogEntry } from '../interfaces/logging/log-entry';
import { LogPublisherInterface } from '../interfaces/logging/log-publisher.interface';
import { LogPublishersService } from './log-publisher.service';
import { LoggingConfig } from 'src/app/core/configs/logging.config';

@Injectable()
export class LogService {
  private readonly level: LogLevel = LoggingConfig.level;
  private publishers: LogPublisherInterface[];

  constructor(private publishersService: LogPublishersService) {
    this.publishers = this.publishersService.publishers;
  }

  trace(message: string, ...extraInfo: any[]) {
    this.writeToLog(message, LogLevel.Trace, extraInfo);
  }

  debug(message: string, ...extraInfo: any[]) {
    this.writeToLog(message, LogLevel.Debug, extraInfo);
  }

  information(message: string, ...extraInfo: any[]) {
    this.writeToLog(message, LogLevel.Information, extraInfo);
  }

  warning(message: string, error: Error | null = null, ...extraInfo: any[]) {
    this.writeToLog(message, LogLevel.Warning, extraInfo, error);
  }

  error(message: string | null, error: Error | null = null, ...extraInfo: any[]) {
    this.writeToLog(message, LogLevel.Error, extraInfo, error);
  }

  critical(message: string | null, error: Error | null = null, ...extraInfo: any[]) {
    this.writeToLog(message, LogLevel.Critical, extraInfo, error);
  }

  private shouldLog(level: LogLevel): boolean {
    return level !== LogLevel.None && level >= this.level;
  }

  private writeToLog(message: string | null, level: LogLevel, extraInfo: any[], error: Error | null = null) {
    if (!this.shouldLog(level)) {
      return;
    }

    const entry = new LogEntry(message, level, new Date(), extraInfo, error);

    for (let logger of this.publishers) {
      try {
        logger.log(entry, message === 'Unhandled error occurred.');
      } catch (logError) {
        /* do nothing; allow other loggers to have a shot at logging */
      }
    }
  }
}
