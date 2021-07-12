import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'km-frequency-form',
  templateUrl: './frequency-form.component.html',
  styleUrls: ['./frequency-form.component.less']
})
export class FrequencyFormComponent implements OnInit {
  frequencyForm: FormGroup;

  @Input()
  set data(data: any) {
    this.frequencyForm = this.fb.group({
      isFocused: [data.isFocused == '已关注' ? true : false, []],
      frequency: [data.frequency, []],
      specifiedDate:[data.specifiedDate]
    });
  }

  constructor(
    private fb: FormBuilder
  ) {
    this.frequencyForm = this.fb.group({
      isFocused: [null, []],
      frequency: [null, []],
      specifiedDate: [null]
    });
  }

  get frequency() {
    return this.frequencyForm.value['frequency']
  }

  ngOnInit() {
  }

}
