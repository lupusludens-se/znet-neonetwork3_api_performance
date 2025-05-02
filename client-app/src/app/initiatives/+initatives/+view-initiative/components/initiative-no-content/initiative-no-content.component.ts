import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'neo-initiative-no-content',
  templateUrl: './initiative-no-content.component.html',
  styleUrls: ['./initiative-no-content.component.scss']
})
export class InitiativeNoContentComponent implements OnInit {
  @Input() section: string;
  @Input() subTitleSection: string;
  @Input() isAdminOrTeamMemberTemplate: boolean = false;

  constructor() { }

  ngOnInit() { }
}
