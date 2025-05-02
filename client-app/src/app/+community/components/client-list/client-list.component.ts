import { Component, OnInit } from '@angular/core';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';

@Component({
  selector: 'neo-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.scss']
})
export class ClientListComponent implements OnInit {
  activityTypeEnum: any;
  constructor() {}
  ngOnInit(): void {
    this.activityTypeEnum = ActivityTypeEnum.CompanyProfileView;
  }

  clients: Record<string, string>[] = [
    {
      companyName: 'Alight',
      companyLogo: 'assets/images/landing/clients/10.png'
    },
    {
      companyName: 'Avery Dennison',
      companyLogo: 'assets/images/landing/clients/1.png'
    },
    {
      companyName: 'Ball Corporation',
      companyLogo: 'assets/images/landing/clients/2.png'
    },
    {
      companyName: 'Blue Scope',
      companyLogo: 'assets/images/landing/clients/3.png'
    },
    {
      companyName: 'EDP Renewables',
      companyLogo: 'assets/images/landing/clients/EDP_Renewables.png'
    },
    {
      companyName: 'Improv Engineers',
      companyLogo: 'assets/images/landing/clients/4.png'
    },
    {
      companyName: 'InterXion',
      companyLogo: 'assets/images/landing/clients/12.png'
    },
    {
      companyName: 'Lockheed Martin',
      companyLogo: 'assets/images/landing/clients/5.png'
    },
    {
      companyName: 'NSG Group',
      companyLogo: 'assets/images/landing/clients/6.png'
    },
    {
      companyName: 'The J.M. Smucker Co.',
      companyLogo: 'assets/images/landing/clients/8.png'
    }
  ];
}
