import { Component, OnInit,Input } from '@angular/core';
import { AnalysisService } from 'src/app/services/analysis.service';
import * as randomColor from 'randomcolor';
import * as echarts from 'echarts';

const colorList = ['#B415FF', '#61DDAA', '#1557FF', '#F6BD16', '#7262FD', '#78D3F8'].concat(randomColor({
  count: 100
}));
const chartConfig = {
  '多发预警': {
    chartOption: {
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
    },
    setOption: (data: any, option: any) => {
      const items = Object.keys(data);
      var len = items.length;
      var temp;
      for (var i = 0; i < len; i++) {
        if (items[i] === '多车款') {
            temp = items[i];
            for (var j = i--; j < items.length-1; j++) {
              items[j] = items[j+1];
            }
            items[items.length-1] = temp;
            len--;
         }
      }
      option.legend.data = items;
      option.xAxis[0].data = (items && items.length > 0) ? data[items[0]].map(v => v.day) : [];
      option.series = items.map((d, i) => {
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
      });
      return option;
    }
  },
  '风险预警': {
    chartOption: {
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
    },
    setOption: (data: any, option: any) => {
      const items = Object.keys(data);
      var len = items.length;
      var temp;
      for (var i = 0; i < len; i++) {
        if (items[i] === '多车款') {
            temp = items[i];
            for (var j = i--; j < items.length-1; j++) {
              items[j] = items[j+1];
            }
            items[items.length-1] = temp;
            len--;
         }
      }
      option.legend.data = items;
      option.xAxis[0].data = (items && items.length > 0) ? data[items[0]].map(v => v.day) : [];
      option.series = items.map((d, i) => {
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
      });
      return option;
    }
  },
  '再发预警': {
    chartOption: {
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
      left: 35,
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
    },
    setOption: (data: any, option: any) => {
      const items = Object.keys(data);
      var len = items.length;
      var temp;
      for (var i = 0; i < len; i++) {
        if (items[i] === '多车款') {
            temp = items[i];
            for (var j = i--; j < items.length-1; j++) {
              items[j] = items[j+1];
            }
            items[items.length-1] = temp;
            len--;
         }
      }
      option.legend.data = items;
      option.xAxis[0].data = (items && items.length > 0) ? data[items[0]].map(v => v.day) : [];
      option.series = items.map((d, i) => {
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
      });
      return option;
    }
  }
};

@Component({
  selector: 'km-analysis-warning-chart',
  templateUrl: './analysis-warning-chart.component.html',
  styleUrls: ['./analysis-warning-chart.component.less']
})
export class AnalysisWarningChartComponent implements OnInit {
  chartInstance: any;

  chartOption: any;

  chartType: string;

  isLoading = false;


  @Input()
  set type(value: string) {
    this.chartType = value;
    this.chartOption = Object.assign(chartConfig[value].chartOption);
    if (this.chartType) {
      this.api.getwarningTypesData(this.chartType).subscribe((res:any)=>{
        this.chartOption = chartConfig[this.chartType].setOption(res, this.chartOption);
        this.renderChart();
      })
    }else {
      this.chartOption = chartConfig[this.chartType].setOption([], this.chartOption);
      this.renderChart();
    }
  }

  constructor(
    private api:AnalysisService
  ) { }

  ngOnInit() {
    
  }

  renderChart() {
    if(this.chartInstance) {
      this.chartInstance.clear();
      this.chartInstance.setOption(this.chartOption);
    }
  }

  onChartInit(e: any) {
    this.chartInstance = e;
  }

}
