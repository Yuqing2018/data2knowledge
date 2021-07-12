import { Component, OnInit, OnDestroy } from '@angular/core';
import { TaskService } from 'src/app/services/task.service';
import { TaskInfo } from 'src/app/interfaces/task.interface';
import { Router, ActivatedRoute } from '@angular/router';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { OptionService } from 'src/app/services/option.service';
import { finalize } from 'rxjs/operators';
import { AnnotatorUploadComponent } from '../annotator-upload/annotator-upload.component';
import { AnnotatorStoreService } from '../annotator-store.service';
import { Location } from '@angular/common';

@Component({
  selector: 'km-annotator-task',
  templateUrl: './annotator-task.component.html',
  styleUrls: ['./annotator-task.component.less']
})
export class AnnotatorTaskComponent implements OnInit {
  taskDataSet: TaskInfo[] = [];
  total = 0;
  pageIndex = 1;
  from = 0;
  to = 0;
  filterStatus: any[];

  searchKey = '';

  selectedStatus: string[] = [];

  isLoading = false;

  checked = false;
  indeterminate = false;

  setOfCheckedId = new Set<string>();

  constructor(
    private taskService: TaskService,
    private router: Router,
    private route: ActivatedRoute,
    private modalService: NzModalService,
    private optionService: OptionService,
    private message: NzMessageService,
    private store: AnnotatorStoreService,
    private location: Location
  ) { }

  ngOnInit() {
    this.route.queryParams.subscribe(queryParams => {
      if (queryParams.snapshot) {
        this.pageIndex = this.store.taskSearch.pageIndex;
        this.selectedStatus = [].concat(this.store.taskSearch.selectedStatus);
        this.searchKey = this.store.taskSearch.searchKey;
      }
      this.searchData(this.pageIndex);
      this.store.resetTaskSearch();
      this.location.go('/annotator/task');
      this.getFilterOptions();
    })
  }

  getFilterOptions() {
    this.optionService.getTaskStatus().subscribe(res => {
      this.filterStatus = res.filter(v => {
        return ['Created', 'KnowledgeMerged', 'ConflictResolved'].indexOf(v.value) !== -1;
      }).map(v => {
        return { text: v.displayName, value: v.value , byDefault: this.selectedStatus.indexOf(v.value) !== -1};
      });
    });
  }

  onAllChecked(value: boolean): void {
    this.taskDataSet.filter(d => d.status !== 'KnowledgeMerged' && d.status !== 'ConflictResolved').forEach(item => this.updateCheckedSet(item.id, value));
    this.refreshCheckedStatus();
  }

  updateCheckedSet(id: string, checked: boolean): void {
    if (checked) {
      this.setOfCheckedId.add(id);
    } else {
      this.setOfCheckedId.delete(id);
    }
  }

  filterStatusChange(event: any) {
    this.selectedStatus = event;
    this.searchData();
  }

  onItemChecked(id: string, checked: boolean): void {
    this.updateCheckedSet(id, checked);
    this.refreshCheckedStatus();
  }

  refreshCheckedStatus(): void {
    const filterData = this.taskDataSet.filter(d => d.status !== 'KnowledgeMerged' && d.status !== 'ConflictResolved');
    this.checked = this.setOfCheckedId.size > 0 && filterData.length > 0 && filterData.every(item => this.setOfCheckedId.has(item.id));
    this.indeterminate = filterData.some(item => this.setOfCheckedId.has(item.id)) && !this.checked;
  }

  searchData(pageIndex: number = 1) {
    this.pageIndex = pageIndex;
    this.isLoading = true;
    this.taskService.getTaskList((this.pageIndex - 1) * 10, 10, this.selectedStatus.map(v => {
      return `statuses=${v}`;
    }).join('&'), this.searchKey).pipe(
      finalize(() => this.isLoading = false)
    ).subscribe((res: any) => {
      this.taskDataSet = res.items || [];
      this.total = res.totalCount;
      this.from = res.from + 1;
      this.to = res.from + res.count;
      this.refreshCheckedStatus();
      this.setStoreSearch();
    });
  }

  onEnter(e) {
    if (e.key == 'Enter') {
      this.searchData();
    }
  }

  goTo(task: TaskInfo) {
    this.router.navigate([`${task.id}`], {
      queryParams: { name: task.name, workspaceId: task.workspace.id, type: task.workspace.type.id },
      relativeTo: this.route
    });
  }

  commit(task: TaskInfo) {
    this.modalService.confirm({
      nzTitle: '提示信息',
      nzContent: '确认提交该标注任务吗？',
      nzOnOk: () => {
        this.taskService.commit(task.workspace.id, task.id).subscribe(res => {
          this.searchData();
        });
      }
    });
  }

  batchComplete() {
    if (this.setOfCheckedId && this.setOfCheckedId.size > 0) {
      this.modalService.confirm({
        nzTitle: '提示信息',
        nzContent: '确认提交这些标注任务吗？',
        nzOnOk: () => {
          this.taskService.submitTask(Array.from(this.setOfCheckedId)).subscribe(res => {
            this.message.success('操作成功');
            this.setOfCheckedId.clear();
            this.searchData(this.pageIndex);
          }, err => {
            this.message.error(err.msg)
          });
        }
      })
    } else {
      this.message.warning('请选择需要提交的标注任务');
    }
  }

  reload() {
    this.searchData();
    this.setOfCheckedId.clear();
  }

  openUpload() {
    this.modalService.create({
      nzTitle: '上传数据',
      nzContent: AnnotatorUploadComponent,
      nzOnOk: (componentRef: AnnotatorUploadComponent) => {
        if (componentRef.uploadFile && (componentRef.fileName.indexOf('.xlsx') !== -1 || componentRef.fileName.indexOf('.xls') !== -1)) {
          let formData = new FormData();
          formData.append('file', componentRef.uploadFile);
          return this.taskService.upload(componentRef.type, formData).toPromise().then((res) => {
            this.message.success('上传成功');
          }).catch(err => {
            this.message.error(err.msg);
            return false;
          });
        } else {
          this.message.warning('文件不能为空或文件格式不正确');
        }
        return false;
      }
    });
  }

  setStoreSearch() {
    this.store.taskSearch = {
      pageIndex: this.pageIndex,
      selectedStatus: [].concat(this.selectedStatus),
      searchKey: this.searchKey
    };
  }

}
