import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { AnalysisService } from 'src/app/services/analysis.service';
import { getYear, subYears } from 'date-fns';

const INIT_YEARS = Array.from({ length: 5}).map((v,i) => getYear(subYears(new Date(), 4 - i)) + '');

@Component({
  selector: 'km-circular-filter',
  templateUrl: './circular-filter.component.html',
  styleUrls: ['./circular-filter.component.less']
})
export class CircularFilterComponent implements OnInit {
  userList = [];

  createdBy: string;

  carModels = null;

  carModelLoading = false;

  carModelList = [];

  carType = null;

  carTypeLoading = false;

  carTypeList = [];

  warningUnit: string;

  yearModels: any = INIT_YEARS;

  partNames = null;

  partNameLoading = false;

  partNameList = [];

  syndromes = null;

  syndromeLoading = false;

  syndromeList = [];

  taskIds = null;

  taskIdLoading = false;

  taskIdList = [];

  // isFocused: boolean;

  focusType = null;
  
  @Output() filterChange = new EventEmitter();

  @Input() 
  set filter(data: any) {
    if(data) {
      this.taskIds = data.taskIds;
      this.createdBy = data.createdBy;
      this.warningUnit = data.warningUnit;
      this.carType =  data.carType;
      this.carModels = data.carModels;
      this.yearModels = data.yearModels;
      this.partNames = data.partNames;
      this.syndromes = data.syndromes;
      this.focusType = data.focusType;
    }
  }

  constructor(
    private userService: UserService,
    private api: AnalysisService
  ) { }

  ngOnInit() {
    this.getTaskIds();
    const user = JSON.parse(localStorage.getItem('user'));
    this.createdBy = user.name;
    this.userService.getAnnotatorList().subscribe(res => {
      this.userList = res.items.map(d => d.name);
    })
   
  }

  getTaskIds(){
    this.taskIdLoading = true;
    this.api.getTaskIds().subscribe(res =>{
      this.taskIdList = res;
       this.taskIdLoading = false;
       this.taskIdChange();
    })
  }

  // getAllCarModelChange() {
  //   this.carTypeLoading = true;
  //   this.api.getCarModel(null).subscribe(res => {
  //     this.carTypeList = res;
  //     this.carTypeLoading = false;
  //   });
  // }

  taskIdChange(){
    this.api.getCarType().subscribe(res => {
      this.carModelList = res;
      this.carModelLoading = false; 
      this.carModelChange();
    });
  }

  carModelChange() {
    this.carType = null;
    this.carTypeLoading = true;
    this.api.getCarModel(this.carModels).subscribe(res => {
      this.carTypeList = res;
      this.carTypeLoading = false;
      this.modelYearsChange();
    });
  }

  modelYearsChange() {
    this.partNames = null;
    this.partNameLoading = true;
    this.api.getPartName(this.carModels, this.carType, this.yearModels).subscribe((res: any) => {
      this.partNameList = res;
      this.partNameLoading = false;
      this.partNameChange();
    });
  }

  partNameChange() {
    this.syndromes = null;
    this.syndromeLoading = true;
    this.api.getSyndromeByPartName(this.carModels, this.carType, this.yearModels, this.partNames).subscribe((res: any) => {
      this.syndromeList = res;
      this.syndromeLoading = false;
    });
  }

  onSearch() {
    this.filterChange.emit({
      taskIds: this.taskIds,
      createdBy: this.createdBy,
      warningUnit: this.warningUnit,
      carModels: this.carModels,
      carType: this.carType,
      yearModels: this.yearModels,
      partNames: this.partNames,
      syndromes: this.syndromes,
      focusType: this.focusType
    });
  }

  onReset() {
   const user = JSON.parse(localStorage.getItem('user'));
   this.taskIds = null;
   this.createdBy = user.name;
   this.warningUnit = null;
   this.carModels = null;
   this.carType = null;
   this.yearModels = null;
   this.yearModels= INIT_YEARS;
   this.partNames = null;
   this.syndromes = null;
   this.focusType = null;
   this.onSearch();
  }

}
