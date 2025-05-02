export interface EventOccurrenceInterface {
  id?: number;
  fromDate: string;
  toDate: string;
  fromDateBrowser: string;
  toDateBrowser: string;
  isToday?: boolean;
  timeZoneName: string;
  timeZoneAbbr: string;
  timeZoneUtcOffset: number;
}
