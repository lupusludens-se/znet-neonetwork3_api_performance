export interface UserEventRoleInterface {
  id: number;
  name: string;
  preSelected: boolean;
  checked: boolean;
}

export const UserEventRoleConst: UserEventRoleInterface[] = [
  { id: 2, name: 'Corporations', preSelected: false, checked: false },
  { id: 4, name: 'Internal', preSelected: false, checked: false },
  { id: 3, name: 'Solution Providers', preSelected: false, checked: false }
];
