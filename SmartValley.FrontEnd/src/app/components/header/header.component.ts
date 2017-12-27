import {Component} from '@angular/core';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Constants} from '../../constants';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';
import {BlockiesService} from '../../services/blockies-service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {

  public currentBalance: number;
  public currentTokens: number;
  public showReceiveEtherButton: boolean;
  public showReceiveSVTButton: boolean;
  public isAuthenticated: boolean;
  public accountAddress: string;
  public accountImgUrl: string;

  constructor(private router: Router,
              private balanceService: BalanceService,
              private blockiesService: BlockiesService,
              private authenticationService: AuthenticationService) {

    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateHeader(balance));
    this.authenticationService.accountChanged.subscribe(async (user) => {
      await this.updateAccountAsync(user);
    });
    const currentUser = this.authenticationService.getCurrentUser();
    this.updateAccountAsync(currentUser);
    this.updateHeader(this.balanceService.balance);
  }

  private async updateAccountAsync(user: User): Promise<void> {
    if (user) {
      this.isAuthenticated = true;
      this.accountAddress = user.account;
      this.accountImgUrl = this.blockiesService.getImageForAddress(user.account);
    } else {
      this.isAuthenticated = false;
    }
  }

  public async loginAsync(): Promise<void> {
    await this.authenticationService.authenticateAsync();
  }

  public logout(): void {
    this.authenticationService.stopUserSession();
  }

  private updateHeader(balance: Balance): void {
    if (balance != null) {
      this.currentBalance = balance.ethBalance;
      this.currentTokens = balance.svtBalance;
      this.showReceiveEtherButton = !balance.wasEtherReceived;
      this.showReceiveSVTButton = balance.canReceiveSvt;
    } else {
      this.showReceiveEtherButton = false;
      this.showReceiveSVTButton = false;
    }
  }

  async receiveEth() {
    await this.balanceService.receiveEtherAsync();
  }

  async receiveSVT() {
    await this.balanceService.receiveSvtAsync();
  }

  async navigateToAccount() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Account]);
    }
  }

  async navigateToMyProjects() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Scoring], {queryParams: {tab: Constants.ScoringMyProjectsTab}});
    }
  }
}
