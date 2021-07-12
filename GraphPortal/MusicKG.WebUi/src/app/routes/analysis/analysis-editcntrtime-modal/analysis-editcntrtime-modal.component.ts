import { Component, OnInit,Input } from '@angular/core';

@Component({
  selector: 'km-analysis-editcntrtime-modal',
  templateUrl: './analysis-editcntrtime-modal.component.html',
  styleUrls: ['./analysis-editcntrtime-modal.component.less']
})
export class AnalysisEditcntrtimeModalComponent implements OnInit {

  @Input() permanentCntrTime:string;

  constructor() { }

  ngOnInit() {
  }

}
