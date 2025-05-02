import { Component, OnInit } from '@angular/core';

import { TechnologiesResourcesInterface } from './interfaces/technologies-resources.interface';
import { TitleService } from 'src/app/core/services/title.service';
import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { HttpService } from 'src/app/core/services/http.service';

@Component({
  selector: 'neo-project-technologies',
  templateUrl: './project-technologies.component.html',
  styleUrls: ['./project-technologies.component.scss']
})
export class ProjectTechnologiesComponent implements OnInit {
  contactUsModal: boolean;
  technologies: TechnologiesResourcesInterface[];
  showModal: boolean;

  constructor(private readonly httpService: HttpService, private titleService: TitleService) {}

  ngOnInit(): void {
    this.titleService.setTitle('projects.techonologies.techonologiesLabel');

    this.httpService
      .get<TechnologiesResourcesInterface[]>(CommonApiEnum.Technologies, { expand: 'resources' })
      .subscribe(technologiesList => {
        this.technologies = technologiesList;
      });
  }
}
