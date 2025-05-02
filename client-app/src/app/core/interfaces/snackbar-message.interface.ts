import { SnackbarTypeEnum } from '../enums/snackbar-type.enum';

export class SnackbarMessageInterface {
  show: boolean;
  type: SnackbarTypeEnum;
  message: string;
}
