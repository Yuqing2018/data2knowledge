import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Task } from 'src/app/interfaces/task.interface';
import { UserService } from 'src/app/services/user.service';
import { UserInfo } from 'src/app/interfaces/user.interface';
import { DocumentData } from 'src/app/interfaces/pre-mark.interface';
import { WOKSPACR_TYPE_LIST } from 'src/app/core/common';
import { OptionService } from 'src/app/services/option.service';
import { Option } from 'src/app/interfaces/api.interface';

@Component({
  selector: 'km-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.less']
})
export class TaskFormComponent implements OnInit {
  taskForm: FormGroup;
  userList: UserInfo[];
  taskTypeList: Option[];
  private _workspaceType: string;

  get isDialog() {
    return this.workspaceType && this.workspaceType == WOKSPACR_TYPE_LIST[4].value;
  }

  @Input()
  set task(task: Task) {
    this.taskForm = this.formBuilder.group({
      name: [task.name, [Validators.required, Validators.maxLength(20)]],
      annotatorIds: [task.annotatorIds, Validators.required],
      overlap: [task.overlap, Validators.required],
      expectedDueAt: [task.expectedDueAt, Validators.required],
      documentIds: [task.documentIds],
      isAutoApproved: [true],
      isAutoMerged: [true]
    });
  }

  @Input() workspaceId: string;

  @Input() docList: DocumentData[];

  @Input()
  set workspaceType(value: string) {
    this._workspaceType = value;
    if (this.showDictionary) {
      this.taskForm.addControl('dictionaryIds', new FormControl(null));
    }
    if (this.isDialog) {
      this.taskForm.addControl('taskType', new FormControl(null, [Validators.required]));
    }
  }

  get workspaceType() {
    return this._workspaceType;
  }

  get showDictionary() {
    return this.workspaceType && this.workspaceType == WOKSPACR_TYPE_LIST[0].value;
  }

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private optionService: OptionService
  ) { }

  ngOnInit() {
    this.userService.getAnnotatorList().subscribe(res => {
      this.userList = res.items;
    });
    this.optionService.getTaskType().subscribe(res => {
      this.taskTypeList = res;
    });
  }

  hasError(controlName: string, error: string) {
    let formControl = this.taskForm.get(controlName);
    return formControl.dirty && formControl.errors && formControl.errors[error];
  }

}
