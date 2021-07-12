import { Component, OnInit, Input } from '@angular/core';
import { CicrularService } from 'src/app/services/cicrular.service';
import { AnalysisService } from 'src/app/services/analysis.service';
import { getYear, subYears } from 'date-fns';
import { forkJoin } from 'rxjs';

const INIT_YEARS = Array.from({ length: 5}).map((v,i) => getYear(subYears(new Date(), 4 - i)) + '');

@Component({
  selector: 'km-circular-task-form',
  templateUrl: './circular-task-form.component.html',
  styleUrls: ['./circular-task-form.component.less']
})
export class CircularTaskFormComponent implements OnInit {

  name: string = '';

  warningUnit = '零件_不良症状';

  warningIndexList: any = [];

  carTypeList = [];

  waringList = [];

  reWarningIndexList: any = [];

  warningSelected = '多发预警';

  warnValue: string;

  reWarnValue: string;

  carType: any;

  carModel: any;

  carModelList = [];

  yearModels: any = INIT_YEARS;

  checkList = [];

  carTypeLoading = false;

  carModelLoading = false;

  circularInfo: any;

  warningIndexes = [];

  constructor(
    private api: CicrularService,
    private analysisService: AnalysisService
  ) { }

  @Input() disabled = false;

  @Input()
  set data(v: any) {
    this.api.taskInfo(v.id).subscribe((data: any) => {
      this.circularInfo = data;
      this.name = data.name;
      this.carType = data.carModels || ['全部'];
      this.warningUnit = data.warningUnit;
      this.carModel = data.carTypes || ['全部'];
      this.yearModels = data.yearModels;
      this.carModelLoading = true;
      this.warningIndexes = data.warningIndex;
      this.setListValue(this.warningIndexes);
      this.analysisService.getCarModel(this.carType == '全部' ? '' : this.carType).subscribe((res: any) => {
        this.carModelLoading = false;
        this.carModelList = res;
      })
    })
  }

  ngOnInit() {
    this.getCarType();
    this.getWarningList();
  }

  get hasSelected() {
    return this.checkList && this.checkList.some(d => d.checked);
  }

  setListValue(data: any) {
    this.waringList = this.waringList.map((d, index) => {
      if( index == 1) {
        return d;
      }else {
        return d.map(ds => {
          let findData = data.find(a => a.id == ds.id);
          return findData || ds;
        })
      }
    });
    if(this.waringList && this.waringList.length > 0) {
      this.warningIndexList = this.waringList[0];
      this.checkList = this.waringList[1][0].value.split(',').map((d,i) => {
        return { label: d, value: d, checked: false };
      });
      this.reWarningIndexList = this.waringList[2];
      if(data && data.length > 0) {
         const reWarnValue = data.find(d => this.reWarningIndexList.find(a => a.id == d.id));
         const warnValue = data.find(d => this.warningIndexList.find(a => a.id == d.id));
         const checked = data.find(d => d.id == this.waringList[1][0].id);
         this.warnValue = warnValue && warnValue.id;
         this.checkList = this.checkList.map(d => {
           d.checked = checked && checked.value.indexOf(d.label) !== -1;
           return d;
         });
         this.reWarnValue = reWarnValue && reWarnValue.id;
      }
    }
  }

  stopBubble(event: Event) {
    event.stopPropagation();
    return false;
  }

  setEmpty(value: string) {
    if(value == 'checkList')  {
      this.checkList = this.checkList.map(d => {
        d.checked = false;
        return d;
      })
    }else {
      this[value] = undefined;
    }
  }

  getWarningList() {
    forkJoin(
      this.api.warnTypeList('多发预警'),
      this.api.warnTypeList('风险预警'),
      this.api.warnTypeList('再发预警')
    ).subscribe(res => {
      this.waringList = res;
      this.setListValue(this.warningIndexes);
    })
  }

  getCarType() {
    this.carTypeLoading = true;
    this.analysisService.getCarType().subscribe((res: any) => {
      this.carTypeList = res;
      this.carTypeLoading = false;
    });
  }

  carTypeChange() {
    this.carModel = null;
    this.carModelLoading = true;
    this.analysisService.getCarModel(this.carType == '全部' ? '' : this.carType).subscribe((res: any) => {
      this.carModelList = res;
      this.carModelLoading = false;
    })
  }

}
