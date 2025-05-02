import { PostTypeEnum } from '../../core/enums/post-type.enum';
import { TagInterface } from '../../core/interfaces/tag.interface';
import { TagParentInterface } from '../../core/interfaces/tag-parent.interface';

export interface PostInterface {
  id: number;
  date: Date;
  slug: string;
  articleType: string;
  title: string;
  content: string;
  categories: TagParentInterface[];
  solutions: TagInterface[];
  typeId: PostTypeEnum;
  technologies: TagInterface[];
  regions: TagParentInterface[];
  tags: number[];
  imageUrl: string;
  videoUrl: string;
  pdfUrl: string;
  isSaved: boolean;
  isPublicArticle: boolean;
  postTags?: { id: number; name: string; taxonomy: string }[];
  regionTags?: { id: number; name: string }[];
  tagsTotalCount?: number;
}

export interface NewAndNoteworthyPostInterface {
  id: number;
  title: string;
  isPublicArticle?: boolean;
}
