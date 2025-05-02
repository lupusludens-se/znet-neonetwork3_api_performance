import { Component, EventEmitter, HostListener, OnInit, Output } from '@angular/core';

@Component({
  selector: 'neo-scroll-loader',
  templateUrl: './scroll-loader.component.html',
  styleUrls: ['./scroll-loader.component.scss']
})
export class ScrollLoaderComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}
  Page = 0;
  @Output() loadMoreData: EventEmitter<number> = new EventEmitter<number>();
  @HostListener('window:scroll', ['$event'])
  onScroll() {
    if (this.isScrolledToBottom()) {
      this.loadMoreData.emit(++this.Page);
    }
  }
  private isScrolledToBottom(): boolean {
    if (Math.ceil(window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
      return true;
    } else {
      return false;
    }
  }
}
