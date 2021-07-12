import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'km-circular-confirm',
  templateUrl: './circular-confirm.component.html',
  styleUrls: ['./circular-confirm.component.less']
})
export class CircularConfirmComponent implements OnInit {
  confirmForm: FormGroup;

  @Input()
  set data(data: any) {
    this.confirmForm = this.fb.group({
      confirmedMessage: [data.confirmedMessage, []],
      pushStatus: [data.pushStatus, []],
      permanentCntrStatus: [data.permanentCntrStatus, []],
      isExcessive: [data.isExcessive, []],
    });
  }

  constructor(
    private fb: FormBuilder,
  ) {
    this.confirmForm = this.fb.group({
      confirmedMessage: [null, []],
      pushStatus: ['未立项', []],
      permanentCntrStatus: [null, []],
      isExcessive: [null, []],
    });
  }

  ngOnInit() {
  }

}
