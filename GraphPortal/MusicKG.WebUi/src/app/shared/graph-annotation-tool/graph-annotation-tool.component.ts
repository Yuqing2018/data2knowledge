import { Component, OnInit, ViewChild, ElementRef, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms'

@Component({
  selector: 'km-graph-annotation-tool',
  templateUrl: './graph-annotation-tool.component.html',
  styleUrls: ['./graph-annotation-tool.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => GraphAnnotationToolComponent),
      multi: true
    }
  ]
})
export class GraphAnnotationToolComponent implements OnInit {

  tags: Array<any> = []
  inputVisible: boolean = false
  inputValue: string = ''
  src: string

  @ViewChild('inputElement') inputElement: ElementRef;

  constructor() { }

  ngOnInit() {
  }

  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any): void {
  }

  propagateChange = (_: any) => { };

  writeValue(value: any) {
    if (value) {
      this.src = value
    }
  }

  handleClose(removedTag): void {
    this.tags = this.tags.filter(val => val.id !== removedTag.id)
  }

  sliceTagName(tag: string): string {
    const isLongTag = tag.length > 20;
    return isLongTag ? `${tag.slice(0, 20)}...` : tag;
  }

}
