import { Pipe, PipeTransform } from '@angular/core';
import { ResourceTypeEnum } from 'src/app/core/enums/resource-type.enum';

@Pipe({
  name: 'resourceTypeToIcon'
})
export class ResourceTypeToIconPipe implements PipeTransform {
  transform(value: ResourceTypeEnum): string {
    switch (value) {
      case ResourceTypeEnum.PDF:
        return 'file';
      case ResourceTypeEnum.Video:
        return 'camera';
      case ResourceTypeEnum.WebsiteLink:
        return 'news';
      case ResourceTypeEnum.QlikApplication:
        return 'topic';
      case ResourceTypeEnum.NativeTool:
        return 'wrench';
    }
  }
}
