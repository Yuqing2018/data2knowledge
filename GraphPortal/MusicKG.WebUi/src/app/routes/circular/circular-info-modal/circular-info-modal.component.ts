import { Component, OnInit, Input } from '@angular/core';
import { format, subMonths, startOfMonth } from 'date-fns';

@Component({
  selector: 'km-circular-info-modal',
  templateUrl: './circular-info-modal.component.html',
  styleUrls: ['./circular-info-modal.component.less']
})
export class CircularInfoModalComponent implements OnInit {
  info: any;
  dataSet = [];
  months = [
    format(subMonths(startOfMonth(new Date()), 0), 'YYYY-MM'),
    format(subMonths(startOfMonth(new Date()), 1), 'YYYY-MM'),
    format(subMonths(startOfMonth(new Date()), 2), 'YYYY-MM')
  ];

  multipleMetrics: any;

  riskMetrics: any

  riskMetricsTH: any;

  againMetrics: any;

  show(type: string) {
    if(this.info && this.info.warningTask){
      return this.info.warningTask.warningIndex.find(d => d.warningType.indexOf(type) !== -1)
    }
    return true;
  }

  @Input()
  set data(data: any) {
    this.info = data.info;
    this.multipleMetrics = Object.keys(data.data.multipleMetrics).map(key => {
      return {
        name: key,
        total: data.data.multipleMetrics[key].lastThreeMonthCount['第1月'] + data.data.multipleMetrics[key].lastThreeMonthCount['第2月'] + data.data.multipleMetrics[key].lastThreeMonthCount['第3月'],
        ...data.data.multipleMetrics[key]
      }
    });
    this.riskMetricsTH = Object.keys(data.data.riskMetrics.dataSourceMetrics);
    this.riskMetrics = [
      {
        ...data.data.riskMetrics,
        syndromeModel: data.data.syndromeModel,
        dataSourceMetrics: this.riskMetricsTH.map(key => {
          return data.data.riskMetrics.dataSourceMetrics[key]
        })
      }
    ];
    this.againMetrics = [
      {
        ...data.data.againMetrics,
        syndromeModel: data.data.syndromeModel
      }
    ];
  }


  constructor() { }

  ngOnInit() {
  }

}
