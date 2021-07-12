import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import { environment } from 'src/environments/environment';
import { UserService } from '../services/user.service';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private localStorage: LocalStorageService,
    private userService: UserService
  ) { }

  login(name: string, password: string) {
    return this.userService.login(name, password).pipe(
      tap(res => this.setToken(res.token)),
      tap(res => this.localStorage.set('user', res))
    );
  }

  setToken(token: string): boolean {
    return this.localStorage.set(environment.authToken, token);
  }

  getToken(): string {
    return this.localStorage.get(environment.authToken);
  }

  getUserRole() {
    return this.getUser() && this.getUser().roles[0];
  }

  getUser() { 
    return this.localStorage.get('user');
  }

}
