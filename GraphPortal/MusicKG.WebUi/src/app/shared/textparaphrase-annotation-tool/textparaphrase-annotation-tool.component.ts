import { Component, OnInit, forwardRef, Output, EventEmitter } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
  NG_VALUE_ACCESSOR
} from '@angular/forms';


export interface TextparaphraseModel {
  Id: string,
  Subject: string,
  Predicate: string,
  Object: string,
  ExampleQuestions: Array<string>,
  preResult: Array<any>
}


@Component({
  selector: 'km-textparaphrase-annotation-tool',
  templateUrl: './textparaphrase-annotation-tool.component.html',
  styleUrls: ['./textparaphrase-annotation-tool.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TextparaphraseAnnotationToolComponent),
      multi: true
    }
  ]
})
export class TextparaphraseAnnotationToolComponent implements OnInit {

  validateForm: FormGroup;
  controlArray: Array<{ id: number, controlInstance: string }> = [];
  dataSet: string
  @Output() formData = new EventEmitter<any>()

  private _currentData: TextparaphraseModel
  private _focusIndex: number

  set currentData(value) {
    this._currentData = value
  }

  get currentData() {
    return this._currentData;
  }

  get newInputValue() {
    return this.dataSet && this.dataSet.indexOf(this.currentData.Subject) === -1 ? true : false
  }

  get focusIndex() {
    if (this.dataSet != undefined && this._focusIndex >= 0) {
      return this._focusIndex
    }
    if (this.dataSet) {
      return this.dataSet.length
    }
    return 0
  }

  set focusIndex(value) {
    this._focusIndex = value
  }


  constructor(
    private fb: FormBuilder,
    private message: NzMessageService
  ) { }

  ngOnInit() {
    this.validateForm = this.fb.group({});
  }

  propagateChange = (_: any) => { };

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
  }

  writeValue(value: TextparaphraseModel) {
    if (value) {
      this.currentData = value;
      this.validateForm.reset();
      this.resetFormControl()
      if (value.preResult.length === 0) {
        // this.addField()
      } else {
        value.preResult.map((item, index) => {
          this.addField()
          this.validateForm.patchValue({ [`question` + index]: item })
        })
      }
    }
  }

  newAddResult(e?: MouseEvent) {
    if (this.newInputValue || !this.dataSet) {
      this.message.warning('请依照标准说明填写相似问题')
      return
    }
    this.getCurrentData()
    this.addField()
  }

  addField(e?: MouseEvent): void {
    if (e) {
      e.preventDefault();
    }
    const id = (this.controlArray.length > 0) ? this.controlArray[this.controlArray.length - 1].id + 1 : 0;

    const control = {
      id,
      controlInstance: `question${id}`
    };
    const index = this.controlArray.push(control);
    let subject = this.parseSubject(this.currentData.Subject)
    this.validateForm.addControl(this.controlArray[index - 1].controlInstance, new FormControl(null, [Validators.pattern(new RegExp(subject))]));
    this.validateForm.patchValue({ [control.controlInstance]: this.dataSet })
    this.dataSet = null
  }

  parseSubject(val) {
    return val.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&')
  }

  keyEventListener(e) {
    if (e.keyCode === 13) {
      this.newAddResult()
    }
  }

  removeField(i: { id: number, controlInstance: string }, e: MouseEvent): void {
    e.preventDefault();
    const index = this.controlArray.indexOf(i);
    this.controlArray.splice(index, 1);
    this.validateForm.removeControl(i.controlInstance);
    this.getCurrentData()
  }

  hasError(controlName: string, error: string) {
    let formControl = this.validateForm.get(controlName);
    return formControl.dirty && formControl.errors && formControl.errors[error];
  }

  resetFormControl() {
    this.controlArray.map(item => {
      this.validateForm.removeControl(item.controlInstance)
    })
    this.controlArray = []
  }

  getCurrentData() {
    let arr = []
    if (this.validateForm.valid) {
      arr = Object.values(this.validateForm.value)
    } else {
      arr = Object.values(this.validateForm.value).filter(val => JSON.stringify(val).indexOf(this.currentData.Subject) !== -1)
    }
    if (!this.newInputValue && this.dataSet != undefined) {
      arr.push(this.dataSet)
    }
    this.formData.emit(Array.from(new Set(arr)))
    this.focusIndex = null
  }

  checkValue() {
    return this.validateForm.valid && !this.newInputValue
  }

  getEvent(e) {
    this.focusIndex = e.target.selectionStart
  }

  addContent(type) {
    if (this.dataSet == undefined) {
      this.dataSet = this.currentData[type]
    } else {
      this.dataSet = this.dataSet.substring(0, this.focusIndex) + this.currentData[type] + this.dataSet.substring(this.focusIndex, this.dataSet.length)
    }
  }

}
