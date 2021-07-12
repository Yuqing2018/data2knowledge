import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AnalysisService } from 'src/app/services/analysis.service';
import { tap, finalize } from 'rxjs/operators';
import { saveAs } from 'file-saver';
import { NzModalService,NzMessageService } from 'ng-zorro-antd';
import { AnalysisTableComponent } from '../analysis-table/analysis-table.component';
import { AnalysisEditdatafromModalComponent } from '../analysis-editdatafrom-modal/analysis-editdatafrom-modal.component';
import { AnalysisEditcntrtimeModalComponent } from '../analysis-editcntrtime-modal/analysis-editcntrtime-modal.component';
import { AnalysisRelationModalComponent } from '../analysis-relation-modal/analysis-relation-modal.component';
import { AnalysisManualaddModalComponent } from '../analysis-manualadd-modal/analysis-manualadd-modal.component';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'km-analysis-select',
  templateUrl: './analysis-select.component.html',
  styleUrls: ['./analysis-select.component.less']
})
export class AnalysisSelectComponent implements OnInit {
  size = 'small'
  items: Array<any> = []

  total: number = 1

  searchLoading: boolean = false

  searchFrom: number = 0

  pageIndex: number = 1

  carModelList$: any;

  carTypeList$: any;

  yearModelList$: any;

  partNameList$: any;

  syndromeList$: any;

  firstSearch = true;

  partNo = '';

  frameNo = '';

  searchParams:any = {
    selectType: 'partName',
    carModel: null,
    carType: null,
    yearModels: null,
    partName: null,
    syndrome: null,
    dataSource: null,
    warningUnit:'零件_不良症状'
  }

  isVisible = false;
  isOkLoading = false;
  okText = null;

  exportLoading = false;

  @Output() selectChange = new EventEmitter();

  @Output() typeChange = new EventEmitter();

  
  constructor(
    private modal: NzModalService,
    private api: AnalysisService,
    private msg: NzMessageService,
    private location: Location,
    private router: Router,
    private route: ActivatedRoute
  ) { }
  ngOnInit() {
    this.route.queryParams.subscribe(queryParams => {
      if(queryParams && Object.keys(queryParams).length > 0) {
        this.searchParams = {
          ...this.searchParams,
          ...queryParams
        };
        for(let key in this.searchParams) {
          this.searchParams[key] = this.searchParams[key] || null;
        }
        this.getData();
        this.firstSearch = false;
      }else {
        this.dataSourceChange();
      }
    })
  }

  getData() {
    this.carTypeList$ = this.api.getCarType(this.searchParams.dataSource);
    this.carModelList$ = this.api.getCarModel(this.searchParams.carType, this.searchParams.dataSource);
    this.yearModelList$ = this.api.getYearModel(this.searchParams.carType, this.searchParams.carModel, this.searchParams.dataSource);
    this.partNameList$ = this.api.getPartName(this.searchParams.carType, this.searchParams.carModel, this.searchParams.yearModels, this.searchParams.dataSource);
    this.syndromeList$ = this.api.getSyndrome(this.searchParams.carType, this.searchParams.carModel, this.searchParams.yearModels);
    this.onSearch();
  }

  dataSourceChange() {
    if(this.searchParams.dataSource == 'MEDIA_SUB'){
      this.searchParams.dataSource = ['MEDIA_SUB','MEDIA_MAIN']
    }
    this.carTypeList$ = this.api.getCarType(this.searchParams.dataSource).pipe(
      tap((v: any) => {
        if (v && v.length > 0) {
          this.searchParams.carType = v[0];
          this.carModelChange();
        }
      })
    );
  }

  selectTypeChange() {
    if(this.searchParams.selectType == 'partName') {
      this.searchParams.syndrome = '';
      this.searchParams.warningUnit = '零件_不良症状';
    }else {
      this.searchParams.partName = '';
      this.searchParams.warningUnit = '不良症状';
    }
    this.firstSearch = true;
    this.partNameAndSyndromeChange();
  }

  carModelChange() {
    this.carModelList$ = this.api.getCarModel(this.searchParams.carType, this.searchParams.dataSource).pipe(
      tap((v: any) => {
        if (v && v.length > 0) {
          this.searchParams.carModel = v[0];
          this.yearModelChange();
        }
      })
    );
  }

  yearModelChange() {
    this.searchParams.yearModels = '';
    this.yearModelList$ = this.api.getYearModel(this.searchParams.carType, this.searchParams.carModel, this.searchParams.dataSource).pipe(
      tap((v: any) => {
        this.searchParams.yearModels = v[0];
        this.partNameAndSyndromeChange();
      })
    );
  }

  partNameAndSyndromeChange() {
    this.searchParams.partName = '';
    this.searchParams.syndrome = '';
    if (this.searchParams.selectType == 'partName') {
      this.partNameList$ = this.api.getPartName(this.searchParams.carType, this.searchParams.carModel, this.searchParams.yearModels, this.searchParams.dataSource).pipe(
        tap((v: any) => {
          this.searchParams.partName = v[0];
          if (this.firstSearch) {
            this.onSearch();
            this.firstSearch = false;
          }
        })
      );
    } else {
      this.syndromeList$ = this.api.getSyndrome(this.searchParams.carType, this.searchParams.carModel, this.searchParams.yearModels).pipe(
        tap((v: any) => {
          this.searchParams.syndrome = v[0].id;
          if (this.firstSearch) {
            this.onSearch();
            this.firstSearch = false;
          }
        })
      );
    }
  }

  onSearch() {
    const urlTree = this.router.createUrlTree([], {
      relativeTo: this.route,
      queryParams: this.searchParams,
      queryParamsHandling: 'merge',
   });
   this.location.replaceState(urlTree.toString());
   this.selectChange.emit(this.searchParams);
  }

  onExport() {
    this.exportLoading = true;
    this.api.exportChart(this.searchParams).pipe(
      finalize(() => this.exportLoading = false)
    ).subscribe(res => {
      var blob = new Blob([res], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
      saveAs(blob, `图表导出数据.xlsx`);
    })
  }
  
  pageIndexChange(index: number) {
    this.searchFrom = (index - 1) * 10
    this.getQisData()
  }

  onmaintain(){
    this.isVisible = true;
    this.getQisData()
  }

  getQisData(){
    this.searchLoading = true
    this.api.getQisList(this.searchParams.carType, this.searchParams.carModel, this.searchParams.yearModels,this.searchParams.partName,this.searchParams.dataSource,this.frameNo,this.searchFrom,10).subscribe((res:any) => {
      this.items = res.items
      this.total = res.totalCount
      if(res.items.length > 0){
        this.partNo = res.items[0].partNo
        this.searchLoading = false
      }else{
        this.searchLoading = false
      }
   }, err => {
     this.msg.error(err.msg)
   })
  }

  searchSure(){
    this.pageIndex = 1
    this.searchFrom = 0
    this.getQisData()
  }

  onReset(){
    this.frameNo = '';
    this.searchFrom = 0;
    this.pageIndex = 1;
    this.getQisData();
  }

  onEditCntrTime(item: any) {
    this.modal.create({
      nzTitle: '修改关联对策日期',
      nzContent: AnalysisEditcntrtimeModalComponent,
      nzComponentParams: {
        permanentCntrTime: item.permanentCntrTime
      },
      nzOnOk: (componentInstance: AnalysisEditcntrtimeModalComponent) => { 
        return this.api.updateCntrTime(item.rawId, {
          permanentCntrTime: componentInstance.permanentCntrTime
        }).toPromise().then(res => {
          this.msg.info('修改成功');
          item.permanentCntrTime = componentInstance.permanentCntrTime
        }).catch(err => {
          this.msg.error('输入日期格式有误!');
          return false;
        });
      }
    });
  }

  onEditParts(item: any) {
    this.modal.create({
      nzTitle: '修改',
      nzContent: AnalysisTableComponent,
      nzComponentParams: {
        carType: item.relatedPartName == null ? '': item.relatedPartName.carType,
        partNo: item.relatedPartName == null ? '': item.relatedPartName.no
      },
      nzOnOk: (componentInstance: AnalysisTableComponent) => { 
        return this.api.updateQisParts(item.rawId, {
          carType: componentInstance.carType,
          partNo: componentInstance.partNo
        }).toPromise().then(res => {
          this.msg.info('修改成功');
          this.getQisData();
        }).catch(err => {
          this.msg.error(err.msg);
          return false;
        });
      }
    });
  }

  onEditDataFrom(item: any) {
    this.modal.create({
      nzTitle: '修改数据来源',
      nzContent: AnalysisEditdatafromModalComponent,
      nzComponentParams: {
        dataFromDesc: item.dataFromDesc
      },
      nzOnOk: (componentInstance: AnalysisEditdatafromModalComponent) => { 
        return this.api.updateQisDataFrom(item.rawId, {
          dataFromDesc: componentInstance.dataFromDesc
        }).toPromise().then(res => {
          this.msg.info('修改成功');
          item.dataFromDesc = componentInstance.dataFromDesc
          item.dataFrom = '人工录入'
        }).catch(err => {
          this.msg.error(err.msg);
          return false;
        });
      }
    });
  }

  onRelation(item: any) {
    this.modal.create({
      nzTitle: '添加QIS数据',
      nzContent: AnalysisRelationModalComponent,
      nzComponentParams: {
        qicNo: item.qicNo,
        qisNo: item.qisNo
      },
      nzOnOk: (componentInstance: AnalysisRelationModalComponent) => {
          return this.api.updateQis(item.rawId, {
            qicNo: componentInstance.qicNo,
            qisNo: componentInstance.qisNo
          }).toPromise().then(res => {
            this.msg.info('自动关联成功');
            item.qicNo = componentInstance.qicNo;
            item.qisNo = componentInstance.qisNo;
            this.getQisData();
          }).catch(err => {
            this.msg.error(err.msg);
            return false;
          });
      }
    });
  }

  onManualAdd(item: any) {
    this.modal.create({
      nzTitle: item.cntrMesrType == null && item.cntrMesrReasonDesc == null
      && item.permanentCntr == null && item.permanentCntrTime == null ?'手动添加':'修改QIS数据',
      nzContent: AnalysisManualaddModalComponent,
      nzComponentParams: {
        cntrMesrType: item.cntrMesrType,
        cntrMesrReasonDesc: item.cntrMesrReasonDesc,
        permanentCntr: item.permanentCntr,
        permanentCntrTime: item.permanentCntrTime,
      },
      nzOnOk: (componentInstance: AnalysisManualaddModalComponent) => {
        if(componentInstance.cntrMesrType == null || componentInstance.cntrMesrReasonDesc == null
          || componentInstance.permanentCntr == null){
            this.msg.warning('必填项不能为空');
            return false
          }else{
            return this.api.manualadd(item.rawId, {
              cntrMesrType: componentInstance.cntrMesrType,
              cntrMesrReasonDesc: componentInstance.cntrMesrReasonDesc,
              permanentCntr: componentInstance.permanentCntr,
              permanentCntrTime: componentInstance.permanentCntrTime
            }).toPromise().then(res => {
              if(item.cntrMesrType == null && item.cntrMesrReasonDesc == null
                && item.permanentCntr == null && item.permanentCntrTime == null){
                this.msg.info('手动添加QIS成功');
              }else{
                this.msg.info('修改成功');
              }
              this.getQisData();
            }).catch(err => {
              this.msg.error(err.msg);
              return false;
            });
          }
        }
    });
  }

  onDelete(data: any) {
    this.api.deleteqis(data.rawId).toPromise().then(res => {
      this.msg.info('删除成功');
      this.getQisData();
    }).catch(err => {
      this.msg.error(err.msg);
    });
  }

  handleCancel(): void {
    this.isVisible = false;
  }

  copyReasonCode(data) {
    let content = document.createElement('textarea')
    content.innerText = `${data.cntrMesrReasonDesc}`
    document.body.appendChild(content)
    content.select()
    let res = document.execCommand('copy')
    if (res) {
      this.msg.success('复制成功')
    } else {
      this.msg.error('复制失败')
    }
    document.body.removeChild(content)
  }

  copyPermanentCntrCode(data){
    let content = document.createElement('textarea')
    content.innerText = `${data.permanentCntr}`
    document.body.appendChild(content)
    content.select()
    let res = document.execCommand('copy')
    if (res) {
      this.msg.success('复制成功')
    } else {
      this.msg.error('复制失败')
    }
    document.body.removeChild(content)
  }

}
