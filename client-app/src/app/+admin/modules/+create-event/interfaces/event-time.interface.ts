export interface EventTimeInterface {
  start: {
    time: string;
    dayPart: string;
    minutes?: number;
  };
  end: {
    time: string;
    dayPart: string;
    minutes?: number;
  };
}
