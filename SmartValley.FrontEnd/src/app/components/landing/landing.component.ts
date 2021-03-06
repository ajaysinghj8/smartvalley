import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ScoredProject} from '../../api/expert/scored-project';
import {ProjectQuery} from '../../api/project/project-query';
import {ProjectsOrderBy} from '../../api/application/projects-order-by.enum';
import {ProjectResponse} from '../../api/project/project-response';
import {SortDirection} from '../../api/sort-direction.enum';

@Component({
  selector: 'app-root',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnInit {

  public projects: ScoredProject[];
  public projectsLink: string;

  constructor(private router: Router,
              private projectApiClient: ProjectApiClient) {
  }

  public async ngOnInit() {
    this.projectsLink = Paths.ProjectList;
    const projectResponse = await this.projectApiClient.getAsync(<ProjectQuery>{
      offset: 0,
      count: 10,
      onlyScored: false,
      orderBy: ProjectsOrderBy.CreationDate,
      direction: SortDirection.Descending
    });
    this.projects = projectResponse.items.map(p => this.createScoredProject(p));
  }

  public navigateToProject(id) {
    this.router.navigate([Paths.Project + '/' + id]);
  }

  public navigateToProjectsList() {
    this.router.navigate(['/' + this.projectsLink]);
  }

  private createScoredProject(response: ProjectResponse): ScoredProject {
    return <ScoredProject> {
      id: response.id,
      address: response.scoring ? response.scoring.contractAddress : '',
      category: response.category,
      country: response.country,
      description: response.description,
      name: response.name,
      score: response.scoring ? response.scoring.score : '',
      scoringEndDate: response.scoring ? response.scoring.scoringEndDate : ''
    };
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Project + '/' + id]).toString()
    );
  }
}
