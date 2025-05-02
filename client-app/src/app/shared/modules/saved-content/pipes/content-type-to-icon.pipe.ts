import { Pipe, PipeTransform } from '@angular/core';
import { SavedContentTypeEnum } from '../enums/saved-content-type.enum';

@Pipe({
  name: 'contentTypeIcon'
})
export class ContentTypeToIconPipe implements PipeTransform {
  transform(value: number): string {
    switch (value) {
      case SavedContentTypeEnum.Project:
        return 'projects';
      case SavedContentTypeEnum.Article:
        return 'learn';
      case SavedContentTypeEnum.Tool:
        return 'tools';
      case SavedContentTypeEnum.Forum:
        return 'forum';
      default:
        return 'star';
    }
  }
}
