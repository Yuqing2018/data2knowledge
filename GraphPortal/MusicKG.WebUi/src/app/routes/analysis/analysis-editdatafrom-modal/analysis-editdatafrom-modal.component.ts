import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'km-analysis-editdatafrom-modal',
  templateUrl: './analysis-editdatafrom-modal.component.html',
  styleUrls: ['./analysis-editdatafrom-modal.component.less']
})
export class AnalysisEditdatafromModalComponent implements OnInit {

  @Input() dataFromDesc:string;

  constructor() { }

  ngOnInit() {
  }

}
