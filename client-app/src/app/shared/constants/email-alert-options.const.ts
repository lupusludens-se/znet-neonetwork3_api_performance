import { TagInterface } from '../../core/interfaces/tag.interface';
import { EmailAlertFrequencyEnum } from '../../+admin/modules/+email-alerts/models/email-alert';

export const DROPDOWN_OPTIONS: TagInterface[] = [
  {
    id: EmailAlertFrequencyEnum.Off,
    name: 'settings.offLabel'
  },
  {
    id: EmailAlertFrequencyEnum.Immediately,
    name: 'settings.immediatelyLabel'
  },
  {
    id: EmailAlertFrequencyEnum.Daily,
    name: 'settings.dailyLabel'
  },
  {
    id: EmailAlertFrequencyEnum.Weekly,
    name: 'settings.weeklyLabel'
  },
  {
    id: EmailAlertFrequencyEnum.Monthly,
    name: 'settings.monthlyLabel'
  }
];
