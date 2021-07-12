import { Component, OnInit, forwardRef } from '@angular/core';
import { FormGroup, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'km-input-name',
  templateUrl: './input-name.component.html',
  styleUrls: ['./input-name.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputNameComponent),
      multi: true
    }
  ]
})
export class InputNameComponent implements OnInit, ControlValueAccessor {

  originName: string;

  propagateChange = (_: any) => { };

  constructor(
  ) { }

  ngOnInit() {
  }

  writeValue(value: any) {
    this.originName = value
  }

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
  }



}
