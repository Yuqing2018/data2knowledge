import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { OptionService } from '../../../services/option.service';
import { Option } from '../../../interfaces/api.interface';

@Component({
  selector: 'km-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.less']
})
export class UserFormComponent implements OnInit {
  validateForm: FormGroup;
  userRole: Option[]

  constructor(
    private fb: FormBuilder,
    private optionService: OptionService
  ) { }

  ngOnInit() {
    this.optionService.getUserRole().subscribe(res => {
      this.userRole = res
    })
    this.validateForm = this.fb.group({
      name  : [ null, [ Validators.required, Validators.maxLength(20) ] ],
      roles: [ null, [ Validators.required ] ],
      password: [ null, [ Validators.required, Validators.minLength(6), Validators.maxLength(128) ] ]
    });
  }

  hasError(controlName: string, error: string) {
    let formControl = this.validateForm.get(controlName);
    return formControl.dirty && formControl.errors && formControl.errors[error];
  }

}
