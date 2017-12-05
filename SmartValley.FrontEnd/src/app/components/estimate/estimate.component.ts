import {Component, QueryList, ViewChildren, ElementRef} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {QuestionService} from '../../services/question-service';
import {Paths} from '../../paths';
import {SubmitEstimatesRequest} from '../../api/estimates/submit-estimates-request';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {AuthenticationService} from '../../services/authentication-service';
import {EstimateRequest} from '../../api/estimates/estimate-request';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {TeamMember} from '../../services/team-member';

@Component({
  selector: 'app-estimate',
  templateUrl: './estimate.component.html',
  styleUrls: ['./estimate.component.css']
})
export class EstimateComponent {
  public hidden: boolean;
  public EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;
  public expertiseArea: ExpertiseArea;
  public projectId: number;
  public projectDetails: ProjectDetailsResponse;
  public estimateForm: FormGroup;
  public teamMembers: Array<TeamMember>;

  @ViewChildren('required') requireds: QueryList<any>;

  constructor(private route: ActivatedRoute,
              private projectApiClient: ProjectApiClient,
              private questionService: QuestionService,
              private router: Router,
              private estimatesApiClient: EstimatesApiClient,
              private authenticationService: AuthenticationService,
              private formBuilder: FormBuilder) {
    this.loadProjectInfo();
  }

  changeHidden() {
    this.hidden = true;
  }

  async send() {
    if (this.estimateForm.invalid) {
      const invalidElements = this.requireds.filter(i => i.nativeElement.classList.contains('ng-invalid'));
      if (invalidElements.length > 0) {
        for (let a = 0; a < invalidElements.length; a++) {
          this.setInvalid(invalidElements[a]);
        }
        this.scrollToElement(invalidElements[0]);
      }

    } else {
      await this.submitAsync();
      await this.router.navigate([Paths.Scoring]);
    }
  }

  private async submitAsync(): Promise<void> {
    const estimates = this.getEstimates();
    const submitEstimatesRequest = <SubmitEstimatesRequest>{
      projectId: this.projectId,
      expertiseArea: this.expertiseArea,
      expertAddress: this.authenticationService.getCurrentUser().account,
      estimates: estimates
    };

    await this.estimatesApiClient.submitEstimatesAsync(submitEstimatesRequest);
  }

  private setInvalid(element: ElementRef) {
    element.nativeElement.classList.add('ng-invalid');
    element.nativeElement.classList.add('ng-dirty');
  }

  private scrollToElement(element: ElementRef) {
    const offsetTop1 = element.nativeElement.offsetTop;
    const offsetTop3 = element.nativeElement.offsetParent.offsetParent.offsetTop;
    const offsetTop4 = element.nativeElement.offsetParent.offsetParent.offsetParent.offsetTop;
    window.scrollTo({left: 0, top: offsetTop1 + offsetTop3 + offsetTop4 , behavior: 'smooth'});
  }

  private getEstimates(): Array<EstimateRequest> {
    const estimates: Array<EstimateRequest> = [];
    const formModel = this.estimateForm.value;

    for (const question of formModel.questions) {
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
    this.expertiseArea = +this.route.snapshot.queryParamMap.get('category');

    const questionsFormGroups = [];
    const questions = await this.questionService.getByExpertiseArea(this.expertiseArea);

    for (const question of questions) {
      const group = this.formBuilder.group({
        name: question.name,
        description: question.description,
        maxScore: question.maxScore,
        score: ['', [
          Validators.required,
          Validators.max(question.maxScore),
          Validators.min(question.minScore),
          Validators.pattern('^-*[0-9]+$')]],
        comments: ['', [Validators.required, Validators.maxLength(250)]],
      });
      questionsFormGroups.push(group);
    }

    this.estimateForm = this.formBuilder.group({questions: this.formBuilder.array(questionsFormGroups)});
    this.projectDetails = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
    this.teamMembers = this.getMembersCollection(this.projectDetails);
  }

  private getMembersCollection(report: ProjectDetailsResponse): Array<TeamMember> {
    const result: TeamMember[] = [];
    const memberTypeNames = Object.keys(EnumTeamMemberType).filter(key => !isNaN(Number(EnumTeamMemberType[key])));

    for (const memberType of memberTypeNames) {
      const teamMember = report.teamMembers.find(value => value.memberType === EnumTeamMemberType[memberType])
        || <TeamMember>{memberType: EnumTeamMemberType[memberType], fullName: '\u2014'};

      result.push(<TeamMember>{
        memberType: teamMember.memberType,
        facebookLink: teamMember.facebookLink,
        linkedInLink: teamMember.linkedInLink,
        fullName: teamMember.fullName
      });
    }
    return result;
  }
}
