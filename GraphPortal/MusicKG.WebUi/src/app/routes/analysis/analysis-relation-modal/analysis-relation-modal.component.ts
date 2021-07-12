import { Component, OnInit,Input } from '@angular/core';

@Component({
  selector: 'km-analysis-relation-modal',
  templateUrl: './analysis-relation-modal.component.html',
  styleUrls: ['./analysis-relation-modal.component.less']
})
export class AnalysisRelationModalComponent implements OnInit {

  @Input() qicNo:string;

  @Input() qisNo:string;

  constructor() { }

  ngOnInit() {
  }

}
