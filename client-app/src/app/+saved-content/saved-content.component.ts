import { Component } from '@angular/core';

import { UntilDestroy } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'neo-saved-content',
  templateUrl: './saved-content.component.html',
  styleUrls: ['../../assets/styles/topics-and-saved-content.scss']
})
export class SavedContentComponent {}
