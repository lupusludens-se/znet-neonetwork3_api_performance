import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UserInterface } from '../../interfaces/user/user.interface';
import { AuthService } from 'src/app/core/services/auth.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'neo-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  @Input() removeLogo: boolean;
  @Input() forRegistration: boolean = false;
  @Input() termOfUseModal: boolean;
  @Input() privacyPolicyModal: boolean;

  @Output() termOfUseModalClosed: EventEmitter<void> = new EventEmitter<void>();
  @Output() privacyPolicyModalClosed: EventEmitter<void> = new EventEmitter<void>();

  currentUser: UserInterface;
  auth = AuthService;

  year: number = new Date().getFullYear();

  constructor(private readonly authService: AuthService) { }

  ngOnInit(): void {
    this.authService
      .currentUser()
      .pipe(untilDestroyed(this))
      .subscribe((user: UserInterface) => {
        this.currentUser = user;
      });
  }
}
