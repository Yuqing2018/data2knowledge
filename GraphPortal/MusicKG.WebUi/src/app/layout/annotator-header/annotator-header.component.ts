import { Component, OnInit } from '@angular/core';
import { LocalStorageService } from 'src/app/core/local-storage.service';
import { PasswordComponent } from '../password/password.component';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'km-annotator-header',
  templateUrl: './annotator-header.component.html',
  styleUrls: ['./annotator-header.component.less']
})
export class AnnotatorHeaderComponent implements OnInit {
  userName: string;
  userId: string;

  constructor(
    private router: Router,
    private localStorage: LocalStorageService,
    private modalService: NzModalService,
    private userService: UserService,
    private messageService: NzMessageService,
  ) { }

  ngOnInit() {
    let userInfo = this.localStorage.get('user');
    this.userName = userInfo && userInfo.name;
    this.userId = userInfo && userInfo.id;
  }


  logout() {
    this.router.navigate(['/login']);
    localStorage.removeItem('user')
    localStorage.clear();
  }

  resetPassword() {
    this.modalService.create({
      nzTitle: '修改密码',
      nzContent: PasswordComponent,
      nzOnOk: (componentInstance) => {
        let form = componentInstance.validateForm
        for (const i in form.controls) {
          form.controls[i].markAsDirty();
          form.controls[i].updateValueAndValidity();
        }
        if (form.valid) {
          let { password } = form.value
          this.userService.updateUser(this.userId, { password }).subscribe(res => {
            this.messageService.success('修改成功');
            this.logout();
          }, err => {
            this.messageService.error(err.msg)
          })
        } else {
          return false
        }
      }
    })
  }

}
