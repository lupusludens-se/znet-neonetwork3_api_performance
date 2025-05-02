import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { TaxonomyEnum } from 'src/app/core/enums/taxonomy.enum';
import { InitialDataInterface } from 'src/app/core/interfaces/initial-data.interface';
import { CommonService } from 'src/app/core/services/common.service';
import { FilterDataInterface } from 'src/app/shared/modules/filter/interfaces/filter-data.interface';
import { SolutionsResourcesInterface } from '../../../../interfaces/solution.interface';
import { ActivityTypeEnum } from 'src/app/core/enums/activity/activity-type.enum';
import { AuthService } from 'src/app/core/services/auth.service';
import { ActivityService } from 'src/app/core/services/activity.service';

@Component({
  selector: 'neo-project-solutions-item-element',
  templateUrl: './project-solutions-item-element.component.html',
  styleUrls: ['./project-solutions-item-element.component.scss']
})
export class ProjectSolutionsItemElementComponent implements OnInit {
  @Input() category: SolutionsResourcesInterface;
  @Output() customEvent = new EventEmitter();
  tagCount: number;
  activityTypeEnum: any;
  auth = AuthService;
  totalResourceCount: number = 2;
  isPublicUser: boolean;

  ngOnInit(): void {
    this.activityTypeEnum = ActivityTypeEnum.ProjectResourceClick;
    this.isPublicUser = this.auth.isLoggedIn() || this.auth.needSilentLogIn();
  }
  constructor(
    private router: Router,
    private readonly commonService: CommonService,
    public authService: AuthService,
    private readonly activityService: ActivityService
  ) {}

  browse(): void {
    const categoriesFilter = this.commonService.filterState$.value.parameter[
      TaxonomyEnum.categories
    ] as FilterDataInterface[];
    const technologiesFilter = this.commonService.filterState$.value.parameter[
      TaxonomyEnum.technologies
    ] as FilterDataInterface[];
    const init = this.commonService.initialData$.value as InitialDataInterface;

    categoriesFilter.forEach(c => {
      c.checked = c.id == this.category.id;
    });

    const availableTechnologyNames = []
      .concat(...init.categories.filter(s => s.id === this.category.id).map(s => s.technologies))
      .map(s => s.name.toLowerCase());

    technologiesFilter.forEach(t => {
      t.disabled = !availableTechnologyNames.includes(t.name?.toLowerCase());
    });

    this.router.navigate(['projects']).then();
  }
  getResourceLength(totalResource: number): boolean {
    if (totalResource > this.totalResourceCount) {
      this.tagCount = totalResource - this.totalResourceCount;
      return true;
    } else {
      return false;
    }
  }
  activitytrack(categoryid: number): void {
    this.activityService
      .trackElementInteractionActivity(ActivityTypeEnum.SolutionTypes, { technologyId: categoryid })
      ?.subscribe();
  }
}
