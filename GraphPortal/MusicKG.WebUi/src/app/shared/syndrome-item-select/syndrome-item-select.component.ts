import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { FormGroup, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'km-syndrome-item-select',
  templateUrl: './syndrome-item-select.component.html',
  styleUrls: ['./syndrome-item-select.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SyndromeItemSelectComponent),
      multi: true
    }
  ]
})
export class SyndromeItemSelectComponent implements OnInit {
  selectedValue: any;
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

  addSelectItem() {
    setTimeout(() => {
      let list = this.dictionaryItemList.slice(0, this.bufferSize);
      const findItem = this.dictionaryItemList.find(item => item.id == this.selectedValue);
      const bufferItem = list.find(item => item.id == this.selectedValue)
      if (this.selectedValue && !bufferItem && findItem) {
        list.unshift(findItem);
      }
      this.dictionaryBuffer = list;
    }, 100);
  }

  propagateChange = (_: any) => { };

  constructor(
  ) { }

  ngOnInit() {
  }

  onSearch(value: string) {
    this.searchedDictionaryItemList = this.dictionaryItemList.filter((t: any) => !value || t.name.toLocaleLowerCase().indexOf(value.toLocaleLowerCase()) !== -1);
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
  }

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
  }
}
