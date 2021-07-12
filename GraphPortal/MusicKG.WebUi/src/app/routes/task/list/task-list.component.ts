import { Component, OnInit, OnDestroy } from '@angular/core';
import { Task, TaskInfo, TaskUpdateModel } from 'src/app/interfaces/task.interface';
import { TaskService } from 'src/app/services/task.service';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { ActivatedRoute } from '@angular/router';
import { withLatestFrom, switchMap, tap, takeUntil, finalize } from 'rxjs/operators';
import { UserInfo } from 'src/app/interfaces/user.interface';
import { TaskUpdateFormComponent } from '../task-update-form/task-update-form.component';
import { Subject } from 'rxjs';
import { TaskFormComponent } from 'src/app/shared/task-form/task-form.component';
import { OptionService } from 'src/app/services/option.service';
import { WorkspaceService } from 'src/app/services/workspace.service';
import { WOKSPACR_TYPE_LIST } from 'src/app/core/common';
import { OntologyDialogueService } from 'src/app/services/ontology-dialogue.service';

@Component({
  selector: 'km-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.less']
})
export class TaskListComponent implements OnInit, OnDestroy {
  private unsubscribe = new Subject<void>();
  taskDataSet: TaskInfo[] = [];
  total = 0;
  pageIndex = 1;
  workspaceId: string;
  workspaceType: string;
  filterStatus: any[];
  entityDocId: string;
  intentDocId: string;
  loadding = false;

  selectedStatus: string[] = [];

  get isDialog() {
    return this.workspaceType && this.workspaceType == WOKSPACR_TYPE_LIST[4].value;
  }

  constructor(
    private api: TaskService,
    private modalService: NzModalService,
    private route: ActivatedRoute,
    private msgService: NzMessageService,
    private optionService: OptionService,
    private workspaceService: WorkspaceService,
    private dialogueService: OntologyDialogueService
  ) { }

  ngOnInit() {
    this.getWorkspaceType();
    this.optionService.getTaskStatus().subscribe(res => {
      this.filterStatus = res.filter(v => {
        return ['Created', 'Accepted', 'ConflictResolved'].indexOf(v.value) !== -1;
      }).map(v => {
        return { text: v.displayName, value: v.value };
      });
    });
    this.searchData();
  }

  getWorkspaceType() {
    this.route.pathFromRoot[1].params.pipe(
      takeUntil(this.unsubscribe),
      tap(w => { this.workspaceId = w.workspace }),
      switchMap(w => this.workspaceService.info(w.workspace))
    ).subscribe(res => {
      this.workspaceType = res.type.id;
      if (this.isDialog) {
        this.getDialogDoc();
      }
    });
  }

  getDialogDoc() {
    this.dialogueService.getOntology(this.workspaceId).subscribe((res: any) => {
      this.intentDocId = res.intentDocumentId;
      this.entityDocId = res.entityDocumentId;
    });
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

  filterStatusChange(event: any) {
    this.selectedStatus = event;
    this.searchData();
  }

  getUserNameByRole(annotators: UserInfo[], role: string = 'Annotator') {
    return annotators.filter(a => a.roles && a.roles.indexOf(role) !== -1).map(a => a.name).join(',');
  }

  deleteTask(taskId: string) {
    this.api.delete(this.workspaceId, taskId).subscribe(() => {
      this.msgService.success(`删除任务成功`);
      this.searchData();
    });
  }

  searchData(pageIndex: number = 1) {
    this.pageIndex = pageIndex;
    this.loadding = true;
    this.route.pathFromRoot[1].params.pipe(
      takeUntil(this.unsubscribe),
      tap(param => { this.workspaceId = param.workspace }),
      switchMap(param => this.api.getTaskListByWorkspace(param.workspace, (this.pageIndex - 1) * 10, 10, this.selectedStatus.map(v => {
        return `statuses=${v}`;
      }).join('&')).pipe(
        finalize(() => {
          this.loadding = false;
        })
      ))
    ).subscribe(res => {
      this.taskDataSet = res.items;
      this.total = res.totalCount;
    });
  }

  updateTask(task: TaskInfo) {
    if (task.status !== 'ConflictResolved') {
      let modal = this.modalService.create({
        nzTitle: '修改任务',
        nzWidth: 600,
        nzContent: TaskUpdateFormComponent,
        nzComponentParams: {
          task: task
        },
        nzOnOk: (componentInstance: TaskUpdateFormComponent) => {
          let form = componentInstance.taskForm;
          let updateData = this.getUpdateTaskData(task, form.value);
          if (Object.keys(updateData).length == 0) {
            return true;
          }
          for (const i in form.controls) {
            form.controls[i].markAsDirty();
            form.controls[i].updateValueAndValidity();
          }
          if (form.valid) {
            this.route.pathFromRoot[1].params.pipe(
              switchMap((param: any) => this.api.update(param.workspace, task.id, updateData))
            ).subscribe(() => {
              modal.destroy();
              this.searchData();
            }, err => {
              this.msgService.create(err.type, err.msg);
            });
          }
          return false;
        }
      })
    }
  }

  getUpdateTaskData(oldTask: TaskInfo, updateTask: TaskUpdateModel) {
    let updateData: TaskUpdateModel = {};
    if (oldTask.name !== updateTask.name) {
      updateData.name = updateTask.name
    }
    if (new Date(oldTask.expectedDueAt).getTime() != new Date(updateTask.expectedDueAt).getTime()) {
      updateData.expectedDueAt = updateTask.expectedDueAt;
    }
    return updateData;
  }

  addTask() {
    let modal = this.modalService.create({
      nzTitle: '新建任务',
      nzWidth: 600,
      nzContent: TaskFormComponent,
      nzComponentParams: {
        task: <Task>{
          overlap: 100,
          expectedDueAt: new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate() + 1)
        },
        workspaceId: this.workspaceId,
        workspaceType: this.workspaceType
      },
      nzOnOk: (componentInstance: TaskFormComponent) => {
        let form = componentInstance.taskForm;
        for (const i in form.controls) {
          form.controls[i].markAsDirty();
          form.controls[i].updateValueAndValidity();
        }
        if (form.valid) {
          if ('5d78523cd6ca8e876f75c432' == form.value.taskType && !this.intentDocId) {
            this.msgService.error('未找到Ontology意图');
            return false;
          }
          if ('5d7851d8d6ca8e876f75c431' == form.value.taskType && !this.entityDocId) {
            this.msgService.error('未找到Ontology实体');
            return false;
          }
          form.value.documentIds = '5d7851d8d6ca8e876f75c431' == form.value.taskType ? [this.entityDocId] : [this.intentDocId];
          this.route.pathFromRoot[1].params.pipe(
            switchMap((param: any) => this.api.add(param.workspace, form.value))
          ).subscribe(() => {
            modal.destroy();
            this.searchData();
          }, err => {
            this.msgService.create(err.type, err.msg);
          });
        }
        return false;
      }
    })
  }

}
