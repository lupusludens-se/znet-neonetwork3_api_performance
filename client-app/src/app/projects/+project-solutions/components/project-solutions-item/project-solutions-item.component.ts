import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonApiEnum } from 'src/app/core/enums/common-api.enum';
import { BreadcrumbService } from 'src/app/core/services/breadcrumb.service';
import { HttpService } from 'src/app/core/services/http.service';
import { CategoryInterface } from '../../interfaces/category.interface';
import { CommonService } from '../../../../core/services/common.service';
import { SolutionsResourcesInterface } from '../../interfaces/solution.interface';
import { ResourceInterface } from '../../../../core/interfaces/resource.interface';
import { AuthService } from 'src/app/core/services/auth.service';
@Component({
  selector: 'neo-project-solutions-item',
  templateUrl: './project-solutions-item.component.html',
  styleUrls: ['./project-solutions-item.component.scss']
})
export class ProjectSolutionsItemComponent implements OnInit {
  categoryItems: ResourceInterface[];
  solutionId: number;
  title: string;
  description: string;
  contactUsModal: boolean;
  category: CategoryInterface[];
  solution: SolutionsResourcesInterface;
  auth = AuthService;
  isPublicUser: boolean;
  constructor(
    private readonly httpService: HttpService,
    private readonly breadcrumbService: BreadcrumbService,
    private readonly activatedRoute: ActivatedRoute,
    public readonly commonService: CommonService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.isPublicUser = this.auth.isLoggedIn() || this.auth.needSilentLogIn();
    this.activatedRoute.params.subscribe((s: Record<string, string>) => {
      this.solutionId = +s.id;
      this.httpService
        .get<SolutionsResourcesInterface[]>(
          CommonApiEnum.Solutions + `?filterBy=ids=${this.solutionId}&expand=resources`
        )
        .subscribe(solutionsList => {
          this.solution = solutionsList[0];
          this.categoryItems = solutionsList[0].categories;
        });
    });
  }
}
