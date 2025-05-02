import { Component, Input, OnInit } from '@angular/core';
import { ResourceInterface } from '../../../../../../core/interfaces/resource.interface';
import { AuthService } from 'src/app/core/services/auth.service';
@Component({
  selector: 'neo-project-solution-tag',
  templateUrl: './project-solution-tag.component.html',
  styleUrls: ['./project-solution-tag.component.scss']
})
export class ProjectSolutionTagComponent implements OnInit {
  @Input() tag: ResourceInterface;
  auth = AuthService;
  isPublicUser: boolean;
  constructor(public authService: AuthService) {}
  ngOnInit(): void {
    this.isPublicUser = this.auth.isLoggedIn() || this.auth.needSilentLogIn();
  }
}
