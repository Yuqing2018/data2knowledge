import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'km-batch-modal',
  templateUrl: './batch-modal.component.html',
  styleUrls: ['./batch-modal.component.less']
})
export class BatchModalComponent implements OnInit {
  partName: string;

  syndrome: string;

  @Input() dataSource: string;
  
  constructor() { }

  ngOnInit() {
  }

}
