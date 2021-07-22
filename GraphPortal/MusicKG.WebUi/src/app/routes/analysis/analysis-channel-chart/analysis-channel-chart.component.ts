import { Component, OnInit,Input } from '@angular/core';
import { AnalysisService } from 'src/app/services/analysis.service';
import * as echarts from 'echarts';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd';
import { fakeAsync } from '@angular/core/testing';

@Component({
  selector: 'km-analysis-channel-chart',
  templateUrl: './analysis-channel-chart.component.html',
  styleUrls: ['./analysis-channel-chart.component.less']
})
export class AnalysisChannelChartComponent implements OnInit {
  pageIndex:number = 1;

  isLoading:boolean = false;

  isTableLoading:boolean = false;

  chartType: string;

  chartInstance: any;

  start:string;

  end:string;

  searchTimes:any ={
    startTime:null,
    endTime:null,
    dataSource:null
  }

  selectTimeType = '日';

  chartOption = {
    tooltip: {
        trigger: 'axis',
        axisPointer: {          
            type: 'shadow'     
        }
    },
    legend: {
        data: ['发生件数'],
        type: 'scroll',
        textStyle: {
          color: "rgba(255,255,255,.6)",
          fontSize: 10
        }
    },
    grid: {
      left:50,
      x:25,
      y:25,
      x2:5,
      y2:20,
      borderWidth:1
    },
    xAxis: {
        show: true,
        type: 'category',
        data: [],
        axisTick: {
          show: false
        },
        axisLabel: {
          textStyle: {
            color: "rgba(255,255,255,.6)",
            fontSize: 10
          },
          formatter: function (value) {
            return value;
          }
        },
        axisLine: {
          lineStyle: {
            color: "rgba(255,255,255,.2)"
          }
        }
    },
    yAxis: {
        type: 'value',
        axisLine: {
          show: false
        },
        axisTick: {
          show: false
        },
        axisLabel: {
          textStyle: {
            color: "rgba(255,255,255,.6)",
            fontSize: 12
          }
        },
        splitLine: {
          lineStyle: {
            color: "rgba(255,255,255,.1)"
          }
        }
    },
    series: [
      {
        name: '发生件数',
        type: 'bar',
        barWidth: '60%',
        itemStyle: {
          normal: {
            color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
              offset: 0,
              color: 'rgba(97, 221, 170, 1)'
            }, {
              offset: 1,
              color: 'rgba(97, 221, 170, 0)'
            }]),
          }
        },
        data: []
      }
    ]
};


  listOfData = [];

  @Input()
  set type(value:string){
    this.chartType = value
    this.getDatasourceData();
    this.getDatasourceTopData();
  }

  constructor(
    private api:AnalysisService,
    private router:Router,
    private msg:NzMessageService
  ) { }

  ngOnInit() {
  }

  pageIndexChange(index: number) {
    this.pageIndex = index;
  }

  getDatasourceData(){
    if (this.chartType) {
      this.isLoading = true;
      this.searchTimes.dataSource = this.chartType == '总局' ? 'GOV':this.chartType == 'MQI' ? 'MQI':this.chartType
      == '技术咨询' ? 'TECH_CONSULTING':this.chartType == '800' ? 'HOTLINE':' MEDIA_MAIN';
      const d1 = new Date();
      const d2 = new Date(d1);
      if(this.searchTimes.startTime == null && this.searchTimes.endTime == null && this.selectTimeType == '日'){
        d2.setDate(d1.getDate() - 30);
        this.searchTimes.startTime = d2.getFullYear() + "-" + (d2.getMonth() + 1) + "-" + d2.getDate();
        this.searchTimes.endTime = d1.getFullYear() + '-' + (d1.getMonth() + 1) + '-' + d1.getDate();
      }
      if(this.searchTimes.startTime == null && this.searchTimes.endTime == null && this.selectTimeType == '周'){
        d2.setDate(d1.getDate() - 180);
        this.searchTimes.startTime = d2.getFullYear() + "-" + (d2.getMonth() + 1) + "-" + d2.getDate();
        this.searchTimes.endTime = d1.getFullYear() + '-' + (d1.getMonth() + 1) + '-' + d1.getDate();
      }
      if(this.searchTimes.startTime == null && this.searchTimes.endTime == null && this.selectTimeType == '月') {
        d2.setDate(d1.getDate() - 730);
        this.searchTimes.startTime = d2.getFullYear() + "-" + (d2.getMonth() + 1) + "-" + d2.getDate();
        this.searchTimes.endTime = d1.getFullYear() + '-' + (d1.getMonth() + 1) + '-' + d1.getDate();
      }
      this.api.getdatasourcesData( this.searchTimes.dataSource,this.selectTimeType,this.searchTimes.startTime,this.searchTimes.endTime).subscribe((res:any)=>{
        this.renderChart(res);
        this.isLoading = false;
        this.searchTimes.startTime = null;
        this.searchTimes.endTime = null;
      },err =>{
        this.msg.error(err.msg);
        this.isLoading = false;
      })
    }
  }

  getDatasourceTopData(){
    this.isTableLoading = true;
    const d1 = new Date();
    this.start = d1.getFullYear() + '-' + (d1.getMonth() + 1) + '-' + '1';
    this.end = d1.getFullYear() + '-' + (d1.getMonth() + 1) + '-' + d1.getDate();
    this.api.getdatasourceTopData(this.start,this.end).subscribe((res:any) =>{
      // console.log(res)
        res.map(item =>{
          if(item.dataSource == this.searchTimes.dataSource){
            this.listOfData = item.results;
          }
          return this.listOfData
        })
        this.isTableLoading = false;
    })
  }

  onChange(result: Date[]): void {
    // console.log('From: ', result[0], ', to: ', result[1]);
    if(result.length > 0){
      const s = new Date(result[0]);
    this.searchTimes.startTime = s.getFullYear() + '-' + (s.getMonth() + 1) + '-' + s.getDate();
    const e = new Date(result[1])
    this.searchTimes.endTime = e.getFullYear() + '-' + (e.getMonth() + 1) + '-' + e.getDate();
    this.getDatasourceData();
    }else{
      this.searchTimes.startTime = null;
      this.searchTimes.endTime = null;
      this.selectTimeType = '日';
      this.getDatasourceData();
    }
  }

  selectTimeTypeChange(){
    this.isLoading = true;
    this.getDatasourceData();
  }

  renderChart(data: any) {
    // console.log(data);
    this.chartInstance.clear();
    this.chartOption.xAxis.data = data.map(d => d.month)
    this.chartOption.series[0].data = data.map(v => v.totalCount)
    this.chartInstance.setOption(this.chartOption);
  }

  onChartInit(e: any) {
    this.chartInstance = e;
  }

  goto(data){
    this.router.navigate(['/annotator/analysis/info'], {
      queryParams: {
        carType: data.carModel,
        partName: data.partName
      }
    })
  }
}
