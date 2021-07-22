import { Component, OnInit, Input } from '@angular/core';
import { AnalysisService } from 'src/app/services/analysis.service';
import { finalize } from 'rxjs/operators';
import * as randomColor from 'randomcolor';
import { format } from 'date-fns';

const colorList = ['#1557FF', '#61DDAA', '#18C492', '#000000'].concat(randomColor({
  count: 100
}));

@Component({
  selector: 'km-analysis-line-chart',
  templateUrl: './analysis-line-chart.component.html',
  styleUrls: ['./analysis-line-chart.component.less']
})
export class AnalysisLineChartComponent implements OnInit {

  chartInstance: any;

  showBack = false;

  _searchParams: any;

  isLoading = false;

  chartType: string;

  @Input() searchParams: any;

  @Input() hasSelect = false;

  defectRateDistribution: any;

  filterTime = '按经过时间'

  dataIndex = 0;

  @Input() defectRateByCntrDistribution: any;

  @Input()
  set data(value: any) {
    this.defectRateDistribution = value;
    this.renderChart();
  }

  filterChange(filter: string) {
    this.filterTime = filter;
    this.renderChart();
  }

  renderChart() {
    const value = this.filterTime == '按经过时间'? this.defectRateDistribution : this.defectRateByCntrDistribution;
    if (value) {
      const years = Object.keys(value);
      this.chartOption.legend.data = Object.keys(value);
      this.chartOption.xAxis.data = value[years[0]].map(v =>  v.month);
      this.chartOption.series = years.map((d, i) => {
        const markPoints = value[d].filter(t => t.cntrTime).map(c => {
          return {
            yAxis: c['defectRate'],
            xAxis: c['month']
          }
        });
        return {
          name: d,
          type: 'line',
          itemStyle: {
            color: colorList[i]
          },
          markPoint: {
            label: {
                formatter: params => {
                  return '对策';
                },
                fontSize: 8
            },
            data: markPoints
        },
          symbol: 'circle',
          symbolSize: 2,
          data: value[d].map(v => {
            return {
              value: v.defectRate,
              cntrTime: v.cntrTime
            }
          })
        }
      });
      this.dataIndex = value.length;
      this.onBack();
    }else {
      this.chartOption.series = null;
      this.chartOption.xAxis.data = [];
      this.chartOption.legend.data = [];
      this.onBack();
    }
  }

  chartOption = {
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
      },
      formatter: (params) => {
        return params.map(d => {
          let result = d.seriesName + ':' + d.axisValueLabel + ' ' + d.data.value * 100 + '%';
          if(d.data.cntrTime) {
            result += `  对策时间(${format(d.data.cntrTime, 'YYYY-MM-DD')})`;
          }
          return result;
        }).join('<br />')
      }
    },
    xAxis: {
      type: 'category',
      boundaryGap: false,
      data: [],
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
      offset: -5,
      axisLine: {
        show: false
      },
      minInterval: 0.005,
      axisTick: {
        show: false
      },
      axisLabel: {
        textStyle: {
          color: "rgba(255,255,255,.6)",
          fontSize: 12
        },
        formatter: (v) => v * 100 + '%'
      },
      splitLine: {
        lineStyle: {
          color: "rgba(255,255,255,.1)"
        }
      },
    },
    series: null
  };

  constructor(
    private api: AnalysisService
  ) { }

  ngOnInit() {
  }

  onBack() {
    this.showBack = false;
    if(this.chartInstance) {
      this.chartInstance.clear();
      this.chartInstance.setOption(this.chartOption);
    }
  }

  onChartClick(event: any) {
    if (!this.showBack && this.chartInstance) {
      this.searchParams.month = event.name;
      this.searchParams.chartType = '经过';
      this.isLoading = true;
      this.api.linedays(this.searchParams, this.filterTime == '按经过时间' ? 'defectrate' : 'cntrrate').pipe(
        finalize(() => this.isLoading = false)
      ).subscribe((res: any) => {
        this.showBack = true;
        this.chartInstance.clear();
        const years = Object.keys(res);
        const data = years.map((d, i) => {
          return {
            name: d,
            type: 'line',
            itemStyle: {
              color: colorList[i]
            },
            symbol: 'circle',
            symbolSize: 2,
            data: res[d].map(v => v.defectRate)
          }
        });
        this.chartInstance.setOption({
          legend: {
            data: years,
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
            },
            formatter: (params) => {
              return params.map(d => d.seriesName + ':' + d.axisValueLabel + ' ' + d.data * 100 + '%').join('<br />')
            }
          },
          xAxis: {
            type: 'category',
            boundaryGap: false,
            data: res[years[0]].map(v => v.day),
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
              show: false
            },
            axisTick: {
              show: false
            },
            axisLabel: {
              textStyle: {
                color: "rgba(255,255,255,.6)",
                fontSize: 12
              },
              formatter: (v) => v * 100 + '%'
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
  }

  onChartInit(e: any) {
    this.chartInstance = e;
  }


}
