import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ExceptionCode } from '../../../interfaces/exception-code';
import { LocalStorageService } from 'src/app/core/local-storage.service';
import { ROLE_MAIN_URL } from 'src/app/core/common';

@Component({
  selector: 'km-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.less']
})
export class MainComponent implements OnInit {

  statusInfo: ExceptionCode

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private localStorage: LocalStorageService
  ) { }

  ngOnInit() {
    this.route.url.subscribe(res => {
      let status = res[0].path
      this.dispatch(status)
    })
  }

  dispatch(status: string): void {
    switch (status) {
      case '401':
        this.statusInfo = { title: status, img: 'assets/img/exception/401.svg', description: '抱歉，您无权访问该页面' }
        break;
      case '404':
        this.statusInfo = { title: status, img: 'assets/img/exception/404.svg', description: '抱歉，您访问的页面不存在' }
        break;
      case '500':
        this.statusInfo = { title: status, img: 'assets/img/exception/500.svg', description: '抱歉，服务器出错了' }
        break;

      default:
        break;
    }
  }

  goBcak() {
    const user = this.localStorage.get('user');
    const navigateUrl = user ? ROLE_MAIN_URL[user.roles[0]] : '/login';
    this.router.navigate([navigateUrl]);
  }

}
