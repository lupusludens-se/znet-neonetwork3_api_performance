export interface AdminNavigationInterface {
  icon: string;
  title: string;
  addButtonName: string;
  addButtonLink: string;
  addButtonDisable?: boolean;
  viewButtonName?: string;
  viewButtonLink?: string;
  secondButtonName?: string;
  secondButtonLink?: string;
  isSecondBadgeEnabled?: boolean;
  isExternalLink?: boolean;
  isSecondButtonRequest?: boolean;
  enableArrowIcon?: boolean;
}
