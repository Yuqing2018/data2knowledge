import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { AnalysisService } from 'src/app/services/analysis.service';
import * as randomColor from 'randomcolor';
import { forkJoin } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { NzModalService, NzMessageService, DateHelperByDateFns } from 'ng-zorro-antd';
import { ExportModalComponent } from '../export-modal/export-modal.component';
import { saveAs } from 'file-saver';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

const colorList1 = ['#E9121C', '#F1B754', '#18C492', '#54A1EE'].concat(randomColor({
  count: 100
}));
const colorList2 = ['#1557FF', '#54A1EE', '#61DDAA', '#F1B754'].concat(randomColor({
  count: 100
}));

@Component({
  selector: 'km-analysis-info',
  templateUrl: './analysis-info.component.html',
  styleUrls: ['./analysis-info.component.less']
})
export class AnalysisInfoComponent implements OnInit {

  selectType = 'partName';

  chartInstance: any;

  statistic: any;

  searchParams: any;

  isLoading = true;

  selectName = '';

  legendList = [];

  showBack = false;

  riskLevel: string;

  chartOption = {
    tooltip: {
        trigger: 'item',
        formatter:function(obj){
          const d = obj.data
          if(d.percentage && d.riskLevel){
           return obj.seriesName + '<br/>' + d.name + ' : ' + d.value + ' (' + d.percentage + ') ' +
          '<br/>' + d.riskLevel 
          }else{
            return obj.seriesName + '<br/>' + d.name + ' : ' + d.value + ' (' + obj.percent + '%) '
          }
        }
    },
    legend: {
      type: 'scroll',
      orient: 'vertical',
      right: '5%',
      top: 'center',
      height: 120,
      width: 100,
      itemGap: 10,
      pagemode: true,
      icon: 'pin',
      pageIconSize: 8,
      textStyle: {
        padding: [3, 0, 0, 0],
        color: "#ffffff"
      },
      selectorItemGap: 2,
      formatter: function (name) {
        return name.length <= 5 ? name : name.substring(0, 5) + '...';
      },
      data: []
    },
    series: [
        {
            name:'车款',
            type: 'pie',
            radius: [0, '30%'],
            center: ['34%', '50%'],
            label: {
              show:false
            },
            labelLine: {
                show: false
            },
            itemStyle: {
              color: function (color) {
                return colorList2[color.dataIndex];
              }
            },
            data: []
        },
        {
            name: '不良症状',
            type: 'pie',
            radius: ['45%', '65%'],
            center: ['34%', '50%'],
            labelLine: {
                length: 30,
            },
            label: {
                show:false
            },
            itemStyle: {
              // borderWidth: 3,
              // borderColor: '#fff',
              color: function (color) {
                return colorList1[color.dataIndex];
              }
            },
            emphasis: {
                itemStyle: {
                    shadowBlur: 10,
                    shadowOffsetX: 0,
                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                }
            },
            data: []
        }
    ]
  };

  constructor(
    private api: AnalysisService,
    private cdr: ChangeDetectorRef,
    private modal: NzModalService,
    private message: NzMessageService,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location
  ) { }

  ngOnInit() {
  }

  selectChange(event: any) {
    this.searchParams = event;
    this.selectName = '';
    this.selectType = this.searchParams.selectType;
    this.isLoading = true;
    this.showBack = false;
    forkJoin(
      this.api.syndrome(event),
      this.api.statistic(event)
    ).pipe(
      finalize(() => this.isLoading = false)
    ).subscribe(([syndrome, statistic]) => {
      this.renderPieChart(syndrome);
      this.statistic = statistic;
    });
    this.api.riskLevel(event).subscribe((res: string) => this.riskLevel = res);
  }

  renderPieChart(data: any) {
    this.chartInstance.clear();
    if(this.selectType == 'partName') {
      let res = [];
      this.chartOption.series[0].data = data.map(i =>{
        return {
          name: i.carModel,
          value:i.totalCount
        }
      })
      data.map(d =>{
        for(var i=0;i<d.metrics.length;i++){
          res.push(d.metrics[i])
        }
        return res
      })
      let res2 = res.sort(function(a,b){
        return b.totalCount-a.totalCount
      })
      this.chartOption.series[1].data = res2.map(item =>{
        return {
          name: item.name,
          value: item.totalCount,
          id: item.id,
          riskLevel: item.riskLevel,
          percentage: Number(item.totalCount/data[0].totalCount*100).toFixed(2) +'%'
        }
      })
      this.legendList = res;
      if(this.searchParams.syndrome) {
        let findItem = res2.find(d => d.id == this.searchParams.syndrome);
        if(findItem) {
          this.showBack = true;
          this.selectName = findItem.name;
        }
      }
    }else {
      this.chartOption.series[0].data = [];
      this.chartOption.series[1].data = data.map(item =>{
        return {
          name: item.name,
          value: item.totalCount
        }
      });
      this.legendList = data;
      if(this.searchParams.partName) {
        this.showBack = true;
          this.selectName = this.searchParams.partName;
      }
    }
    this.chartOption.series[1].name = this.selectType == 'partName' ? '不良症状' : '零件';
    this.chartOption.legend.data = this.legendList;
    this.chartInstance.setOption(this.chartOption);
  }

  onChartClick(event: any) {
    if(event.seriesName == '车款') {
      return 
    }
    this.isLoading = true;
    this.selectName = event.name;
    if (this.searchParams.selectType == 'partName') {
      this.searchParams.syndrome = event.data.id;
    } else {
      this.searchParams.partName = event.name;
    }
    this.setUrl();
    this.api.statistic(this.searchParams).subscribe(statistic => {
      this.statistic = statistic;
      this.isLoading = false;
      this.showBack = true;
      // this.cdr.detectChanges();
    });
    this.api.riskLevel(this.searchParams).subscribe((res: string) => this.riskLevel = res);
  }

  setUrl() {
    const urlTree = this.router.createUrlTree([], {
      relativeTo: this.route,
      queryParams: this.searchParams,
      queryParamsHandling: 'merge',
   });
   this.location.replaceState(urlTree.toString());
  }

  onChartInit(e: any) {
    this.chartInstance = e;
  }

  onBack() {
    this.showBack = false;
    if (this.searchParams.selectType == 'partName') {
      this.searchParams.syndrome = '';
    } else {
      this.searchParams.partName = '';
    }
    this.selectChange(this.searchParams);
  }

  onExport() {
    this.modal.create({
      nzTitle: '导出',
      nzContent: ExportModalComponent,
      nzOnOk: (componentInstance: ExportModalComponent) => {
          const data = {
            carType: componentInstance.carModel,
            carModel: componentInstance.carType,
            yearModels: componentInstance.yearModels
          };
          const isEmpty = Object.keys(data).some(key => {
            return !data[key]
          });
          if (!data.carModel) {
            this.message.warning('车款不能为空');
            return false;
          }
          return this.api.export(data).toPromise().then(res => {
            var blob = new Blob([res], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            saveAs(blob, `${data.carModel}_export.xlsx`);
          }).catch(err => {
            this.message.error(err.msg);
            return false;
          });
      }
    });
  }

}
