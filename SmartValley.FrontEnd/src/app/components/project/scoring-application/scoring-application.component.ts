import {Component, ElementRef, Inject, Input, OnInit} from '@angular/core';
import {Paths} from '../../../paths';
import {Router} from '@angular/router';
import {ScoringApplicationApiClient} from '../../../api/scoring-application/scoring-application-api-client';
import {isNullOrUndefined} from 'util';
import {ProjectApplicationInfoResponse} from '../../../api/scoring-application/project-application-info-response';
import {ScoringApplicationPartition} from '../../../api/scoring-application/scoring-application-partition';
import {QuestionControlType} from '../../../api/scoring-application/question-control-type.enum';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {UserContext} from '../../../services/authentication/user-context';
import {ProjectSummaryResponse} from '../../../api/project/project-summary-response';
import {ProjectComponent} from '../project.component';
import {ScoringStatus} from '../../../services/scoring-status.enum';

@Component({
  selector: 'app-scoring-application',
  templateUrl: './scoring-application.component.html',
  styleUrls: ['./scoring-application.component.scss']
})
export class ScoringApplicationComponent implements OnInit {

  @Input() public project: ProjectSummaryResponse;
  @Input() scoringStatus: ScoringStatus;

  public projectInfo: ProjectApplicationInfoResponse;
  public haveSocials: boolean;
  public articles: string[];
  public partitions: ScoringApplicationPartition[];
  public questionControlType = QuestionControlType;
  public questionTypeLine = QuestionControlType[0];
  public questionTypeMultiLine = QuestionControlType[1];
  public questionTypeCombobox = QuestionControlType[2];
  public questionTypeDateTime = QuestionControlType[3];
  public questionTypeCheckBox = QuestionControlType[4];
  public questionTypeUrl = QuestionControlType[5];
  public activePartition = '';

  public doesScoringApplicationExists: boolean;
  public isAuthor: boolean;
  public isScoringPayed: boolean;
  public editorFormats = [
      'bold',
      'underline',
      'strike',
      'header',
      'italic',
      'list',
      'indent',
      'color',
      'align',
      'background'
  ];
  public editorOptions = {
      toolbar: {
          container:
              [
                  ['bold', 'italic', 'underline', 'strike'],
                  [{ 'header': 1 }, { 'header': 2 }],
                  [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                  [{ 'indent': '-1' }, { 'indent': '+1' }],
                  [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
                  [{ 'color': [] }, { 'background': [] }],
                  [{ 'align': [] }],

              ]
      },
      clipboard: {
          matchVisual: false
      }
  };

  constructor(@Inject(ProjectComponent) public parent: ProjectComponent,
              private router: Router,
              private htmlElement: ElementRef,
              private scoringApplicationApiClient: ScoringApplicationApiClient,
              private projectApiClient: ProjectApiClient,
              private userContext: UserContext) {
  }

  async ngOnInit() {
    const response = await this.scoringApplicationApiClient.getScoringApplicationsAsync(this.project.id);

    if (!isNullOrUndefined(response.created)) {
      this.projectInfo = response.projectInfo;
      this.articles = JSON.parse(this.projectInfo.articles);
      this.haveSocials = this.checkSocials();
      this.partitions = response.partitions;
      this.doesScoringApplicationExists = true;

      this.isScoringPayed = this.project.scoring.id !== null;
    }

    const project = await this.projectApiClient.getProjectSummaryAsync(this.project.id);
    const currentUser = await this.userContext.getCurrentUser();

    if (!isNullOrUndefined(currentUser) && currentUser.id === project.authorId) {
      this.isAuthor = true;
    }
  }

  private checkSocials(): boolean {
    return !isNullOrUndefined(this.projectInfo.socialNetworks.bitcoinTalk) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.facebook) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.github) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.linkedin) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.medium) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.reddit) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.telegram) ||
      !isNullOrUndefined(this.projectInfo.socialNetworks.twitter)
      ;
  }

  public getQuestionTypeById(id: number) {
    return this.questionControlType[id];
  }

  public isParentQuestionAnswered(id: number, parentTriggerValue) {
    if (!id) {
      return true;
    }
    if (parentTriggerValue) {
      const parentQuestion = this.partitions.selectMany(i => i.questions).filter(i => i.id === id)[0];
      return +parentQuestion.answer === +parentTriggerValue;
    }
    return this.partitions.selectMany(i => i.questions).some(i => i.id === id);
  }

  public scrollToElement(elementId: string) {
    const element = this.htmlElement.nativeElement.querySelector('#partition_' + elementId);
    const containerOffset = element.offsetTop;
    const fieldOffset = element.offsetParent.offsetTop;
    window.scrollTo({left: 0, top: containerOffset + fieldOffset, behavior: 'smooth'});
  }

  public navigateToCreateForm() {
    this.router.navigate([Paths.ScoringApplication + '/' + this.project.id]);
  }

  public navigateToEdit() {
    this.router.navigate([Paths.ScoringApplication + '/' + this.project.id]);
  }

  public onAppear(event, id) {
      if (event.status) {
          this.activePartition = id;
      }
  }

  public isEditApplicationCommandAvailable(): boolean {
    const editApplicationCommandAvailableStates = [ScoringStatus.FillingApplication, ScoringStatus.ReadyForPayment];
    return this.isAuthor && editApplicationCommandAvailableStates.includes(this.scoringStatus);
  }
}
