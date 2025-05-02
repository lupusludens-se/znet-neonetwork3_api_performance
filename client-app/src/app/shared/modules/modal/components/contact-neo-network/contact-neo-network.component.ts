import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { HttpService } from 'src/app/core/services/http.service';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';

@UntilDestroy()
@Component({
  selector: 'neo-contact-neo-network',
  templateUrl: './contact-neo-network.component.html',
  styleUrls: ['./contact-neo-network.component.scss']
})
export class ContactNeoNetworkComponent implements OnInit {
  @Output() closed = new EventEmitter<Record<string, string>>();

  isMessageSent: boolean = false;
  currentUser: UserInterface;
  message: string;
  isMessage: boolean = false;
  requiredLength: number = 4000;
  constructor(private readonly httpService: HttpService, private readonly authService: AuthService) { }

  ngOnInit(): void {
    this.getCurrentUser();
  }

  getCurrentUser(): void {
    this.authService.currentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
    });
  }

  sendMessage(): void {
    this.isMessage = true;
    if (this.message?.length <= this.requiredLength) {
      this.httpService
        .post('conversations/' + CommonApiEnum.ContactUs, {
          subject: 'Contact Zeigo Network Message',
          message: this.message
        })
        .pipe(untilDestroyed(this))
        .subscribe(() => {
          this.isMessageSent = true;
        });
    }
  }

  validateInput(): boolean {
    return !(this.message && this.message.trim().length > 0);
  }
}
