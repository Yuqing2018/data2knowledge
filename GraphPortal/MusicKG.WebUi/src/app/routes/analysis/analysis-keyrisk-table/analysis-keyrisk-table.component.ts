import { Component, OnInit } from '@angular/core';
import { AnalysisService } from 'src/app/services/analysis.service';
import { Router } from '@angular/router';

@Component({
  selector: 'km-analysis-keyrisk-table',
  templateUrl: './analysis-keyrisk-table.component.html',
  styleUrls: ['./analysis-keyrisk-table.component.less']
})
export class AnalysisKeyriskTableComponent implements OnInit {

  items: Array<any> = [];

  searchLoading:boolean = false;

  listOfData: any[] = [];

  sortName: string | null = null;

  sortValue: string | null = null;

  warningUnit = '零件_不良症状';

  constructor(
    private api:AnalysisService,
    private router: Router
  ) { }

  ngOnInit() {
    this.getriskData();
  }

  getriskData(){
    this.searchLoading = true;
    this.api.getkeyriskData().subscribe((res:any) =>{ 
      if (this.sortName && this.sortValue) {
            let res2 = res.sort((a, b) => 
              this.sortValue === 'ascend'
              ? a[this.sortName!] > b[this.sortName!]
                ? 1
                : -1
              : b[this.sortName!] > a[this.sortName!]
              ? 1
              : -1
            );
            this.items = res2
          }else{
            this.items = res;
          }
      this.searchLoading = false;
    })
  }

  // goto(data){
  //   if(data.partName == ''){
  //     this.warningUnit = '不良症状'
  //   }
  //   const selectType = this.warningUnit == '零件_不良症状' ? 'partName' : 'syndrome';
  //   this.router.navigate(['/annotator/analysis/info'], {
  //     queryParams: {
  //       carType: data.carModels,
  //       carModel:'',
  //       yearModels: data.yearModels,
  //       partName: data.partName,
  //       syndrome: data.syndrome,
  //       selectType,
  //       warningUnit:this.warningUnit
  //     }
  //   })
  // }

  sort(sort: { key: string; value: string }): void {
    this.sortName = sort.key;
    this.sortValue = sort.value;
    this.getriskData();
  }

}
