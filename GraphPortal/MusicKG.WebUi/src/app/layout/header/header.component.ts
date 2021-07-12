import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { LocalStorageService } from '../../core/local-storage.service';
import { NzModalService } from 'ng-zorro-antd';
import { PasswordComponent } from '../password/password.component';
import { UserService } from '../../services/user.service';
import { NzMessageService } from 'ng-zorro-antd';
import { WorkspaceService } from '../../services/workspace.service';
import { Workspace } from '../../interfaces/workspace.interface';
import { ROLE_MAIN_URL } from 'src/app/core/common';
@Component({
  selector: 'km-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less']
})
export class HeaderComponent implements OnInit {

  userName: string
  userId: string
  visiable: boolean
  workSpaceList: Workspace[]
  selectedValue: string
  adminNav: boolean = false
  selected: boolean = false
  annotatorNav: boolean = false

  constructor(
    private router: Router,
    private localStorage: LocalStorageService,
    private modalService: NzModalService,
    private userService: UserService,
    private messageService: NzMessageService,
    private workspaceService: WorkspaceService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    let userInfo = this.localStorage.get('user');
    if (userInfo) {
      this.userName = userInfo.name
      this.userId = userInfo.id
      if (userInfo.roles[0] == 'Manager') {
        this.getWorkSpaceList()
      }
      if (userInfo.roles[0] == 'Administrator') {
        this.adminNav = true
        if (this.router.url == '/user') {
          this.selected = true
        }
      }
      if (userInfo.roles[0] == 'Annotator') {
        this.annotatorNav = true
      }
    }
  }

  logout() {
    this.router.navigate(['/login']);
    localStorage.clear()
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
            this.messageService.success('修改成功')
          }, err => {
            this.messageService.error(err.msg)
          })
        } else {
          return false
        }
      }
    })
  }

  toggle(value) {
    if (!value) {
      this.router.navigate(['/workspace'])
      return
    }
    this.router.navigate([`/workspace/${value}/task`])
  }

  getWorkSpaceList() {
    if (!(this.router.url.indexOf('workspace') > -1)) {
      this.visiable = false
      return
    }
    this.visiable = true
    this.workspaceService.getList().subscribe(res => {
      this.workSpaceList = res.items;
    })
    this.route.pathFromRoot[1].params.subscribe(res => {
      if (res.workspace) {
        this.selectedValue = res.workspace
      }
    });
  }

  goToMainPage() {
    const user = this.localStorage.get('user');
    const navigateUrl = user ? (user.roles[0] == 'Annotator' ? '/annotator/task' : ROLE_MAIN_URL[user.roles[0]]) : '/login';
    this.router.navigate([navigateUrl]);
  }

}
