import { SearchResultInterface } from '../../../../interfaces/search-result.interface';
import { ImageInterface } from '../../../../interfaces/image.interface';
import { UserRoleInterface } from '../../../../interfaces/user/user-role.interface';

export interface MultiselectOptionInterface extends SearchResultInterface {
  image?: ImageInterface;
  firstName?: string;
  lastName?: string;
  roles: UserRoleInterface[];
}
