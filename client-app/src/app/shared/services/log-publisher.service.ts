import { Injectable } from '@angular/core';
import { LogPublisherInterface } from '../interfaces/logging/log-publisher.interface';
import { LoggingConfig } from 'src/app/core/configs/logging.config';
import { ConsoleLogPublisher } from '../interfaces/logging/console-log-publisher';
import { LogPublisherEnum } from '../enums/log-publisher.enum';
import { HttpService } from 'src/app/core/services/http.service';
import { WebApiLogPublisher } from '../interfaces/logging/web-api-log-publisher';

@Injectable()
export class LogPublishersService {
  readonly publishers: LogPublisherInterface[] = [];

  constructor(private readonly httpService: HttpService) {
    this.publishers = this.getPublishers();
  }

  private getPublishers(): LogPublisherInterface[] {
    const publishers: LogPublisherInterface[] = [];

    for (let publisher of LoggingConfig.publishers) {
      switch (publisher) {
        case LogPublisherEnum.Console:
          publishers.push(new ConsoleLogPublisher());
          break;

        case LogPublisherEnum.WebApi:
          publishers.push(new WebApiLogPublisher(this.httpService));
          break;
      }
    }

    return publishers;
  }
}
