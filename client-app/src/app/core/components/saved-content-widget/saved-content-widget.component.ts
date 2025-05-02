import { Component, OnInit, Renderer2 } from '@angular/core';
import { NavigationEnd, Router, RouterEvent } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { filter } from 'rxjs';

@UntilDestroy()
@Component({
  selector: 'neo-saved-content-widget',
  templateUrl: './saved-content-widget.component.html',
  styleUrls: ['./saved-content-widget.component.scss']
})
export class SavedContentWidgetComponent implements OnInit {
  menuOpen: boolean;
  private menuBtnClick: boolean;
  isSavedContentPage: boolean;
  constructor(private readonly renderer: Renderer2, private readonly router: Router) {}

  ngOnInit(): void {
    this.listenForNavigationEnd();
    this.listenForWindowClick();

    this.router.events.pipe(untilDestroyed(this)).subscribe((val: RouterEvent) => {
      if (val instanceof NavigationEnd) {
        this.isSavedContentPage = this.router.url === '/saved-content';
      }
    });

    this.isSavedContentPage = this.router.url === '/saved-content';
  }

  toggleMenu(): void {
    if (!this.isSavedContentPage) {
      this.menuOpen = !this.menuOpen;
    }
  }

  get isActive(): boolean {
    return this.menuOpen || location.pathname.includes('saved-content');
  }

  preventCloseOnClick(): void {
    this.menuBtnClick = true;
  }

  private listenForNavigationEnd(): void {
    this.router.events
      .pipe(
        untilDestroyed(this),
        filter(event => event instanceof NavigationEnd)
      )
      .subscribe(() => {
        if (!this.menuBtnClick) {
          this.menuOpen = false;
        }

        this.menuBtnClick = false;
      });
  }

  private listenForWindowClick(): void {
    this.renderer.listen('window', 'click', () => {
      if (!this.menuBtnClick) {
        this.menuOpen = false;
      }

      this.menuBtnClick = false;
    });
  }
}
