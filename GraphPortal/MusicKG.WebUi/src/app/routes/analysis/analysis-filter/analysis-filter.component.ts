import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AnalysisService } from 'src/app/services/analysis.service';

@Component({
  selector: 'km-analysis-filter',
  templateUrl: './analysis-filter.component.html',
  styleUrls: ['./analysis-filter.component.less']
})
export class AnalysisFilterComponent implements OnInit {
  dateRange = null;

  dataSource = null;

  carModel = null;

  carModelLoading = false;

  carModelList = [];

  carType = null;

  carTypeLoading = false;

  carTypeList = [];

  modelYears = null;

  modelYearsLoading = false;

  modelYearsList = [];

  partName = null;

  partNameLoading = false;

  partNameList = [];

  syndrome = null;

  syndromeLoading = false;

  syndromeList = [];

  faultDateStart = null;

  faultDateEnd = null;

  @Output() filterChange = new EventEmitter();

  constructor(
    private api: AnalysisService
  ) { }

  ngOnInit() {
    this.dataSourceChange();
  }

  dataSourceChange() {
    this.carModel = null;
    this.carModelLoading = true;
    this.api.getCarType(this.dataSource).subscribe(res => {
      this.carModelList = res;
      this.carModelLoading = false;
      this.carModelChange();
    });
  }

  carModelChange() {
    this.carType = null;
    this.carTypeLoading = true;
    this.api.getCarModel(this.carModel, this.dataSource).subscribe(res => {
      this.carTypeList = res;
      this.carTypeLoading = false;
      this.carTypeChange();
    });
  }

  carTypeChange() {
    this.modelYears = null;
    this.modelYearsLoading = true;
    this.api.getYearModel(this.carModel, this.carType, this.dataSource).subscribe(res => {
      this.modelYearsList = res;
      this.modelYearsLoading = false;
      this.modelYearsChange();
    });
  }

  modelYearsChange() {
    this.partName = null;
    this.partNameLoading = true;
    this.api.getPartName(this.carModel, this.carType, this.modelYears, this.dataSource).subscribe((res: any) => {
      this.partNameList = res;
      this.partNameLoading = false;
      this.partNameChange();
    });
  }

  partNameChange() {
    this.syndrome = null;
    this.syndromeLoading = true;
    this.api.getSyndromeByPartName(this.carModel, this.carType, this.modelYears, this.partName, this.dataSource).subscribe((res: any) => {
      this.syndromeList = res;
      this.syndromeLoading = false;
    });
  }

  onSearch() {
    this.filterChange.emit({
      dataSource: this.dataSource,
      carModel: this.carModel,
      carType: this.carType,
      yearModels: this.modelYears,
      partName: this.partName,
      syndrome: this.syndrome,
      faultDateStart: this.dateRange && this.dateRange[0],
      faultDateEnd: this.dateRange && this.dateRange[1]
    });
  }

  onReset() {
    this.dataSource = null;
    this.carModel = null;
    this.carType = null;
    this.modelYears = null;
    this.partName = null;
    this.syndrome = null;
    this.faultDateStart = null;
    this.faultDateEnd = null;
    this.dateRange = null;
    this.onSearch();
  }

}
