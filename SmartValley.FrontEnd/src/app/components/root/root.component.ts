import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from '../../services/authentication-service';
import {Web3Service} from '../../services/web3-service';
import {NotificationService} from '../../services/notification-service';
import {UserInfo} from '../../services/user-info';
import {Router} from '@angular/router';
import {Paths} from '../../paths';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.css']
})
export class RootComponent implements OnInit {

  public userInfo: UserInfo;

  constructor(private web3Service: Web3Service,
              private authenticationService: AuthenticationService,
              private notificationService: NotificationService,
              private router: Router) {
    this.authenticationService.userInfoChanged.subscribe(async () => await this.updateUserInfo());
  }

  async ngOnInit() {
    await this.updateUserInfo();
  }

  async updateUserInfo() {
    this.userInfo = await this.authenticationService.getUserInfo();
  }

  redirect(pagename: string) {
    this.router.navigate(['/' + pagename]);
  }
  
  async createProject() {
    try {
      const isRinkeby = await
        this.web3Service.isRinkebyNetwork();
      if (!isRinkeby) {
        this.notificationService.notify('warn', 'Please switch to the Rinkeby network');
        return;
      }
    } catch (reason) {
      this.notificationService.notify('error', reason);
      return;
    }
    const isOk = await this.authenticationService.authenticate();
    if (isOk) {
      this.userInfo = await this.authenticationService.getUserInfo();
      await this.router.navigate([Paths.Application]);
    }
  }
}
