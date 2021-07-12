import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { LocalStorageService } from 'src/app/core/local-storage.service';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { ROLE_MAIN_URL } from './common';

@Injectable({
  providedIn: 'root'
})
export class RouterRedirectGuard implements CanActivate {
  constructor(
    private localStorage: LocalStorageService,
    private router: Router
  ) { }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    let user = this.localStorage.get('user');
    if (user) {
      if (user.roles[0] == 'Administrator') {
        return this.router.parseUrl(ROLE_MAIN_URL[user.roles[0]][0])
      } else {
        return this.router.parseUrl(ROLE_MAIN_URL[user.roles[0]])
      }
    }

    return true;
  }
}
