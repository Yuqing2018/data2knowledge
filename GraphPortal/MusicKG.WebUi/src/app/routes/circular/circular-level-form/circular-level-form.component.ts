import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'km-circular-level-form',
  templateUrl: './circular-level-form.component.html',
  styleUrls: ['./circular-level-form.component.less']
})
export class CircularLevelFormComponent implements OnInit {
  levelForm: FormGroup;

  @Input()
  set data(data: any) {
    this.levelForm = this.fb.group({
      aiRiskLevel: [data.aiRiskLevel, []],
      usedForModel: [data.usedForModel, []]
    });
  }


  constructor(
    private fb: FormBuilder
  ) {
    this.levelForm = this.fb.group({
      aiRiskLevel: [null, []],
      usedForModel: [null, []]
    });
  }

  ngOnInit() {
  }

}
