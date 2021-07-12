import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { UserFormComponent } from '../user-form/user-form.component';
import { UserPasswordComponent } from '../user-password/user-password.component';
import { UserEditComponent } from '../user-edit/user-edit.component';
import { UserInfo } from '../../../interfaces/user.interface';


@Component({
  selector: 'km-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.less']
})
export class UserListComponent implements OnInit {

  dataSet: UserInfo[] = []
  loading: boolean = true

  constructor(
    private userService: UserService,
    private modalService: NzModalService,
    private nzMessageService: NzMessageService
  ) { }

  ngOnInit() {
    this.userService.getUserList().subscribe(res => {
      this.dataSet = res.items.filter(user => !!user.createdBy)
      this.loading = false
    })
  }

  userDispatch(type: string, arg?: string): void {
    switch (type) {
      case 'new':
        this.createModal('新建用户', UserFormComponent, (componentInstance) => {
          if (!this.formCheck(componentInstance)) {
            return {}
          } else {
            return { type: 'new', data: componentInstance.validateForm.value }
          }
        })
        break;
      case 'edit':
        this.createModal('编辑用户', UserEditComponent, (componentInstance) => {
          if (!this.formCheck(componentInstance)) {
            return {}
          } else {
            return { type: 'edit', data: componentInstance.validateForm.value }
          }
        }, this.searchUser(arg)[0])
        break;

      case 'reset':
        this.createModal('密码重置', UserPasswordComponent, (componentInstance) => {
          if (!this.formCheck(componentInstance)) {
            return {}
          } else {
            return { type: 'reset', data: componentInstance.validateForm.value }
          }
        }, this.searchUser(arg)[0])
        break;

      default:
        break;
    }
  }

  /**
   * createModal
   * title: modal title
   * component: modal content component
   * arg?: modal params
   * */
  createModal(title: string, component, sucessCallback: Function, targetUser?: UserInfo): void {
    this.modalService.create({
      nzTitle: title,
      nzContent: component,
      nzWidth: 600,
      nzComponentParams: {
        data: targetUser || {}
      },
      nzOnOk: (componentInstance) => {
        let { type, data } = sucessCallback(componentInstance)
        if (!type) {
          return false
        }
        switch (type) {
          case 'new':
            data.roles = new Array(data.roles)
            this.userService.createUser(data).subscribe(res => {
              this.dataSet = [res, ...this.dataSet]
              this.nzMessageService.success('添加成功')
            }, err => this.hasError(err.msg))
            break;

          case 'edit':
            data.roles = new Array(data.roles)
            this.userService.updateUser(targetUser.id, data).subscribe(res => {
              this.dataSet.forEach(item => {
                if(item.id === res.id) {
                  item.roles = res.roles
                  item.name = res.name
                }
              })
              this.nzMessageService.success('更新成功')
            }, err => this.hasError(err.msg))
            break;

          case 'reset':
            this.userService.updateUser(targetUser.id, { password: data.password }).subscribe(res => {
              this.nzMessageService.success('密码重置成功')
            }, err => this.hasError(err.msg))
            break;

          default:
            break;
        }
      }
    })
  }
  
  formCheck(componentInstance): boolean {
    for (const i in componentInstance.validateForm.controls) {
      componentInstance.validateForm.controls[i].markAsDirty();
      componentInstance.validateForm.controls[i].updateValueAndValidity();
    }
    if (componentInstance.validateForm.invalid) {
      return false
    } else {
      return true
    }
  }

  userDelete(isDelete: boolean, id?: string): void {
    if (isDelete) {
      this.userService.deleteUser(id).subscribe(res => {
        this.dataSet = this.dataSet.filter(item => item.id !== id)
        this.nzMessageService.success('删除成功')
      }, err => this.hasError(err.msg))
    }
  }

  searchUser(id: string): Array<UserInfo> {
    return this.dataSet.filter(item => item.id === id)
  }

  modifyStatus(id: string): void {
    const targetUser = this.searchUser(id)[0]
    let status
    if (targetUser.status === 'Enabled') {
      status = 'Disabled'
    } else {
      status = 'Enabled'
    }
    this.userService.updateUser(id, { status }).subscribe(res => {
      this.dataSet.forEach(item => {
        if(item.id === res.id) {
          item.status = res.status
        }
      })
      this.nzMessageService.success('状态更新成功')
    }, err => this.hasError(err.msg))
  }

  hasError(msg: string): void {
    this.nzMessageService.error(msg)
  }

}
