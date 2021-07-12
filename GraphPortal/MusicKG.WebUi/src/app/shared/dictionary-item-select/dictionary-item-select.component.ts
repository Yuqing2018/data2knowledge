import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { FormGroup, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { RuleService } from 'src/app/services/rule.service';

@Component({
  selector: 'km-dictionary-item-select',
  templateUrl: './dictionary-item-select.component.html',
  styleUrls: ['./dictionary-item-select.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DictionaryItemSelectComponent),
      multi: true
    }
  ]
})
export class DictionaryItemSelectComponent implements OnInit {
  selectedValue: string;
  dictionaryItemList = [];
  searchedDictionaryItemList = [];
  dictionaryBuffer = [];
  bufferSize = 10;
  loading = true;

  @Input()
  set data(value: any[]) {
    this.loading = false;
    this.dictionaryItemList = value;
    this.searchedDictionaryItemList = this.dictionaryItemList;
    this.addSelectItem();
  }

  propagateChange = (_: any) => { };

  constructor(
  ) { }

  ngOnInit() {
  }

  addSelectItem() {
    setTimeout(() => {
      let list = this.dictionaryItemList.slice(0, this.bufferSize);
      if (this.selectedValue && list.indexOf(this.selectedValue) == -1 && this.dictionaryItemList.indexOf(this.selectedValue) !== -1) {
        list.unshift(this.selectedValue);
      }
      this.dictionaryBuffer = list;
    }, 100);
  }

  onSearch(value: string) {
    this.searchedDictionaryItemList = this.dictionaryItemList.filter((t: string) => !value || (t && t.toLocaleLowerCase().indexOf(value.toLocaleLowerCase()) !== -1));
    this.dictionaryBuffer = this.searchedDictionaryItemList.slice(0, this.bufferSize);
  }

  onScrollToBottom() {
    const len = this.dictionaryBuffer.length;
    if (len < this.searchedDictionaryItemList.length) {
      this.loading = true;
      const more = this.searchedDictionaryItemList.slice(len, this.bufferSize + len);
      this.dictionaryBuffer = this.dictionaryBuffer.concat(more)
      this.loading = false;
    }
  }

  valueChange() {
    this.propagateChange(this.selectedValue);
  }

  writeValue(value: any) {
    this.selectedValue = value;
    this.addSelectItem();
  }

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
  }

}
