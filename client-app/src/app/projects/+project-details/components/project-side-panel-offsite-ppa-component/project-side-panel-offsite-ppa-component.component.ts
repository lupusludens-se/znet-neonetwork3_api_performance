import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { ProjectTypesSteps } from 'src/app/projects/+add-project/enums/project-types-name.enum';
import { RolesEnum } from 'src/app/shared/enums/roles.enum';
import { ProjectOffsitePpaInterface } from 'src/app/shared/interfaces/projects/project-creation.interface';
import { ProjectInterface } from 'src/app/shared/interfaces/projects/project.interface';
import { UserInterface } from 'src/app/shared/interfaces/user/user.interface';

@Component({
  selector: 'neo-project-side-panel-offsite-ppa-component',
  templateUrl: './project-side-panel-offsite-ppa-component.component.html',
  styleUrls: ['../project-side-panel/project-side-panel.component.scss']
})
export class ProjectSidePanelOffsitePpaComponentComponent implements OnInit {
  @Input() projectDetails: ProjectInterface;
  technologiesList: string;
  valueToOfftaker: string;
  showPrivateInfo: boolean = false;
  currentUser$: Observable<UserInterface> = this.authService.currentUser();
  projectTypes = ProjectTypesSteps;
  roles = RolesEnum;
  enablePrivateView: boolean = false;
  constructor(public authService: AuthService) {}

  ngOnInit(): void {
    const currentUser: UserInterface = { ...this.authService.currentUser$.getValue() };
    this.enablePrivateView =
      currentUser.roles.find(x => x.id === this.roles.SolutionProvider) !== undefined &&
      this.projectDetails?.companyId === currentUser.companyId
        ? true
        : false;

    const details = this.projectDetails?.projectDetails as ProjectOffsitePpaInterface;

    this.technologiesList = this.projectDetails?.technologies?.map(t => t.name).join(', ');
    this.valueToOfftaker = details.valuesToOfftakers.map(v => v.name).join(',');
  }

  openPrivateInfoModal() {
    this.showPrivateInfo = true;
  }

  closePrivateInfoModal(): void {
    this.showPrivateInfo = false;
  }
}
