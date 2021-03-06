import {Component, OnDestroy, OnInit} from '@angular/core';
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
import {ExpertsRegistryContractClient} from '../../services/contract-clients/experts-registry-contract-client';
import {TranslateService} from '@ngx-translate/core';
import {Subscription} from 'rxjs/Subscription';
import BigNumber from 'bignumber.js';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {

  public currentBalance: BigNumber;
  public showReceiveEtherButton: boolean;
  public isAuthenticated: boolean;
  public isAdmin: boolean;
  public haveProject: boolean;
  public accountAddress: string;
  public accountImgUrl: string;
  public projectsLink: string;
  public accountLink: string;
  public scoringsLink: string;
  public adminPanelLink: string;
  public freeTokenPlaceLink: string;
  public myProjectLink: string;
  public isExpert = false;
  public showExpertPanel = false;
  public isUserExpert = false;
  public isExpertActive: boolean;
  public isMobileMenuVisible = false;
  public routerSubscriber: Subscription;

  constructor(private balanceService: BalanceService,
              private blockiesService: BlockiesService,
              private authenticationService: AuthenticationService,
              private dialogService: DialogService,
              private projectApiClient: ProjectApiClient,
              private userContext: UserContext,
              private expertApiClient: ExpertApiClient,
              private router: Router,
              private translateService: TranslateService,
              private expertsRegistryContractClient: ExpertsRegistryContractClient,
              private projectService: ProjectService) {
    this.projectService.projectsDeleted.subscribe(async () => {
      this.haveProject = false;
      this.myProjectLink = '';
    });

    this.routerSubscriber = this.router.events.subscribe(async (event: RouterEvent) => await this.checkExpertPanelVisibilityAsync(event));
    this.projectService.projectsCreated.subscribe(async () => await this.updateProjectsAsync());
    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalance(balance));
    this.userContext.userContextChanged.subscribe(async (user) => await this.updateAccountAsync(user));
  }

  async ngOnInit() {
    this.projectsLink = Paths.ProjectList;
    this.accountLink = Paths.Account;
    this.adminPanelLink = Paths.Admin;
    this.freeTokenPlaceLink = Paths.FreeTokenPlace;

    const user = this.userContext.getCurrentUser();
    await this.updateAccountAsync(user);
  }

  private async updateExpertBarAsync() {
    const currentUser = this.userContext.getCurrentUser();
    await this.balanceService.updateBalanceAsync();
    if (currentUser) {
      this.isUserExpert = currentUser.isExpert;
    }
    if (this.isUserExpert) {
      const availabilityExpert = await this.expertApiClient.getAvailabilityStatusAsync();
      this.isExpertActive = availabilityExpert.isAvailable;
    }
    await this.updateAccountAsync(currentUser);
  }

  private async checkExpertPanelVisibilityAsync(event: RouterEvent): Promise<void> {
    if (event instanceof NavigationEnd) {
      const currentUser = this.userContext.getCurrentUser();
      if (event['url'] === '/') {
        this.showExpertPanel = false;
      }
      if (currentUser) {
        if (event['url'] !== '/' && currentUser.isExpert) {
          await this.updateExpertBarAsync();
        }
      }
    }
  }

  private async updateProjectsAsync(): Promise<void> {
    const myProjectResponse = await this.projectApiClient.getMyProjectAsync();
    if (!isNullOrUndefined(myProjectResponse)) {
      this.myProjectLink = Paths.Project + '/' + myProjectResponse.id;
      this.haveProject = true;
    }
  }

  private async updateAccountAsync(user: User): Promise<void> {
    if (user) {
      await this.updateProjectsAsync();
      this.isAuthenticated = true;
      this.accountAddress = user.account;
      this.isExpert = user.isExpert;
      this.isAdmin = user.isAdmin;
      this.scoringsLink = Paths.ScoringList;
      this.accountImgUrl = this.blockiesService.getImageForAddress(user.account);
      this.showExpertPanel = user.isExpert && this.router.url !== '/' && this.router.url !== '/' + Paths.FreeTokenPlace;
    } else {
      this.showExpertPanel = false;
      this.isAuthenticated = false;
      this.isAdmin = false;
      this.isExpert = false;
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
      const status = (await this.expertApiClient.getExpertStatusAsync(this.accountAddress)).status;
      if (status === ExpertApplicationStatus.Pending) {
        await this.router.navigate([Paths.ExpertStatus]);
      } else if (status === ExpertApplicationStatus.Accepted) {
        await this.router.navigate([Paths.ScoringList]);
      } else if (status === ExpertApplicationStatus.Rejected) {
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

  public async showChangeActivityModalAsync(isExpertActive: boolean) {
    const changeStatus = await this.dialogService.changeStatusDialogAsync(!isExpertActive, this.accountAddress);
    if (changeStatus) {
      let transactionHash = '';
      if (isExpertActive) {
        transactionHash = await this.expertsRegistryContractClient.disableAsync(this.accountAddress);
      } else {
        transactionHash = await this.expertsRegistryContractClient.enableAsync(this.accountAddress);
      }

      const transactionDialog = this.dialogService.showTransactionDialog(
        this.translateService.instant('Header.ChangeActivityDetails'),
        transactionHash
      );

      await this.expertApiClient.switchAvailabilityAsync(transactionHash, !isExpertActive);
      transactionDialog.close();
      this.isExpertActive = !isExpertActive;
    }
  }

  public ngOnDestroy() {
      this.routerSubscriber.unsubscribe();
  }

  public switchMobileMenuVisibility() {
    this.isMobileMenuVisible = !this.isMobileMenuVisible;
  }
}
