import { Component, OnInit, Input } from '@angular/core';
import { AnalysisService } from 'src/app/services/analysis.service';
import { finalize } from 'rxjs/operators';
import * as randomColor from 'randomcolor';
import * as echarts from 'echarts';
import { HttpClient } from '@angular/common/http';
import { DateHelperByDateFns } from 'ng-zorro-antd';
import { data } from '../../task/annotation/data';

const colorList = ['#5B8FF9', '#61DDAA', '#65789B', '#F6BD16', '#7262FD', '#78D3F8'].concat(randomColor({
  count: 100
}));
// const colorList = ['#ffa631', '#afdd22', '#ff7500', '#00bc12', '#808080', '#88ada6','#725e82','#44cef6','#4b5cc4','#801dae','#b36d61',
// '#edd1d8','#9d2933','#003472','#2e4e7e','#c0ebd7','#e9e7ef','#3d3b4f','#549688','#fff2df'
// ].concat(randomColor({
//   count: 100
// }));
const chartConfig = {
  '里程分布': {
    chartOption: {
      color: ["#18C492"],
      tooltip: {
        trigger: "axis",
        axisPointer: {
          type: "shadow"
        }
      },
      grid: {
        top: 35
      },
      yAxis: {
        type: "value",
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
      xAxis: [{
        show: true,
        type: "category",
        axisTick: {
          show: true
        },
        axisLabel: {
          textStyle: {
            color: "rgba(255,255,255,.6)",
            fontSize: 12
          }
        },
        axisLine: {
          lineStyle: {
            color: "rgba(255,255,255,.2)"
          }
        }
      }],
      series: [{
        name: "里程分布",
        type: "bar",
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
        barWidth: "60%"
      }]
    },
    setOption: (data: any, option: any) => {
      option.xAxis[0].data = Object.keys(data);
      option.series[0].data = Object.keys(data).map(v => {
        return data[v]
      });
      return option;
    }
  },
  '片区': {
    chartOption: {
      tooltip: {
        trigger: 'item',
        show: true,
        extraCssText: 'box-shadow: 0 0 3px rgba(0, 0, 0, 0.3);',
        formatter: function (obj) {
          if (obj.data.top5Dealer != null) {
            var name2 = obj.data.top5Dealer
            const items = Object.keys(name2)
            return '特约店Top 5' + ':' + '<br />' + items.map(d => {
              return d + ':' + name2[d];
            }).join('<br />')
          } else {
            return obj.name + '：' + obj.value[2] + '件';
          }
        },
        textStyle: {
          fontSize: 10
        }
      },
      series: [],
      geo: {
        map: 'china',
        label: {
          emphasis: {
            show: false
          }
        },
        roam: true,
        zoom: 0.9,
        layoutCenter: ['50%', '50%'],
        layoutSize: "120%",
        itemStyle: {
          normal: {
            areaColor: 'rgba(34, 70, 168, 0.7)',
            borderColor: '#0692a4'
          },
          emphasis: {
            areaColor: 'rgba(119, 139, 224, 0.548)',
          }
        }
      }
    },
    setOption: (data: any, option: any) => {
      var geoCoordMap = {
        '新疆': [86.61, 40.79],
        '西藏': [89.13, 30.66],
        '黑龙江': [128.34, 47.05],
        '吉林': [126.32, 43.38],
        '辽宁': [123.42, 41.29],
        '内蒙古': [112.17, 42.81],
        '北京': [116.40, 40.40],
        '宁夏': [106.27, 36.76],
        '山西': [111.95, 37.65],
        '河北': [115.21, 38.44],
        '天津': [117.04, 39.52],
        '青海': [97.07, 35.62],
        '甘肃': [103.82, 36.05],
        '山东': [118.01, 36.37],
        '陕西': [108.94, 34.46],
        '河南': [113.46, 34.25],
        '安徽': [117.28, 31.86],
        '江苏': [120.26, 32.54],
        '上海': [121.46, 31.28],
        '四川': [103.36, 30.65],
        '湖北': [112.29, 30.98],
        '浙江': [120.15, 29.28],
        '重庆': [107.51, 29.63],
        '湖南': [112.08, 27.79],
        '江西': [115.89, 27.97],
        '贵州': [106.91, 26.67],
        '福建': [118.31, 26.07],
        '云南': [101.71, 24.84],
        '台湾': [121.01, 23.54],
        '广西': [108.67, 23.68],
        '广东': [113.98, 22.82],
        '海南': [110.03, 19.33],
        '澳门': [113.54, 22.19],
        '香港': [114.17, 22.32]
      };
      var convertData = function (data) {
        var res = [];
        for (var i = 0; i < data.length; i++) {
          var geoCoord = geoCoordMap[data[i].name];
          if (geoCoord) {
            res.push({
              name: data[i].name,
              value: geoCoord.concat(data[i].value),
              top5Dealer: data[i].top5Dealer
            });
          }
        }
        return res;
      };
      option.series = [{
        name: '特约店Top5',
        type: 'effectScatter',
        coordinateSystem: 'geo',
        data: convertData(data),
        symbolSize: function (val) {
          if (val[2] < 5) {
            return val[2] / 0.25
          } else if (val[2] < 20) {
            return val[2] / 1
          } else if(val[2] >100) {
            return val[2] / 100
          }else{
            return val[2] / 2
          }
        },
        encode: {
          value: 2
        },
        showEffectOn: 'render',
        rippleEffect: {
          period: 4,
          brushType: 'stroke',
          scale: 10
        },
        hoverAnimation: true,
        label: {
          formatter: function (obj) {
            return obj.name + '：' + obj.value[2] + '件';
          },
          position: 'right',
          show: true,
          color: '#fff'
        },
        emphasis: {
          label: {
            show: true
          }
        },
        itemStyle: {
          shadowBlur: 5,
          shadowColor: '#fff'
        },
        zlevel: 2
      }]
      return option;
    }
  },
  '各渠道信息': {
    chartOption: {
      legend: {
        data: [],
        type: 'scroll',
        textStyle: {
          color: "rgba(255,255,255,.6)",
          fontSize: 10
        }
      },
      grid: {
        top: 35
      },
      tooltip: {
        trigger: 'axis',
        axisPointer: {
          type: "shadow"
        }
      },
      xAxis: [{
        show: true,
        type: "category",
        data: [],
        axisTick: {
          show: false
        },
        axisLabel: {
          textStyle: {
            color: "rgba(255,255,255,.6)",
            fontSize: 12
          },
          formatter: function (value, index) {
            return value.split('-')[1] + '月';
          }
        },
        axisLine: {
          lineStyle: {
            color: "rgba(255,255,255,.2)"
          }
        }
      },
      {
        show: true,
        position: 'bottom',
        type: "category",
        axisTick: {
          interval: function (index, value) {
            return value.split('-')[1] == '01' || index === 0;
          }
        },
        axisLabel: {
          margin: 30,
          align: 'left',
          color: "rgba(255,255,255,.6)",
          interval: function (index, value) {
            return value.indexOf('isFirst') !== -1;
          },
          formatter: function (value, index) {
            return value.split('-')[0] + '年';
          }
        },
        data: []
      }],
      yAxis: {
        type: "value",
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
      series: []
    },
    setOption: (data: any, option: any) => {
      const items = Object.keys(data);
      option.legend.data = Object.keys(data);
      option.xAxis[0].data = (items && items.length > 0) ? data[items[0]].map(v => v.month) : [];
      const firstMonths = [];
      if(items && items.length) {
         const years = new Set(data[items[0]].map(v => v.month).map(value => value.split('-')[0]));
         for(let year of Array.from(years)) {
            const inYears = data[items[0]].map(v => v.month).filter(d => d.indexOf(year) !== -1);
            firstMonths.push(inYears[0])
         }
      }
      option.xAxis[1].data = (items && items.length > 0) ? data[items[0]].map(v => {
        return { value: v.month + (firstMonths.indexOf(v.month) !== -1 ? 'isFirst' : '') }
      }) : [];
      option.series = items.map((d, i) => {
        return {
          name: d,
          type: 'line',
          smooth: true,
          itemStyle: {
            color: colorList[i]
          },
          symbol: 'circle',
          symbolSize: 2,
          data: data[d].map(v => v.totalCount)
        }
      });
      return option;
    }
  },
  '特约店': {
    chartOption: {
      tooltip: {
        trigger: "axis",
        axisPointer: {
          type: "shadow"
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
        top: 35
      },
      yAxis: {
        type: "value",
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
      xAxis: [{
        show: true,
        type: "category",
        data: [],
        axisTick: {
          show: false
        },
        axisLabel: {
          textStyle: {
            color: "rgba(255,255,255,.6)",
            fontSize: 12
          }
        },
        axisLine: {
          lineStyle: {
            color: "rgba(255,255,255,.2)"
          }
        }
      }],
      series: []
    },
    setOption: (data: any, option: any) => {
      const items = Object.keys(data);
      option.legend.data = Object.keys(data);
      option.xAxis[0].data = (items && items.length > 0) ? Object.keys(data[option.legend.data[0]]) : [];
      option.series = items.map((d, i) => {
        return {
          name: d,
          type: 'bar',
          itemStyle: {
            color: colorList[i]
          },
          barWidth: "60%",
          stack: '总量',
          data: Object.keys(data[d]).map(k => {
            return {
              name: k,
              value: data[d][k]
            }
          })
        }
      });
      return option;
    }
  },
  '生产': {
    chartOption: {
      tooltip: {
        trigger: "axis",
        axisPointer: {
          type: "shadow"
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
        top: 35
      },
      yAxis: {
        type: "value",
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
      xAxis: [{
        show: true,
        type: "category",
        data: [],
        axisTick: {
          show: false
        },
        axisLabel: {
          textStyle: {
            color: "rgba(255,255,255,.6)",
            fontSize: 12
          },
          formatter: function (value, index) {
            return value.split('-')[1] + '月';
          }
        },
        axisLine: {
          lineStyle: {
            color: "rgba(255,255,255,.2)"
          }
        }
      }, {
        show: true,
        position: 'bottom',
        type: "category",
        axisTick: {
          interval: (index, value) => {
            return value.split('-')[1] == '01' || index == 0;
          }
        },
        axisLabel: {
          margin: 30,
          align: 'left',
          color: "rgba(255,255,255,.6)",
          interval: (index, value) => {
            return value.indexOf('isFirst') !== -1;
          },
          formatter: function (value, index) {
            return value.split('-')[0] + '年';
          }
        },
        data: []
      }],
      series: []
    },
    setOption: (data: any, option: any) => {
      const items = Object.keys(data);
      option.legend.data = Object.keys(data);
      option.xAxis[0].data = (items && items.length > 0) ? data[items[0]].map(v => v.month) : [];
      const firstMonths = [];
      if(items && items.length) {
         const years = new Set(data[items[0]].map(v => v.month).map(value => value.split('-')[0]));
         for(let year of Array.from(years)) {
            const inYears = data[items[0]].map(v => v.month).filter(d => d.indexOf(year) !== -1);
            firstMonths.push(inYears[0])
         }
      }
      option.xAxis[1].data = (items && items.length > 0) ? data[items[0]].map(v => {
        return { value: v.month + (firstMonths.indexOf(v.month) !== -1 ? 'isFirst' : '') }
      }) : [];
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
          data: data[d].map(v => v.totalCount)
        }
      });
      return option;
    }
  },
  '故障': {
    chartOption: {
      tooltip: {
        trigger: "axis",
        axisPointer: {
          type: "shadow"
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
        top: 35
      },
      yAxis: {
        type: "value",
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
      xAxis: [{
        show: true,
        type: "category",
        data: [],
        axisTick: {
          show: false
        },
        axisLabel: {
          textStyle: {
            color: "rgba(255,255,255,.6)",
            fontSize: 12
          },
          formatter: function (value, index) {
            return value.split('-')[1] + '月';
          }
        },
        axisLine: {
          lineStyle: {
            color: "rgba(255,255,255,.2)"
          }
        }
      }, {
        show: true,
        position: 'bottom',
        type: "category",
        axisTick: {
          interval: (index, value) => {
            return value.split('-')[1] == '01' || index == 0;
          }
        },
        axisLabel: {
          margin: 30,
          align: 'left',
          color: "rgba(255,255,255,.6)",
          interval: (index, value) => {
            return value.indexOf('isFirst') !== -1;
          },
          formatter: function (value, index) {
            return value.split('-')[0] + '年';
          }
        },
        data: []
      }],
      series: []
    },
    setOption: (data: any, option: any) => {
      const items = Object.keys(data);
      option.legend.data = Object.keys(data);
      option.xAxis[0].data = (items && items.length > 0) ? data[items[0]].map(v => v.month): [];
      const firstMonths = [];
      if(items && items.length) {
         const years = new Set(data[items[0]].map(v => v.month).map(value => value.split('-')[0]));
         for(let year of Array.from(years)) {
            const inYears = data[items[0]].map(v => v.month).filter(d => d.indexOf(year) !== -1);
            firstMonths.push(inYears[0])
         }
      }
      option.xAxis[1].data = (items && items.length > 0) ? data[items[0]].map(v => {
        return { value: v.month + (firstMonths.indexOf(v.month) !== -1 ? 'isFirst' : '') }
      }) : [];
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
          data: data[d].map(v => v.totalCount)
        }
      });
      return option;
    }
  },
  '经过': {
    chartOption: {
      tooltip: {
        trigger: "axis",
        axisPointer: {
          type: "shadow"
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
        top: 35
      },
      yAxis: {
        type: "value",
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
      xAxis: [{
        show: true,
        type: "category",
        data: [],
        axisTick: {
          show: false
        },
        axisLabel: {
          textStyle: {
            color: "rgba(255,255,255,.6)",
            fontSize: 12
          }
        },
        axisLine: {
          lineStyle: {
            color: "rgba(255,255,255,.2)"
          }
        }
      }],
      series: []
    },
    setOption: (data: any, option: any) => {
      const items = Object.keys(data);
      option.legend.data = Object.keys(data);
      option.xAxis[0].data = (items && items.length > 0) ? data[items[0]].map(v => v.month) : [];
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
          data: data[d].map(v => v.totalCount)
        }
      });
      return option;
    }
  }
};

@Component({
  selector: 'km-analysis-common-chart',
  templateUrl: './analysis-common-chart.component.html',
  styleUrls: ['./analysis-common-chart.component.less']
})
export class AnalysisCommonChartComponent implements OnInit {

  chartInstance: any;

  _searchParams: any;

  chartType: string;

  showBack = false;

  isLoading = false;

  typeMap = {
    '生产': 0,
    '故障': 1,
    '经过': 2,
    '各渠道信息': 3,
  };

  @Input()
  set type(value: string) {
    this.chartType = value;
    this.chartOption = Object.assign(chartConfig[value].chartOption);
  }

  @Input() searchParams: any;


  @Input()
  set data(value: any) {
    if (value) {
      this.chartOption = chartConfig[this.chartType].setOption(value, this.chartOption);
      this.renderChart();
    }else {
      this.chartOption = chartConfig[this.chartType].setOption([], this.chartOption);
      this.renderChart();
    }
  }

  chartOption: any;

  constructor(
    private api: AnalysisService,
    private http: HttpClient
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

  onBack() {
    this.showBack = false;
    this.chartInstance.clear();
    this.chartInstance.setOption(this.chartOption);
  }

  onChartClick(event: any) {
    if (!this.showBack && this.chartInstance && this.typeMap[this.chartType] !== undefined) {
      if (this.chartType == '各渠道信息') {
        this.onChannelClick(event);
      } else {
        this.onDayChartClick(event);
      }
    }
  }

  onChannelClick(event: any) {
    this.searchParams.month = event.name;
    this.searchParams.chartType = this.typeMap[this.chartType];
    this.isLoading = true;
    this.api.days(this.searchParams).pipe(
      finalize(() => this.isLoading = false)
    ).subscribe((res: any) => {
      this.showBack = true;
      this.chartInstance.clear();
      const channels = Array.from(new Set(res.map(d => d.dataSource)));
      const data = Array.from(channels).map((d, i) => {
        return {
          name: d,
          type: 'line',
          itemStyle: {
            color: colorList[i]
          },
          symbol: 'circle',
          symbolSize: 8,
          data: res.filter(v => v.dataSource == d).map(v => v.count)
        }
      });
      this.chartInstance.setOption({
        legend: {
          data: channels,
          type: 'scroll',
          textStyle: {
            color: "rgba(255,255,255,.6)",
            fontSize: 10
          }
        },
        grid: {
          top: 35
        },
        tooltip: {
          trigger: 'axis'
        },
        xAxis: {
          type: 'category',
          boundaryGap: false,
          data: res.filter(d => d.dataSource == channels[0]).map(v => v.day),
          axisTick: {
            show: false
          },
          axisLabel: {
            textStyle: {
              color: "rgba(255,255,255,.6)",
              fontSize: 12
            }
          },
          axisLine: {
            lineStyle: {
              color: "rgba(255,255,255,.2)"
            }
          }
        },
        yAxis: {
          type: "value",
          axisLine: {
            show: false,
            lineStyle: {
              color: "rgba(255,255,255,.1)"
            }
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
        series: data
      });
    })
  }

  onDayChartClick(event: any) {
    if (!this.showBack && this.chartInstance) {
      this.searchParams.month = event.name;
      this.searchParams.chartType = this.typeMap[this.chartType];
      this.isLoading = true;
      this.api.days(this.searchParams).pipe(
        finalize(() => this.isLoading = false)
      ).subscribe((res: any) => {
        this.showBack = true;
        this.chartInstance.clear();
        this.chartInstance.setOption({
          color: ["#54A1EE"],
          tooltip: {
            trigger: "axis",
            axisPointer: {
              type: "shadow"
            }
          },
          grid: {
            top: 35
          },
          yAxis: {
            type: "value",
            axisLine: {
              show: false,
              lineStyle: {
                color: "rgba(255,255,255,.1)"
              }
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
          xAxis: [{
            show: true,
            type: "category",
            data: res.map(v => v.day),
            axisTick: {
              show: false
            },
            axisLine: {
              show: false,
              lineStyle: {
                color: "rgba(255,255,255,.1)"
              }
            },
            axisLabel: {
              textStyle: {
                color: "rgba(255,255,255,.6)",
                fontSize: 12
              },
              formatter: function (value, index) {
                return value.indexOf('日') !== -1 ? value : value.split('-')[2] + '日';
              }
            },
          }],
          series: [{
            name: this.chartType,
            type: "bar",
            barWidth: "60%",
            data: res.map(v => v.count)
          }]
        });
      })
    }
  }

}
