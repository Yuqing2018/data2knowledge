import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'km-analysis-table',
  templateUrl: './analysis-table.component.html',
  styleUrls: ['./analysis-table.component.less']
})
export class AnalysisTableComponent implements OnInit {

  @Input() carType: string;

  @Input() partNo: string;

  constructor() { }

  ngOnInit() {
  }

}
