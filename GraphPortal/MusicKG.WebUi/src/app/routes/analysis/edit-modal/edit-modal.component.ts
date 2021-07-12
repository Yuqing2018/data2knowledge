import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'km-edit-modal',
  templateUrl: './edit-modal.component.html',
  styleUrls: ['./edit-modal.component.less']
})
export class EditModalComponent implements OnInit {

  @Input() partName: string;

  @Input() syndrome: string;

  @Input() dataSource: string;

  isAddToTraining = true;

  constructor() { }

  ngOnInit() {
  }

}
