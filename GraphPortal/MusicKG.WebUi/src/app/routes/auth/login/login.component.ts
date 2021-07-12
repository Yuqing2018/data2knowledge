import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/core/auth.service';
import { LocalStorageService } from '../../../core/local-storage.service';
import { ROLE_MAIN_URL } from 'src/app/core/common';
import { OptionService } from 'src/app/services/option.service';
import { NzNotificationService } from 'ng-zorro-antd';

@Component({
  selector: 'km-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class LoginComponent implements OnInit, AfterViewInit {
  loginForm: FormGroup;
  isLoading = false;
  error: string;
  redirectURL: string;

  alert = false;

  login(): void {
    let params = this.route.snapshot.queryParams;
    if (params['redirectURL']) {
      this.redirectURL = params['redirectURL'];
    }
    for (const i in this.loginForm.controls) {
      this.loginForm.controls[i].markAsDirty();
      this.loginForm.controls[i].updateValueAndValidity();
    }
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.authService.login(this.loginForm.value.userName, this.loginForm.value.password).subscribe(res => {
        this.optionService.getAllOptions().subscribe();
        if (this.redirectURL) {
          location.href = this.redirectURL;
        } else {
          this.goHomePage(res)
        }
      }, err => {
        this.isLoading = false;
        this.error = err.msg || err.error.message;
      });
    }
  }

  goHomePage(res: any) {
    if (res.roles[0] == 'Annotator') {
      this.router.navigateByUrl('/annotator/analysis/home')
    } else {
      this.router.navigate([ROLE_MAIN_URL[res.roles[0]]])
    }
  }

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private localStorage: LocalStorageService,
    private route: ActivatedRoute,
    private optionService: OptionService,
    private notificationService: NzNotificationService
  ) {
  }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      userName: [null, [Validators.required]],
      password: [null, [Validators.required]]
    });
    this.notificationService.remove();
  }

  ngAfterViewInit() {
    // for IE 
    setTimeout(() => {
      for (const key in this.loginForm.controls) {
        if (this.loginForm.controls[key]) {
          this.loginForm.controls[key].markAsPristine();
          this.loginForm.controls[key].setErrors({ 'required': false });
        }
      }
    }, 100);
  }

}
