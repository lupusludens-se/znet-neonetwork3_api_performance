import { Component } from '@angular/core';

import { MEDIUM_BREAKPOINT, MEDIUM_CONTAINER_WIDTH, SMALL_CONTAINER_WIDTH } from '../../consts/landing-container.const';

@Component({
  selector: 'neo-clients-feedback',
  templateUrl: './clients-feedback.component.html',
  styleUrls: ['./clients-feedback.component.scss']
})
export class ClientsFeedbackComponent {
  index: number = 0;

  clientFeedbacks: Record<string, string>[] = [
    {
      name: 'Vinh Thông Le Nhut',
      position: 'InterXion',
      companyLogo: 'assets/images/landing/clients/interxion.png',
      feedback:
        'The Zeigo Network platform, the events, the connections, and the team – together they’re a powerful force for helping companies like InterXion make progress toward our strategy and objectives.'
    },
    {
      name: 'Kyle Hoppe',
      position: 'Lockheed Martin',
      companyLogo: 'assets/images/landing/clients/5.png',
      feedback:
        'While we’ve been looking at establishing a more ambitious decarbonization commitment, the Cleantech team and Zeigo Network platform have provided great insight into our analysis with market opportunities on VPPA’s to carbon offsets. Their expertise is much appreciated!'
    },
    {
      name: 'Christopher Goldsberry',
      position: 'Associate Director of Origination, EDP Renewables North America',
      companyLogo: 'assets/images/landing/clients/EDP_Renewables.png',
      feedback:
        'The Zeigo Network is at the top of its class in driving connections between market-leading companies and solution providers. EDPR has continually benefited from being a part of this community.'
    },
    {
      name: 'Daniel Granlycke',
      position: 'Chief Commercial Officer at Alight',
      companyLogo: 'assets/images/landing/clients/Alight_Lockup.png',
      feedback:
        'Zeigo Network Platform has been very valuable for Alight, creating opportunities to connect with C&I members and explore decarbonization solutions, as well as providing the latest market intelligence, allowing us to continuously develop our offer.'
    }
  ];

  get getOffset(): number {
    return window.innerWidth > MEDIUM_BREAKPOINT ? MEDIUM_CONTAINER_WIDTH : SMALL_CONTAINER_WIDTH;
  }
}
