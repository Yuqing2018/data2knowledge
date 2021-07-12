import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { FormGroup, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Platform } from '@angular/cdk/platform';

@Component({
  selector: 'km-input-password',
  templateUrl: './password.component.html',
  styleUrls: ['./password.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => PasswordComponent),
      multi: true
    }
  ]
})
export class PasswordComponent implements OnInit, ControlValueAccessor {
  passwordVisible = false;
  passwordValue: string;

  propagateChange = (_: any) => { };
  
  constructor(
    public platform:　Platform
  ) { }

  ngOnInit() {
  }

  writeValue(value: any) {
    this.passwordValue = value;
  }

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
  }

}
