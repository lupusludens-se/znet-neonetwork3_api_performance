import { Component, OnInit } from '@angular/core';

import { SolutionsResourcesInterface } from './interfaces/solution.interface';
import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { TitleService } from 'src/app/core/services/title.service';
import { HttpService } from 'src/app/core/services/http.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { SignTrackingSourceEnum } from '../../core/enums/sign-tracking-source-enum';

@Component({
  selector: 'neo-project-solutions',
  templateUrl: './project-solutions.component.html',
  styleUrls: ['./project-solutions.component.scss']
})
export class ProjectSolutionsComponent implements OnInit {
  solutionItems: SolutionsResourcesInterface[];
  contactUsModal: boolean;
  showModal: boolean;
  solutions: SolutionsResourcesInterface[];
  auth = AuthService;
  isPublicUser: boolean;
  signTrackingSourceEnum = SignTrackingSourceEnum.ZeigoNetwork;
  constructor(private readonly httpService: HttpService, private titleService: TitleService) {}

  ngOnInit(): void {
    this.isPublicUser = !(this.auth.isLoggedIn() || this.auth.needSilentLogIn());
    this.httpService.get<SolutionsResourcesInterface[]>(CommonApiEnum.Solutions).subscribe(solutionsList => {
      this.solutions = solutionsList;
    });

    this.titleService.setTitle(this.isPublicUser ? 'title.projectsLabel' : 'projects.solutions.solutionsLabel');
  }
}
