import { Component, OnInit } from '@angular/core';
import { CicrularService } from 'src/app/services/cicrular.service';
import { ActivatedRoute, Router } from '@angular/router';
import { map, switchMapTo, switchMap, tap } from 'rxjs/operators';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { CircularConfirmComponent } from '../circular-confirm/circular-confirm.component';
import { FrequencyFormComponent } from '../frequency-form/frequency-form.component';
import { LinkSettingComponent } from '../link-setting/link-setting.component';
import { CircularLevelFormComponent } from '../circular-level-form/circular-level-form.component';
import { CircularInfoModalComponent } from '../circular-info-modal/circular-info-modal.component';

@Component({
  selector: 'km-circular-info',
  templateUrl: './circular-info.component.html',
  styleUrls: ['./circular-info.component.less']
})
export class CircularInfoComponent implements OnInit {
  dataSet = [];
  id: string;
  info: any;

  from: number = 0;

  total = 0;

  searchLoading = false;

  status: string;

  constructor(
    private cicrularService: CicrularService,
    private route: ActivatedRoute,
    private modal: NzModalService,
    private message: NzMessageService,
    private router: Router
  ) { }

  ngOnInit() {
    this.searchLoading = true;
    this.route.params.pipe(
      map(p => p.id),
      tap(id => this.id = id),
      switchMap(id => this.cicrularService.info(id, null, this.from, 10))
    ).subscribe((res: any) => {
      this.dataSet = res.items.map(item => {
        item.displayThreeNum = this.getTaskIndex(item, '近三个月发生件数');
        item.displayThreeYearRate = this.getTaskIndex(item, '年款不良率');
        item.displayThreeRate = this.getTaskIndex(item, '近三个月相对上升率');
        return item;
      }).map(item => {
        item.hasAgain = item.warningTask.warningIndex.some(w => {
          return w.warningType == '再发预警'
        })
        return item;
      });
      this.total = res.totalCount;
      this.searchLoading = false;
    })
    this.route.params.pipe(
      switchMap(q => this.cicrularService.warningRecordInfo(q.id))
    ).subscribe(res => {
      this.info = res;
    })
  }

  gotoInfo(item: any) {
    this.modal.create({
      nzTitle: '预警详情',
      nzWidth: '90%',
      nzContent: CircularInfoModalComponent,
      nzComponentParams: {
        data: {
          info: this.info,
          data: item
        }
      },
      nzFooter: null
    });
  }

  dataSourceType(type: string) {
    const data = {
      MQI: 'MQI',
      TECH_CONSULTING: '技术咨询',
      HOTLINE: '800',
      GOV: '总局',
      MEDIA_SUB: '网络媒体',
    };
    return data[type] || '未知';
  }

  goto(type?: string) {
    if(type !== 'setting') {
      const selectType = this.info.warningTask.warningUnit == '零件_不良症状' ? 'partName' : 'syndrome';
      const data = Object.assign(this.info.warningTask);
      this.router.navigate(['/annotator/analysis/info'], {
        queryParams: {
          carModel: (this.info.settings && this.info.settings.carTypes) || data.carTypes,
          carType: (this.info.settings && this.info.settings.carModels) || data.carModels,
          yearModels: (this.info.settings && this.info.settings.yearModels) || data.yearModels,
          partName: selectType == 'syndrome' ? '' : ((this.info.settings && this.info.settings.partName) || this.info.partName),
          syndrome: (this.info.settings && this.info.settings.syndroms) || this.info.syndrome,
          selectType
        }
      });
      return;
    }
    this.modal.create({
      nzTitle: '跳转设置',
      nzContent: LinkSettingComponent,
      nzComponentParams: {
        data: this.info.settings || {
          carModels: this.info.warningTask.carModels,
          carTypes: this.info.warningTask.carTypes,
          yearModels: this.info.warningTask.yearModels,
          partName: this.info.partName,
          syndroms: this.info.syndrome
        }
      },
      nzOnOk: (component: LinkSettingComponent) => {
        return this.cicrularService.saveSetting(this.id, component.searchParams).toPromise().then(() => this.info.settings = Object.assign(component.searchParams));
      }
    });
  }

  editRiskLevel(item: any) {
    this.modal.create({
      nzTitle: 'AI风险等级修改',
      nzWidth: '600px',
      nzContent: CircularLevelFormComponent,
      nzComponentParams: {
        data: item.riskMetrics
      },
      nzOnOk: (componentInstance: CircularLevelFormComponent) => {
        const data = componentInstance.levelForm.value;
        return new Promise(resolve => {
          this.cicrularService.editRishLevel(
            item.id,
            data
          ).subscribe(res => {
            item.riskMetrics.aiRiskLevel = data.aiRiskLevel;
            item.riskMetrics.usedForModel = data.usedForModel;
            resolve(true);
          }, err => {
            this.message.error(err.msg);
            resolve(false);
          })
        });
      }
    });
  }

  editFreqencyAndFocused() {
    this.modal.create({
      nzTitle: '修改',
      nzWidth: '600px',
      nzContent: FrequencyFormComponent,
      nzComponentParams: {
        data: {
          frequency: this.info.frequency,
          isFocused: this.info.focusType,
          specifiedDate: this.info.specifiedDate
        }
      },
      nzOnOk: (componentInstance: FrequencyFormComponent) => {
        const data = componentInstance.frequencyForm.value;
        return new Promise(resolve => {
          this.cicrularService.editFreqencyAndFocused(
            this.info.id,
            data
          ).subscribe(res => {
            this.info.frequency = data.frequency;
            this.info.focusType = data.isFocused == true ? '已关注':('已退出' ||'未关注');
            this.info.specifiedDate = data.specifiedDate;
            resolve(true);
          }, err => {
            this.message.error(err.msg);
            resolve(false);
          })
        });
      }
    });
  }

  pageIndexChange(index: any) {
    this.from = (index - 1) * 10;
    this.filterChange();
  }

  filterChange() {
    this.searchLoading = true;
    this.cicrularService.info(
      this.id,
      this.status,
      this.from,
      10
    ).subscribe((res: any) => {
      this.dataSet = res.items.map(item => {
        item.displayThreeNum = this.getTaskIndex(item, '近三个月发生件数');
        item.displayThreeYearRate = this.getTaskIndex(item, '年款不良率');
        item.displayThreeRate = this.getTaskIndex(item, '近三个月相对上升率');
        return item;
      });
      this.total = res.totalCount;
      this.searchLoading = false;
    });
  }

  getTaskIndex(item: any, label: string) {
    if (!!item.warningTask.warningIndex.find(i => i.indexName == label)) {
      return item.isMultipleWarning ? '异常' : '正常'
    }
    return '-';
  }

  onConfirm(item: any) {
    this.modal.create({
      nzTitle: '预警确认',
      nzWidth: '600px',
      nzContent: CircularConfirmComponent,
      nzOnOk: (componentInstance: CircularConfirmComponent) => {
        const data = componentInstance.confirmForm.value;
        if(!data.confirmedMessage) {
          this.message.warning('确认记录不能未空');
          return false;
        }
        return new Promise(resolve => {
          this.cicrularService.confirm(
            item.id,
            data
          ).subscribe(res => {
            item.confirmRecord = data;
            item.confirmRecord.lastConfirmdTime = new Date();
            item.isHandled = '已处理';
            resolve(true);
          }, err => {
            this.message.error(err.msg);
            resolve(false);
          })
        });
      }
    });
  }

  onSearch() {
    this.from = 0;
    this.filterChange();
  }

  onRest() {
    this.status = null;
    this.onSearch();
  }

  onEdit(item: any) {
    this.modal.create({
      nzTitle: '预警确认修改',
      nzWidth: '600px',
      nzContent: CircularConfirmComponent,
      nzComponentParams: {
        data: item.confirmRecord
      },
      nzOnOk: (componentInstance: CircularConfirmComponent) => {
        const data = componentInstance.confirmForm.value;
        if(!data.confirmedMessage || !data.pushStatus) {
          this.message.warning('必填项不能为空');
          return false;
        }
        return new Promise(resolve => {
          this.cicrularService.confirm(
            item.id,
            data
          ).subscribe(res => {
            item.confirmRecord = data;
            item.confirmRecord.lastConfirmdTime = new Date();
            resolve(true);
          }, err => {
            this.message.error(err.msg);
            resolve(false);
          })
        });
      }
    });
  }

  getCount(data: any) {
    let result = 0;
    for(let key in data) {
      result += data[key];
    }
    return result;
  }

  back() {
    this.router.navigateByUrl('/annotator/circular/track?snapshot=true');
  }

}
