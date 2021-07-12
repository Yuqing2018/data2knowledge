import { Component, OnInit, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Platform } from '@angular/cdk/platform'

@Component({
  selector: 'km-password',
  templateUrl: './password.component.html',
  styleUrls: ['./password.component.less']
})
export class PasswordComponent implements OnInit {

  validateForm: FormGroup;

  passwordVisible: boolean = false
  password: boolean

  constructor(
    private fb: FormBuilder,
    private el:ElementRef,
    public platform:　Platform
  ) { }

  ngOnInit() {
    this.platform.TRIDENT ? this.password = true : this.password = false
    this.validateForm = this.fb.group({
      password: [null, [Validators.required,Validators.maxLength(128), Validators.minLength(6)]]
    });
  }

  hasError(controlName: string, error: string) {
    let formControl = this.validateForm.get(controlName);
    return formControl.dirty && formControl.errors && formControl.errors[error];
  }

  isPasswordVisible() {
    this.passwordVisible = !this.passwordVisible
    if (this.platform.TRIDENT) {
      this.password = !this.password
    }
  }
}
