import { Component, OnInit } from '@angular/core';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { CircularTaskFormComponent } from '../circular-task-form/circular-task-form.component';
import { CicrularService } from 'src/app/services/cicrular.service';
import { data } from '../../task/annotation/data';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'km-circular-task',
  templateUrl: './circular-task.component.html',
  styleUrls: ['./circular-task.component.less']
})
export class CircularTaskComponent implements OnInit {
  type = ''

  status = ''

  pageIndex: number = 1;

  searchLoading = false;

  carModelLoading = false;

  total = 0;

  dataSet = [];

  warningUnit = '';

  createdBy = null;

  userList = [];

  carModels = null;

  carModelList = [];

  constructor(
    private modal: NzModalService,
    private api: CicrularService,
    private message: NzMessageService,
    private userService: UserService
  ) { }

  ngOnInit() {
    const user = JSON.parse(localStorage.getItem('user'));
    this.createdBy = user.name;
    this.getCarModelList();
    this.onSearch();
    this.userService.getAnnotatorList().subscribe(res => {
      this.userList = res.items.map(d => d.name);
    })
  }

  getCarModelList(){
    this.api.getCarType().subscribe(res => {
      this.carModelList = res;
      this.carModelLoading = false; 
    });
  }

  onSearch() {
    this.searchLoading = true;
    this.api.getTask(this.type, this.status, (this.pageIndex - 1) * 10, this.createdBy, this.warningUnit,this.carModels).subscribe((res: any) => {
      this.dataSet = res.items;
      this.total = res.totalCount;
      this.searchLoading = false;
    });
  }

  onReset() {
    const user = JSON.parse(localStorage.getItem('user'));
    this.createdBy = user.name;
    this.pageIndex = 1;
    this.status = '';
    this.type = '';
    this.warningUnit = '';
    this.carModels = null;
    this.onSearch();
  }

  pageIndexChange(index: any) {
    this.pageIndex = index;
    this.onSearch();
  }

  onDelete(data: any) {
    this.api.delTask(data.id).subscribe(res => {
      this.message.info('删除成功');
      this.pageIndex = 1;
      this.onSearch();
    });
  }

  onStop(data: any) {
    this.api.stopTask(data.id).subscribe(res => {
      this.message.info('操作成功');
      this.onSearch();
    });
  }

  onView(item: any) {
    this.modal.create({
      nzTitle: '查看预警任务',
      nzWidth: '600px',
      nzContent: CircularTaskFormComponent,
      nzComponentParams: {
        data: item,
        disabled: true
      },
      nzFooter: null
    });
  }

  onUpdate(item: any) {
    this.modal.create({
      nzTitle: '修改预警任务',
      nzWidth: '600px',
      nzContent: CircularTaskFormComponent,
      nzComponentParams: {
        data: item
      },
      nzOnOk: (componentInstance: CircularTaskFormComponent) => {
        if (!this.notEmpty(componentInstance)) {
          this.message.warning('请填写必填项');
          return false;
        }
        const data = this.getData(componentInstance);
        return this.api.updateTask(item.id, data).toPromise().then(res => {
          this.onSearch();
        }).catch(err => {
          this.message.error(err.msg);
          return false;
        });
      }
    });
  }

  add() {
    this.modal.create({
      nzTitle: '新增预警任务',
      nzWidth: '600px',
      nzContent: CircularTaskFormComponent,
      nzOnOk: (componentInstance: CircularTaskFormComponent) => {
        if (!this.notEmpty(componentInstance)) {
          this.message.warning('请填写必填项');
          return false;
        }
        const data = this.getData(componentInstance);
        return this.api.addTask(data).toPromise().then(res => {
          this.onReset();
        }).catch(err => {
          this.message.error(err.msg);
          return false;
        });
      }
    });
  }

  getData(componentInstance: any) {
    const data = {
      name: componentInstance.name,
      warningUnit: componentInstance.warningUnit,
      carModel: (componentInstance.carType && componentInstance.carType.indexOf('全部') !== -1) ? null : componentInstance.carType,
      carTypes: (componentInstance.carModel && componentInstance.carModel.indexOf('全部') !== -1) ? null : componentInstance.carModel,
      yearModels: componentInstance.yearModels,
      warningIndex: []
    };
    if(componentInstance.checkList && componentInstance.checkList.find(d => d.checked)) {
      data.warningIndex.push({
        ...componentInstance.waringList[1][0],
        value: componentInstance.checkList.filter(d => d.checked).map(d => d.value).join(',')
      });
    }
    if(componentInstance.warnValue) {
      const warnValue = componentInstance.warningIndexList.find(d => d.id == componentInstance.warnValue)
      data.warningIndex.push(warnValue);
    }
    if(componentInstance.reWarnValue) {
      const reWarnValue = componentInstance.reWarningIndexList.find(d => d.id == componentInstance.reWarnValue)
      data.warningIndex.push(reWarnValue);
    }
    return data;
  }

  notEmpty(data: any) {
    return ['name', 'carType', 'carModel', 'yearModels'].every(key => {
      return !!data[key]
    }) && ( data.warnValue || data.reWarnValue || data.checkList.filter(d => d.checked).length > 0);
  }

}
