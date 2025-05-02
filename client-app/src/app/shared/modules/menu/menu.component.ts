import {
  AfterViewChecked,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  Renderer2
} from '@angular/core';
import { MenuOptionInterface } from './interfaces/menu-option.interface';

@Component({
  selector: 'neo-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit, AfterViewChecked {
  @Input() icon: string;
  @Input() activeIcon: string;
  @Input() cssClasses: string;
  @Input() options: MenuOptionInterface[];
  @Output() optionClick: EventEmitter<MenuOptionInterface> = new EventEmitter<MenuOptionInterface>();
  menuActive: boolean = false;
  menuBtnClick: boolean = false;

  constructor(private readonly renderer: Renderer2, private changeDetector: ChangeDetectorRef) {}

  public ngOnInit(): void {
    this.listenForWindowClick();
  }

  public ngAfterViewChecked(): void {
    this.changeDetector.detectChanges();
  }

  private listenForWindowClick(): void {
    this.renderer.listen('window', 'click', () => {
      if (!this.menuBtnClick) {
        this.menuActive = false;
      }
      this.menuBtnClick = false;
    });
  }
}
