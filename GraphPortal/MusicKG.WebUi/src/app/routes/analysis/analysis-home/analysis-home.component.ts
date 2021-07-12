import { Component, OnInit } from '@angular/core';
import { AnalysisService } from 'src/app/services/analysis.service';
import { NzMessageService } from 'ng-zorro-antd';
import * as randomColor from 'randomcolor';
// import { forkJoin } from 'rxjs';
// import { finalize } from 'rxjs/operators';

const colorList = ['#1557FF', '#61DDAA', '#F1B754', '#F6BD16', '#7262FD', '#78D3F8'].concat(randomColor({
  count: 100
}));

@Component({
  selector: 'km-analysis-home',
  templateUrl: './analysis-home.component.html',
  styleUrls: ['./analysis-home.component.less']
})
export class AnalysisHomeComponent implements OnInit {

  chartInstance: any;

  isLoading = true;


  chartOption = {
    tooltip: {
        trigger: 'axis',
        axisPointer: {          
            type: 'shadow'     
        },
        position: function(point, params, dom, rect, size){ // point: 鼠标位置
            //解决tip被遮挡或者溢出屏幕外问题
            let css = {};
            let  obj = {};
            if(dom.getBoundingClientRect().height>220){
                css['height'] ='220px';
                if (dom.getBoundingClientRect().width>703) {
                    css[ 'width']='703px'
                }
            }
            else if (dom.getBoundingClientRect().width>703) {
                css[ 'width']='703px';
                if (dom.getBoundingClientRect().height>220) {
                    css['height']='220px';
                }
            }

            //适配
            if (window.innerWidth>1360){
                obj[['left', 'right'][+(point[0] < size.viewSize[0] / 2)]] = 180;
                if(point[0]<110||point[0]>900){//鼠标在left
                    obj[['left', 'right'][+(point[0] < size.viewSize[0] / 2)]] = 280;
                }
            }
            else {//小屏
                // console.log('小屏')
                // console.log('point',point)
                obj[['left', 'right'][+(point[0] < size.viewSize[0] / 2)]] = 150;
            }

            dom.style.css
            return obj
        }
    },
    legend: {
        data: [],
        type: 'scroll',
        textStyle: {
          color: "rgba(255,255,255,.6)",
          fontSize: 10
        }
    },
    grid: {
      left: 50,
      top: 35
    },
    xAxis: [
        {
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
              rotate: 45 ,
              formatter: function (value) {
                let strArray = value.split('-');
                strArray = strArray.map(function(val) {
                  if (val[0] == "0") {
                    return (val = val.slice(1));
                  } else {
                    return val;
                  }
                });
                return strArray[0] + '月'+strArray[1]+'日';
              }
            },
            axisLine: {
              lineStyle: {
                color: "rgba(255,255,255,.2)"
              }
            }
        }
    ],
    yAxis: [
        {
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
        }
    ],
    series: []
};

  constructor(
    private api: AnalysisService,
    private message: NzMessageService
  ) { }

  ngOnInit() {
    this.getWarningData();
  }

  getWarningData(){
    this.api.getwarningData().subscribe((res:any)=>{
      this.renderPieChart(res)
      this.isLoading = false
    })
  }

  renderPieChart(data: any) {
    const items = Object.keys(data);
    this.chartInstance.clear();
    this.chartOption.legend.data = Object.keys(data);
    this.chartOption.xAxis[0].data = (items && items.length > 0) ? data[items[0]].map(v => v.day) : [];
    this.chartOption.series = items.map((d,i)=>{
      return {
        name: d,
        type: 'bar',
        itemStyle: {
          color: colorList[i]
        },
        emphasis: {
          focus: 'series'
        },
        barWidth: "60%",
        stack: '总量',
        data: data[d].map(v => v.count)
      }
    })
    this.chartInstance.setOption(this.chartOption);
  }

  onChartInit(e: any) {
    this.chartInstance = e;
  }

}
