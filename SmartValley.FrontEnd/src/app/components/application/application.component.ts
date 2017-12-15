import {Component, ViewChild, ElementRef, ViewChildren, QueryList} from '@angular/core';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {ApplicationApiClient} from '../../api/application/application-api.client';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Application} from '../../services/application';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {v4 as uuid} from 'uuid';
import {ProjectManagerContractClient} from '../../services/project-manager-contract-client';
import {ContractApiClient} from '../../api/contract/contract-api-client';
import {TokenReceivingService} from '../../services/token-receiving/token-receiving-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {NotificationsService} from 'angular2-notifications';
import {BalanceApiClient} from '../../api/balance/balance-api-client';
import {DialogService} from '../../services/dialog-service';
import {EtherReceivingService} from '../../services/ether-receiving/ether-receiving-service';
import {TranslateService} from '@ngx-translate/core';
import {BalanceService} from '../../services/balance/balance.service';
import {TokenContractClient} from '../../services/token-receiving/token-contract-client';

@Component({
  selector: 'app-application',
  templateUrl: './application.component.html',
  styleUrls: ['./application.component.css']
})
export class ApplicationComponent {

  applicationForm: FormGroup;
  isTeamShow = false;
  isLegalShow = false;
  isFinanceShow = false;
  isTechShow = false;

  @ViewChild('name') public nameRow: ElementRef;
  @ViewChild('area') public areaRow: ElementRef;
  @ViewChild('description') public descriptionRow: ElementRef;
  @ViewChildren('required') requireds: QueryList<any>;

  public isProjectCreating: boolean;

  constructor(private balanceApiClient: BalanceApiClient,
              private formBuilder: FormBuilder,
              private authenticationService: AuthenticationService,
              private contractApiClient: ContractApiClient,
              private projectManagerContractClient: ProjectManagerContractClient,
              private router: Router,
              private notificationsService: NotificationsService,
              private dialogService: DialogService,
              private etherReceivingService: EtherReceivingService,
              private applicationApiClient: ApplicationApiClient,
              private translateService: TranslateService,
              private tokenService: TokenReceivingService,
              private balanceService: BalanceService,
              private tokenClient: TokenContractClient) {
    this.createForm();
  }

  createForm() {
    const teamMembers = [];
    for (const item in EnumTeamMemberType) {
      if (typeof EnumTeamMemberType[item] === 'number') {

        const group = this.formBuilder.group({
          memberType: EnumTeamMemberType[item],
          title: item,
          fullName: ['', Validators.maxLength(100)],
          facebookLink: ['', [Validators.maxLength(200), Validators.pattern('https?://.+')]],
          linkedInLink: ['', [Validators.maxLength(200), Validators.pattern('https?://.+')]],
        });
        teamMembers.push(group);
      }
    }

    this.applicationForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      whitePaperLink: ['', [Validators.maxLength(400), Validators.pattern('https?://.+')]],
      projectArea: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.maxLength(1000)]],
      projectStatus: ['', Validators.maxLength(100)],
      softCap: ['', Validators.maxLength(40)],
      hardCap: ['', Validators.maxLength(40)],
      financeModelLink: ['', [Validators.maxLength(400), Validators.pattern('https?://.+')]],
      country: ['', Validators.maxLength(100)],
      attractedInvestments: false,
      blockChainType: ['', Validators.maxLength(100)],
      mvpLink: ['', [Validators.maxLength(400), Validators.pattern('https?://.+')]],
      teamMembers: this.formBuilder.array(teamMembers)
    });
  }

  showNext() {
    if (this.isTeamShow === false) {
      this.isTeamShow = true;
      return;
    }
    if (this.isLegalShow === false) {
      this.isLegalShow = true;
      return;
    }
    if (this.isFinanceShow === false) {
      this.isFinanceShow = true;
      return;
    }
    if (this.isTechShow === false) {
      this.isTechShow = true;
      return;
    }
  }

  private async submitIfFormValid() {
    if (this.applicationForm.invalid) {
      if (this.applicationForm.controls['name'].invalid) {
        this.scrollToElement(this.nameRow);
      } else if (this.applicationForm.controls['projectArea'].invalid) {
        this.scrollToElement(this.areaRow);
      } else if (this.applicationForm.controls['description'].invalid) {
        this.scrollToElement(this.descriptionRow);
      }
      return;
    }
    this.isProjectCreating = true;
    const application = await this.fillApplication();

    if (application == null) {
      this.notificationsService.error(this.translateService.instant('Common.Error'), this.translateService.instant('Common.TryAgain'));
      this.isProjectCreating = false;
      return;
    }

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('Application.Dialog'),
      application.transactionHash
    );

    await this.applicationApiClient.createApplicationAsync(application);

    await this.balanceService.updateBalanceAsync();

    transactionDialog.close();

    await this.router.navigate([Paths.Scoring], {queryParams: {tab: 'myProjects'}});
    this.notificationsService.success(
      this.translateService.instant('Common.Success'),
      this.translateService.instant('Application.ProjectCreated')
    );
  }

  public async onSubmit() {
    const userHasETH = await this.balanceService.hasUserEth();
    if (!userHasETH) {
      if (await this.dialogService.showGetEtherDialog()) {
        await this.etherReceivingService.receiveAsync();
      } else {
        return;
      }
    }

    const userHasSVT = await this.balanceService.hasUserSvt();
    if (!userHasSVT) {
      if (await this.dialogService.showGetTokenDialog()) {
        await this.tokenService.receiveAsync();
      } else {
        return;
      }
    }

    await this.submitIfFormValid();
  }

  private scrollToElement(element: ElementRef) {
    const containerOffset = element.nativeElement.offsetTop;
    const fieldOffset = element.nativeElement.offsetParent.offsetTop;
    window.scrollTo({left: 0, top: containerOffset + fieldOffset - 15, behavior: 'smooth'});
    element.nativeElement.children[1].classList.add('ng-invalid');
    element.nativeElement.children[1].classList.add('ng-dirty');
    element.nativeElement.children[1].classList.remove('ng-valid');
    const invalidElements = this.requireds.filter(i => i.nativeElement.classList.contains('ng-invalid'));
    if (invalidElements.length > 0) {
      for (let a = 0; a < invalidElements.length; a++) {
        invalidElements[a].nativeElement.classList.add('ng-invalid');
        invalidElements[a].nativeElement.classList.add('ng-dirty');
      }
    }
  }

  private async fillApplication(): Promise<Application> {

    const formModel = this.applicationForm.value;

    const application = {} as Application;
    application.attractedInvestments = formModel.attractedInvestments;
    application.blockChainType = formModel.blockChainType;
    application.country = formModel.country;
    application.financeModelLink = formModel.financeModelLink;
    application.hardCap = formModel.hardCap;
    application.mvpLink = formModel.mvpLink;
    application.name = formModel.name;
    application.description = formModel.description;
    application.projectArea = formModel.projectArea;
    application.projectStatus = formModel.projectStatus;
    application.softCap = formModel.softCap;
    application.whitePaperLink = formModel.whitePaperLink;

    application.projectId = uuid();

    const user = await this.authenticationService.getCurrentUser();
    application.authorAddress = user.account;
    application.teamMembers = [];
    for (const teamMember of formModel.teamMembers) {
      if (teamMember.fullName) {
        application.teamMembers.push(teamMember);
      }
    }

    const projectManagerContract = await this.contractApiClient.getProjectManagerContractAsync();

    try {
      application.transactionHash = await this.projectManagerContractClient.addProjectAsync(
        projectManagerContract.address,
        projectManagerContract.abi,
        application.projectId,
        formModel.name);

      return application;

    } catch (e) {
      return null;
    }
  }
}
