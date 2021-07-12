import { Injectable, Injector } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpHeaders, HttpErrorResponse, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private authService: AuthService,
    private injector: Injector
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let headers = new HttpHeaders();
    headers = headers.set('Cache-Control', `no-cache`);
    headers = headers.set('Pragma', `no-cache`);

    if (this.authService.getToken()) {
      headers = headers.set('Authorization', `Bearer ${this.authService.getToken()}`);
    }

    let newReq = req.clone({
      headers: headers
    });

    return next.handle(newReq).pipe(
      catchError((error: any) => {
        if (error instanceof HttpErrorResponse) {
          switch (error.status) {
            case 400:
            case 404: error = { type: 'warning', msg: error.error.message || error.error.title }; break;
            case 401: this.handleUnAuth(newReq); break;
            case 500: error = { type: 'error', msg: error.error.message }; break;
            default: error = { type: 'warning', msg: error.message }
          }
        }
        return throwError(error);
      })
    );
  }

  handleUnAuth(newReq: any) {
    if (newReq.url.indexOf('Login') == -1 && location.href.indexOf('/login') == -1) {
      let modalService = this.injector.get(NzModalService);
      let router = this.injector.get(Router);
      localStorage.removeItem(environment.authToken);
      localStorage.removeItem('user');
      router.navigate(['/login'], { queryParams: { redirectURL: location.href } });
      modalService.closeAll();
    }
  }
}
