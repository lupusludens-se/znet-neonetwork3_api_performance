import { TagInterface } from './tag.interface';
import { TagParentInterface } from './tag-parent.interface';

export interface InitialDataInterface {
  categories: TagInterface[];
  technologies: TagInterface[];
  regions: TagParentInterface[];
  solutions: TagInterface[];
  roles?: TagInterface[];
  statuses: TagInterface[];
  industries: TagInterface[];
  projectCapabilities: TagInterface[];
}
