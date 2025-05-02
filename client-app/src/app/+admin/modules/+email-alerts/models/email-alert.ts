export interface EmailAlertInterface {
  id: number;
  title: string;
  description: string;
  frequency: EmailAlertFrequencyEnum;
}

export interface EmailAlertRequestInterface {
  id: number;
  frequency: EmailAlertFrequencyEnum;
}

export enum EmailAlertFrequencyEnum {
  Off,
  Immediately,
  Daily,
  Weekly,
  Monthly
}

export enum EmailAlertApiEnum {
  EmailAlerts = 'email-alerts',
  UsersEmailAlerts = 'users/current/email-alerts'
}
