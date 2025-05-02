import { Directive, EventEmitter, HostListener, Input, Output } from '@angular/core';
const scrollEventName = 'scroll';
const scrollEventArgs = ['$event.target.scrollTop', '$event.target.scrollHeight', '$event.target.offsetHeight'];
@Directive({
  selector: '[neoInfiniteSectionScroll]'
})
export class InfiniteSectionScrollDirective {
  @Input() public infiniteScrollThreshold: string;
  @Input() public infiniteScrollDisabled = false;
  @Output() public loadMoreData = new EventEmitter<void>();

  private didFire = false;

  @HostListener(scrollEventName, scrollEventArgs)
  public onScroll(scrollTop: number, scrollHeight: number, offsetHeight: number): void {
    if (this.infiniteScrollDisabled) {
      return;
    }
    if (Math.ceil(offsetHeight + scrollTop) >= scrollHeight) {
      if (!this.didFire) {
        this.didFire = true;
        this.loadMoreData.emit();
      }
    } else {
      if (this.didFire) {
        this.didFire = false;
      }
    }
  }
}
