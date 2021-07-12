import { Component, OnInit, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { FaqService, KnowledgeTemplateModel } from 'src/app/services/faq.service';

@Component({
  selector: 'shared-knowledge-template-select',
  templateUrl: './knowledge-template-select.component.html',
  styleUrls: ['./knowledge-template-select.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => KnowledgeTemplateSelectComponent),
      multi: true
    }
  ]
})
export class KnowledgeTemplateSelectComponent implements OnInit, ControlValueAccessor {
  selectedValue: number;
  knowleadgeTemplateList: KnowledgeTemplateModel[];  
  propagateChange = (_: any) => { };

  constructor(
    private ktService: FaqService
  ) { }

  ngOnInit() {
    this.ktService.getTemplateList().subscribe(res => {
      this.knowleadgeTemplateList = res.beans;
    });
  }

  valueChange() {
    this.propagateChange(this.selectedValue);
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
