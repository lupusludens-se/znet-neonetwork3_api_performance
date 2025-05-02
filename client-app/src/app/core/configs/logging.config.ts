import { LogLevel } from 'src/app/shared/enums/log-level-enum';
import { LogPublisherEnum } from 'src/app/shared/enums/log-publisher.enum';
import { environment } from 'src/environments/environment';

export const LoggingConfig = {
  level: LogLevel[environment.logging.level],
  publishers: environment.logging.publishers.map(publisherName => LogPublisherEnum[publisherName])
};
