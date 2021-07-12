import { Component, OnInit } from '@angular/core';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { AnalysisService } from 'src/app/services/analysis.service';
import { EditFill } from '@ant-design/icons-angular/icons/public_api';
import { EditModalComponent } from '../edit-modal/edit-modal.component';
import { saveAs } from 'file-saver';

@Component({
  selector: 'km-analysis-list',
  templateUrl: './analysis-list.component.html',
  styleUrls: ['./analysis-list.component.less']
})
export class AnalysisListComponent implements OnInit {

  dataSet = [
  ];

  pageIndex: number = 1;

  total = 0;

  searchLoading = false;

  filter: any;

  infoVisible = false;

  activeInfo: any;

  infoLoading = false;

  exportLoading  = false;

  constructor(
    private modal: NzModalService,
    private api: AnalysisService,
    private msg: NzMessageService
  ) { }

  filterChange(filter: any) { 
    this.filter = filter;
    this.pageIndex = 1;
    this.onSearch();
  }

  onSearch() {
    this.searchLoading = true;
    this.api.getList({
      ...this.filter,
      from: (this.pageIndex - 1) * 10,
      size: 10,
      maxSize:500
    }).subscribe((res: any) => {
      this.dataSet = res.items;
      this.total = res.totalCount;
      this.searchLoading = false;
    });
  }

  ngOnInit() {
    this.searchLoading = true;
    this.api.getList({
      from: (this.pageIndex - 1) * 10,
      size: 10,
      maxSize:500
    }).subscribe((res: any) => {
      this.dataSet = res.items;
      this.total = res.totalCount;
      this.searchLoading = false;
    });
  }

  onExport() {
    this.exportLoading = true;
    this.api.listExport({...this.filter}).subscribe(res => {
      var blob = new Blob([res], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
      saveAs(blob, `不良症状查询导出数据.xlsx`);
      this.exportLoading = false;
    });
  }

  pageIndexChange(index: any) {
    this.pageIndex = index;
    this.onSearch();
  }

  onInfo(data: any) {
    this.infoVisible = true;
    this.infoLoading = true;
    this.api.info(data.id).subscribe(res => {
      this.activeInfo = res;
      this.infoLoading = false;
    });
  }

  onEdit(item: any) {
    this.modal.create({
      nzTitle: '修改',
      nzContent: EditModalComponent,
      nzComponentParams: {
        partName: item.partName,
        syndrome: item.syndrome,
        dataSource: item.dataSource
      },
      nzOnOk: (componentInstance: EditModalComponent) => { 
        const data = {
          partName: componentInstance.partName,
          syndrome: componentInstance.syndrome,
          isAddToTraining: componentInstance.isAddToTraining
        };
        return this.api.update(item.id, data).toPromise().then(res => {
          this.msg.info('修改成功');
          // this.filterChange(this.filter);
          this.onSearch()
        }).catch(err => {
          this.msg.error(err.msg);
          return false;
        });
      }
    });
  }

  close() {
    this.infoVisible = false;
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

}
