import { LogEntry } from './log-entry';

export interface LogPublisherInterface {
  log(entry: LogEntry, isUnhandled?: boolean);
}
