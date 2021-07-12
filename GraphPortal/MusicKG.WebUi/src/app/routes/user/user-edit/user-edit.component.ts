import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Option } from '../../../interfaces/api.interface';
import { OptionService } from '.././../../services/option.service';

@Component({
  selector: 'km-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.less']
})
export class UserEditComponent implements OnInit {

  @Input() data
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
      name  : [ this.data.name, [ Validators.required ] ],
      roles: [ null, [ Validators.required ] ]
    });
  }

}
