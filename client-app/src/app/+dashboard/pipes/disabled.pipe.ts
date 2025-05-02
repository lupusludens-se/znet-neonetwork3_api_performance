import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'disableCarousel' })
export class DisablePipe implements PipeTransform {
  // Maybe you need additional params
  transform(noOfSlides: number[], position: number) {
    // Do you loop over the array
    let filteredArray = noOfSlides.filter(x => x === position);
    if (filteredArray == null || filteredArray?.length == 0) {
      return true;
    }
  }
}
