import {Component} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {Application} from '../../services/application';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {QuestionService} from '../../services/question-service';
import {Question} from '../../services/question';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {SubmitEstimatesRequest} from '../../api/estimates/submit-estimates-request';
import {EnumExpertType} from '../../services/enumExpertType';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {AuthenticationService} from '../../services/authentication-service';
import {ApplicationApiClient} from '../../api/application/application-api.client';
import {EstimateRequest} from '../../api/estimates/estimate-request';

@Component({
  selector: 'app-estimate',
  templateUrl: './estimate.component.html',
  styleUrls: ['./estimate.component.css']
})
export class EstimateComponent {
  hidden: boolean;
  EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;
  expertType: EnumExpertType;
  projectId: number;

  public application: Application;
  public questions: Array<Question>;

  constructor(private route: ActivatedRoute,
              private applicationApiClient: ApplicationApiClient,
              private questionService: QuestionService,
              private router: Router,
              private estimatesApiClient: EstimatesApiClient,
              private authenticationService: AuthenticationService) {
    this.loadProjectInfo();
  }

  changeHidden() {
    this.hidden = true;
  }

  async send() {
    await this.submitAsync();
    await this.router.navigate([Paths.Scoring]);
  }

  private async submitAsync(): Promise<void> {
    const estimates = this.getEstimates();
    const submitEstimatesRequest = <SubmitEstimatesRequest>{
      projectId: this.projectId,
      category: this.expertType,
      expertAddress: this.authenticationService.getCurrentUser().account,
      estimates: estimates
    };

    await this.estimatesApiClient.submitEstimatesAsync(submitEstimatesRequest);
  }

  private getEstimates(): Array<EstimateRequest> {
    const estimates: Array<EstimateRequest> = [];

    for (const question of this.questions) {
      estimates.push(<EstimateRequest>{
        questionIndex: question.indexInCategory,
        score: question.score,
        comment: question.comments
      });
    }

    return estimates;
  }

  private async loadProjectInfo() {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.expertType = EnumExpertType.Lawyer; // TODO
    this.questions = this.questionService.getByExpertType(this.expertType);
    this.application = await this.applicationApiClient.getByProjectIdAsync(this.projectId);
  }
}
