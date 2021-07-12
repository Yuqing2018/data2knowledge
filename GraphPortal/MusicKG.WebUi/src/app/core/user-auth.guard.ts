import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { LocalStorageService } from './local-storage.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { ROLE_MAIN_URL } from './common';

@Injectable({
  providedIn: 'root'
})
export class UserAuthGuard implements CanActivate {

  constructor(
    private localStorage: LocalStorageService,
    private router: Router
  ) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    let user = this.localStorage.get('user')
    if (!user || new Date(user.tokenExpiredAt) < new Date()
      || !this.localStorage.get(environment.authToken)) {
      this.localStorage.remove(environment.authToken)
      this.localStorage.remove('user')
      this.router.navigate(['/login'], { queryParams: { 'redirectURL': state.url } });
      return false;
    }

    return true
  }
}
