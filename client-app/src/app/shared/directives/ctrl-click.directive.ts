import { Directive, ElementRef, EventEmitter, OnDestroy, OnInit, Output, Renderer2 } from '@angular/core';

@Directive({
  selector: '[neoCtrlClick]'
})
export class CtrlClickDirective implements OnInit, OnDestroy {
  private unsubscribe: any;
  @Output() ctrlClickEvent = new EventEmitter();

  constructor(private readonly renderer: Renderer2, private readonly element: ElementRef) {}

  ngOnInit() {
    this.unsubscribe = this.renderer.listen(this.element.nativeElement, 'click', event => {
      if (event.ctrlKey) {
        event.preventDefault();
        event.stopPropagation();
        document.getSelection().removeAllRanges();

        this.ctrlClickEvent.emit(true);
      } else {
        this.ctrlClickEvent.emit(false);
      }
    });
  }

  ngOnDestroy() {
    if (!this.unsubscribe) {
      return;
    }
    this.unsubscribe();
  }
}
