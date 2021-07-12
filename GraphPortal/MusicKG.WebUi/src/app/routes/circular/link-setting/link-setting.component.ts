import { Component, OnInit, Input } from '@angular/core';
import { getYear, subYears } from 'date-fns';
import { AnalysisService } from 'src/app/services/analysis.service';
import { tap } from 'rxjs/operators';

const INIT_YEARS = Array.from({ length: 5 }).map((v, i) => getYear(subYears(new Date(), 4 - i)) + '');

@Component({
  selector: 'km-link-setting',
  templateUrl: './link-setting.component.html',
  styleUrls: ['./link-setting.component.less']
})
export class LinkSettingComponent implements OnInit {

  carModelList$: any;

  carTypeList$: any;

  yearModelList$: any;

  partNameList$: any;

  syndromeList$: any;

  isFirst = true;

  syndromLoading = false;

  partNameLoading = false;

  yearModelLoading = false;

  carTypeLoading = false;

  carModelLoading = false;

  @Input()
  set data(value: any) {
    this.carModelLoading = true;
    this.carModelList$ = this.api.getCarType().pipe(
      tap((v: any) => {
        if (v && v.length > 0) {
          this.searchParams = value;
          this.carModelLoading = false;
          this.carModelChange();
        }
      })
    );
  }

  searchParams = {
    carModels: null,
    carTypes: null,
    yearModels: '',
    partName: '',
    syndroms: ''
  }

  constructor(
    private api: AnalysisService
  ) {
  }

  ngOnInit() {
  }

  carModelChange() {
    if (!this.isFirst) {
      this.searchParams.carTypes = null;
    }
    this.carTypeLoading = true;
    this.carTypeList$ = this.api.getCarModel(this.searchParams.carModels).pipe(
      tap((v: any) => {
        if (v && v.length > 0) {
          this.carTypeLoading = false;
          this.yearModelChange();
        }
      })
    );
  }

  yearModelChange() {
    if (!this.isFirst) {
      this.searchParams.yearModels = null;
    }
    this.yearModelLoading = true;
    this.yearModelList$ = this.api.getYearModel(this.searchParams.carModels, this.searchParams.carTypes).pipe(
      tap((v: any) => {
        this.yearModelLoading = false;
        this.partNameChange();
      })
    );
  }

  partNameChange() {
    if (!this.isFirst) {
      this.searchParams.partName = null;
    }
    this.partNameLoading = true;
    this.partNameList$ = this.api.getPartName(this.searchParams.carModels, this.searchParams.carTypes, this.searchParams.yearModels).pipe(
      tap((v: any) => {
        this.partNameLoading = false;
        this.syndromeChange();
      })
    );
  }

  syndromeChange() {
    if (!this.isFirst) {
      this.searchParams.syndroms = null;
    }
    this.syndromLoading = true;
    this.syndromeList$ = this.api.getSyndromeByPartName(this.searchParams.carModels, this.searchParams.carTypes, this.searchParams.yearModels, this.searchParams.partName).pipe(
      tap((v: any) => {
        this.syndromLoading = false;
        this.isFirst = false;
      })
    );
  }

}
