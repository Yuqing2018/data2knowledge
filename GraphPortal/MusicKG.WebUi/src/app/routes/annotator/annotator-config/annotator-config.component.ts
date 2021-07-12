import { Component, OnInit } from '@angular/core';
import { TaskService } from 'src/app/services/task.service';
import { UserService } from 'src/app/services/user.service';
import { forkJoin } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LocalStorageService } from 'src/app/core/local-storage.service';
import { NzMessageService } from 'ng-zorro-antd';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'km-annotator-config',
  templateUrl: './annotator-config.component.html',
  styleUrls: ['./annotator-config.component.less']
})
export class AnnotatorConfigComponent implements OnInit {
  configList: any = [];

  isLoading = false;

  taskForm: FormGroup;

  isVisible = false;
  isEdit: any = null;

  userList = [];

  userInfo: any;

  constructor(
    private taskService: TaskService,
    private userService: UserService,
    private fb: FormBuilder,
    private localStorage: LocalStorageService,
    private messageService: NzMessageService
  ) { }

  ngOnInit() {
    this.userInfo = this.localStorage.get('user');
    this.isLoading = true;
    forkJoin(
      this.taskService.getTaskCreateRule(),
      this.userService.getAnnotatorList()
    ).subscribe(([task, users]) => {
      this.configList = task.rules;
      this.userList = users.items;
      this.isLoading = false;
    });
  }

  displayUser(ids: string[]) {
    return this.userList.filter(u => ids && ids.indexOf(u.id) !== -1).map(u => u.name).join(',');
  }

  add() {
    this.isVisible = true;
    this.isEdit = null;
    this.taskForm = this.fb.group({
      name: [null, [Validators.required]],
      documentTags: [null, [Validators.required]],
      annotators: [null, [Validators.required]],
    });
  }

  edit(item: any) {
    this.isVisible = true;
    this.isEdit = item;
    this.taskForm = this.fb.group({
      name: [item.name, [Validators.required]],
      documentTags: [item.documentTags, [Validators.required]],
      annotators: [item.annotators, [Validators.required]],
    });
    if(item.name == 'Default') {
      this.taskForm.get('documentTags').clearValidators()
    }
  }

  remove(item: any) {
    this.isLoading = true;
    this.configList = this.configList.filter(c => c !== item);
    this.taskService.putTaskCreateRule({
      createUser: this.userInfo.id,
      rules: this.configList
    }).pipe(
      finalize(() => this.isLoading = false)
    ).subscribe(res => {
      this.messageService.success('操作成功')
    }, err => {
      this.messageService.error(err.msg)
    });
  }

  handleOk() {
    for (const i in this.taskForm.controls) {
      this.taskForm.controls[i].markAsDirty();
      this.taskForm.controls[i].updateValueAndValidity();
    }
    if (!this.taskForm.valid) {
      this.messageService.warning('新增任务设置存在未填项');
      return;
    };
    this.isLoading = true;
    if (this.isEdit) {
      const item = this.configList.find(c => c == this.isEdit);
      const formValue = this.taskForm.value;
      item.name = formValue.name;
      item.documentTags = formValue.documentTags;
      item.annotators = Array.isArray(formValue.annotators) ? formValue.annotators : [formValue.annotators];
      this.taskService.putTaskCreateRule({
        createUser: this.userInfo.id,
        rules: this.configList
      }).pipe(
        finalize(() => {
          this.isLoading = false;
        })
      ).subscribe(res => {
        this.isVisible = false;
        this.messageService.success('操作成功')
      }, err => {
        this.messageService.error(err.msg)
      });
    } else {
      const addData = {
        documentCount: 1,
        isAutoApproved: true,
        isAutoMerged: true,
        maxFinishDays: 30,
        overlap: 0,
        ...this.taskForm.value,
        annotators: Array.isArray(this.taskForm.value.annotators) ? this.taskForm.value.annotators : [this.taskForm.value.annotators]
      };
      this.taskService.putTaskCreateRule({
        createUser: this.userInfo.id,
        rules: [
          addData,
          ...this.configList
        ]
      }).pipe(
        finalize(() => {
          this.isLoading = false;
        })
      ).subscribe(res => {
        this.messageService.success('操作成功');
        this.configList = [addData, ...this.configList];
        this.isVisible = false;
      }, err => {
        this.messageService.error(err.msg)
      });
    }
  }

  handleCancel() {
    this.isVisible = false;
    this.taskForm = undefined;
  }

}
