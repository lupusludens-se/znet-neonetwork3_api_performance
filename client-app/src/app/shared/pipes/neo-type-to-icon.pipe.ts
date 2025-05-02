import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'neoTypeToIcon'
})
export class NeoTypeToIconPipe implements PipeTransform {
  transform(value: string): string {
    switch (value) {
      case 'articles':
        return 'news';
      case 'audio':
        return 'audio';
      case 'market-brief':
        return 'graph';
      case 'pdf':
        return 'file';
      case 'video':
        return 'camera';
      case 'white-papers':
        return 'paper';
    }
  }
}
