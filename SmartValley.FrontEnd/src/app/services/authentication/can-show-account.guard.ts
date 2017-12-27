import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {AuthenticationService} from './authentication-service';
import {Paths} from '../../paths';

@Injectable()
export class CanShowAccountGuard implements CanActivate {
  constructor(private router: Router,
              private authenticationService: AuthenticationService) {
  }




  public canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authenticationService.getCurrentUser() !== null) {
      return true;
    } else {
      this.router.navigate([Paths.Root]);
      return false;
    }
  }
}
