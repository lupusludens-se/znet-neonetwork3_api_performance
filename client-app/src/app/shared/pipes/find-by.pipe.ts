import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'findBy'
})
export class FindByPipe implements PipeTransform {
  transform(arr: unknown[], value: unknown, prop: string) {
    return arr.find(el => el[prop] === value);
  }
}
