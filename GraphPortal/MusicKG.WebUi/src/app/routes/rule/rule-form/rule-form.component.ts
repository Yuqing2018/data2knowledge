import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';

@Component({
  selector: 'km-rule-form',
  templateUrl: './rule-form.component.html',
  styleUrls: ['./rule-form.component.less']
})
export class RuleFormComponent implements OnInit {

  @Input() type: string;
  isDict: boolean

  validateForm: FormGroup;

  className = ['DictMonth', 'RegExpYear', 'RuleDate']
  entityName = ['AGE', 'ANIMAL', 'DATE', 'EVENT']

  dataSet = [
    {
      key         : '1',
      word        : 'January',
      Shorthand   : 'January,Jan,Jan.',
      speech      : 'Noun'
    },
    {
      key         : '1',
      word        : 'January',
      Shorthand   : 'January,Jan,Jan.',
      speech      : 'Noun'
    },
    {
      key         : '1',
      word        : 'January',
      Shorthand   : 'January,Jan,Jan.',
      speech      : 'Noun'
    }
  ];

  constructor(
    private fb: FormBuilder
  ) { }

  submitForm(): void {
    for (const i in this.validateForm.controls) {
      this.validateForm.controls[ i ].markAsDirty();
      this.validateForm.controls[ i ].updateValueAndValidity();
    }
  }

  ngOnInit(): void {
    if (this.type === 'dict') {
      this.isDict = true
    } else {
      this.isDict = false
    }
    this.validateForm = this.fb.group({
      name  : [ null, [ Validators.required ] ],
      calss_name: [ null, [ Validators.required ] ],
      entity_name: [ null, [ Validators.required ] ]
    });
  }



  ngModelChange() {
    console.log('=')
  }


}
