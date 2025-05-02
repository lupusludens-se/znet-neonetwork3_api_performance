import { TagInterface } from '../../core/interfaces/tag.interface';
import { TagParentInterface } from '../../core/interfaces/tag-parent.interface';

export interface ContentInterface<T> {
  id: number;
  title: string;
  description: string;
  type: T;
  regions: TagParentInterface[];
  solutions: TagInterface[];
  technologies: TagInterface[];
  categories: TagParentInterface[];
  contentTags: TagParentInterface[];
  tags?: TagInterface[];
}
