import { Component, OnInit } from '@angular/core';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { WorkspaceFormComponent } from '../form/workspace-form.component';
import { WorkspaceService } from 'src/app/services/workspace.service';
import { Workspace } from 'src/app/interfaces/workspace.interface';

@Component({
  selector: 'km-workspace-main',
  templateUrl: './workspace-main.component.html',
  styleUrls: ['./workspace-main.component.less']
})
export class WorkspaceMainComponent implements OnInit {
  workspaceList: Workspace[];

  constructor(
    private modalService: NzModalService,
    private api: WorkspaceService,
    private msgService: NzMessageService
  ) { }

  ngOnInit() {
    this.getWorkspaceList();
  }

  getWorkspaceList() {
    this.workspaceList = null;
    this.api.getList().subscribe(res => { 
      this.workspaceList = res.items;
    });
  }

  addWorkspace() {
    let modal = this.modalService.create({
      nzTitle: '创建标注空间',
      nzWidth: 600,
      nzContent: WorkspaceFormComponent,
      nzComponentParams: {
        workspace: <Workspace>{}
      },
      nzOnOk: (componentInstance: WorkspaceFormComponent) => {
        let form = componentInstance.workspaceForm;
        for (const i in form.controls) {
          form.controls[i].markAsDirty();
          form.controls[i].updateValueAndValidity();
        }
        if (form.valid) {
          this.api.add(form.value).subscribe(() => {
            modal.destroy();
            this.getWorkspaceList();
          }, err => {
            this.msgService.create(err.type, err.msg);
          });
        }
        return false;
      }
    })
  }

  removeWorkspace(event: Event, item: Workspace) {
    this.modalService.confirm({
      nzTitle: '确定要删除该标注空间吗?',
      nzContent: '标注空间所关联的信息也会被删除',
      nzOnOk: () => {
        this.api.delete(item.id).subscribe(() => {
          this.getWorkspaceList();
        });
      }
    });
    event.stopPropagation();
  }

  editWorkspace(event: Event, workspace: Workspace) {
    let modal = this.modalService.create({
      nzTitle: '修改标注空间',
      nzWidth: 600,
      nzContent: WorkspaceFormComponent,
      nzComponentParams: {
        workspace: {
          ...workspace, 
          type: { value: workspace.type.id, disabled: true },
          language: { value: workspace.language, disabled: true },
        }
      },
      nzOnOk: (componentInstance: WorkspaceFormComponent) => {
        let form = componentInstance.workspaceForm;
        for (const i in form.controls) {
          form.controls[i].markAsDirty();
          form.controls[i].updateValueAndValidity();
        }
        if (form.valid) {
          this.api.update({
            ...form.value,
            type: workspace.type,
            language: workspace.language
          }).subscribe(() => {
            modal.destroy();
            this.getWorkspaceList();
          }, err => {
            this.msgService.create(err.type, err.msg);
          });
        }
        return false;
      }
    })
    event.stopPropagation();
  }

}
