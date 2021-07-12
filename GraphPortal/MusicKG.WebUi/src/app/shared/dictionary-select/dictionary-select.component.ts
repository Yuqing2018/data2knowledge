import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { FormGroup, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { RuleService } from 'src/app/services/rule.service';

@Component({
  selector: 'km-dictionary-select',
  templateUrl: './dictionary-select.component.html',
  styleUrls: ['./dictionary-select.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DictionarySelectComponent),
      multi: true
    }
  ]
})
export class DictionarySelectComponent implements OnInit, ControlValueAccessor {
  selectedValue: string;
  dictionaryList = [];

  @Input()
  set workspaceId(id: string) {
    if(id) {
      this.ruleService.getList(id).subscribe((res: any) => {
        this.dictionaryList = res.items;
      })
    }
  }

  propagateChange = (_: any) => { };

  constructor(
    private ruleService: RuleService
  ) { }

  ngOnInit() {
  }

  valueChange() {
    this.propagateChange(this.selectedValue ? [this.selectedValue] : null);
  }

  writeValue(value: any) {
    this.selectedValue = value;
  }

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
  }

}
