import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';

@Component({
  selector: 'km-user-password',
  templateUrl: './user-password.component.html',
  styleUrls: ['./user-password.component.less']
})
export class UserPasswordComponent implements OnInit {

  @Input() data
  validateForm: FormGroup;

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.validateForm = this.fb.group({
      password: [null, [Validators.required, Validators.minLength(6), Validators.maxLength(128)]]
    });
  }

  hasError(controlName: string, error: string) {
    let formControl = this.validateForm.get(controlName);
    return formControl.dirty && formControl.errors && formControl.errors[error];
  }

}
