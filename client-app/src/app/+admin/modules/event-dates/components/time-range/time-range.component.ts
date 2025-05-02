import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { EventTimeInterface } from '../../../+create-event/interfaces/event-time.interface';
import { HourInterface } from '../../../../interfaces/hour.interface';
import { HOUR_STEP } from '../../../../constants/events.constants';

@Component({
  selector: 'neo-time-range',
  templateUrl: 'time-range.component.html',
  styleUrls: ['time-range.component.scss']
})
export class TimeRangeComponent implements OnInit {
  @Input() eventTime: EventTimeInterface = {
    start: {
      time: '12:00',
      dayPart: 'am',
      minutes: 0
    },
    end: {
      time: '1:00',
      dayPart: 'am',
      minutes: 60
    }
  };

  @Input() disabled: boolean;

  @Output() timeUpdated: EventEmitter<EventTimeInterface> = new EventEmitter<EventTimeInterface>();
  @Output() removeTimeSlot: EventEmitter<boolean> = new EventEmitter<boolean>();

  showStartList: boolean;
  showEndList: boolean;
  form: FormGroup;

  // by default time range is 12:00 AM to 1:00 AM
  startHourIndex = 0;

  private oneHourStep = HOUR_STEP;

  hoursList: HourInterface[] = Array(12 * this.oneHourStep)
    .fill({})
    .map((el, i: number) => {
      const hh = Math.floor(i / this.oneHourStep);
      const mm = (i % this.oneHourStep) * (60 / this.oneHourStep);
      return {
        id: i + 1,
        name: `${hh > 0 ? hh : '12'}:${mm > 0 ? mm : '00'}`,
        minutes: hh * 60 + mm
      };
    });

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.form = this.formBuilder.group({
      start: [{ value: this.eventTime.start.time, disabled: this.disabled }, []],
      end: [{ value: this.eventTime.end.time, disabled: this.disabled }, []]
    });
  }

  changeTime(hour: HourInterface, type: string, hourIndex?: number) {
    this.eventTime[type].time = hour.name;
    this.eventTime[type].minutes = hour.minutes;

    if (hourIndex >= 0) {
      this.startHourIndex = hourIndex;

      if (this.eventTime.start.dayPart === this.eventTime.end.dayPart) {
        this.checkTimeRange();
      }
    }

    this.form.patchValue({ [type]: hour.name });
    this.timeUpdated.emit(this.eventTime);
  }

  setDayPart(propName: 'start' | 'end', dayPart: 'am' | 'pm') {
    this.eventTime[propName]['dayPart'] = dayPart;

    if (propName === 'start' && dayPart === 'pm') {
      this.eventTime.end.dayPart = 'pm';
      this.checkTimeRange();
    } else if (propName === 'end' && dayPart === 'am') {
      this.eventTime.start.dayPart = 'am';
      this.checkTimeRange();
    }

    this.timeUpdated.emit(this.eventTime);
  }

  toggleStartList(): void {
    if (!this.disabled) {
      this.showStartList = !this.showStartList;
    }
  }

  toggleEndList(): void {
    if (!this.disabled) {
      this.showEndList = !this.showEndList;
    }
  }

  closeStartListOnOutside(): void {
    if (this.showStartList) {
      this.showStartList = false;
    }
  }

  closeEndListOnOutside(): void {
    if (this.showEndList) {
      this.showEndList = false;
    }
  }

  showEndListItem(hour: HourInterface): boolean {
    return (
      this.eventTime.start.dayPart !== this.eventTime.end.dayPart ||
      (this.eventTime.start.dayPart === this.eventTime.end.dayPart && hour.minutes >= this.eventTime.start.minutes)
    );
  }

  private checkTimeRange(): void {
    if (this.eventTime.start.minutes >= 0 && this.eventTime.end.minutes >= 0) {
      for (let i = HOUR_STEP; i >= 0; i--) {
        if (this.hoursList[this.startHourIndex + i]) {
          this.eventTime.end.time = this.hoursList[this.startHourIndex + i].name;
          this.eventTime.end.minutes = this.hoursList[this.startHourIndex + i].minutes;
          this.form.patchValue({ end: this.hoursList[this.startHourIndex + i].name });
          break;
        }
      }
    }
  }
}
