import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {NavigationEnd, Router, RouterEvent} from '@angular/router';
import {Paths} from '../../paths';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';
import {BlockiesService} from '../../services/blockies-service';
import {UserContext} from '../../services/authentication/user-context';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {ExpertApplicationStatus} from '../../services/expert/expert-application-status.enum';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {isNullOrUndefined} from 'util';
import {ProjectService} from '../../services/project/project.service';
import {DialogService} from '../../services/dialog-service';
import {User} from '../../services/authentication/user';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  public currentBalance: number;
  public showReceiveEtherButton: boolean;
  public isAuthenticated: boolean;
  public isAdmin: boolean;
  public haveProject: boolean;
  public accountAddress: string;
  public accountImgUrl: string;
  public projectsLink: string;
  public accountLink: string;
  public scroginsLink: string;
  public adminPanelLink: string;
  public myProjectLink: string;
  public expertStatus?: ExpertApplicationStatus;
  public ExpertApplicationStatus = ExpertApplicationStatus;
  public showExpertPanel = false;
  public isUserExpert = false;
  public isExpertActive: boolean;

  constructor(private balanceService: BalanceService,
              private blockiesService: BlockiesService,
              private authenticationService: AuthenticationService,
              private dialogService: DialogService,
              private projectApiClient: ProjectApiClient,
              private userContext: UserContext,
              private expertApiClient: ExpertApiClient,
              private router: Router,
              private projectService: ProjectService) {
    this.projectService.projectsDeleted.subscribe(async () => {
      this.haveProject = false;
      this.myProjectLink = '';
    });

    this.router.events.subscribe((event: RouterEvent) => this.checkExpertPanelVisibility(event));
    this.projectService.projectsCreated.subscribe(async () => await this.updateProjectsAsync());
    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalance(balance));
    this.userContext.userContextChanged.subscribe(async (user) => await this.updateAccountAsync(user));
  }

  async ngOnInit() {
    const currentUser = this.userContext.getCurrentUser();
    await this.updateAccountAsync(currentUser);
    this.projectsLink = Paths.ProjectList;
    this.accountLink = Paths.Account;
    this.adminPanelLink = Paths.Admin;
    await this.balanceService.updateBalanceAsync();
    if (currentUser) {
      this.isUserExpert = currentUser.isExpert;
    }
    if (this.isUserExpert) {
      const availabilityExpert = await this.expertApiClient.getAvailabilityStatusAsync();
      this.isExpertActive = availabilityExpert.isAvailable;
    }
    if (this.router.url !== '/' && this.isUserExpert) {
      this.showExpertPanel = true;
    }
  }

  private checkExpertPanelVisibility(event: RouterEvent) {
    if (event instanceof NavigationEnd) {
      const currentUser = this.userContext.getCurrentUser();
      if (event['url'] === '/') {
        this.showExpertPanel = false;
      }
      if ( currentUser ) {
        if (event['url'] !== '/' && currentUser.isExpert) {
          this.showExpertPanel = true;
        }
      }
    }
  };

  private async updateProjectsAsync(): Promise<void> {
    const myProjectResponse = await this.projectApiClient.getMyProjectAsync();
    if (!isNullOrUndefined(myProjectResponse)) {
      this.myProjectLink = Paths.Project + '/' + myProjectResponse.id;
      this.haveProject = true;
    }
  }

  private async updateAccountAsync(user: User): Promise<void> {
    if (user) {
      if (user.isExpert && this.router.url !== '/') {
        this.showExpertPanel = true;
      }else {
        this.showExpertPanel = false;
      }
      await this.updateProjectsAsync();
      this.isAuthenticated = true;
      this.accountAddress = user.account;
      const expertStatusResponse = await this.expertApiClient.getExpertStatusAsync(this.accountAddress);
      this.expertStatus = expertStatusResponse.status;
      this.scroginsLink = Paths.ScoringList;
      this.accountImgUrl = this.blockiesService.getImageForAddress(user.account);
      this.isAdmin = user.roles.includes('Admin');
    } else {
      this.showExpertPanel = false;
      this.isAuthenticated = false;
      this.isAdmin = false;
      this.haveProject = false;
      this.myProjectLink = '';
    }
  }

  public async loginAsync(): Promise<void> {
    await this.authenticationService.authenticateAsync();
  }

  public logout(): void {
    this.userContext.deleteCurrentUser();
  }

  public async receiveEtherAsync(): Promise<boolean> {
    return this.balanceService.receiveEtherAsync();
  }

  private updateBalance(balance: Balance): void {
    if (balance != null) {
      this.currentBalance = balance.ethBalance;
      this.showReceiveEtherButton = !balance.wasEtherReceived;
    } else {
      this.showReceiveEtherButton = false;
    }
  }

  async navigateToExpertApplication() {
    if (await this.authenticationService.authenticateAsync()) {
      if (this.expertStatus === ExpertApplicationStatus.Pending) {
        await this.router.navigate([Paths.ExpertStatus]);
      } else if (this.expertStatus === ExpertApplicationStatus.Accepted) {
        await this.router.navigate([Paths.ScoringList]);
      } else if (this.expertStatus === ExpertApplicationStatus.Rejected) {
        await this.router.navigate([Paths.ExpertStatus]);
      } else {
        await this.router.navigate([Paths.RegisterExpert]);
      }
    }
  }

  async navigateToProjectApplication() {
    if (await this.authenticationService.authenticateAsync()) {
      const myProjectResponse = await this.projectApiClient.getMyProjectAsync();
      if (!isNullOrUndefined(myProjectResponse)) {
        this.myProjectLink = Paths.Project + '/' + myProjectResponse.id;
        await this.router.navigate([this.myProjectLink]);
        this.haveProject = true;
      } else {
        await this.router.navigate([Paths.Project]);
      }
    }
  }

  public showChangeActivityModal(isExpertActive: boolean) {
    this.dialogService.changeStatusDialog(isExpertActive, this.accountAddress);
  }
}
