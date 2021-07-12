import { Component, OnInit } from '@angular/core';
import { AnalysisService } from 'src/app/services/analysis.service';

@Component({
  selector: 'km-export-modal',
  templateUrl: './export-modal.component.html',
  styleUrls: ['./export-modal.component.less']
})
export class ExportModalComponent implements OnInit {

  type = '单台件数统计表';

  carTypeList = [];

  carType: string[];

  carModel: string[];

  carModelList = [];

  yearModels: string;

  yearModelList = [];

  carTypeLoading = false;

  carModelLoading = false;

  yearModelsLoading = false;


  constructor(
    private analysisService: AnalysisService
  ) { }

  ngOnInit() {
    this.getCarType();
  }

  carTypeChange() {
    this.carModel = null;
    this.yearModels = null;
    this.carModelLoading = true;
    this.analysisService.getCarModel(this.carType).subscribe((res: any) => {
      this.carModelList = res;
      this.carModelLoading = false;
      this.carModelChange();
    })
  }

  carModelChange() {
    this.yearModels = null;
      this.yearModelsLoading = true;
      this.analysisService.getYearModel(this.carType, this.carModel).subscribe((res: any) => {
        this.yearModelList = res;
        this.yearModelsLoading = false;
      });
  }

  getCarType() {
    this.carTypeLoading = true;
    this.analysisService.getCarType().subscribe((res: any) => {
      this.carTypeList = res;
      this.carTypeLoading = false;
    });
  }

}
